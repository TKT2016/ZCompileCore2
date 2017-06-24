using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public abstract class Exp:Tree
    {
        #region Context
        public ContextExp ExpContext { get;protected set; }
        public ContextProc ProcContext { get { return this.ExpContext.ProcContext; } }
        public ContextClass ClassContext { get { return this.ExpContext.ProcContext.ClassContext; } }
        public override ContextFile FileContext { get { return this.ExpContext.ProcContext.ClassContext.FileContext; } }
        public ContextProject ProjectContext { get { return this.ExpContext.ProcContext.ClassContext.FileContext.ProjectContext; } }
        #endregion

        public bool IsNested { get; protected set; }
        public virtual void SetIsNested(bool b)
        {
            this.IsNested = b;
            Exp[] subs = GetSubExps();
            if (subs != null && subs.Length > 0)
            {
                foreach (var exp in subs)
                {
                    if (exp != null)
                    {
                        exp.SetIsNested(b);
                    }
                }
            }
        }
        public abstract Exp[] GetSubExps();

        public virtual void SetContext(ContextExp context)
        {
            this.ExpContext = context;
            Exp[] subs =  GetSubExps();
            if (subs!=null && subs.Length > 0)
            {
                foreach (var exp in subs)
                {
                    if (exp != null)
                    {
                        exp.SetContext(context);
                    }
                }
            }
        }

        public ZType RetType { get; protected set; }
        public ZType RequireType { get;  set; }

        //public Exp ParentExp { get; set; }

        //public Exp GetTopExp()
        //{
        //    Exp exp = this;
        //    while(exp.ParentExp!=null)
        //    {
        //        exp = exp.ParentExp;
        //    }
        //    return exp;
        //}

        public virtual Exp Parse()
        {
            return this;
        }

        public virtual Exp Analy( )
        {
            return this;
        }

        public virtual void Emit()
        {
            throw new NotImplementedException();
        }

        //public bool AnalyResult { get;protected set; }

        protected CodePosition ZeroCodePostion = new CodePosition(0, 0);
        public virtual CodePosition Postion { get { return ZeroCodePostion; } }

        protected Exp AnalySubExp(Exp exp)
        {
            if (exp == null)
            {
                AnalyCorrect = false;
                return null;
            }
            else
            {
                exp = exp.Analy();
                AnalyCorrect = AnalyCorrect && exp.AnalyCorrect;
                return exp;
            }
        }

        public ILGenerator IL
        {
            get
            {
                return this.ExpContext.ProcContext.EmitContext.ILout;
            }
        }

        protected void EmitConv( )
        {
            if (RequireType != null && RetType!=null)
            EmitHelper.EmitConv(IL, RequireType.SharpType , RetType.SharpType);
        }

        protected void EmitArgsExp( ParameterInfo[] paramInfos, Exp[] args)
        {
            var size = paramInfos.Length;

            for (int i = 0; i < size; i++)
            {
                Exp argExp = args[i];
                ParameterInfo parameter = paramInfos[i];
                argExp.RequireType = ZTypeManager.GetBySharpType(parameter.ParameterType) as ZType;
                argExp.Emit();
            }
        }

        protected void EmitArgs(ZCallDesc expProcDesc, ZMethodDesc searchedProcDesc)
        {
            var size = expProcDesc.Args.Count; ;

            for (int i = 0; i < size; i++)
            {
                var arg = expProcDesc.Args[i];
                var exp = (arg.Data as Exp);
                ZMethodArg procArg = searchedProcDesc.Args[i];
                exp.RequireType = (procArg as ZMethodNormalArg).ArgZType;// ZRealType.CreateZType(procArg.ArgType);
                exp.Emit();
            }
        }

        /// <summary>
        /// 报告错误并把分析结果设为false
        /// </summary>
        //protected virtual void ErrorE(CodePosition postion, string msgFormat, params string[] msgParams)
        //{
        //    this.errorf(postion, msgFormat, msgParams);
        //    AnalyResult = false;
        //}
    }
}
