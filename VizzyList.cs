using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace REWVIZZY
{
    // ******************************** 列表指令 ***************************


    public class InitListInstruction : VzInstruction
    {
        public VzExpression targetList;
        public VzExpression newList;

        public InitListInstruction(VzExpression list, VzExpression newlist)
        {
            this.targetList = list;
            this.newList = newlist;
        }

        public override XElement Serialize()
        {
            XElement xInit = new XElement("SetVariable", 
                new XAttribute("id", this.id),
                new XAttribute("style", "list-init")
                );
            xInit.Add(targetList.Serialize());
            xInit.Add(newList.Serialize());
            return xInit;
        }
    }

    public enum ListInstructionType
    {
        Add,
        Insert,
        Remove,
        Set,
        Clear,
        Sort,
        Reverse
    }
    public class ListOpInstruction : VzInstruction
    {
        public ListInstructionType type;

        public VzExpression targetList;
        public VzExpression item;
        public List<VzExpression> otherParameters;

        public ListOpInstruction(ListInstructionType type, VzExpression targetList, VzExpression item = null, params VzExpression[] otherArgs)
        {
            this.type = type;
            this.targetList = targetList;
            this.item = item;
            this.otherParameters = otherArgs.ToList();
        }



        public override XElement Serialize()
        {
            XElement xListInstruction = new XElement("SetList",
                new XAttribute("op", this.type.ToString().ToLower()),
                new XAttribute("id", this.id),
                new XAttribute("style", "list-" + this.type.ToString().ToLower())
                );

            //list param
            xListInstruction.Add(targetList.Serialize());

            //item param
            if (!VzExpression.IsNull(item))
            {
                xListInstruction.Add(item.Serialize());
            }

            //other params
            if (otherParameters != null)
            {
                foreach (var otherparam in otherParameters)
                {
                    xListInstruction.Add(otherparam.Serialize());
                }
            }

            return xListInstruction;
        }
    }


    // ******************************** 列表相关表达式 ***************************
    public enum ListExpressionType
    {
        Create,
        Get,
        Length,
        Index,
    }
    public class ListExpression : VzExpression
    {
        public ListExpressionType type;

        public VzExpression targetList = null;

        public VzExpression[] otherParameters = null;

        public ListExpression(ListExpressionType type, VzExpression list, params VzExpression[] otherArgs)
        {
            this.type = type;
            this.targetList = list;
            this.otherParameters = otherArgs;
        }

        public static ListExpression CreateList(VzExpression lnitlist)
        {
            return new ListExpression(ListExpressionType.Create, lnitlist);
        }
        public static ListExpression GetListItem(VzExpression list, VzExpression idx)
        {
            return new ListExpression(ListExpressionType.Get, list, idx);
        }
        public static ListExpression GetListLength(VzExpression list)
        {
            return new ListExpression(ListExpressionType.Length, list);
        }
        public static ListExpression IndexOf(VzExpression list, VzExpression item)
        {
            return new ListExpression(ListExpressionType.Index, list, item);
        }



        public override XElement Serialize()
        {
            XElement xexpression = new XElement("ListOp",
                new XAttribute("op", type.ToString().ToLower()),
                new XAttribute("style", "list-" + type.ToString().ToLower())
                );

            if(!VzExpression.IsNull( targetList ))
            {
                xexpression.Add(targetList.Serialize());
            }
            if(otherParameters != null)
            {
                foreach(var parm in otherParameters)
                {
                    xexpression.Add(parm.Serialize());
                }
            }

            return xexpression;
        }
    }
}
