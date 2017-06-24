using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZMemberInfo: IWordDictionary
    {
        public bool IsStatic { get; protected set; }
        public abstract ZType MemberZType { get;}//protected set; }
        public string[] ZNames { get; protected set; }
        public bool CanRead { get; protected set; }
        public bool CanWrite { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }

        public virtual bool HasZName(string zname)
        {
            foreach(var item in ZNames)
            {
                if (item == zname)
                    return true;
            }
            return false;
        }

        public abstract string SharpMemberName { get; }
        //public abstract WordInfo[] GetWordInfos();

        public virtual bool ContainsWord(string text)
        {
            return this.HasZName(text);
        }

        public virtual WordInfo SearchWord(string text)
        {
            if(!HasZName(text)) return null;
            WordInfo info = new WordInfo(text, WordKind.MemberName,this);
            return info;
        }

        //public static bool HasZCode
        //public bool IsSelf { get;  set; }
        //protected List<string> _ZyyNames;
        //public abstract List<string> ZyyNames { get; }

        //protected List<string> GetZyyNames(MemberInfo member)
        //{
        //    List<string> names = new List<string>();
        //    Attribute[] attrs = Attribute.GetCustomAttributes(member, typeof(ZCodeAttribute));
        //    if (attrs.Length == 0)
        //    {
        //        names.Add(member.Name);
        //    }
        //    else
        //    {
        //        foreach (Attribute attr in attrs)
        //        {
        //            ZCodeAttribute zCodeAttribute = attr as ZCodeAttribute;
        //            names.Add(zCodeAttribute.Code);
        //        }
        //    }
        //    return names; 
        //}
    }
}
