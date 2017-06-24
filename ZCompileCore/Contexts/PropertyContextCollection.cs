using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class PropertyContextCollection
    {
        public ContextClass ClassContext { get; set; }

        public WordDictionary Dict { get; private set; }

        public PropertyContextCollection()
        {
            Dict = new WordDictionary("代码属性表");
        }

        //public bool ContainsName(string propertyName)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Add(SymbolDefProperty symbolDefProperty)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
