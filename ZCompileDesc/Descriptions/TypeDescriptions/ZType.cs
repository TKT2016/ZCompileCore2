using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZType : IZDescType, IWordDictionary
    {
        public Type MarkType { get; protected set; }
        public Type SharpType { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }

        public virtual string ZName { get { return MarkType.Name; } }
        public virtual bool IsMarkSelf { get { return MarkType == SharpType; } }

        public abstract bool ContainsWord(string text);
        public abstract WordInfo SearchWord(string text); 
        //#region Word Dictionary
        //protected abstract WordDictionary GetWordTable();

        //protected WordDictionary _WordTable;
        //public WordDictionary WordTable
        //{
        //    get
        //    {
        //        if(_WordTable==null)
        //        {
        //            _WordTable = GetWordTable();
        //        }
        //        return _WordTable;
        //    }
        //}

        //protected string CreateWordKey(string zname, WordKind wkind)
        //{
        //    string str = zname + "-" + Enum.GetName(typeof(WordKind), wkind);
        //    return str;
        //}
        //#endregion 
    }
}
