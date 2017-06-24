using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Words;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZFieldInfo : ZMemberInfo
    {
        public FieldInfo MarkField { get; private set; }
        public FieldInfo SharpField { get; private set; }

        public ZFieldInfo(FieldInfo fieldInfo)
        {
            MarkField = fieldInfo;
            SharpField = fieldInfo;
            Init();
        }

        public ZFieldInfo(FieldInfo zfieldInfo, FieldInfo sfieldInfo)
        {
            MarkField = zfieldInfo;
            SharpField = sfieldInfo;
            Init();
        }

        private void Init()
        {
            //MemberZType = ZTypeCache.GetBySharpType(SharpField.FieldType);
            IsStatic = SharpField.IsStatic;
            ZNames = ZDescriptionHelper.GetZNames(MarkField);
            CanRead = true;
            CanWrite = !SharpField.IsInitOnly;
            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpField);
        }

        //public override WordInfo[] GetWordInfos()
        //{
        //    List<WordInfo> words = new List<WordInfo>();
        //    foreach (var zname in ZNames)
        //    {
        //        WordInfo info = new WordInfo(zname, WordKind.MemberName,this);
        //        words.Add(info);
        //    }
        //    return words.ToArray();
        //}

        public override ZType MemberZType
        {
            get { return ZTypeManager.GetBySharpType(SharpField.FieldType) as ZType; }
        }

        public override string SharpMemberName
        {
            get { return this.MarkField.Name; }
        }

        public override string ToString()
        {
            return this.MarkField.Name + "(" + string.Join(",", ZNames) + ")";
        }
    }
}
