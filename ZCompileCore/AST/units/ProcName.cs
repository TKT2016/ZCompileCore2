using System.Collections.Generic;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileKit;

namespace ZCompileCore.AST
{
    public class ProcName : UnitBase
    {
        public List<object> NameTerms = new List<object>();
        public ZMethodDesc ProcDesc;
        public ContextProc ProcContext;

        public void AddNamePart(Token token)
        {
            NameTerms.Add(token);
        }

        public void AddArg(ProcArg arg)
        {
            NameTerms.Add(arg);
        }

        public bool IsConstructor()
        {
            foreach (var item in NameTerms)
            {
                if (item is Token)
                    return false;
            }
            return true;
        }

        public void AnalyName(NameTypeParser parser)
        {
            AnlayNameBody(parser);
            this.ProcContext.ProcDesc = ProcDesc;
        }

        private bool AnlayNameBody(NameTypeParser parser)
        {
            bool isStatic = this.ProcContext.IsStatic;

            ProcDesc = new ZMethodDesc();
            //int argIndex = this.procContext.IsStatic ? 0 : 1;
            int argIndex = isStatic ? 0 : 1;

            for (int i = 0; i < NameTerms.Count; i++)
            {
                var term = NameTerms[i];
                if (term is Token)
                {
                    var textterm = term as Token;
                    string namePart = textterm.GetText();
                    ProcDesc.Add(namePart);

                    WordInfo info = new WordInfo(namePart, WordKind.ProcNamePart ,this);
                    this.ProcContext.ProcNameWordDictionary.Add(info);
                }
                else if (term is ProcArg)
                {
                    AnalyArg(term as ProcArg, parser, ProcDesc);
                }
            }
            return true;
        }

        public void AnalyArg(ProcArg argTree, NameTypeParser parser,ZMethodDesc desc)
        {
            //ProcArg argTree = term as ProcArg;
            argTree.ProcContext = this.ProcContext;
            argTree.Analy(parser);
            ZMethodNormalArg procArg = new ZMethodNormalArg(argTree.ArgName, argTree.ArgZType);
            desc.Add(procArg);
            //argIndex++;
        }

        public string GetMethodName()
        {
            ContextClass context = this.ProcContext.ClassContext;
            ZClassType baseZType = context.BaseZType;
            if(baseZType!=null)
            {
                ZCallDesc callDesc = ToZCallDesc(ProcDesc);
                var zmethod = baseZType.SearchZMethod(callDesc);
                if(zmethod!=null)
                {
                    return zmethod.SharpMethod.Name;
                }
            }
            return this.ProcDesc.ToMethodName();
        }

        private ZCallDesc ToZCallDesc(ZMethodDesc methodDesc)
        {
            ZCallDesc callDesc = new ZCallDesc();
            foreach(var part in methodDesc.Parts)
            {
                if(part is string)
                {
                    callDesc.Add(part);
                }
                else if (part is ZMethodNormalArg)
                {
                    ZMethodNormalArg mna = part as ZMethodNormalArg;
                    ZCallValueArg callArg = new ZCallValueArg(mna.ArgZType);
                    callDesc.Add(callArg);
                }
                else if (part is ZMethodGenericArg)
                {
                    ZMethodGenericArg mna = part as ZMethodGenericArg;
                    ZCallValueArg callArg = new ZCallValueArg(mna.ArgBaseZType);
                    callDesc.Add(callArg);
                }
                else
                {
                    throw new CompileException();
                }
            }
            return callDesc;
        }

        public override string ToString()
        {
            List<string> buflist = new List<string>();
            foreach (var term in NameTerms)
            {
                buflist.Add(term.ToString());
            }
            //if (RetTypeToken != null)
            //{
            //    buflist.Add(":");
            //    buflist.Add(RetTypeToken.GetText());
            //}
            string fnname = string.Join("", buflist);
            return fnname;
        }
    }
}

