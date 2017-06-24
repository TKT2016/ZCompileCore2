using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZLangRT;
using ZCompileDesc.Descriptions;
using ZCompileKit;

namespace ZCompileCore.AST
{
    public class ExpCall:Exp
    {
        public List<Exp> Elements { get; set; }
        ZCallDesc ExpProcDesc;

        public override Exp[] GetSubExps()
        {
            return Elements.ToArray();
        }

        public ExpCall()
        {
            Elements = new List<Exp>();
        }

        public override void SetContext(ContextExp context)
        {
            this.ExpContext = context;
            foreach (var expr in this.Elements)
            {
                expr.SetContext(context);
            }
        }

        public override Exp Analy( )
        {
            AnalyProcDesc();
            Exp exp = SearchProc();
            exp = exp.Analy();
            return exp;
        }

        private Exp SearchProc()
        {
            Exp temp = null;

            temp = SearchThis();
            if (temp != null) return temp;

            temp = SearchUse();
            if (temp != null) return temp;

            temp = SearchSubject();
            if (temp != null) return temp;
            
            ErrorE(this.Postion,"无法找到调用相应的过程");
            ExpCallNone expCallNone = new ExpCallNone(this.ExpContext, ExpProcDesc, this);
            return expCallNone;     
        }

        private Exp SearchSubject()
        {
            var SubjectExp = Elements[0];
            if (SubjectExp is ExpProcNamePart)
            {
                return null;
            }
            else
            {
                ZCallDesc tailDesc =  ExpProcDesc.CreateTail();
                ExpCallSubject expCallSubject = new ExpCallSubject(this.ExpContext, SubjectExp, tailDesc, this);
                return expCallSubject;
            }
        }

        private Exp SearchUse()
        {
            ZMethodInfo[] zmethods = this.ExpContext.ClassContext.FileContext.SearchUseProc(ExpProcDesc);
            if (zmethods.Length == 0)
            {
                return null;
            }
            else if (zmethods.Length > 1)
            {
                ErrorE(this.Postion, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
                return null;
            }
            else
            {
                ExpCallUse expCallForeign = new ExpCallUse(this.ExpContext, ExpProcDesc, zmethods[0], this);
                return expCallForeign;
            }
        }

        private Exp SearchThis()
        {
            ZMethodDesc[] descArray = this.ExpContext.ClassContext.SearchThisProc(ExpProcDesc);
            if (descArray.Length == 0)
            {
                return null;
            }
            else if (descArray.Length >1)
            {
                ErrorE(this.Postion, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
                return null;
            }
            else
            {
                ExpCallThis expCallThis = new ExpCallThis(this.ExpContext, ExpProcDesc, descArray[0], this);
                return expCallThis;
            }
        }

        private void AnalyProcDesc()
        {
            ExpProcDesc = new ZCallDesc();
            for (int i = 0; i < this.Elements.Count; i++)
            {
                Exp exp = this.Elements[i];
                exp.SetContext(this.ExpContext);
                Exp subExp = exp.Analy();
                Elements[i] = subExp;
                if (subExp != null)
                {
                    if (subExp is ExpProcNamePart)
                    {
                        ExpProcDesc.Add((subExp as ExpProcNamePart).PartName);
                    }
                    else if (subExp is ExpBracket)
                    {
                        ExpProcDesc.Add((subExp as ExpBracket).GetDimArgs().ToArray());
                    }
                    else
                    {
                        ExpBracket bracket = new ExpBracket();
                        bracket.AddInnerExp(subExp);
                        ExpProcDesc.Add(bracket.GetDimArgs().ToArray());
                    }
                }
            }
        }

        public override void Emit()
        {
            throw new CompileException();
        }

        #region 辅助
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            List<string> tempcodes = new List<string>();
            foreach (var expr in Elements)
            {
                if (expr != null)
                {
                    tempcodes.Add(expr.ToString());
                }
                else
                {
                    tempcodes.Add(" ");
                }
            }
            buf.Append(string.Join("", tempcodes));
            return buf.ToString();
        }

        public override CodePosition Postion
        {
            get
            {
                return Elements[0].Postion; ;
            }
        }
        #endregion
    }
}
