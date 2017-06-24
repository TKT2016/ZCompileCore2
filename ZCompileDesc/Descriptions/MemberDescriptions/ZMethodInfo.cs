using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Utils;
using ZCompileDesc.Words;
using ZCompileDesc.Collections;

namespace ZCompileDesc.Descriptions
{
    public class ZMethodInfo : IWordDictionary //: ZMemberInfo
    {
        public MethodInfo MarkMethod { get; private set; }
        public MethodInfo SharpMethod { get; private set; }
        public bool IsStatic { get; protected set; }
        public ZType RetZType { get { return ZTypeManager.GetBySharpType(SharpMethod.ReturnType) as ZType; } }
        public ZMethodDesc[] ZDesces { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }

        //protected ZCodeParser parser = new ZCodeParser();

        public ZMethodInfo(MethodInfo method)
        {
            MarkMethod = method;
            SharpMethod = method;
            Init();
        }

        public ZMethodInfo(MethodInfo markMethod,MethodInfo sharpMethod)
        {
            MarkMethod = markMethod;
            SharpMethod = sharpMethod;
            Init();
        }

        public ZMethodInfo(MethodBuilder builder, bool isStatic, ZMethodDesc[] desces, AccessAttributeEnum accAttr)
        {
            MarkMethod = builder;
            SharpMethod = builder;
            IsStatic = isStatic;
            ZDesces = desces;
            AccessAttribute = accAttr;
        }

        protected void Init()
        {
            //RetZType = ZTypeCache.GetBySharpType(SharpMethod.ReturnType) as ZType;
            IsStatic = SharpMethod.IsStatic;
            ZDesces = GetProcDesc(MarkMethod, SharpMethod);

            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpMethod);
        }

        //public WordInfo[] GetWordInfos()
        //{
        //    List<WordInfo> words = new List<WordInfo>();
        //    foreach (ZMethodDesc part in this.ZDesces)
        //    {
        //        WordInfo[] infos = part.GetWordInfos();
        //        words.AddRange(infos);
        //    }
        //    return words.ToArray();
        //}

        public bool ContainsWord(string text)
        {
            foreach (ZMethodDesc part in this.ZDesces)
            {
                 if(part.ContainsWord(text))
                 {
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

        public virtual bool HasZProcDesc(ZCallDesc procDesc)
        {
            foreach (ZMethodDesc item in ZDesces)
            {
                if (procDesc.Compare(item))
                    return true;
                //if (item.Eq(procDesc))
                //    return true;
            }
            return false;
        }

        protected ZMethodDesc[] GetProcDesc(MethodInfo markMethod, MethodInfo sharpMethod)
        {
            List<ZMethodDesc> list = new List<ZMethodDesc>();
            ZCodeAttribute[] attrs = AttributeUtil.GetAttributes<ZCodeAttribute>(markMethod);
            foreach (ZCodeAttribute attr in attrs)
            {
                //if (markMethod.Name == "SetTitle")
                //{
                //    Console.WriteLine("ZMethodInfo.SetTitle");
                //}
                ZCodeParser parser = new ZCodeParser(sharpMethod.DeclaringType,sharpMethod);
                ZMethodDesc typeProcDesc = parser.Parser(attr.Code);
                typeProcDesc.ZMethod = this;
                //ZMethodInfo exMethod = ZTypeUtil.CreatExMethodInfo(method, this.SharpType);
                //typeProcDesc.ExMethod = exMethod;
                list.Add(typeProcDesc);
            }
            return list.ToArray();
        }

        public override string ToString()
        {
            return this.MarkMethod.Name + "(" + string.Join(",", ZDesces.Select(p=>p.ToString())) + ")";
        }

        public string SharpMemberName
        {
            get { return this.MarkMethod.Name; }
        }

        //public ZType RetZType
        //{
        //    get
        //    {
        //        return ZTypeCache.GetByZType(Method.ReturnType);
        //    }
        //}

        //public override List<string> ZyyNames
        //{
        //    get
        //    {
        //        if (_ZyyNames == null)
        //        {
        //            _ZyyNames = GetZyyNames(this.Method);
        //        }
        //        return _ZyyNames;
        //    }
        //}
    }
}
