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
    public class ExpCallThis : ExpCallAnalyedBase
    {
        //ZMethodDesc[] SearchedProcDescArray;
        ZMethodDesc SearchedProcDesc;

        public ExpCallThis(ContextExp context, ZCallDesc expProcDesc, ZMethodDesc searchedMethod, ExpCall srcExp)
        {
            this.ExpContext = context;
            this.ExpProcDesc = expProcDesc;
            this.SearchedProcDesc = searchedMethod;
            this.SrcExp = srcExp;
        }

        public override Exp Analy( )
        {
            RetType = this.SearchedProcDesc.ZMethod.RetZType;
            return this;
        }

        //private void SearchProc()
        //{
        //    if (SearchedProcDescArray.Length == 1)
        //    {
        //        SearchedProcDesc= SearchedProcDescArray[0];
        //        this.RetType = SearchedProcDesc.ZMethod.RetZType;
        //    }
        //    else if (SearchedProcDescArray.Length > 1)
        //    {
        //        ErrorE(this.Postion, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
        //    }
        //    else
        //    {
        //        ErrorE(this.Postion, "没有找到对应的过程");
        //    }
        //}

        public override void Emit()
        {
            EmitSubject();
            base.EmitArgs(ExpProcDesc, SearchedProcDesc);
            EmitHelper.CallDynamic(IL, SearchedProcDesc.ZMethod.SharpMethod);
            EmitConv();
        }

        private void EmitSubject()
        {
            if(IsNested && this.ClassContext.NestedOutFieldSymbol!=null)
            {
                IL.Emit(OpCodes.Ldarg_0);
                EmitHelper.LoadField(IL, this.ClassContext.NestedOutFieldSymbol.Field);
            }
            else if (SearchedProcDesc.ZMethod.IsStatic==false)
            {
                IL.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                //...
            }
        }

    }
}
