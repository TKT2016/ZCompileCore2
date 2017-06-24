using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class ContextClass
    {
        public string ClassName { get; set; }
        public string ExtendsName { get; set; }
        public ContextFile FileContext { get; set; }
        
        public ZClassType BaseZType { get; set; }
        public bool IsStaticClass { get; set; }
        public ClassEmitContext EmitContext { get; set; }

        public PropertyContextCollection PropertyContext { get; set; }
        public ProcContextCollection ProcManagerContext { get; set; }
        public MethodBuilder InitPropertyMethod { get; set; }

        public ClassSymbolTable Symbols { get{return CurrentTable;} }

        public NameDictionary<SymbolDefMember> MemberDictionary { get;private set; }

        public SuperSymbolTable SuperTable { get; private set; }
        public ClassSymbolTable CurrentTable { get; private set; }

        public SymbolDefField NestedOutFieldSymbol { get; set; }

        public ContextClass(ContextFile fileContext)
        {
            FileContext = fileContext;

            PropertyContext = new PropertyContextCollection();
            PropertyContext.ClassContext = this;

            ProcManagerContext = new ProcContextCollection();
            ProcManagerContext.ClassContext = this;
            EmitContext = new ClassEmitContext();

            MemberDictionary = new NameDictionary<SymbolDefMember>();
            CurrentTable = new ClassSymbolTable("Class");
        }

        public void SetSuperTable(SuperSymbolTable superTable)
        {
            SuperTable = superTable;
            CurrentTable.ParentTable = SuperTable;
        }

        public SymbolDefMember FindMember(string name)
        {
            if(MemberDictionary.ContainsKey(name))
            {
                SymbolDefMember member = MemberDictionary.Get(name);
                return member;
            }
            if (IsStaticClass) return null;
            ZMemberInfo zmember = BaseZType.SearchZMember(name);
            if (zmember == null) return null;
            SymbolDefMember symbol = SymbolDefMember.Create(name,zmember);
            return symbol;
        }

        public ZMethodDesc[] SearchThisProc(ZCallDesc procDesc)
        {
            return ProcManagerContext.SearchProc(procDesc);
        }

        public class ClassEmitContext
        {
            public TypeBuilder ClassBuilder { get; set; }
            public ConstructorBuilder ZeroConstructor { get;  set; }
            public MethodBuilder InitMemberValueMethod { get;  set; }
            public ISymbolDocumentWriter IDoc { get;  set; }
        }

        IWordDictionaryList _WordCollection;
        public IWordDictionary ClassWordDictionary
        {
            get
            {
                if (_WordCollection == null)
                {
                    _WordCollection = new IWordDictionaryList();
                    _WordCollection.Add(this.FileContext.ImportContext.ImportPackageDescList);
                    _WordCollection.Add(this.PropertyContext.Dict);
                    _WordCollection.AddRange(this.ProcManagerContext.GetWordCollection());
                }
                return _WordCollection;
            }
        }
    }
}
