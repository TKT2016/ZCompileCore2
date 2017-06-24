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
    public class ExpCallUse : ExpCallAnalyedBase
    {
        //ZMethodInfo[] SearchedZMethods;
        ZMethodInfo SearchedMethod;

        public ExpCallUse(ContextExp context, ZCallDesc expProcDesc, ZMethodInfo zmethod, ExpCall srcExp)
        {
            this.ExpContext = context;
            this.ExpProcDesc = expProcDesc;
            this.SearchedMethod = zmethod;
            this.SrcExp = srcExp;
        }

        public override Exp Analy( )
        {
            if (SearchedMethod!=null)
            {
                AnalyArgLambda(SearchedMethod.ZDesces[0]);
            }
            RetType = SearchedMethod.RetZType;
            return this;
        }

        public override void Emit()
        {
            EmitSubject();
            base.EmitArgs(ExpProcDesc, SearchedMethod.ZDesces[0]); //GenerateArgs(context);
            EmitHelper.CallDynamic(IL, SearchedMethod.SharpMethod);
            EmitConv(); //base.GenerateConv(context);
        }

        private void EmitSubject()//必须为Static
        {
            return;
        }
    }
}
