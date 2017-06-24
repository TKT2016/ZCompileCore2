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
    public class ExpCallSubject : ExpCallAnalyedBase
    {
        Exp SubjectExp;
        ZMethodInfo CallZMethod;
        ZClassType SubjectZType;

        public ExpCallSubject(ContextExp context, Exp SubjectExp, ZCallDesc expProcDesc, ExpCall srcExp)
        {
            this.ExpContext = context;
            this.SubjectExp = SubjectExp;
            this.ExpProcDesc = expProcDesc;
            this.SrcExp = srcExp;
        }

        public override Exp Analy( )
        {
            if (SubjectExp.RetType is ZEnumType)
            {
                ErrorE(this.Postion, "约定类型没有过程");
            }
            else
            {
                SubjectZType =  (SubjectExp.RetType as ZClassType);
                CallZMethod = SubjectZType.SearchZMethod(ExpProcDesc);
                if (CallZMethod == null)
                {
                    ErrorE(this.Postion, "没有找到对应的过程");
                }
                else
                {
                    this.RetType = CallZMethod.RetZType;
                }
            }
            return this;
        }

        public override void Emit( )
        {
            EmitSubject();
            base.EmitArgs(ExpProcDesc,CallZMethod.ZDesces[0]);// SearchedProcDesc); //GenerateArgs(context);
            EmitHelper.CallDynamic(IL,CallZMethod.SharpMethod);// SearchedProcDesc.ExMethod);
            EmitConv(); //base.GenerateConv(context);
        }

        private void EmitSubject()
        {
            if (!CallZMethod.IsStatic)
            {
                SubjectExp.Emit();
            }
        }

     
    }
}
