using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using Z语言系统;

namespace ZCompileCore.AST
{
    public class SectionProc : SectionBase
    {
        public ProcName NamePart;
        public Token RetToken;
        public StmtBlock Body;
        public ZType RetZType;
        public ContextProc ProcContext;

        public void AnalyName(NameTypeParser parser)
        {
            //NameTypeParser parser = new NameTypeParser(collection);
            NamePart.ProcContext = this.ProcContext;
            NamePart.AnalyName(parser);
            AnalyRet(parser);
        }

        public void EmitName()
        {
            var classBuilder = this.ProcContext.ClassContext.EmitContext.ClassBuilder;
            var argTypes = NamePart.ProcDesc.GetArgZTypes();
            var MethodName = NamePart.GetMethodName();
            MethodAttributes methodAttributes;
            bool isSstatic = (this.ProcContext.IsStatic);
            if (isSstatic)
            {
                methodAttributes = MethodAttributes.Public | MethodAttributes.Static;
            }
            else
            {
                methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual;
            }

            MethodBuilder methodBuilder = classBuilder.DefineMethod(MethodName, methodAttributes, RetZType.SharpType, argTypes.Select(p => p.SharpType).ToArray());
            if (MethodName == "启动")
            {
                Type myType = typeof(STAThreadAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
                methodBuilder.SetCustomAttribute(attributeBuilder);
            }
            else
            {
                SetAttrZCode(methodBuilder);
            }
            ProcContext.EmitContext.SetBuilder(methodBuilder);
            ProcContext.EmitContext.ILout = methodBuilder.GetILGenerator();

            List<ZMethodArg> normalArgs = this.ProcContext.ProcDesc.Args;
            int start_i = isSstatic ? 0 : 1;// int start_i = 0 ;
            for (var i = 0; i < normalArgs.Count; i++)
            {
                methodBuilder.DefineParameter(i + start_i, ParameterAttributes.None, normalArgs[i].ArgName);
            }
            this.ProcContext.ProcDesc.ZMethod =
                new ZMethodInfo(methodBuilder, isSstatic, new ZMethodDesc[] { NamePart.ProcDesc }, AccessAttributeEnum.Public);
        }

        private void SetAttrZCode(MethodBuilder methodBuilder)
        {
            Type myType = typeof(ZCodeAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
            string code = this.NamePart.ProcDesc.ToZCode();
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { code });
            methodBuilder.SetCustomAttribute(attributeBuilder);
        }

        private bool AnalyRet(NameTypeParser parser)
        {
            if (RetToken == null)
            {
                RetZType = ZTypeManager.ZVOID;
            }
            else
            {
                NameTypeParser.ParseResult result = parser.ParseType(RetToken);
                if (result != null)
                {
                    RetZType = result.ZType;
                    return true;
                }
                else
                {
                    RetZType = ZTypeManager.ZVOID;
                    errorf(RetToken.Position,"过程的结果'{0}'不存在",RetToken.GetText());
                }
            }
            this.ProcContext.RetZType = RetZType;
            return false;
        }

        public void AnalyBody()
        {
            Body.ProcContext = this.ProcContext;
            Body.Analy();
        }

        public void EmitBody()
        {
            var symbols = this.ProcContext.Symbols;
            var IL = this.ProcContext.EmitContext.ILout;
            this.ProcContext.LoacalVarList.Reverse();
            for (int i = 0; i < this.ProcContext.LoacalVarList.Count; i++)
            {
                string ident = this.ProcContext.LoacalVarList[i];
                SymbolLocalVar varSymbol = symbols.Get(ident) as SymbolLocalVar;
                varSymbol.VarBuilder = IL.DeclareLocal(varSymbol.SymbolZType.SharpType);
                varSymbol.VarBuilder.SetLocalSymInfo(varSymbol.SymbolName);
            }

            Body.Emit();
            ProcContext.EmitContext.ILout.Emit(OpCodes.Ret);
        }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(this.NamePart.ToString());
            buff.Append(":");
            if (this.RetToken != null)
                buff.Append(this.RetToken.GetText());
            buff.AppendLine();
            buff.Append(this.Body.ToString());
            return buff.ToString(); 
        }
    }
}
