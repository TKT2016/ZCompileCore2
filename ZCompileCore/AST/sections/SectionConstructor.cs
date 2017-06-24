using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class SectionConstructor : SectionBase
    {
        public List<ProcArg> Args = new List<ProcArg>();
        public StmtBlock Body;

        public ContextProc ProcContext;
        ZConstructorDesc constructorDesc;

		public SectionConstructor(SectionProc proc)
        {
            foreach (var item in proc.NamePart.NameTerms)
            {
                Args.Add(item as ProcArg);
            }
            this.Body = proc.Body;

            constructorDesc = new ZConstructorDesc();
        }

        public void AnalyName(NameTypeParser parser)
        {
           foreach(var arg in Args)
           {
               arg.ProcContext = this.ProcContext;
               arg.Analy(parser);
               ZMethodNormalArg procArg = new ZMethodNormalArg(arg.ArgName, arg.ArgZType);
               constructorDesc.Add(procArg);
           }
        }

        public void EmitName()
        {
            var classBuilder = this.ProcContext.ClassContext.EmitContext.ClassBuilder;
            MethodAttributes methodAttributes;
            CallingConventions callingConventions;
            bool isSstatic = (this.ProcContext.IsStatic);
            if (isSstatic)
            {
                methodAttributes = MethodAttributes.Private | MethodAttributes.Static;
                callingConventions = CallingConventions.Standard;
            }
            else
            {
                methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual;
                callingConventions = CallingConventions.HasThis;
            }
            var argTypes = this.constructorDesc.GetArgTypes();
            ConstructorBuilder constructorBuilder = classBuilder.DefineConstructor(methodAttributes, callingConventions, argTypes);
            ProcContext.EmitContext.SetBuilder(constructorBuilder);
            ProcContext.EmitContext.ILout = constructorBuilder.GetILGenerator();

            List<ZMethodNormalArg> normalArgs = this.constructorDesc.Args;
            int start_i = isSstatic ? 0 : 1;
            for (var i = 0; i < normalArgs.Count; i++)
            {
                constructorBuilder.DefineParameter(i + start_i, ParameterAttributes.None, normalArgs[i].ArgName);
            }
        }

        public void AnalyBody()
        {
            Body.ProcContext = this.ProcContext;
            if (this.ProcContext.ClassContext.InitPropertyMethod!=null)
                EmitHelper.CallDynamic(Body.IL, this.ProcContext.ClassContext.InitPropertyMethod);
            Body.Analy();
        }

        public void EmitBody()
        {
            Body.Emit();
            ProcContext.EmitContext.ILout.Emit(OpCodes.Ret);
        }

    }
}
