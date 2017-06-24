using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;
using ZLangRT;

namespace ZCompileDesc.Descriptions
{
    public class ZMethodDesc : ZMethodDescBase, IWordDictionary
    {
        public ZMethodInfo ZMethod { get; set; }
        public List<ZMethodArg> Args { get; protected set; }

        public ZMethodDesc()
        {
            Parts = new List<object>();
            Args = new List<ZMethodArg>();
        }

        public void Add(string str)
        {
            Parts.Add(str);
        }

        public void Add(params ZMethodArg[] args)
        {
            Parts.AddRange(args);
            Args.AddRange(args);
        }

        public bool ContainsWord(string text)
        {
            foreach (var part in this.Parts)
            {
                if (part is string)
                {
                    string str = part as string;
                    if (str == text)
                        return true;
                }
            }
            return false;
        }

        public WordInfo SearchWord(string text)
        {
            if (!ContainsWord(text)) return null;
            WordInfo info = new WordInfo(text, WordKind.ProcNamePart, this);
            return info;
        }

        public int Count
        {
            get
            {
                return Parts.Count;
            }
        }

        public ZType[] GetArgZTypes()
        {
            List<ZType> list = new List<ZType>();
            foreach(var arg in Args)
            {
                if(arg is ZMethodNormalArg)
                {
                    list.Add((arg as ZMethodNormalArg).ArgZType);
                }
            }
            return list.ToArray();
        }

        public override string ToString()
        {
             List<string> list = new List<string>();
            for (int i = 0; i < this.Parts.Count; i++)
            {
                if (this.Parts[i] is string)
                {
                    list.Add(this.Parts[i] as string);
                }
                else if (this.Parts[i] is ZMethodArg)
                {
                    list.Add("(" + (this.Parts[i] as ZMethodArg).ToString()+")");
                }
                else
                {
                    throw new ZyyRTException();
                }
            }
            return string.Join("", list);
        }

        public string ToZCode()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < this.Parts.Count; i++)
            {
                if (this.Parts[i] is string)
                {
                    list.Add(this.Parts[i] as string);
                }
                else if (this.Parts[i] is ZMethodArg)
                {
                    list.Add("(" + (this.Parts[i] as ZMethodArg).ToZCode() + ")");
                }
                else
                {
                    throw new ZyyRTException();
                }
            }
            return string.Join("", list);
        }

        public string ToMethodName()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < this.Parts.Count; i++)
            {
                object item = this.Parts[i];
                if (item is string)
                {
                    list.Add(item as string);
                }
                else if (item is ZMethodNormalArg)
                {
                    ZMethodNormalArg arg = item as ZMethodNormalArg;
                    list.Add(arg.ArgZType.ZName + arg.ArgName??"");
                }
                else
                {
                    throw new ZyyRTException();
                }
            }
            return string.Join("", list);
        }
    }
}
