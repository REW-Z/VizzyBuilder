using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace REWVIZZY
{
    // *************************** 一元运算符表达式 ***********************************

    public enum UnaryOpType
    {
        abs,
        floor,
        ceiling,
        round,
        sqrt,
        sin,
        cos,
        tan,
        asin,
        acos,
        atan,
        ln,
        log,
        deg2rad,
        rad2deg,
    }

    public class UnaryExpression : VzExpression
    {
        public VzExpression expression;
        public UnaryOpType op;

        public UnaryExpression(UnaryOpType op, VzExpression expression)
        {
            this.expression = expression;
            this.op = op;
        }
        public override XElement Serialize()
        {
            XElement xAbs = new XElement("MathFunction",
                new XAttribute("function", op.ToString()),
                new XAttribute("style", "op-math")
                );

            xAbs.Add(expression.Serialize());

            return xAbs;
        }
    }

    public class NotExpression : VzExpression
    {
        public VzExpression expressionParam;

        public NotExpression(VzExpression expression)
        {
            this.expressionParam = expression;
        }

        public override XElement Serialize()
        {
            XElement xNot = new XElement("Not",
                new XAttribute( "style", "op-not")
                );

            xNot.Add(expressionParam.Serialize());

            return xNot;
        }
    }


    public class FUNKExpression : VzExpression
    {
        public FUNK fUNK;

        public FUNKExpression(FUNK funk)
        {
            this.fUNK = funk;
        }

        public override XElement Serialize()
        {
            XElement xFUNK = new XElement("EvaluateExpression",
                new XAttribute("style", "evaluate-expression")
                ) ;

            xFUNK.Add(new Constant(fUNK.Value).Serialize());

            return xFUNK;
        }
    }






    // **************************** 二元运算符表达式 ************************************



    public enum BinaryOpType
    {
        Plus,
        Sub,
        Mul,
        Div,
        Exp,
        Mod,
        Rand,
        Min,
        Max,

        And,
        Or,

        Equal,
        LessThan,
        LessEqual,
        GreaterThan,
        GreaterEqual,

        ATan2,
    }


    public class BinaryOp : VzExpression
    {
        VzExpression paramA;
        VzExpression paramB;

        public BinaryOpType operaterType;

        public BinaryOp(VzExpression a, VzExpression b, BinaryOpType op)
        {
            this.paramA = a;
            this.paramB = b;
            this.operaterType = op;
        }
        public BinaryOp(VzExpression a, VzExpression b, string operatorchar)
        {
            this.paramA = a;
            this.paramB = b;
            this.operaterType = GetOperatorTypeFromChar(operatorchar);
        }


        public static string GetXElementName(BinaryOpType type)
        {
            switch (type)
            {
                case BinaryOpType.Or:
                case BinaryOpType.And:
                    return "BoolOp";
                case BinaryOpType.Equal:
                case BinaryOpType.LessThan:
                case BinaryOpType.LessEqual:
                case BinaryOpType.GreaterThan:
                case BinaryOpType.GreaterEqual:
                    return "Comparison";
                default:
                    return "BinaryOp";
            }
        }

        public static string GetStyle(BinaryOpType type)
        {
            switch (type)
            {
                case BinaryOpType.Plus: return "op-add";
                case BinaryOpType.Sub: return "op-sub";
                case BinaryOpType.Mul: return "op-mul";
                case BinaryOpType.Div: return "op-div";
                case BinaryOpType.Exp: return "op-exp";
                case BinaryOpType.Mod: return "op-mod";
                case BinaryOpType.Rand: return "op-rand";
                case BinaryOpType.Min: return "op-min";
                case BinaryOpType.Max: return "op-max";

                case BinaryOpType.Or: return "op-or";
                case BinaryOpType.And: return "op-and";

                case BinaryOpType.Equal: return "op-eq";
                case BinaryOpType.LessThan: return "op-lt";
                case BinaryOpType.LessEqual: return "op-lte";
                case BinaryOpType.GreaterThan: return "op-gt";
                case BinaryOpType.GreaterEqual: return "op-gte";

                case BinaryOpType.ATan2: return "op-atan-2";
            }
            return "op-add";
        }
        public static string GetOperatorSerializeChar(BinaryOpType type)
        {
            switch (type)
            {
                case BinaryOpType.Plus: return "+";
                case BinaryOpType.Sub: return "-";
                case BinaryOpType.Mul: return "*";
                case BinaryOpType.Div: return "/";
                case BinaryOpType.Exp: return "^";
                case BinaryOpType.Mod: return "%";
                case BinaryOpType.Rand: return "rand";
                case BinaryOpType.Min: return "min";
                case BinaryOpType.Max: return "max";

                case BinaryOpType.Or: return "or";
                case BinaryOpType.And:return "and";

                case BinaryOpType.Equal: return "=";
                case BinaryOpType.LessThan: return "l";
                case BinaryOpType.LessEqual: return "le";
                case BinaryOpType.GreaterThan: return "g";
                case BinaryOpType.GreaterEqual: return "ge";

                case BinaryOpType.ATan2: return "atan2";
            }
            return "+";
        }
        public static BinaryOpType GetOperatorTypeFromChar(string opChar)
        {
            switch (opChar)
            {
                case "+": return BinaryOpType.Plus;
                case "-": return BinaryOpType.Sub;
                case "*": return BinaryOpType.Mul;
                case "/": return BinaryOpType.Div;
                case "^": return BinaryOpType.Exp;
                case "%": return BinaryOpType.Mod;
                case "rand": return BinaryOpType.Rand;
                case "min": return BinaryOpType.Min;
                case "max": return BinaryOpType.Max;

                case "||":return BinaryOpType.Or;
                case "&&": return BinaryOpType.And;

                case "==":return BinaryOpType.Equal;
                case "<":return BinaryOpType.LessThan;
                case "<=": return BinaryOpType.LessEqual;
                case ">": return BinaryOpType.GreaterThan;
                case ">=": return BinaryOpType.GreaterEqual;

                case "atan2": return BinaryOpType.ATan2;
            }
            return BinaryOpType.Plus;
        }

        public override XElement Serialize()
        {
            return new XElement(GetXElementName(this.operaterType),
                new XAttribute("op", GetOperatorSerializeChar(this.operaterType)),
                new XAttribute("style", GetStyle(this.operaterType)),
                paramA.Serialize(),
                paramB.Serialize()
                );
        }
    }



    public enum VectorOpType
    {
        X,
        Y,
        Z,
        Magnitude,
        Norm,
        Angle,
        Clamp,
        Cross,
        Dot,
        Distance,
        Min,
        Max,
        Project,
        Scale,
    }


    public class VectorOp : VzExpression
    {
        public VzExpression vecParam;

        public VzExpression vecParam2; //optional

        public bool isBinary;

        public VectorOpType opType;

        public VectorOp(VzExpression vec1, VectorOpType type, VzExpression vec2 = null)
        {
            if(!VzExpression.IsNull( vec2))
            {
                isBinary = true;
            }
            else
            {
                isBinary = false;
            }

            vecParam = vec1;
            vecParam2 = vec2;

            opType = type;
        }


        public static string GetSerializeOpStr(VectorOpType type)
        {
            switch (type)
            {
                case VectorOpType.X: return "x";
                case VectorOpType.Y: return "y";
                case VectorOpType.Z: return "z";

                case VectorOpType.Magnitude: return "length";
                case VectorOpType.Norm: return "norm";

                case VectorOpType.Angle:return "angle";
                case VectorOpType.Clamp: return "clamp";
                case VectorOpType.Cross: return "cross";
                case VectorOpType.Dot: return "dot";
                case VectorOpType.Distance: return "dist";
                case VectorOpType.Min: return "min";
                case VectorOpType.Max: return "max";
                case VectorOpType.Project: return "project";
                case VectorOpType.Scale: return "scale";
                default: return "x";
            }
        }
        public static string GetSerializeStyleStr(VectorOpType type)
        {
            switch (type)
            {
                case VectorOpType.X:
                case VectorOpType.Y:
                case VectorOpType.Z:
                case VectorOpType.Magnitude:
                case VectorOpType.Norm:
                    return "vec-op-1";
                default:
                    return "vec-op-2";
            }
        }


        public override XElement Serialize()
        {
            XElement xVecOp = new XElement("VectorOp",
                new XAttribute("op", GetSerializeOpStr(this.opType)),
                new XAttribute("style", GetSerializeStyleStr(this.opType))
                );

            //vec1
            xVecOp.Add(vecParam.Serialize());

            //vec2 ?
            if (isBinary && (! VzExpression.IsNull( vecParam2)))
            {
                xVecOp.Add(vecParam2.Serialize());
            }


            return xVecOp;
        }
    }




    // *********************** 字符串运算符 *********************  
    
    //<StringOp op = "contains" style="contains" pos="0.8347168,-100.5072">
    //            <Constant text = "alpha" />
    //            < Constant text="a" />
    //          </StringOp>
    
    public enum StringOpType
    {
        Letter, //params: idx, str
        Length,//params: str
        Join,//params: strings
        SubString, //params: start end str
        Contains, //param:string substring
        Format,//params:fmt strings
    }
    public class StringOp : VzExpression
    {
        public StringOpType type;
        public VzExpression[] strings;

        public StringOp(StringOpType type, params VzExpression[] strings)
        {
            this.type = type;
            this.strings = strings;
        }

        public static string GetSerializeOpStr(StringOpType type)
        {
            switch (type)
            {
                case StringOpType.Length: return "length";
                case StringOpType.Join: return "join";
                case StringOpType.SubString: return "substring";
                case StringOpType.Contains: return "contains";
                case StringOpType.Format: return "format";
                default: return "length";
            }
        }
        public static string GetSerializeStyleStr(StringOpType type)
        {
            switch (type)
            {
                case StringOpType.Length: return "length";
                case StringOpType.Join: return "join";
                case StringOpType.SubString: return "substring";
                case StringOpType.Contains: return "contains";
                case StringOpType.Format: return "format";
                default: return "length";
            }
        }

        public override XElement Serialize()
        {
            XElement xStringOP = new XElement("StringOp",
                new XAttribute("op", GetSerializeOpStr(this.type)),
                new XAttribute("style", GetSerializeStyleStr(this.type))
                ) ;

            foreach(var str in this.strings)
            {
                xStringOP.Add(str.Serialize());
            }

            return xStringOP;
        }
    }

    public class StringFriendOp : VzExpression
    {
        public VzExpression str;

        public PhysicalQuantity quantity;

        public StringFriendOp(VzExpression expression, PhysicalQuantity quantity)
        {
            this.str = expression;
            this.quantity = quantity;
        }

        public override XElement Serialize()
        {
            XElement xStrFriendOp = new XElement("StringOp", 
                new XAttribute("op", "friendly"),
                new XAttribute("subOp", this.quantity.ToString().ToLower()),
                new XAttribute("style", "friendly")
                );
            xStrFriendOp.Add(str.Serialize());
            return xStrFriendOp;
        }
    }


    // *********************** 其他多元运算符 ***********************

    public class ConditionalExpression : VzExpression //三元运算符
    {
        public VzExpression condition;
        public VzExpression expression1;
        public VzExpression expression2;

        public ConditionalExpression(VzExpression condition, VzExpression e1, VzExpression e2)
        {
            this.condition = condition;
            this.expression1 = e1;
            this.expression2 = e2;
        }

        public override XElement Serialize()
        {
            XElement xConditional = new XElement("Conditional", 
                new XAttribute("style", "conditional")
                );
            xConditional.Add(condition.Serialize());
            xConditional.Add(expression1.Serialize());
            xConditional.Add(expression2.Serialize());

            return xConditional;
        }
    }



    public class NewVectorExpression: VzExpression // new Vector3
    {
        public VzExpression x;
        public VzExpression y;
        public VzExpression z;

        public NewVectorExpression(VzExpression newx, VzExpression newy, VzExpression newz)
        {
            this.x = newx;
            this.y = newy;
            this.z = newz;
        }

        public override XElement Serialize()
        {
            XElement xVecNew = new XElement("Vector",
                new XAttribute("style", "vec")
                );
            xVecNew.Add(x.Serialize());
            xVecNew.Add(y.Serialize());
            xVecNew.Add(z.Serialize());

            return xVecNew;
        }

    }





}
