using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Symbols;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class ExpAssign:Exp
    {
        public Exp ToExp { get; set; }
        public Exp ValueExp { get; set; }
        public bool IsAssignTo { get; set; } //true: => 

        public override Exp[] GetSubExps()
        {
            return new Exp[] { ToExp, ValueExp };
        }

        public override Exp Analy( )
        {
            ValueExp = AnalySubExp(ValueExp);// AnalyValueExp();
            
            if(ToExp is ExpVar )
            {
                AnalyToExp_Var();
            }
            else if (ToExp is ExpDe || ToExp is ExpDi)
            {
                AnalyToExp_Other();//ToExp = ToExp.Analy();
            }
            else
            {
                ErrorE(this.ToExp.Postion, "该表达式不能被赋值");
                //AnalyResult = false;
            }
            this.RetType = ZTypeManager.ZVOID;
            return this;
        }

        private void AnalyToExp_Var()
        {
            var varExp = ToExp as ExpVar;
            var table = this.ProcContext.Symbols;
            if (!table.Contains(varExp.VarName))
            {
                varExp.SetAssigned(ValueExp.RetType);
                ToExp = varExp.AnalyDim();
                AnalyCorrect = AnalyCorrect && ValueExp.AnalyCorrect;
            }
            else
            {
                AnalyToExp_Other();//ToExp = varExp.Analy();
            }
        }

        private void AnalyToExp_Other()
        {
            ToExp = ToExp.Analy();
            AnalyCorrect = AnalyCorrect && ToExp.AnalyCorrect;
        }

        //private bool AnalyValueExp()
        //{
        //    if (ValueExp == null)
        //    {
        //        AnalyResult = false;
        //        return false;
        //    }
        //    else
        //    {
        //        ValueExp = ValueExp.Analy();
        //        AnalyResult = AnalyResult && ValueExp.AnalyResult;
        //        return true;
        //    }
        //}

        public override void Emit()
        {
            (ToExp as ISetter).EmitSet(ValueExp);
        }

        public override string ToString()
        {
           if(IsAssignTo)
           {
               return ValueExp.ToString()+"=>"+ToExp.ToString(); 
           }
           else
           {
               return ToExp.ToString() + "=" + ValueExp.ToString();
           }
        }
    }
}
