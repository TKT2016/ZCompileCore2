using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Words;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZDimType : IZDescType
    {
        public Type MarkType { get; protected set; }
        public Type SharpType { get; private set; }

        public ZDimType(Type type)
        {
            MarkType = type;
            SharpType = type;
        }
        
        public string ZName
        {
            get
            {
                return SharpType.Name;
            }
        }
        Dictionary<string, string> _Dims;
        public Dictionary<string, string> Dims
        {
            get
            {
                if (_Dims == null)
                {
                    _Dims = new Dictionary<string, string>();
                    FieldInfo[] fields = this.SharpType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (FieldInfo fieldInfo in fields)
                    {
                        if (!fieldInfo.IsStatic) continue;
                        if (!ReflectionUtil.IsDeclare(SharpType, fieldInfo)) continue;
                        
                        string propertyValue = fieldInfo.GetValue(null) as string;
                        if(!string.IsNullOrEmpty(propertyValue))
                        {
                            _Dims.Add(fieldInfo.Name, propertyValue);
                        }
                    }
                }
                return _Dims;
            }
        }

        public override string ToString()
        {
            return this.SharpType.Name + "[" + Dims.Keys.Count + "]";
        }
    }
}
