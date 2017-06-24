using System;
using System.Collections.Generic;
using System.Reflection;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class ExpNew:Exp
    {
        public ExpType TypeExp { get; set; }
        public ExpBracket BracketExp { get; set; }

        ZNewDesc newDesc;
        ZConstructorInfo ZConstructor;

        public override Exp Analy( )
        {
            TypeExp = AnalySubExp(TypeExp) as ExpType; //TypeExp = ArgExp.Analy();
            BracketExp = AnalySubExp(BracketExp) as ExpBracket; //BracketExp.Analy();
            if (!AnalyCorrect) return this;

            //Type subjectType = TypeExp.RetType.SharpType;
            if (IsList())
            {
                ExpNewList newListExp = new ExpNewList(this.ExpContext,TypeExp, BracketExp);
                //newListExp.SetContext(this.ExpContext);
                return newListExp.Analy();
            }
            else
            {
                var args = BracketExp.GetCallNormalArgs();
                //ZMethodArgCollection argCollection = new ZMethodArgCollection(args);
                newDesc = new ZNewDesc(args);
                ZConstructor = (TypeExp.RetType as ZClassType).FindDeclaredZConstructor(newDesc);
                if (ZConstructor == null)
                {
                    ErrorE(BracketExp.Postion, "没有正确的创建过程");
                }
                else
                {
                    RetType = TypeExp.RetType;
                }
            }
            return this;
        }

        private bool IsList()
        {
             Type subjectType = TypeExp.RetType.SharpType;
            if (!subjectType.Name.StartsWith(CompileConstant.ZListClassZName)) return false;
            if (subjectType.Namespace!= CompileConstant.LangPackageName) return false;
            return true;
        }

        public override void Emit()
        {
            var Constructor = ZConstructor.Constructor;
            EmitConstructor();
            EmitHelper.NewObj(IL, Constructor);
            base.EmitConv();
        }

        private void EmitConstructor( )
        {
            EmitArgs(newDesc.Args, ZConstructor.Constructor.GetParameters());
        }

        private void EmitArgs(List<ZCallValueArg> args, ParameterInfo[] parameterInfos)
        {
            if (args == null) return;
            List<Exp> argExps = new List<Exp>();
            foreach (var procArg in args)
            {
                Exp arg = procArg.Data as Exp;
                argExps.Add(arg);
            }
            base.EmitArgsExp( parameterInfos, argExps.ToArray());
        }
        
        public override string ToString()
        {
            return TypeExp.ToString() + BracketExp.ToString();
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { TypeExp, BracketExp };
        }

    }
}
