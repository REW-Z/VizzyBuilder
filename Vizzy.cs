using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace REWVIZZY
{

    // ***************************************** Base ***************************************

    /// <summary>
    /// Vz程序
    /// </summary>
    public class VzProgram
    {
        public string name;
        public bool requiresMfd;



        public List<VzBaseGlobalVariable> variables;
        public List<VzInstructionStart> instructionStarts;
        public List<VzCustomExpression> customExpressions;

        public VzProgram(string name, bool isMfd)
        {
            this.name = name;
            this.requiresMfd = isMfd;

            variables = new List<VzBaseGlobalVariable>();
            instructionStarts = new List<VzInstructionStart>();
            customExpressions = new List<VzCustomExpression>();
        }

        public XElement Serialize()
        {
            XElement xProgram = new XElement("Program", new XAttribute("name", (name != null ? name : "New Program") ));

            if (this.requiresMfd == true)
                xProgram.Add(new XAttribute("requiresMfd", "true"));

            //ID Generator
            IDGenerator idGenerator = new IDGenerator();
            PosLayoutTool posTool = new PosLayoutTool();


            //Variables
            XElement xVarables = new XElement("Variables");
            xProgram.Add(xVarables);
            foreach (var variable in this.variables)
            {
                xVarables.Add(variable.DeclarationSerialize());
            }



            //Instructions  
            foreach (var instructionStart in this.instructionStarts)
            {
                //id
                instructionStart.SetId(idGenerator);

                //pos
                instructionStart.SetPos(posTool);

                XElement xInstructions = new XElement("Instructions");

                xInstructions.Add(instructionStart.Serialize());
                
                foreach (var instruction in instructionStart.followingInstructions)
                {
                    //id
                    instruction.SetId(idGenerator);

                    xInstructions.Add(instruction.Serialize());
                }

                xProgram.Add(xInstructions);
            }

            //Expressions
            XElement xCustomExpressions = new XElement("Expressions");
            xProgram.Add(xCustomExpressions);
            foreach (var customExpression in this.customExpressions)
            {
                xCustomExpressions.Add(customExpression.Serialize());
            }


            return xProgram;
        }
    }

    /// <summary>
    /// 表达式 （REW：可取值对象）
    /// </summary>
    public abstract class VzExpression
    {
        public static implicit operator VzExpression(float value)
        {
            return new Constant(value);
        }
        public static implicit operator VzExpression(int value)
        {
            return new Constant((float)value);
        }
        public static implicit operator VzExpression(bool value)
        {
            return new Constant(value);
        }
        public static implicit operator VzExpression(string value)
        {
            return new Constant(value);
        }
        public static implicit operator VzExpression(Vec3 value)
        {
            if (value.IsConstant)
                return value.AsConstant();
            else
                return value.AsExpression();
        }
        public static implicit operator VzExpression(Vec2 value)
        {
            return new Constant(value);
        }

        public static VzExpression operator -(VzExpression a)
        {
            return new BinaryOp(a, new Constant(-1f), BinaryOpType.Mul);
        }
        public static VzExpression operator +(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Plus);
        }
        public static VzExpression operator -(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Sub);
        }
        public static VzExpression operator *(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Mul);
        }
        public static VzExpression operator /(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Div);
        }
        public static VzExpression operator ^(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Exp);
        }
        public static VzExpression operator %(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Mod);
        }
        public static VzExpression operator !(VzExpression a) 
        {
            return new NotExpression(a);
        }
        public static VzExpression operator ==(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Equal);
        }
        public static VzExpression operator !=(VzExpression a, VzExpression b)
        {
            return new NotExpression(new BinaryOp(a, b, BinaryOpType.Equal));
        }
        public static VzExpression operator <(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.LessThan);
        }
        public static VzExpression operator >(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.GreaterThan);
        }
        public static VzExpression operator <=(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.LessEqual);
        }
        public static VzExpression operator >=(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.GreaterEqual);
        }
        public static VzExpression operator &(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.And);
        }
        public static VzExpression operator |(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Or);
        }
        public static bool operator true(VzExpression expr)
        {
            return false;
        }
        public static bool operator false(VzExpression expr)
        {
            return false;
        }


        //一元向量运算表达式
        public VzExpression X => new VectorOp(this, VectorOpType.X, null);
        public VzExpression Y => new VectorOp(this, VectorOpType.Y, null);
        public VzExpression Z => new VectorOp(this, VectorOpType.Z, null);
        public VzExpression Magnitude => new VectorOp(this, VectorOpType.Magnitude, null);
        public VzExpression Normal => new VectorOp(this, VectorOpType.Norm, null);

        // ---------------------------------------------------- 上层调用 ---------------------------------------------------------
        // 变量操作  
        public VzInstruction Set(VzExpression value)
        {
            return new SetVariableInstruction(this, value).AppendToCurrentContext();
        }
        public VzInstruction ChangeBy(VzExpression valueby)
        {
            return new ChangeVariableInstruction(this, valueby).AppendToCurrentContext();
        }
        public VzInstruction ReadUserInput(VzExpression textShow)
        {
            return new UserInputInstruction(this, textShow).AppendToCurrentContext();
        }
        public VzExpression Value
        {
            get { return this; }
            set { new SetVariableInstruction(this, value).AppendToCurrentContext(); }
        }

        // 列表操作
        public ListExpression Get(VzExpression idx)
        {
            return new ListExpression( ListExpressionType.Get, this, idx);
        }
        public VzInstruction Set(VzExpression idx, VzExpression value)
        {
            return new ListOpInstruction(ListInstructionType.Set, this, value, idx).AppendToCurrentContext();
        }
        public ListExpression this[VzExpression idx]
        {
            get { return new ListExpression(ListExpressionType.Get, this, idx); }
            set { new ListOpInstruction(ListInstructionType.Set, this, value, idx).AppendToCurrentContext(); }
        }

        public ListExpression Count => ListExpression.GetListLength(this);

        public VzInstruction AddToList(VzExpression item)
        {
            return new ListOpInstruction(ListInstructionType.Add, this, item).AppendToCurrentContext();
        }
        public VzInstruction InsertToList(VzExpression index, VzExpression item)
        {
            return new ListOpInstruction(ListInstructionType.Insert, this, item, index).AppendToCurrentContext();
        }
        public VzInstruction RemoveFromList(VzExpression index)
        {
            return new ListOpInstruction(ListInstructionType.Remove, this, null, index).AppendToCurrentContext();
        }
        public VzInstruction ClearList()
        {
            return new ListOpInstruction(ListInstructionType.Clear, this, null).AppendToCurrentContext();
        }
        public VzInstruction SortList()
        {
            return new ListOpInstruction(ListInstructionType.Sort, this, null).AppendToCurrentContext();
        }
        // ---------------------------------------------------- 上层调用 ---------------------------------------------------------




        public static bool IsNull(VzExpression expr)
        {
            return object.Equals(expr, null); 
        }

        public abstract XElement Serialize();
    }

    /// <summary>
    /// Vz全局变量基类  
    /// </summary>
    public abstract class VzBaseGlobalVariable : VzExpression
    {
        public string name;

        public abstract XElement DeclarationSerialize();
    }
    /// <summary>
    /// Vz全局变量
    /// </summary>
    public class VzVariable : VzBaseGlobalVariable
    {
        private object value;

        public VzVariable(string varName)
        {
            this.name = varName;
        }

        public override XElement DeclarationSerialize()
        {
            XElement xVar = new XElement("Variable");

            xVar.Add(new XAttribute("name", this.name));
            xVar.Add(new XAttribute("number", "0"));

            return xVar;
        }

        public override XElement Serialize()
        {
            XElement xVar = new XElement("Variable",
                new XAttribute("list", "false"),
                new XAttribute("local", "false"),
                new XAttribute("variableName", this.name)
                );

            return xVar;
        }
    }
    public class VzListVariable : VzBaseGlobalVariable
    {
        public List<object> items;

        public VzListVariable(string listName)
        {
            this.name = listName;
        }

        public override XElement DeclarationSerialize()
        {
            XElement xVar = new XElement("Variable");

            xVar.Add(new XAttribute("name", this.name));
            xVar.Add(new XElement("Items"));

            return xVar;
        }

        public override XElement Serialize()
        {
            XElement xVar = new XElement("Variable",
                new XAttribute("list", "true"),
                new XAttribute("local", "false"),
                new XAttribute("variableName", this.name)
                );

            return xVar;
        }
    }


    ///// <summary>
    ///// 事件/命令参数
    ///// </summary>
    //public abstract class VzParameter : VzExpression
    //{
    //    public abstract XElement Serialize();
    //}
    /// <summary>
    /// 常量参数
    /// </summary>
    public class Constant : VzExpression
    {
        public bool canReplace = true; //事件常量例如Receive的Message不能替代    

        public object value;

        public Constant(object constValue, bool canreplace = true)
        {
            this.canReplace = canreplace;
            this.value = constValue;
        }

        public static implicit operator Constant(float value)
        {
            return new Constant(value);
        }
        public static implicit operator Constant(int value)
        {
            return new Constant((float)value);
        }
        public static implicit operator Constant(bool value)
        {
            return new Constant(value);
        }
        public static implicit operator Constant(string value)
        {
            return new Constant(value);
        }
        public static implicit operator Constant(Vec3 value)
        {
            return new Constant(value);
        }
        public static implicit operator Constant(Vec2 value)
        {
            return new Constant(value);
        }

        public override XElement Serialize()// <Constant canReplace="false" text="" />
        {
            List<XAttribute> xValues = new List<XAttribute>();

            if (canReplace == false)
            {
                xValues.Add(new XAttribute("canReplace", "false"));
            }

            switch (value.GetType().Name)
            {
                case "Single":
                    xValues.Add(new XAttribute("number", ((float)this.value).ToString()));
                    break;
                case "Int32":
                    xValues.Add(new XAttribute("number", ((int)this.value).ToString()));
                    break;
                case "String":
                    xValues.Add(new XAttribute("text", ((string)this.value)));
                    break;
                case "Boolean":
                    xValues.Add(new XAttribute("style", ((bool)this.value).ToString().ToLower()));
                    xValues.Add(new XAttribute("bool", ((bool)this.value).ToString().ToLower()));
                    break;
                case "Vec3":
                    xValues.Add(new XAttribute("vector", ((Vec3)value).ToConstantFormat() ));
                    break;
                case "Vec2":
                    xValues.Add(new XAttribute("text", ((Vec2)value).ToConstantFormat()));
                    break;
                default:
                    xValues.Add(new XAttribute("number", "0"));
                    break;
            }

            return new XElement("Constant",
                xValues
                );
        }
    }
    /// <summary>
    /// 局部变量参数
    /// </summary>
    public class LocalVariable : VzExpression
    {
        public bool islist = false;

        public string variableName;

        public LocalVariable(string variableName, bool isList = false)
        {
            this.variableName = variableName;
            this.islist = isList;
        }

        public override XElement Serialize()
        {
            return new XElement("Variable",
                new XAttribute("list", islist.ToString().ToLower()),
                new XAttribute("local", "true"),
                new XAttribute("variableName", this.variableName)
                );
        }
    }



    /// <summary>
    /// Vz节点
    /// </summary>
    public abstract class VzNode
    {
        public int id;

        public int posx;
        public int posy;


        public virtual void SetId(IDGenerator generator)
        {
            this.id = generator.Next();

            if (this is ISubInstructionContainer)
            {
                foreach (VzInstruction sub in (this as ISubInstructionContainer).GetSubInstructions())
                {
                    sub.SetId(generator);
                }
            }
        }

        public virtual void SetPos(PosLayoutTool posTool)
        {
            posTool.NextPos(out this.posx, out this.posy);
        }

        public abstract XElement Serialize();
    }

    /// <summary>
    /// Vz起始节点（事件或者表达式）  
    /// </summary>
    public abstract class VzInstructionStart : VzInstruction, ISubInstructionContainer
    {
        public List<VzInstruction> followingInstructions;

        public List<VzInstruction> GetSubInstructions() { return this.followingInstructions; }

        public void Append(VzInstruction newinstruction) { this.followingInstructions.Add(newinstruction); }

        public VzInstructionStart()
        {
            followingInstructions = new List<VzInstruction>();
        }

    }

    /// <summary>
    /// 事件基类节点
    /// </summary>
    public abstract class VzEvent : VzInstructionStart
    {
    }

    /// <summary>
    /// 指令节点基类
    /// </summary>
    public abstract class VzInstruction : VzNode
    {
    }
    /// <summary>
    /// 子指令容器接口
    /// </summary>
    public interface ISubInstructionContainer
    {
        List<VzInstruction> GetSubInstructions();

        void Append(VzInstruction newinstruction);
    }




    public delegate VzExpression VzFunc(params VzExpression[] args);
    public delegate void VzAction(params VzExpression[] args);

    /// <summary>
    /// 自定义指令节点
    /// </summary>
    public class VzCustomInstruction : VzInstructionStart
    {
        public VzAction call = null;

        public string customInstructionName;

        public LocalVariable[] localVarParameters;

        public VzCustomInstruction(string instructionName, params string[] parameterNames)
        {
            localVarParameters = new LocalVariable[parameterNames.Length];

            for (int i = 0; i < localVarParameters.Length; ++i)
            {
                this.localVarParameters[i] = new LocalVariable(parameterNames[i]);
            }
            this.customInstructionName = instructionName;
            this.followingInstructions = new List<VzInstruction>();

            this.call = (args) => { new CallCustomInstruction(this, args).AppendToCurrentContext(); };
        }

        public LocalVariable this[string name]
        {
            get
            {
                return localVarParameters.FirstOrDefault(p => p.variableName == name);
            }
            set
            {
                var param = localVarParameters.FirstOrDefault(p => p.variableName == name);
                if (!VzExpression.IsNull(param)) param = value;
            }
        }

        public void SetInstructionsInternal(params VzInstruction[] instructions)
        {
            this.followingInstructions = instructions.ToList();
        }


        public string GetCallFormt()
        {
            string result = customInstructionName + " ";
            for (int i = 0; i < localVarParameters.Length; ++i)
            {
                result += "(";
                result += i.ToString();
                result += ") ";
            }
            return result;
        }
        public string GetFormat()
        {
            string result = customInstructionName + " ";
            foreach (var localVar in localVarParameters)
            {
                result += "|";
                result += localVar.variableName;
                result += "| ";
            }

            //result += " result (0)"; //no return  
            return result;
        }

        public override XElement Serialize()
        {
            XElement xCustomInstruction = new XElement("CustomInstruction",
                new XAttribute("callFormat", GetCallFormt()),
                new XAttribute("format", GetFormat()),
                new XAttribute("name", customInstructionName),
                new XAttribute("id", this.id),
                new XAttribute("style", "custom-instruction"),
                new XAttribute("pos", this.posx.ToString() + "," + this.posy.ToString())
                );

            return xCustomInstruction;
        }

    }



    /// <summary>
    /// 自定义表达式
    /// </summary>
    public class VzCustomExpression : VzExpression
    {
        public VzFunc call = null;


        public float posx;
        public float posy;

        public string customExpressionName;

        public LocalVariable[] localVarParameters;

        public VzExpression returnExpression = null;


        public VzCustomExpression(string expressionName, params string[] parameterNames)
        {
            this.customExpressionName = expressionName;

            this.localVarParameters = new LocalVariable[parameterNames.Length];

            for (int i = 0; i < parameterNames.Length; ++i)
            {
                this.localVarParameters[i] = new LocalVariable(parameterNames[i]);
            }

            this.call = (args) => { return new CallCustomExpression(this, args); };
        }

        public LocalVariable this[string name]
        {
            get
            { 
                return localVarParameters.FirstOrDefault(p => p.variableName == name);
            }
            set 
            {
                var param = localVarParameters.FirstOrDefault(p => p.variableName == name);
                if (!VzExpression.IsNull(param)) param = value;
            }
        }


        public void SetReturnInternal(VzExpression returnExpressionSet)
        {
            this.returnExpression = returnExpressionSet;
        }

        public string GetCallFormt()
        {
            string result = customExpressionName + " ";
            for (int i = 0; i < localVarParameters.Length; ++i)
            {
                result += "(";
                result += i.ToString();
                result += ") ";
            }
            return result;
        }
        public string GetFormat()
        {
            string result = customExpressionName + " ";
            foreach (var localVar in localVarParameters)
            {
                result += "|";
                result += localVar.variableName;
                result += "| ";
            }

            result += " result (0)";
            return result;
        }
        public override XElement Serialize()
        {
            if (VzExpression.IsNull( returnExpression))
            {
                throw new NullReferenceException("Return Expression Not Set !");
            }

            XElement xCustomExpression = new XElement("CustomExpression",
                new XAttribute("callFormat", GetCallFormt()),
                new XAttribute("format", GetFormat()),
                new XAttribute("name", this.customExpressionName),
                new XAttribute("style", "custom-expression"),
                new XAttribute("pos", posx.ToString() + "," + posy.ToString())
                );

            xCustomExpression.Add(returnExpression.Serialize());

            return xCustomExpression;
        }
    }

    public class CallCustomExpression : VzExpression
    {
        public VzCustomExpression customExpression = null;

        public VzExpression[] args = null;

        public CallCustomExpression(VzCustomExpression expression, params VzExpression[] args)
        {
            this.customExpression = expression;
            this.args = args;
        }

        public override XElement Serialize()
        {
            XElement xCall = new XElement("CallCustomExpression",
                new XAttribute("call", customExpression.customExpressionName),
                new XAttribute("style", "call-custom-expression")
                );

            foreach (var arg in args)
            {
                xCall.Add(arg.Serialize());
            }

            return xCall;
        }
    }

    public class CallCustomInstruction : VzInstruction
    {
        public VzCustomInstruction customInstruction = null;

        public List<VzExpression> args = null;

        public CallCustomInstruction(VzCustomInstruction customInstruction, params VzExpression[] args)
        {
            this.customInstruction = customInstruction;
            this.args = args.ToList();
        }


        public override XElement Serialize()
        {
            XElement xCall = new XElement("CallCustomInstruction",
                new XAttribute("call", customInstruction.customInstructionName),
                new XAttribute("id", this.id),
                new XAttribute("style", "call-custom-instruction")
                );

            foreach (var arg in args)
            {
                xCall.Add(arg.Serialize());
            }

            return xCall;
        }
    }





    // ***************************************** Events *********************************************

    public class VzOnStartEvent : VzEvent
    {
        public override XElement Serialize()
        {
            return new XElement("Event",
                new XAttribute("event", "FlightStart"),
                new XAttribute("id", this.id),
                new XAttribute("style", "flight-start"),
                new XAttribute("pos", posx.ToString() + "," + posy.ToString())
                );
        }
    }

    public class VzOnPartCollisionEvent : VzEvent
    {
        public LocalVariable part;
        public LocalVariable other;
        public LocalVariable velocity;
        public LocalVariable impulse;

        public VzOnPartCollisionEvent()
        {
            part = new LocalVariable("part", false);
            other = new LocalVariable("other", false);
            velocity = new LocalVariable("velocity", false);
            impulse = new LocalVariable("impulse", false);
        }

        public override XElement Serialize()
        {
            return new XElement("Event",
                new XAttribute("event", "PartCollision"),
                new XAttribute("id", this.id),
                new XAttribute("style", "part-collision"),
                new XAttribute("pos", posx.ToString() + "," + posy.ToString())
                );
        }
    }

    public class VzOnDockedEvent : VzEvent
    {
        public LocalVariable craftA;
        public LocalVariable craftB;

        public VzOnDockedEvent()
        {
            craftA = new LocalVariable("craftA", false);
            craftB = new LocalVariable("craftB", false);
        }

        public override XElement Serialize()
        {
            return new XElement("Event",
                new XAttribute("event", "Docked"),
                new XAttribute("id", this.id),
                new XAttribute("style", "craft-docked"),
                new XAttribute("pos", posx.ToString() + "," + posy.ToString())
                );
        }
    }

    public class VzOnChangeSoiEvent: VzEvent
    {
        public LocalVariable planet;

        public VzOnChangeSoiEvent()
        {
            planet = new LocalVariable("planet");
        }

        public override XElement Serialize()
        {
            return new XElement("Event",
                new XAttribute("event", "ChangeSoi"),
                new XAttribute("id", this.id),
                new XAttribute("style", "change-soi"),
                new XAttribute("pos", posx.ToString() + "," + posy.ToString())
                );
        }
    }

    public class VzOnPartExplodeEvent : VzEvent
    {
        public LocalVariable part;

        public VzOnPartExplodeEvent()
        {
            part = new LocalVariable("part", false);
        }

        public override XElement Serialize()
        {
            return new XElement("Event",
                new XAttribute("event", "PartExplode"),
                new XAttribute("id", this.id),
                new XAttribute("style", "part-explode"),
                new XAttribute("pos", posx.ToString() + "," + posy.ToString())
                );
        }
    }

    public class VzOnReceiveMessageEvent : VzEvent
    {
        public Constant msg;

        public LocalVariable data;

        public VzOnReceiveMessageEvent(string msgStr)
        {
            this.msg = new Constant(msgStr, canreplace: false); // canReplace = false

            this.data = new LocalVariable("data", false); //is list ??
        }

        public override XElement Serialize()
        {
            XElement xEvent = new XElement("Event",
                new XAttribute("event", "ReceiveMessage"),
                new XAttribute("id", this.id),
                new XAttribute("style", "receive-msg"),
                new XAttribute("pos", posx.ToString() + "," + posy.ToString())
                );
            xEvent.Add(msg.Serialize());
            return xEvent;
        }
    }

    // ***************************************** Instructions ***************************************


    // *** Flows ***

    public class WaitInstruction : VzInstruction
    {
        public VzExpression timeParameter;

        public WaitInstruction(VzExpression time)
        {
            this.timeParameter = time;
        }

        public override XElement Serialize()
        {
            var xWaitSec = new XElement("WaitSeconds",
                new XAttribute("id", this.id),
                new XAttribute("style", "wait-seconds")
                );

            xWaitSec.Add(timeParameter.Serialize());

            return xWaitSec;
        }
    }

    public class WaitUntilInstruction : VzInstruction
    {
        public VzExpression conditionParameter;

        public WaitUntilInstruction(VzExpression condition)
        {
            this.conditionParameter = condition;
        }

        public override XElement Serialize()
        {
            var xWaitUntil = new XElement("WaitUntil",
                new XAttribute("id", this.id),
                new XAttribute("style", "wait-until")
                );

            xWaitUntil.Add(this.conditionParameter.Serialize());
            return xWaitUntil;
        }
    }

    public class RepeatInstruction : VzInstruction, ISubInstructionContainer
    {
        public VzExpression countParameter;

        public List<VzInstruction> subInstructions;

        public List<VzInstruction> GetSubInstructions() { return this.subInstructions; }

        public void Append(VzInstruction newinstruction) { this.subInstructions.Add(newinstruction); }

        public RepeatInstruction(VzExpression count, params VzInstruction[] instructions)
        {
            this.countParameter = count;
            this.subInstructions = new List<VzInstruction>();
            this.subInstructions.AddRange(instructions);
        }


        public override XElement Serialize()
        {
            XElement xRepeat = new XElement("Repeat",
                new XAttribute("id", this.id),
                new XAttribute("style", "repeat")
                );
            xRepeat.Add(countParameter.Serialize());

            XElement xSubInstructions = new XElement("Instructions");

            foreach (var instruction in subInstructions)
            {
                XElement xinstruction = instruction.Serialize();
                xSubInstructions.Add(xinstruction);
            }

            xRepeat.Add(xSubInstructions);

            return xRepeat;
        }
    }

    public class WhileInstruction : VzInstruction, ISubInstructionContainer
    {
        public VzExpression conditionParameter;

        public List<VzInstruction> subInstructions;

        public List<VzInstruction> GetSubInstructions() { return this.subInstructions; }

        public void Append(VzInstruction newinstruction) { this.subInstructions.Add(newinstruction); }

        public WhileInstruction(VzExpression condition, params VzInstruction[] instructions)
        {
            this.conditionParameter = condition;

            subInstructions = new List<VzInstruction>();
            subInstructions.AddRange(instructions);
        }

        public override XElement Serialize()
        {
            XElement xWhile = new XElement("While",
                new XAttribute("id", this.id),
                new XAttribute("style", "while")
                );
            xWhile.Add(conditionParameter.Serialize());

            XElement xSubInstructions = new XElement("Instructions");

            foreach (var instruction in subInstructions)
            {
                XElement xinstruction = instruction.Serialize();
                xSubInstructions.Add(xinstruction);
            }

            xWhile.Add(xSubInstructions);

            return xWhile;
        }
    }

    public class ForInstruction : VzInstruction, ISubInstructionContainer
    {
        public LocalVariable i;

        public VzExpression from;

        public VzExpression to;

        public VzExpression by;

        public List<VzInstruction> subInstructions;

        public List<VzInstruction> GetSubInstructions() { return this.subInstructions; }

        public void Append(VzInstruction newinstruction) { this.subInstructions.Add(newinstruction); }

        public ForInstruction(VzExpression _from, VzExpression _to, VzExpression _by, params VzInstruction[] instructions)
        {
            this.from = _from;
            this.to = _to;
            this.by = _by;

            this.i = new LocalVariable("i", false);

            this.subInstructions = new List<VzInstruction>();
            this.subInstructions.AddRange(instructions);
        }

        public override XElement Serialize()
        {
            XElement xFor = new XElement("For",
                new XAttribute("id", this.id),
                new XAttribute("style", "for")
                );

            xFor.Add(from.Serialize());
            xFor.Add(to.Serialize());
            xFor.Add(by.Serialize());


            XElement xSubInstructions = new XElement("Instructions");

            foreach (var instruciton in this.subInstructions)
            {
                xSubInstructions.Add(instruciton.Serialize());
            }

            xFor.Add(xSubInstructions);

            return xFor;
        }
    }


    public class IfInstruction : VzInstruction, ISubInstructionContainer
    {
        public VzExpression conditionParameter;

        public List<VzInstruction> subInstructions;

        public List<VzInstruction> GetSubInstructions() { return this.subInstructions; }

        public void Append(VzInstruction newinstruction) { this.subInstructions.Add(newinstruction); }

        public IfInstruction(VzExpression condition, params VzInstruction[] instructions)
        {
            this.conditionParameter = condition;
            this.subInstructions = new List<VzInstruction>();
            subInstructions.AddRange(instructions);
        }

        public override XElement Serialize()
        {
            XElement xIf = new XElement("If",
                new XAttribute("id", this.id),
                new XAttribute("style", "if")
                );

            xIf.Add(conditionParameter.Serialize());


            XElement xSubInstructions = new XElement("Instructions");

            foreach (var instruction in this.subInstructions)
            {
                xSubInstructions.Add(instruction.Serialize());
            }

            xIf.Add(xSubInstructions);

            return xIf;
        }
    }

    public class ElseIfInstruction : VzInstruction, ISubInstructionContainer
    {
        public VzExpression conditionParameter;

        public List<VzInstruction> subInstructions;

        public List<VzInstruction> GetSubInstructions() { return this.subInstructions; }

        public void Append(VzInstruction newinstruction) { this.subInstructions.Add(newinstruction); }

        public ElseIfInstruction(VzExpression condition, params VzInstruction[] instructions)
        {
            this.conditionParameter = condition;
            this.subInstructions = new List<VzInstruction>();
            this.subInstructions.AddRange(instructions);
        }

        public override XElement Serialize()
        {
            XElement xElif = new XElement("ElseIf",
                new XAttribute("id", this.id),
                new XAttribute("style", "else-if")
                );

            xElif.Add(conditionParameter.Serialize());

            XElement xSubInstructions = new XElement("Instructions");

            foreach (var instruction in this.subInstructions)
            {
                xSubInstructions.Add(instruction.Serialize());
            }

            xElif.Add(xSubInstructions);


            return xElif;
        }
    }

    public class ElseInstruction : VzInstruction, ISubInstructionContainer
    {
        public Constant conditionTrue;  //true  

        public List<VzInstruction> subInstructions;

        public List<VzInstruction> GetSubInstructions() { return this.subInstructions; }

        public void Append(VzInstruction newinstruction) { this.subInstructions.Add(newinstruction); }

        public ElseInstruction(params VzInstruction[] instructions)
        {
            conditionTrue = new Constant(true);

            subInstructions = new List<VzInstruction>();
            subInstructions.AddRange(instructions);
        }

        public override XElement Serialize()
        {
            XElement xElif = new XElement("ElseIf",
                new XAttribute("id", this.id),
                new XAttribute("style", "else")
                );

            xElif.Add(conditionTrue.Serialize());

            XElement xSubInstructions = new XElement("Instructions");

            foreach (var instruction in this.subInstructions)
            {
                xSubInstructions.Add(instruction.Serialize());
            }

            xElif.Add(xSubInstructions);

            return xElif;
        }
    }

    public class BreakInstrucion : VzInstruction
    {
        public override XElement Serialize()
        {
            return new XElement("Break", new XAttribute("id", this.id), new XAttribute("style", "break"));
        }
    }


    public class DisplayMessageInstruction : VzInstruction
    {
        public VzExpression textParameter;
        public Constant constnumber;// 7

        public DisplayMessageInstruction(VzExpression text)
        {
            textParameter = text;
            constnumber = new Constant(7);
        }

        public override XElement Serialize()
        {
            XElement xDisplay = new XElement("DisplayMessage",
                new XAttribute("id", this.id),
                new XAttribute("style", "display")
                );

            xDisplay.Add(textParameter.Serialize());
            xDisplay.Add(constnumber.Serialize());

            return xDisplay;
        }
    }

    public class LogInstruction : VzInstruction
    {
        public VzExpression textParameter;

        public LogInstruction(VzExpression text)
        {
            textParameter = text;
        }

        public override XElement Serialize()
        {
            XElement xLog = new XElement("LogMessage",
                new XAttribute("id", this.id),
                new XAttribute("style", "log")
                );

            xLog.Add(textParameter.Serialize());
            return xLog;
        }
    }

    public class FlightLogInstruction : VzInstruction
    {
        public VzExpression textParameter;
        public VzExpression overrideParameter;

        public FlightLogInstruction(VzExpression text, VzExpression isOverride)
        {
            textParameter = text;
            overrideParameter = isOverride;
        }

        public override XElement Serialize()
        {
            XElement xLog = new XElement("LogFlight",
                new XAttribute("id", this.id),
                new XAttribute("style", "flightlog")
                );

            xLog.Add(textParameter.Serialize());
            xLog.Add(overrideParameter.Serialize());
            return xLog;
        }
    }




    // **************************** 变量指令 ***********************

    public class SetVariableInstruction : VzInstruction
    {
        public VzExpression targetVariable = null;
        public VzExpression targetValue = null;

        public SetVariableInstruction(VzExpression variable, VzExpression value)
        {
            this.targetVariable = variable;
            this.targetValue = value;
        }

        public override XElement Serialize()
        {
            XElement xSet = new XElement("SetVariable",
                new XAttribute("id", this.id),
                new XAttribute("style", "set-variable")
                );

            xSet.Add(targetVariable.Serialize());
            xSet.Add(targetValue.Serialize());

            return xSet;
        }
    }


    public class ChangeVariableInstruction : VzInstruction
    {
        public VzExpression targetVariable = null;
        public VzExpression targetValueChange = null;

        public ChangeVariableInstruction(VzExpression variable, VzExpression valueBy)
        {
            this.targetVariable = variable;
            this.targetValueChange = valueBy;
        }

        public override XElement Serialize()
        {
            XElement xChange = new XElement("ChangeVariable",
                new XAttribute("id", this.id),
                new XAttribute("style", "change-variable")
                );
            xChange.Add(targetVariable.Serialize());
            xChange.Add(targetValueChange.Serialize());

            return xChange;
        }
    }

    public class UserInputInstruction : VzInstruction
    {
        public VzExpression targetVariable;
        public VzExpression textShow;

        public UserInputInstruction(VzExpression targetVariable, VzExpression textShow)
        {
            this.targetVariable = targetVariable;
            this.textShow = textShow;
        }

        public override XElement Serialize()
        {
            XElement xUserInput = new XElement("UserInput",
                new XAttribute("id", this.id),
                new XAttribute("style", "user-input")
                );
            xUserInput.Add(targetVariable.Serialize());
            xUserInput.Add(textShow.Serialize());

            return xUserInput;
        }
    }



    // ***************************** 其他基础指令 ***************************

    public enum BroadCastType
    {
        Normal,
        Craft,
        AllCraft
    }
    public class BroadcastInstruction : VzInstruction
    {
        public BroadCastType type;
        public VzExpression msg;
        public VzExpression data;

        public BroadcastInstruction(BroadCastType type, VzExpression msg, VzExpression data)
        {
            this.type = type;
            this.msg = msg;
            this.data = data;
        }

        public override XElement Serialize()
        {
            string global = "";
            string local = "";
            string style = "";
            switch (type)
            {
                case BroadCastType.Normal:
                    {
                        global = "false";
                        local = "true";
                        style = "broadcast-msg";
                    }
                    break;
                case BroadCastType.Craft:
                    {
                        global = "false";
                        local = "false";
                        style = "broadcast-msg-craft";
                    }
                    break;
                case BroadCastType.AllCraft:
                    {
                        global = "true";
                        local = "true";
                        style = "broadcast-msg-all-crafts";
                    }
                    break;
                default:
                    break;
            }

            XElement xB = new XElement("BroadcastMessage",
                new XAttribute("global", global),
                new XAttribute("local", local),
                new XAttribute("id", this.id),
                new XAttribute("style", style)
                );

            xB.Add(msg.Serialize());
            xB.Add(data.Serialize());

            return xB;
        }

    }


    public class CommentInstruction : VzInstruction
    {
        public Constant comment;

        public CommentInstruction(string txt)
        {
            comment = new Constant(txt, false);
        }

        public override XElement Serialize()
        {
            XElement xComment = new XElement("Comment",
                new XAttribute("id", this.id),
                new XAttribute("style", "comment")
                );
            xComment.Add(comment.Serialize());
            return xComment;
        }
    }





    // ****************************** 其他定义 *****************************
    public enum PhysicalQuantity
    {
        Acceleration,
        AngularVelocity,
        Density,
        Distance,
        Energy,
        Force,
        SpecificImpulse,
        Mass,
        Power,
        Pressure,
        Temperature,
        Time,
        Velocity,
    }



    /// <summary>
    /// Vec3 Constant / VzExpression
    /// </summary>
    public class Vec3
    {
        private VzExpression x;
        private VzExpression y;
        private VzExpression z;

        private bool isConstant = false;
        public bool IsConstant => this.isConstant;


        public Vec3(float x, float y, float z)
        {
            this.x = x; this.y = y; this.z = z;
            this.isConstant = true;
        }

        public Vec3(VzExpression x, VzExpression y, VzExpression z)
        {
            this.x = x; this.y = y; this.z = z;
            this.isConstant = false;
        }

        public static VzExpression Angle(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Angle, vec2);
        }
        public static VzExpression Clamp(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Clamp, vec2);
        }
        public static VzExpression Cross(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Cross, vec2);
        }
        public static VzExpression Dot(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Dot, vec2);
        }
        public static VzExpression Distance(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Distance, vec2);
        }
        public static VzExpression Min(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Min, vec2);
        }
        public static VzExpression Max(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Max, vec2);
        }
        public static VzExpression Project(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Project, vec2);
        }
        public static VzExpression Scale(VzExpression vec1, VzExpression vec2)
        {
            return new VectorOp(vec1, VectorOpType.Scale, vec2);
        }

        public string ToConstantFormat()
        {
            if (!isConstant) throw new VizzyException("this Vec3 Is Not Constant");

            string result = "(";
            result += ((float)(x as Constant).value).ToString();
            result += ", ";
            result += ((float)(y as Constant).value).ToString();
            result += ", ";
            result += ((float)(z as Constant).value).ToString();
            result += ")";
            return (result);
        }
        public VzExpression AsConstant()
        {
            return new Constant(this);
        }
        public VzExpression AsExpression()
        {
            if (isConstant) throw new VizzyException("Cant Serialize Non-Constant Vec3 To XElement");
            return new NewVectorExpression(x, y, z);
        }
    }

    /// <summary>
    /// Vec2 Constant
    /// </summary>
    public class Vec2
    {
        public float x;
        public float y;
        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public string ToConstantFormat()
        {
            string result = "";
            result += x.ToString();
            result += ",";
            result += y.ToString();
            return result;
        }
    }
}
