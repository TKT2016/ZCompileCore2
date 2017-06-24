using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class ContextProc
    {
        public bool IsConstructor { get; set; }
        public ZMethodDesc ProcDesc { get; set; }
        public ProcContextCollection ProcManagerContext { get; set; }
        public ContextClass ClassContext { get;private set; }
        public ZType RetZType { get; set; }
        public ProcSymbolTable Symbols { get; set; }
        public string ProcName { get; set; }

        public WordDictionary ProcNameWordDictionary { get;private set; }
        public WordDictionary ProcVarWordDictionary { get; private set; }

        public ContextProc(ContextClass classContext)
        {
            this.ClassContext = classContext;

            //EachIndex = 0;
            EmitContext = new ProcEmitContext();
            Symbols = new ProcSymbolTable("PROC", classContext.Symbols);
            ProcNameWordDictionary = new WordDictionary("代码过程表");
            ProcVarWordDictionary = new WordDictionary("代码变量表");

            //ArgDictionary = new NameDictionary<SymbolArg>();
            //LocalVarTable = new NameDictionary<SymbolLocalVar>();
        }

        #region create index :localvar ,arg, each
        int LoacalVarIndex = -1;
        public List<string> LoacalVarList = new List<string>();
        public int CreateLocalVarIndex(string name)
        {
            LoacalVarIndex++;
            LoacalVarList.Add(name);
            return LoacalVarIndex;
        }

        int ArgIndex = -1;
        public List<string> ArgList = new List<string>();
        public int CreateArgIndex(string name)
        {
            if (ArgIndex == -1)
            {
                if(IsStatic)
                {
                    ArgIndex = 0;
                }
                else
                {
                    ArgIndex = 1;
                }
                ArgList.Add(name);
            }
            else
            {
                ArgIndex++;
                ArgList.Add(name);
            }
            return ArgIndex;
        }

        int EachIndex = -1;
        //public List<string> ArgList = new List<string>();
        public int CreateEachIndex( )
        {
            EachIndex ++;
            //ArgList.Add(name);
            return EachIndex;
        }

        #endregion

        public ProcEmitContext EmitContext { get; set; }
        public bool IsStatic
        {
            get
            {
                return this.ClassContext.IsStaticClass;
            }
        }

        IWordDictionaryList _WordCollection;
        public IWordDictionary ClassWordDictionary
        {
            get
            {
                //return this.ProcContext.GetWordCollection();
                if (_WordCollection == null)
                {
                    _WordCollection = new IWordDictionaryList();
                    _WordCollection.Add(this.ClassContext.ClassWordDictionary);
                    _WordCollection.Add(this.ProcVarWordDictionary);
                }
                return _WordCollection;
            }
        }

        private int NestedIndex = 0;
        public string CreateNestedClassName()
        {
            NestedIndex++;
            return (ProcName ?? "") + "Nested" + NestedIndex;
        }

        public class ProcEmitContext
        {
            public MethodBuilder CurrentMethodBuilder { get; private set; }
            public ConstructorBuilder CurrentConstructorBuilder { get; private set; }

            public void SetBuilder(MethodBuilder methodBuilder)
            {
                CurrentMethodBuilder = methodBuilder;
            }

            public void SetBuilder(ConstructorBuilder constructorBuilder)
            {
                CurrentConstructorBuilder = constructorBuilder;
            }

            public ILGenerator ILout { get; set; }
        }
    }
}
