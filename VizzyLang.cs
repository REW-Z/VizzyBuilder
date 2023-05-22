using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Collections;

namespace REWVIZZY
{
    // ********************** VizzyLang *************************  

    /// <summary>
    /// Vizzy指令上下文
    /// </summary>
    public class InstructionContext
    {
        /// <summary>
        /// 当前程序
        /// </summary>
        public VzProgram currentProgram;

        /// <summary>
        /// 当前代码块堆栈
        /// </summary>
        public Stack<ISubInstructionContainer> instructionStack = new Stack<ISubInstructionContainer>();
    }
    public static partial class Vz
    {
        /// <summary>
        /// 当前代码上下文
        /// </summary>
        public static InstructionContext context;
        
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            Init("New Program", false);
        }
        public static void Init(string name, bool requireMfd)
        {
            context = new InstructionContext();
            context.currentProgram = new VzProgram(name, requireMfd);
        }



        /// <summary>
        /// 声明全局变量
        /// </summary>
        public static VzVariable DeclareGlobal(string varName)
        {
            var variable = new VzVariable(varName);
            context.currentProgram.variables.Add(variable);
            return variable;
        }

        /// <summary>
        /// 声明全局列表
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public static VzListVariable DeclareListGlobal(string listName)
        {
            var list = new VzListVariable(listName);
            context.currentProgram.variables.Add(list);
            return list;
        }
        
        /// <summary>
        /// 获取全局变量
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static VzVariable GetGlobal(string varName)
        {
            var variable = context.currentProgram.variables.FirstOrDefault(v => v.name == varName);

            if (VzExpression.IsNull(variable)) throw new VizzyException("Variable Not Found!");
            if (!(variable is VzVariable)) throw new VizzyException("Not A Vizzy Varible");

            return variable as VzVariable;
        }

        /// <summary>
        /// 查找全局列表变量
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public static VzListVariable GetGlobalList(string listName)
        {
            var listVariable = context.currentProgram.variables.FirstOrDefault(v => v.name == listName);

            if (VzExpression.IsNull(listVariable)) throw new VizzyException("Variable Not Found!");
            if (!(listVariable is VzListVariable)) throw new VizzyException("Not A Vizzy Varible");

            return listVariable as VzListVariable;
        }
        
        
        /// <summary>
        /// 自定义表达式
        /// </summary>
        /// <param name="name">表达式名称</param>
        /// <param name="paramNames">参数名列表</param>
        public static VzCustomExpression DeclareCustomExpression(string name, params string[] paramNames)
        {
            var customExp = new VzCustomExpression(name, paramNames);
            context.currentProgram.customExpressions.Add(customExp);
            return customExp;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, VzExpression ret)
        {
            // ---- Set Return ----
            customExpr.SetReturnInternal(ret);
            // --------------------
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder());
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression, VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder(customExpr.localVarParameters[0]));
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression, VzExpression, VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder(customExpr.localVarParameters[0], customExpr.localVarParameters[1]));
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression, VzExpression, VzExpression, VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder(customExpr.localVarParameters[0], customExpr.localVarParameters[1], customExpr.localVarParameters[2]));
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder(customExpr.localVarParameters[0], customExpr.localVarParameters[1], customExpr.localVarParameters[2], customExpr.localVarParameters[3]));
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder(customExpr.localVarParameters[0], customExpr.localVarParameters[1], customExpr.localVarParameters[2], customExpr.localVarParameters[3], customExpr.localVarParameters[4]));
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder(customExpr.localVarParameters[0], customExpr.localVarParameters[1], customExpr.localVarParameters[2], customExpr.localVarParameters[3], customExpr.localVarParameters[4], customExpr.localVarParameters[5]));
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder(customExpr.localVarParameters[0], customExpr.localVarParameters[1], customExpr.localVarParameters[2], customExpr.localVarParameters[3], customExpr.localVarParameters[4], customExpr.localVarParameters[5], customExpr.localVarParameters[6]));
            return customExpr.call;
        }
        public static VzFunc SetReturn(this VzCustomExpression customExpr, Func<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> funcBuilder)
        {
            customExpr.SetReturnInternal(funcBuilder(customExpr.localVarParameters[0], customExpr.localVarParameters[1], customExpr.localVarParameters[2], customExpr.localVarParameters[3], customExpr.localVarParameters[4], customExpr.localVarParameters[5], customExpr.localVarParameters[6], customExpr.localVarParameters[7]));
            return customExpr.call;
        }




        /// <summary>
        /// Custom Instruction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public static VzCustomInstruction DeclareCustomInstruction(string name, params string[] paramNames )
        {
            var customInstruction = new VzCustomInstruction(name, paramNames);
            context.currentProgram.instructionStarts.Add(customInstruction);
            return customInstruction;
        }

        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder());
        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action<VzExpression> instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder(args[0]));
        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action<VzExpression, VzExpression> instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder(args[0], args[1]));
        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action<VzExpression, VzExpression, VzExpression> instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder(args[0], args[1], args[2]));
        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action<VzExpression, VzExpression, VzExpression, VzExpression> instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder(args[0], args[1], args[2], args[3]));
        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder(args[0], args[1], args[2], args[3], args[4]));
        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder(args[0], args[1], args[2], args[3], args[4], args[5]));
        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder(args[0], args[1], args[2], args[3], args[4], args[5], args[6]));
        public static VzAction SetInstructions(this VzCustomInstruction customInstruction, Action<VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression, VzExpression> instuctionsBuilder)
            => SetInstructionsParams(customInstruction, (args) => instuctionsBuilder(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]));

        private static VzAction SetInstructionsParams(VzCustomInstruction customInstruction, VzAction instuctionsBuilder)
        {
            // ---- Sub Instructions Build ---
            context.instructionStack.Push(customInstruction);
            instuctionsBuilder.Invoke(customInstruction.localVarParameters);
            context.instructionStack.Pop();
            // -------------------------------
            return customInstruction.call;
        }
        

        #region Events

        // ********************** Events *************************

        /// <summary>
        /// Invoke When Create Event Context
        /// </summary>
        private static void OnEventContextCreate()
        {
            VzClass.CheckInitialization();
        }

        /// <summary>
        /// OnFlightStart
        /// </summary>
        public static VzOnStartEvent OnStartBegin()
        {
            VzOnStartEvent onStart = new VzOnStartEvent();
            context.currentProgram.instructionStarts.Add(onStart);
            context.instructionStack.Push(onStart);

            OnEventContextCreate();
            return onStart;
        }


        /// <summary>
        /// OnPartCollision
        /// </summary>
        public static VzOnPartCollisionEvent OnPartCollisionBegin()
        {
            VzOnPartCollisionEvent onCollision = new VzOnPartCollisionEvent();
            context.currentProgram.instructionStarts.Add(onCollision);
            context.instructionStack.Push(onCollision);

            OnEventContextCreate();
            return onCollision;
        }

        /// <summary>
        /// OnDocked
        /// </summary>
        public static VzOnDockedEvent OnDockedBegin()
        {
            VzOnDockedEvent onDocked = new VzOnDockedEvent();
            context.currentProgram.instructionStarts.Add(onDocked);
            context.instructionStack.Push(onDocked);

            OnEventContextCreate();
            return onDocked;
        }

        /// <summary>
        /// On Change Soi
        /// </summary>
        public static VzOnChangeSoiEvent OnChangeSoiBegin()
        {
            VzOnChangeSoiEvent onChangeSoi = new VzOnChangeSoiEvent();
            context.currentProgram.instructionStarts.Add(onChangeSoi);
            context.instructionStack.Push(onChangeSoi);

            OnEventContextCreate();
            return onChangeSoi;
        }


        /// <summary>
        /// OnPartExplode
        /// </summary>
        public static VzOnPartExplodeEvent OnPartExplodeBegin()
        {
            VzOnPartExplodeEvent onPartExplode = new VzOnPartExplodeEvent();
            context.currentProgram.instructionStarts.Add(onPartExplode);
            context.instructionStack.Push(onPartExplode);

            OnEventContextCreate();
            return onPartExplode;
        }

        /// <summary>
        /// OnPartExplode
        /// </summary>
        public static VzOnReceiveMessageEvent OnReceiveMessageBegin(string msg)
        {
            VzOnReceiveMessageEvent onReceiveMessage = new VzOnReceiveMessageEvent(msg);
            context.currentProgram.instructionStarts.Add(onReceiveMessage);
            context.instructionStack.Push(onReceiveMessage);

            OnEventContextCreate();
            return onReceiveMessage;
        }


        /// <summary>
        /// End Current (Event / CustomInstruction)
        /// </summary>
        public static void EndBlock()
        {
            if (context.instructionStack.Peek() is VzInstructionStart)
            {
                context.instructionStack.Pop();
            }
            else
            {
                throw new VizzyContextException();
            }
        }
        #endregion


        #region BaseInstructions

        // ********************** Base Instructions *************************


        public static VzInstruction WaitSeconds(VzExpression sec)
        {
            return new WaitInstruction(sec).AppendToCurrentContext();
        }

        public static VzInstruction WaitUntil(VzExpression condition)
        {
            return new WaitUntilInstruction(condition).AppendToCurrentContext();
        }


        public static WhileInstruction WhileBegin(VzExpression condition, params VzInstruction[] subinstructions)
        {
            var whileInstruction = new WhileInstruction(condition, subinstructions);
            context.instructionStack.Peek().Append(whileInstruction);
            context.instructionStack.Push(whileInstruction);
            return whileInstruction;
        }
        public static void WhileEnd()
        {
            if (context.instructionStack.Peek() is WhileInstruction)
            {
                context.instructionStack.Pop();
            }
            else
            {
                throw new VizzyContextException();
            }
        }


        public static RepeatInstruction RepeatBegin(VzExpression count, params VzInstruction[] subinstructions)
        {
            var repeatInstruction = new RepeatInstruction(count, subinstructions);
            context.instructionStack.Peek().Append(repeatInstruction);
            context.instructionStack.Push(repeatInstruction);
            return repeatInstruction;
        }
        public static void RepeatEnd()
        {
            if (context.instructionStack.Peek() is RepeatInstruction)
            {
                context.instructionStack.Pop();
            }
            else
            {
                throw new VizzyContextException();
            }
        }
        public static ForInstruction ForBegin(VzExpression from, VzExpression to, VzExpression by, params VzInstruction[] subinstructions)
        {
            var instruction = new ForInstruction(from, to, by, subinstructions);
            context.instructionStack.Peek().Append(instruction);
            context.instructionStack.Push(instruction);
            return instruction;
        }

        public static void ForEnd()
        {
            if (context.instructionStack.Peek() is ForInstruction)
            {
                context.instructionStack.Pop();
            }
            else
            {
                throw new VizzyContextException();
            }
        }

        public static IfInstruction IfBegin(VzExpression condition, params VzInstruction[] subinstructions)
        {
            var instruction = new IfInstruction(condition, subinstructions);
            context.instructionStack.Peek().Append(instruction);
            context.instructionStack.Push(instruction);
            return instruction;
        }
        public static void IfEnd()
        {
            if (context.instructionStack.Peek() is IfInstruction)
            {
                context.instructionStack.Pop();
            }
            else
            {
                throw new VizzyContextException();
            }
        }
        public static ElseIfInstruction ElseIfBegin(VzExpression condition, params VzInstruction[] subinstructions)
        {
            var instruction = new ElseIfInstruction(condition, subinstructions);
            context.instructionStack.Peek().Append(instruction);
            context.instructionStack.Push(instruction);
            return instruction;
        }
        public static void ElseIfEnd()
        {
            if (context.instructionStack.Peek() is ElseIfInstruction)
            {
                context.instructionStack.Pop();
            }
            else
            {
                throw new VizzyContextException();
            }
        }
        public static ElseInstruction ElseBegin(params VzInstruction[] subinstructions)
        {
            var instruction = new ElseInstruction(subinstructions);
            context.instructionStack.Peek().Append(instruction);
            context.instructionStack.Push(instruction);
            return instruction;
        }
        public static void ElseEnd()
        {
            if (context.instructionStack.Peek() is ElseInstruction)
            {
                context.instructionStack.Pop();
            }
            else
            {
                throw new VizzyContextException();
            }
        }


        public static VzInstruction Break()
        {
            return new BreakInstrucion().AppendToCurrentContext();
        }


        public static VzInstruction Display(VzExpression text)
        {
            return new DisplayMessageInstruction(text).AppendToCurrentContext();
        }

        
        public static VzInstruction Log(VzExpression text)
        {
            return new LogInstruction(text).AppendToCurrentContext();
        }

        public static VzInstruction Broadcast(BroadCastType type, VzExpression msg, VzExpression data)
        {
            return new BroadcastInstruction(type, msg, data).AppendToCurrentContext();
        }

        public static VzInstruction CMT(string text)
        {
            return new CommentInstruction(text).AppendToCurrentContext();
        }

        #endregion



        #region List
        public static VzInstruction InitList(VzExpression list, VzExpression values)
        {
            return new InitListInstruction(list, CreateList(values)).AppendToCurrentContext();
        }
        public static ListExpression CreateList(VzExpression values)
        {
            return ListExpression.CreateList(values);
        }
        #endregion




        #region Math
        public static UnaryExpression Abs(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.abs, value);
        }
        public static UnaryExpression Floor(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.floor, value);
        }
        public static UnaryExpression Ceiling(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.ceiling, value);
        }
        public static UnaryExpression Round(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.round, value);
        }
        public static UnaryExpression Sqrt(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.sqrt, value);
        }
        public static UnaryExpression Sin(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.sin, value);
        }
        public static UnaryExpression Cos(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.cos, value);
        }
        public static UnaryExpression Tan(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.tan, value);
        }
        public static UnaryExpression ASin(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.asin, value);
        }
        public static UnaryExpression ACos(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.acos, value);
        }
        public static UnaryExpression ATan(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.atan, value);
        }
        public static UnaryExpression In(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.ln, value);
        }
        public static UnaryExpression Lg10(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.log, value);
        }
        public static UnaryExpression Deg2Rad(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.deg2rad, value);
        }
        public static UnaryExpression Rad2Deg(VzExpression value)
        {
            return new UnaryExpression(UnaryOpType.rad2deg, value);
        }

        public static  BinaryOp Max(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Max);
        }
        public static BinaryOp Min(VzExpression a, VzExpression b)
        {
            return new BinaryOp(a, b, BinaryOpType.Min);
        }
        #endregion 

        #region MFD


        // *************************** Instructions ***********************

        public static VzInstruction CreateWidget(WidgetType type, VzExpression name)
        {
            var instruction = new CreateWidgetInstruction(type, name).AppendToCurrentContext();
            return instruction;
        }
        public static VzInstruction SetWidget(VzExpression name, WidgetPropertyType property, VzExpression value)
        {
            return new SetWidgetInstruction(name, property, value).AppendToCurrentContext();
        }
        public static VzInstruction SetWidgetAnchor(VzExpression name, WidgetAnchor anchor)
        {
            return new SetWidgetAnchorInstruction(name, anchor).AppendToCurrentContext();
        }
        public static VzInstruction SetLabel(VzExpression name, LabelProperyType property, VzExpression value)
        {
            return new SetLabelInstruction(name, property, value).AppendToCurrentContext();
        }
        public static VzInstruction SetLabelAlignment(VzExpression name, Alignment alignment)
        {
            return new SetLabelAlignmentInstruction(name, alignment).AppendToCurrentContext();
        }
        public static VzInstruction InitTexture(VzExpression name, VzExpression width, VzExpression height)
        {
            return new InitTextureInstruction(name, width, height).AppendToCurrentContext();
        }
        public static VzInstruction SetPixel(VzExpression name, VzExpression x, VzExpression y, VzExpression color)
        {
            return new SetPixelInstruction(name, x, y, color).AppendToCurrentContext();
        }
        public static VzInstruction SetSprite(VzExpression name, SpritePropertyType property, VzExpression value)
        {
            return new SetSpriteInstruction(name, property, value).AppendToCurrentContext();
        }
        public static VzInstruction SetGauge(VzExpression name, GaugePropertyType property, VzExpression value)
        {
            return new SetGaugeInstruction(name, property, value).AppendToCurrentContext();
        }
        public static VzInstruction SetLine(VzExpression name, LinePropertyType property, VzExpression value)
        {
            return new SetLineInstruction(name, property, value).AppendToCurrentContext();
        }
        public static VzInstruction SetLineStartEnd(VzExpression name, VzExpression start, VzExpression end)
        {
            return new SetLinePointsInstructions(name, start, end).AppendToCurrentContext();
        }
        public static VzInstruction SetNavBall(VzExpression name, NavBallPropertyType property, VzExpression value)
        {
            return new SetNavBallInstruction(name, property, value).AppendToCurrentContext();
        }
        public static VzInstruction SetMap(VzExpression name, MapPropertyType property, VzExpression value)
        {
            return new SetMapInstruction(name, property, value).AppendToCurrentContext();
        }
        public static VzInstruction WidgetSendFront(VzExpression name, VzExpression target)
        {
            return new SendWidgeToFront(name, target).AppendToCurrentContext();
        }
        public static VzInstruction WidgetSendBack(VzExpression name, VzExpression target)
        {
            return new SendWidgeToBack(name, target).AppendToCurrentContext();
        }
        public static VzInstruction WidgetEvent(VzExpression widgetName, WidgetEventType eventType, VzExpression message, VzExpression data)
        {
            return new SetWidgetEventInstruction(widgetName, eventType, message, data).AppendToCurrentContext();
        }
        public static VzInstruction DestroyWidget(VzExpression widgetName)
        {
            return new DestroyWidgetInstruction(widgetName).AppendToCurrentContext();
        }
        public static VzInstruction DestroyAllWidget()
        {
            return new DestroyAllWidgetInstruction().AppendToCurrentContext();
        }


        // *************************** Expressions ***********************

        public static GetWidgePropertyExpression GetWidgetProperty(VzExpression name, WidgetPropertyGetType property)
        {
            return new GetWidgePropertyExpression(name, property);
        }
        public static GetLabelPropertyExpression GetLabelProperty(VzExpression name, LabelProperyType property)
        {
            return new GetLabelPropertyExpression(name, property);
        }
        public static GetSpritePropertyExpression GetSpriteProperty(VzExpression name, SpritePropertyType property)
        {
            return new GetSpritePropertyExpression(name, property);
        }
        public static GetGaugePropertyExpression GetGaugeProperty(VzExpression name, GaugePropertyType property)
        {
            return new GetGaugePropertyExpression(name, property);
        }
        public static GetPixelExpression GetPixel(VzExpression name, VzExpression x, VzExpression y)
        {
            return new GetPixelExpression(name, x, y);
        }
        public static WidgetLocalToDisplayExpression WidgetLocalToDisplay(VzExpression name, VzExpression pos)
        {
            return new WidgetLocalToDisplayExpression(name, pos);
        }
        public static WidgetDisplayToLocalExpression WidgetDisplayToLocal(VzExpression name, VzExpression pos)
        {
            return new WidgetDisplayToLocalExpression(name, pos);
        }
        public static GetWidgetEventMsgExpression GetWidgetEventMsg(VzExpression name, WidgetEventType eventType)
        {
            return new GetWidgetEventMsgExpression(name, eventType);
        }
        public static HexColorExpression HexColor(VzExpression text)
        {
            return new HexColorExpression(text);
        }


        #endregion


        #region 其他运算符

        public static FUNKExpression FUNK(FUNK funk)
        {
            return new FUNKExpression(funk);
        }

        public static ConditionalExpression Conditional(VzExpression condition, VzExpression e1, VzExpression e2)
        {
            return new ConditionalExpression(condition, e1, e2);
        }

        public static VzExpression StringLetter(VzExpression idx, VzExpression str)
        {
            return new StringOp(StringOpType.Letter, idx, str);
        }
        public static VzExpression StringLength(VzExpression str)
        {
            return new StringOp(StringOpType.Length, str);
        }
        public static VzExpression StringJoin(params VzExpression[] strings)
        {
            return new StringOp(StringOpType.Join, strings);
        }
        public static VzExpression SubString(VzExpression start, VzExpression end, VzExpression str)
        {
            return new StringOp(StringOpType.SubString, start, end, str);
        }
        public static  VzExpression StringContains(VzExpression stringA, VzExpression stringB)
        {
            return new StringOp(StringOpType.Contains, stringA, stringB);
        }
        public static VzExpression StringFormat(params VzExpression[] strings)
        {
            return new StringOp(StringOpType.Format, strings);
        }
        public static VzExpression StringFriend(VzExpression expr, PhysicalQuantity quantity)
        {
            return new StringFriendOp(expr, quantity);
        }

        #endregion


        #region Craft Info
        public static VzInstruction ActivateStage()
        {
            return new ActivateStageInstruction().AppendToCurrentContext();
        }
        public static VzInstruction SetInput(CraftInput input, VzExpression value)
        {
            return new SetInputInstruction(input, value).AppendToCurrentContext();
        }
        public static VzInstruction SetTargetHeading(TargetHeadingProperty property, VzExpression value)
        {
            return new SetTargetHeadingInstruction(property, value).AppendToCurrentContext();
        }
        public static VzInstruction SetTarget(VzExpression value)
        {
            return new SetTargetInstruction(value).AppendToCurrentContext();
        }
        public static VzInstruction SetAG(VzExpression ag, VzExpression newvalue)
        {
            return new SetActivateGroupInstruction(ag, newvalue).AppendToCurrentContext();
        }
        public static VzInstruction LockNavSphere(LockNavSphereIndicatorType type, VzExpression value)
        {
            return new LockNavSphereInstruction(type, value).AppendToCurrentContext();
        }
        public static VzInstruction SetTimeMode (TimeMode mode)
        {
            return new SetTimeModeInstruction(mode).AppendToCurrentContext();
        }
        public static VzInstruction SetCameraProperty(CameraProperty property, VzExpression newvalue)
        {
            return new SetCameraPropertyInstruction(property, newvalue).AppendToCurrentContext();
        }
        public static VzInstruction SetPartProperty(VzExpression partId, PartPropertySetType setProperty, VzExpression newvalue)
        {
            return new SetPartPropertyInstruction(partId, setProperty, newvalue).AppendToCurrentContext();
        }
        public static VzInstruction SwitchCraft(VzExpression craftId)
        {
            return new SwitchCraftInstruction(craftId).AppendToCurrentContext();
        }
        public static VzInstruction Beep(VzExpression frequency, VzExpression volume, VzExpression time)
        {
            return new BeepInstruction(frequency, volume, time).AppendToCurrentContext();
        }


        // ************************** Expressions *******************
        public static VzExpression GetCraftInput(CraftInput input)
        {
            return new CraftInputExpression(input);
        }
        public static VzExpression GetCraftBaseProperty(CraftBasePropertyType property)
        {
            return new CraftBaseProperty(property);
        }
        public static VzExpression PosToLatLongAgl(VzExpression pos)
        {
            return new PositionToLatLongAgl(pos);
        }
        public static VzExpression LatLongAglToPos(VzExpression location)
        {
            return new LatLongAglToPosition(location);
        }
        public static VzExpression TerrainQuery(TerrainPropertyType property, VzExpression location)
        {
            return new TerrainPropertyExpression(property, location);
        }
        public static VzExpression PlanetProperty(VzExpression planet, PlanetPropertyType property)
        {
            return new PlanetPropertyExpression(planet, property);
        }
        public static VzExpression PartProperty(VzExpression partID, PartPropertyGetType propertyGet)
        {
            return new PartProperty(partID, propertyGet);
        }
        public static VzExpression PartNameToID(VzExpression partName)
        {
            return new PartNameToPartIDExpression(partName);
        }
        public static VzExpression GetAG(VzExpression ag)
        {
            return new ActivationGroupExpression(ag);
        }
        public static VzExpression PartLocalToPci(VzExpression partId, VzExpression coords)
        {
            return new PartLocalToPciExpression(partId, coords);
        }
        public static VzExpression PartPciToLocal(VzExpression partId, VzExpression coords)
        {
            return new PartPciToLocalExpression(partId, coords);
        }
        public static VzExpression CraftOtherProperty(VzExpression craftId, CraftOtherPropertyType property)
        {
            return new CraftOtherProperty(craftId, property);
        }


        #endregion

        // SET (Short)
        public static VzInstruction Set(CraftInput input, VzExpression value)
        {
            return Vz.SetInput(input, value);
        }
        public static VzInstruction Set(TargetHeadingProperty property, VzExpression value)
        {
            return Vz.SetTargetHeading(property, value);
        }
        public static VzInstruction Set(PartPropertySetType partPropertySet, VzExpression partId, VzExpression value)
        {
            return Vz.SetPartProperty(partId, partPropertySet, value);
        }
        public static VzInstruction Set(CameraProperty property, VzExpression value)
        {
            return Vz.SetCameraProperty(property, value);
        }

        // GET (Short)
        public static VzExpression Get(CraftInput input)
        {
            return GetCraftInput(input);
        }
        public static VzExpression Get(CraftBasePropertyType property)
        {
            return GetCraftBaseProperty(property);
        }
        public static VzExpression Get(TerrainPropertyType propertyType, VzExpression location)
        {
            return TerrainQuery(propertyType, location);
        }
        public static VzExpression Get(PlanetPropertyType propertyType, VzExpression planet)
        {
            return PlanetProperty(planet, propertyType);
        }
        public static VzExpression Get(PartPropertyGetType propertyGet, VzExpression partId)
        {
            return PartProperty(partId, propertyGet);
        }
        public static VzExpression Get(CraftOtherPropertyType propertyType, VzExpression craftId)
        {
            return CraftOtherProperty(craftId, propertyType);
        }
    }














    // ******************** Proxy Classes *******************  
    public abstract class VizzyProxy
    {
    }

    public class VzString : VizzyProxy
    {
        public static VzExpression Letter(VzExpression idx, VzExpression str)
        {
            return Vz.StringLetter(idx, str);
        }
        public static VzExpression LengthOf(VzExpression str)
        {
            return Vz.StringLength(str);
        }
        public static VzExpression Join(params VzExpression[] strings)
        {
            return Vz.StringJoin(strings);
        }
        public static VzExpression SubString(VzExpression start, VzExpression end, VzExpression str)
        {
            return Vz.SubString(start, end, str);
        }
        public static VzExpression Contains(VzExpression stringA, VzExpression stringB)
        {
            return Vz.StringContains(stringA, stringB);
        }
        public static VzExpression Format(params VzExpression[] strings)
        {
            return Vz.StringFormat(strings);
        }
        public static VzExpression Friend(VzExpression expr, PhysicalQuantity quantity)
        {
            return Vz.StringFriend(expr, quantity);
        }
    }




    public abstract class VizzyEventProxy : VizzyProxy, IDisposable
    {
        public void Dispose()
        {
            Vz.EndBlock();
        }
    }

    public class OnStart : VizzyEventProxy
    {
        public VzOnStartEvent onstartEvt;
        public OnStart()
        {
            this.onstartEvt = Vz.OnStartBegin();
        }
    }

    public class OnPartCollision : VizzyEventProxy
    {
        public VzOnPartCollisionEvent evt;

        public OnPartCollision()
        {
            this.evt = Vz.OnPartCollisionBegin();
        }
    }

    public class OnDocked : VizzyEventProxy
    {
        public VzOnDockedEvent evt;

        public OnDocked()
        {
            this.evt = Vz.OnDockedBegin();
        }
    }
    public class OnChangeSoi : VizzyEventProxy
    {
        public VzOnChangeSoiEvent evt;
        public OnChangeSoi()
        {
            this.evt = Vz.OnChangeSoiBegin();
        }
    }
    public class OnPartExplode: VizzyEventProxy
    {
        public VzOnPartExplodeEvent evt;
        public OnPartExplode()
        {
            this.evt = Vz.OnPartExplodeBegin();
        }
    }
    public class OnReceiveMessage : VizzyEventProxy
    {
        private VzOnReceiveMessageEvent realEvt;

        public LocalVariable Data => this.realEvt.data;

        public VzExpression Msg => this.realEvt.msg;

        public OnReceiveMessage(string msg)
        {
            this.realEvt = Vz.OnReceiveMessageBegin(msg);
        }
    }












    public class While : VizzyProxy, IDisposable
    {
        //public WhileInstruction whileInstruction;

        public While(VzExpression condition)
        {
            Vz.WhileBegin(condition);
        }

        public void Dispose()
        {
            Vz.WhileEnd();
        }
    }

    public class Repeat: VizzyProxy, IDisposable
    {
        //public RepeatInstruction repeatInstruction;

        public Repeat(VzExpression count)
        {
            Vz.RepeatBegin(count);
        }
        public void Dispose()
        {
            Vz.RepeatEnd();
        }
    }

    public class For : VizzyProxy, IDisposable
    {
        private ForInstruction forInstruction;

        public LocalVariable i => forInstruction.i;

        public For(VzExpression from, VzExpression to, VzExpression by)
        {
            this.forInstruction = Vz.ForBegin(from, to, by);
        }
        public void Dispose()
        {
            Vz.ForEnd();
        }
    }

    public class If : VizzyProxy, IDisposable
    { 
        public If(VzExpression condition)
        {
            Vz.IfBegin(condition);
        }
        public void Dispose()
        {
            Vz.IfEnd();
        }
    }
    public class ElseIf : VizzyProxy, IDisposable
    {
        public ElseIf(VzExpression condition)
        {
            Vz.ElseIfBegin(condition);
        }
        public void Dispose()
        {
            Vz.ElseIfEnd();
        }
    }
    public class Else : VizzyProxy, IDisposable
    {
        public Else()
        {
            Vz.ElseBegin();
        }
        public void Dispose()
        {
            Vz.ElseEnd();
        }
    }






    



    public class VzCraft : VizzyProxy
    {
        public VzExpression craftID;

        public VzCraft(VzExpression craftid)
        {
            this.craftID = craftid;
        }


        //Specific Instance Property  
        public VzExpression GetProperty(CraftOtherPropertyType property)
        {
            return Vz.CraftOtherProperty(this.craftID, property);
        }
        public VzExpression Altitude
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Altitude);}
        }
        public VzExpression Destroyed
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Destroyed);}
        }
        public VzExpression Grounded
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Grounded);}
        }
        public VzExpression Mass
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Mass);}
        }
        public VzExpression CraftName
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.IDToName);}
        }
        public VzExpression PartCount
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.PartCount);}
        }
        public VzExpression Planet
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Planet);}
        }
        public VzExpression Position
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Position);}
        }
        public VzExpression Velocity
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Velocity);}
        }
        public VzExpression IsPlayer
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.IsPlayer);}
        }
        public VzExpression Apoapsis
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Apoapsis);}
        }
        public VzExpression Periapsis
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Periapsis);}
        }
        public VzExpression Period
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Period);}
        }
        public VzExpression ApoapsisTime
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.ApoapsisTime);}
        }
        public VzExpression PeriapsisTime
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.PeriapsisTime);}
        }
        public VzExpression Inclination
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Inclination);}
        }
        public VzExpression Eccentricity
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.Eccentricity);}
        }
        public VzExpression MeanAnomaly
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.MeanAnomaly);}
        }
        public VzExpression MeanMotion
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.MeanMotion);}
        }
        public VzExpression PeriapsisArgument
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.PeriapsisArgument);}
        }
        public VzExpression RightAscension
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.RightAscension);}
        }
        public VzExpression TrueAnomaly
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.TrueAnomaly);}
        }
        public VzExpression SemiMajorAxis
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.SemiMajorAxis);}
        }
        public VzExpression SemiMinorAxis
        {
            get { return Vz.CraftOtherProperty(this.craftID, CraftOtherPropertyType.SemiMinorAxis);}
        }





        // Current Craft Properties

        public static void ActivateStage()
        {
            Vz.ActivateStage();
        }

        public static VzExpression Roll
        {
            get { return Vz.GetCraftInput(CraftInput.Roll); }
            set { Vz.SetInput(CraftInput.Roll, value); }
        }
        public static VzExpression Pitch
        {
            get { return Vz.GetCraftInput(CraftInput.Pitch); }
            set { Vz.SetInput(CraftInput.Pitch, value); }
        }
        public static VzExpression Yaw
        {
            get { return Vz.GetCraftInput(CraftInput.Yaw); }
            set { Vz.SetInput(CraftInput.Yaw, value); }
        }
        public static VzExpression Brake
        {
            get { return Vz.GetCraftInput(CraftInput.Brake); }
            set { Vz.SetInput(CraftInput.Brake, value); }
        }
        public static VzExpression Throttle
        {
            get { return Vz.GetCraftInput(CraftInput.Throttle); }
            set { Vz.SetInput(CraftInput.Throttle, value); }
        }
        public static VzExpression TranslateForward
        {
            get { return Vz.GetCraftInput(CraftInput.TranslateForward); }
            set { Vz.SetInput(CraftInput.TranslateForward, value); }
        }
        public static VzExpression TranslateMode
        {
            get { return Vz.GetCraftInput(CraftInput.TranslateMode); }
            set { Vz.SetInput(CraftInput.TranslateMode, value); }
        }
        public static VzExpression Slider1
        {
            get { return Vz.GetCraftInput(CraftInput.Slider1); }
            set { Vz.SetInput(CraftInput.Slider1, value); }
        }
        public static VzExpression Slider2
        {
            get { return Vz.GetCraftInput(CraftInput.Slider2); }
            set { Vz.SetInput(CraftInput.Slider2, value); }
        }
        public static VzExpression Slider3
        {
            get { return Vz.GetCraftInput(CraftInput.Slider3); }
            set { Vz.SetInput(CraftInput.Slider3, value); }
        }
        public static VzExpression Slider4
        {
            get { return Vz.GetCraftInput(CraftInput.Slider4); }
            set { Vz.SetInput(CraftInput.Slider4, value); }
        }




        public static VzExpression TargetHeading_Heading
        {
            set { Vz.SetTargetHeading(TargetHeadingProperty.Heading, value); }
        }
        public static VzExpression TargetHeading_Pitch
        {
            set { Vz.SetTargetHeading(TargetHeadingProperty.Pitch, value); }
        }
        public static VzExpression TargetHeading_Pid_Pitch
        {
            set { Vz.SetTargetHeading(TargetHeadingProperty.Pid_Pitch, value); }
        }
        public static VzExpression TargetHeading_Pid_Roll
        {
            set { Vz.SetTargetHeading(TargetHeadingProperty.Pid_Roll, value); }
        }



        public static VzExpression Target
        {
            set { Vz.SetTarget(value); }
        }


        public static VzExpression AG1
        {
            get { return Vz.GetAG(new Constant(1)); }
            set { Vz.SetAG(new Constant(1), value); }
        }
        public static VzExpression AG2
        {
            get { return Vz.GetAG(new Constant(2)); }
            set { Vz.SetAG(new Constant(2), value); }
        }
        public static VzExpression AG3
        {
            get { return Vz.GetAG(new Constant(3)); }
            set { Vz.SetAG(new Constant(3), value); }
        }
        public static VzExpression AG4
        {
            get { return Vz.GetAG(new Constant(4)); }
            set { Vz.SetAG(new Constant(4), value); }
        }
        public static VzExpression AG5
        {
            get { return Vz.GetAG(new Constant(5)); }
            set { Vz.SetAG(new Constant(5), value); }
        }
        public static VzExpression AG6
        {
            get { return Vz.GetAG(new Constant(6)); }
            set { Vz.SetAG(new Constant(6), value); }
        }
        public static VzExpression AG7
        {
            set { Vz.SetAG(new Constant(7), value); }
        }
        public static VzExpression AG8
        {
            get { return Vz.GetAG(new Constant(8)); }
            set { Vz.SetAG(new Constant(8), value); }
        }
        public static VzExpression AG9
        {
            get { return Vz.GetAG(new Constant(9)); }
            set { Vz.SetAG(new Constant(9), value); }
        }
        public static VzExpression AG10
        {
            get { return Vz.GetAG(new Constant(10)); }
            set { Vz.SetAG(new Constant(10), value); }
        }


        //LockNavSphere
        public static VzExpression LockNavSphereVector
        {
            set { Vz.LockNavSphere(LockNavSphereIndicatorType.Vector, value); }
        }
        public static LockNavSphereIndicatorType LockNavSphere
        {
            set 
            {
                if (value == LockNavSphereIndicatorType.Vector)
                    throw new VizzyException("Use LockNavSphereVector Instead!");
                Vz.LockNavSphere(value, null);    
            }
        }

        //Time
        public static TimeMode TimeMode
        {
            set { Vz.SetTimeMode(value); }
        }



        //Camera
        public static VzExpression CameraMode
        {
            set { Vz.SetCameraProperty(CameraProperty.mode, value); }
        }
        public static VzExpression CameraModeIndex
        {
            set { Vz.SetCameraProperty(CameraProperty.modeIndex, value); }
        }
        public static VzExpression CameraRotateX
        {
            set { Vz.SetCameraProperty(CameraProperty.rotateX, value); }
        }
        public static VzExpression CameraRotateY
        {
            set { Vz.SetCameraProperty(CameraProperty.rotateY, value); }
        }
        public static VzExpression CameraTilt
        {
            set { Vz.SetCameraProperty(CameraProperty.tilt, value); }
        }
        public static VzExpression CameraZoom
        {
            set { Vz.SetCameraProperty(CameraProperty.zoom, value); }
        }

        //Part...
        public static VzPart FindPartByName(VzExpression partName)
        {
            VzExpression partId = Vz.PartNameToID(partName);
            return new VzPart(partId);
        }
        public static void SetPartProperty(VzExpression partID)
        {

        }



        public static VzExpression Altitude_AGL
        {
            get { return Vz.GetCraftBaseProperty(CraftBasePropertyType.Altitude_AGL); }
        }
        public static VzExpression GetProperty(CraftBasePropertyType property)
        {
            return Vz.GetCraftBaseProperty(property);
        }
    }

    public class VzTerrain : VizzyProxy
    {
        public static VzExpression GetHeight(VzExpression location)
        {
            return Vz.TerrainQuery(TerrainPropertyType.Height, location);
        }
        public static VzExpression GetColor(VzExpression location)
        {
            return Vz.TerrainQuery(TerrainPropertyType.Color, location);
        }
    }
    public class VzPart : VizzyProxy
    {
        private VzExpression partID;

        public VzPart (VzExpression partID)
        {
            this.partID = partID;
        }

        public VzExpression ThisID
        {
            get { return Vz.PartProperty(this.partID, PartPropertyGetType.ThisID); }
        }
        public VzExpression MaxID
        {
            get { return Vz.PartProperty(this.partID, PartPropertyGetType.MaxID); }
        }
        public VzExpression MinID
        {
            get { return Vz.PartProperty(this.partID, PartPropertyGetType.MinID); }
        }


        public VzExpression PartName
        {
            get { return Vz.PartProperty(this.partID, PartPropertyGetType.IDToName); }
            set { Vz.SetPartProperty(this.partID, PartPropertySetType.Name, value); }
        }

        public VzExpression Mass
        {
            get{ return Vz.PartProperty(this.partID, PartPropertyGetType.Mass);}
        }
        public VzExpression Activated
        {
            get { return Vz.PartProperty(this.partID, PartPropertyGetType.Activated);}
            set { Vz.SetPartProperty(this.partID, PartPropertySetType.Activated, value);}
        }
        public VzExpression PartType
        {
            get{ return Vz.PartProperty(this.partID, PartPropertyGetType.PartType);}
        }
        public VzExpression Position
        {
            get{ return Vz.PartProperty(this.partID, PartPropertyGetType.Position);}
        }
        public VzExpression Temperature
        {
            get{ return Vz.PartProperty(this.partID, PartPropertyGetType.Temperature);}
        }
        public VzExpression Drag
        {
            get{ return Vz.PartProperty(this.partID, PartPropertyGetType.Drag);}
        }
        public VzExpression UnderWater
        {
            get{ return Vz.PartProperty(this.partID, PartPropertyGetType.UnderWater); }
        }


        public VzExpression Focused
        {
            set { Vz.SetPartProperty(this.partID, PartPropertySetType.Focused, value);}
        }
        public VzExpression Explode
        {
            set { Vz.SetPartProperty(this.partID, PartPropertySetType.Explode, value);  }
        }

        public VzExpression LocalToPCI(VzExpression coords)
        {
             return Vz.PartLocalToPci(this.partID, coords);
        }
        public VzExpression PCIToLocal(VzExpression coords)
        {
            return Vz.PartPciToLocal(this.partID, coords);
        }
    }





    public abstract class VzWidget : VizzyProxy
    {
        protected VzExpression name = null;

        public VzExpression Name =>  this.name;

        public VzWidget(VzExpression name)
        {
            this.name = name;
        }


        public VzInstruction EventMessage(WidgetEventType eventType, VzExpression msg, VzExpression data)
        {
            return Vz.WidgetEvent(this.name, eventType, msg, data);
        }

        public void Subscribe(WidgetEventType eventType, string msg, VzExpression data, System.Action<VzExpression> callback)
        {
            Vz.WidgetEvent(this.name, eventType, msg, data);
            using (var onReceiveProxy = new OnReceiveMessage(msg))
            {
                callback(onReceiveProxy.Data);
            }
        }


        public VzExpression AnchoredPosition
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.AnchoredPosition); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.AnchoredPosition, value); }
        }
        public VzExpression AnchorMin
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.AnchorMin); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.AnchorMin, value); }
        }
        public VzExpression AnchorMax
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.AnchorMax); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.AnchorMax, value); }
        }
        public VzExpression Color
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Color); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Color, value); }
        }
        public VzExpression Opacity
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Opacity); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Opacity, value); }
        }
        public VzExpression Parent
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Parent); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Parent, value); }
        }
        public VzExpression Pivot
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Pivot); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Pivot, value); }
        }
        public VzExpression Position
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Position); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Position, value); }
        }
        public VzExpression Rotation
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Rotation); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Rotation, value); }
        }
        public VzExpression Scale
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Scale); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Scale, value); }
        }
        public VzExpression Size
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Size); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Size, value); }
        }
        public VzExpression Visible
        {
            get { return Vz.GetWidgetProperty(this.name, WidgetPropertyGetType.Visible); }
            set { Vz.SetWidget(this.name, WidgetPropertyType.Visible, value); }
        }
    }

    public class VzLabel : VzWidget
    {
        public VzLabel(VzExpression labelname) : base(labelname)
        {
            Vz.CreateWidget(WidgetType.Label, labelname);
        }


        public VzExpression Text
        {
            get { return Vz.GetLabelProperty(this.name, LabelProperyType.Text); }
            set { Vz.SetLabel(this.name, LabelProperyType.Text, value); }
        }
        public VzExpression FontSize
        {
            get { return Vz.GetLabelProperty(this.name, LabelProperyType.FontSize); }
            set { Vz.SetLabel(this.name, LabelProperyType.FontSize, value); }
        }
        public VzExpression AutoSize
        {
            get { return Vz.GetLabelProperty(this.name, LabelProperyType.AutoSize); }
            set { Vz.SetLabel(this.name, LabelProperyType.AutoSize, value); }
        }
        public Alignment Alignment
        {
            set { Vz.SetLabelAlignment(this.name, value); }
        }
    }

    public abstract class VzSprite : VzWidget
    {
        public VzSprite(VzExpression name) : base(name) { }



        public VzExpression FillMethod
        {
            get { return Vz.GetSpriteProperty(this.name, SpritePropertyType.FillMethod); }
            set { Vz.SetSprite(this.name, SpritePropertyType.FillMethod, value); }
        }

        public VzExpression Icon
        {
            get { return Vz.GetSpriteProperty(this.name, SpritePropertyType.Icon); }
            set { Vz.SetSprite(this.name, SpritePropertyType.Icon, value); }
        }

        public VzExpression FillAmount
        {
            get { return Vz.GetSpriteProperty(this.name, SpritePropertyType.FillAmount); }
            set { Vz.SetSprite(this.name, SpritePropertyType.FillAmount, value); }
        }
    }

    public class VzEllipse : VzSprite
    {
        public VzEllipse(VzExpression name) : base(name)
        {
            Vz.CreateWidget(WidgetType.Ellipse, name);
        }
    }



    public class VzRectangle : VzSprite
    {
        public VzRectangle(VzExpression name) : base(name)
        {
            Vz.CreateWidget(WidgetType.Rectangle, name);
        }
    }



    public class VzLine : VzSprite
    {
        public VzLine(VzExpression name):base(name)
        {
            Vz.CreateWidget(WidgetType.Line, this.name);
        }

        public VzExpression Thickness
        {
            set { Vz.SetLine(this.name, LinePropertyType.Thickness, value); }
        }
        public VzExpression Length
        {
            set { Vz.SetLine(this.name, LinePropertyType.Length, value); }
        }
    }

    public class VzRadialGauge : VzSprite
    {
        public VzRadialGauge(VzExpression name) : base(name)
        {
            Vz.CreateWidget(WidgetType.RadialGauge, name);
        }

        public VzExpression BackgroundColor
        {
            get { return Vz.GetGaugeProperty(this.name, GaugePropertyType.BackgroundColor); }
            set { Vz.SetGauge(this.name, GaugePropertyType.BackgroundColor, value); }
        }
        public VzExpression FillColor
        {
            get { return Vz.GetGaugeProperty(this.name, GaugePropertyType.FillColor); }
            set { Vz.SetGauge(this.name, GaugePropertyType.FillColor, value); }
        }
        public VzExpression Text
        {
            get { return Vz.GetGaugeProperty(this.name, GaugePropertyType.Text); }
            set { Vz.SetGauge(this.name, GaugePropertyType.Text, value); }
        }
        public VzExpression TextColor
        {
            get { return Vz.GetGaugeProperty(this.name, GaugePropertyType.TextColor); }
            set { Vz.SetGauge(this.name, GaugePropertyType.TextColor, value); }
        }
        public VzExpression Value
        {
            get { return Vz.GetGaugeProperty(this.name, GaugePropertyType.Value); }
            set { Vz.SetGauge(this.name, GaugePropertyType.Value, value); }
        }
    }

    public class VzTexture : VzSprite
    {
        public VzTexture(VzExpression name) : base(name)
        {
            Vz.CreateWidget(WidgetType.Texture, name);
        }

        public void InitTexture(VzExpression width, VzExpression height)
        {
            Vz.InitTexture(this.name, width, height);
        }

        public void SetPixel(VzExpression x, VzExpression y, VzExpression color)
        {
            Vz.SetPixel(this.name, x, y, color);
        }
        public VzExpression GetPixel(VzExpression x, VzExpression y)
        {
            return Vz.GetPixel(this.name, x, y);
        }
    }

    public class VzNavBall : VzSprite
    {
        public VzNavBall(VzExpression name) : base(name)
        {
            Vz.CreateWidget(WidgetType.Navball, name);
        }

        public VzExpression TopColor
        {
            set { Vz.SetNavBall(this.name, NavBallPropertyType.TopColor, value); }
        }
        public VzExpression BottomColor
        {
            set { Vz.SetNavBall(this.name, NavBallPropertyType.BottomColor, value); }
        }
    }

    public class VzMap: VzSprite
    {
        public VzMap(VzExpression name) : base(name)
        {
            Vz.CreateWidget(WidgetType.Map, name);
        }

        public VzExpression NorthUp
        {
            set{ Vz.SetMap(this.name, MapPropertyType.NorthUp, value); }
        }
        public VzExpression Zoom
        {
            set { Vz.SetMap(this.name, MapPropertyType.Zoom, value); }
        }
        public VzExpression ManualMode
        {
            set { Vz.SetMap(this.name, MapPropertyType.ManualMode, value); }
        }
        public VzExpression PlanetName
        {
            set { Vz.SetMap(this.name, MapPropertyType.PlanetName, value); }
        }
        public VzExpression Coordinates
        {
            set { Vz.SetMap(this.name, MapPropertyType.Coordinates, value); }
        }
        public VzExpression Heading
        {
            set { Vz.SetMap(this.name, MapPropertyType.Heading, value); }
        }

    }


    // ********************** Error ************************

    public class VizzyException : Exception
    {
        public VizzyException()
        {
        }
        public VizzyException(string txt) : base(txt)
        {
        }
    }
    public class VizzyContextException : VizzyException
    {
        public VizzyContextException()
        {
        }
        public VizzyContextException(string txt) : base(txt)
        {
        }
        public override string ToString()
        {
            return base.ToString() + " -- ContextError";
        }
    }


    // ********************** Extensions ************************

    public static  class VizzyExtensions
    {
        public static VzInstruction AppendToCurrentContext(this VzInstruction instr)
        {
            Vz.context.instructionStack.Peek().Append(instr);
            return instr;
        }
    }

}
