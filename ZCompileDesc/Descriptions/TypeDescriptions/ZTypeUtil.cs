using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Utils;

namespace ZCompileDesc.Descriptions
{
    public static class ZTypeUtil
    {
        internal static FieldInfo[] GetEnumItems(Type type)
        {
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields;
        }

        //internal static FieldInfo SearchFieldByZCode(string name, FieldInfo[] fields)
        //{
        //    foreach (var field in fields)
        //    {
        //        ZCodeAttribute zcodeAttr = AttributeUtil.GetAttribute<ZCodeAttribute>(field); // GetZCodeAttribute(field);//Attribute.GetCustomAttribute(property, typeof(ZCodeAttribute)) as ZCodeAttribute;
        //        if (zcodeAttr != null)
        //        {
        //            if (zcodeAttr.Code == name)
        //            {
        //                return field;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //internal static ZConstructorDesc SearchConstructor(ZConstructorDesc desc, Type ForType)
        //{
        //    ZConstructorDesc bracket2 = desc;
        //    ConstructorInfo[] constructorInfoArray = ForType.GetConstructors();
        //    foreach (ConstructorInfo ci in constructorInfoArray)
        //    {
        //        if (ci.IsPublic)
        //        {
        //            ZConstructorDesc bracketCi = ProcDescHelper.CreateZConstructorDesc(ci);
        //            if (bracketCi.Eq(bracket2))
        //            {
        //                return bracketCi;
        //            }
        //        }
        //    }
        //    return null;
        //}
        /*
        public static PropertyInfo SearchPropertyByZCode(string name, PropertyInfo[] propertyArray)
        {
            foreach (var property in propertyArray)
            {
                ZCodeAttribute zcodeAttr = GetZCodeAttribute(property);
                if (zcodeAttr != null)
                {
                    if (zcodeAttr.Code == name)
                    {
                        return property;
                    }
                }
            }
            return null;
        }*/

        //public static ZCodeAttribute GetZCodeAttribute(MemberInfo member)
        //{
        //    Attribute attr = Attribute.GetCustomAttribute(member, typeof(ZCodeAttribute));
        //    if(attr!=null)
        //    {
        //        ZCodeAttribute zcodeAttr = attr as ZCodeAttribute;
        //        return zcodeAttr;
        //    }
        //    else
        //    {
        //        return null;
        //    }
           
        //}

        //internal static ZMethodInfo CreatExMethodInfo(MethodInfo methodInfo, Type forType)
        //{
        //    if (methodInfo == null) return null;
        //    ZMethodInfo exMethodInfoInfo = new ZMethodInfo(methodInfo, methodInfo.DeclaringType == forType);
        //    return exMethodInfoInfo;
        //}

        //public static ExPropertyInfo CreatExPropertyInfo(PropertyInfo propertyInfo, Type forType)
        //{
        //    if (propertyInfo == null) return null;
        //    ExPropertyInfo exFieldInfo = new ExPropertyInfo(propertyInfo, propertyInfo.DeclaringType == forType);
        //    return exFieldInfo;
        //}

        //public static ExPropertyInfo CreatExPropertyInfo(PropertyInfo propertyInfo, Type forType)
        //{
        //    if (propertyInfo == null) return null;
        //    ExPropertyInfo exFieldInfo = new ExPropertyInfo(propertyInfo, propertyInfo.DeclaringType == forType);
        //    return exFieldInfo;
        //}
    }
}
