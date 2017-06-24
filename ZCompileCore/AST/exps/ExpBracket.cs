using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class ExpBracket:Exp
    {
        public Token LeftBracketToken { get; set; }
        public Token RightBracketToken { get;  set; }
        public List<Exp> InneExps { get; protected set; }

        public override Exp[] GetSubExps()
        {
            return InneExps.ToArray();
        }

        public ExpBracket()
        {
            InneExps = new List<Exp>();
        }

        public override void SetContext(ContextExp context)
        {
            this.ExpContext = context;
            foreach (var expr in this.InneExps)
            {
                expr.SetContext(context);
            }
        }

        public void AddInnerExp(Exp exp)
        {
            InneExps.Add(exp);
        }

        public override Exp Analy( )
        {
            this.AnalyCorrect = true;
            for (int i=0;i<InneExps.Count;i++)
            {
                Exp exp = InneExps[i];
                exp.SetContext(this.ExpContext);
                exp = exp.Analy();
                if(exp==null)
                {
                    AnalyCorrect = false;
                }
                else
                {
                    InneExps[i] = exp;
                    AnalyCorrect = AnalyCorrect && exp.AnalyCorrect;
                }
            }
            if (InneExps.Count == 1)
            {
                RetType = InneExps[0].RetType; 
            }
            else
            {
                RetType = ZTypeManager.ZVOID;
            }
            return this;
        }

        public List<ZType> GetInnerTypes()
        {
            List<ZType> list = new List<ZType>();
            foreach (var expr in this.InneExps)
            {
                list.Add(expr.RetType);
            }
            return list;
        }

        public List<ZCallValueArg> GetCallNormalArgs()
        {
            List<ZCallValueArg> args = new List<ZCallValueArg>();
            foreach (var exp in this.InneExps)
            {     
                if (!(exp is ExpType))
                {
                    var type = exp.RetType;
                    ZCallValueArg arg = new ZCallValueArg(type);
                    arg.Data = exp;
                    args.Add(arg);
                }
            }
            return args;
        }

        public List<ZCallArg> GetDimArgs()
        {
            List<ZCallArg> args = new List<ZCallArg>();
            foreach (var exp in this.InneExps)
            {
                bool isGeneric = false;
                var type = exp.RetType;
                if (exp is ExpType)
                {
                    var idExp = exp as ExpType;
                    isGeneric = idExp.RetType.SharpType.IsGenericType;
                    ZCallGenericArg arg = new ZCallGenericArg(type);
                    arg.Data = exp;
                     args.Add(arg);
                }
                else
                {
                    ZCallValueArg arg = new ZCallValueArg (type);
                    arg.Data = exp;
                     args.Add(arg);
                }
            }
            return args;
        }

        public int InnerCount
        {
            get
            {
                return InneExps.Count;
            }
        }

        #region 覆盖
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(");
            if (InneExps != null && InneExps.Count > 0)
            {
                List<string> tempcodes = new List<string>();
                foreach (var expr in InneExps)
                {
                    if (expr != null)
                    {
                        tempcodes.Add(expr.ToString());
                    }
                    else
                    {
                        tempcodes.Add("  ");
                    }
                }
                buf.Append(string.Join(",", tempcodes));
            }
            buf.Append(")");
            return buf.ToString();
        }

        public override CodePosition Postion
        {
            get
            {
                return LeftBracketToken!=null ?LeftBracketToken.Position:InneExps[0].Postion;
            }
        }
        #endregion
    }
}
