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
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    /// <summary>
    /// 无法确定的Call
    /// </summary>
    public class ExpCallAnalyedBase:Exp
    {
        protected ZCallDesc ExpProcDesc;
        protected ExpCall SrcExp;

        public override Exp[] GetSubExps()
        {
            return SrcExp.GetSubExps();
        }

        protected void AnalyArgLambda(ZMethodDesc zdesc)
        {
            //ZMethodDesc zdesc = SearchedMethod.ZDesces[0];
            for (int i = 0; i < ExpProcDesc.Args.Count; i++)
            {
                ZMethodArg procArg = zdesc.Args[i];
                if (procArg is ZMethodNormalArg)
                {
                    ZMethodNormalArg znarg = (procArg as ZMethodNormalArg);
                    if (ZLambda.IsFn(znarg.ArgZType.SharpType))
                    {
                        ZCallArg expArg = ExpProcDesc.Args[i];
                        Exp exp = expArg.Data as Exp;
                        ExpNewLambda newLambdaExp = new ExpNewLambda(exp, znarg.ArgZType);
                        newLambdaExp.SetContext(this.ExpContext);
                        expArg.Data = newLambdaExp;
                        newLambdaExp.Analy();
                    }
                }
            }
        }

        #region 辅助
        public override string ToString()
        {
            return SrcExp.ToString();
        }

        public override CodePosition Postion
        {
            get
            {
                return SrcExp.Postion; ;
            }
        }
        #endregion
    }
}
