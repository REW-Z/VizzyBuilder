using System;
using System.Collections.Generic;
using System.Text;

namespace REWVIZZY
{
    public class FUNK
    {
        protected string value;
        public FUNK(string input)
        {
            this.value = input;
        }
        public string Value => this.value;



        public static implicit operator FUNK(string input)
        {
            return new FUNK(input);
        }



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static FUNK operator +(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "+" + b.value + ")");
        }
        public static FUNK operator -(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "-" + b.value + ")");
        }
        public static FUNK operator *(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "*" + b.value + ")");
        }
        public static FUNK operator /(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "/" + b.value + ")");
        }
        public static FUNK operator ^(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "^" + b.value + ")");
        }
        public static FUNK operator %(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "%" + b.value + ")");
        }
        public static FUNK operator !(FUNK a)
        {
            return new FUNK("(!" + a.value + ")");
        }
        public static FUNK operator ==(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "==" + b.value + ")");
        }
        public static FUNK operator !=(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "!=" + b.value + ")");
        }
        public static FUNK operator <(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "<" + b.value + ")");
        }
        public static FUNK operator >(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + ">" + b.value + ")");
        }
        public static FUNK operator <=(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + "<=" + b.value + ")");
        }
        public static FUNK operator >=(FUNK a, FUNK b)
        {
            return new FUNK("(" + a.value + ">=" + b.value + ")");
        }


        public static FUNK Function(string functionName, params FUNK[] args)
        {
            string expr = functionName + "(";
            for (int i = 0; i < args.Length; i++)
            {
                if (i != 0) expr += ",";
                expr += args[i].ToString();
            }
            expr += ")";
            return new FUNK(expr);
        }
    }
}
