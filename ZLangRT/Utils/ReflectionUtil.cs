﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZLangRT.Utils
{
    public static class ReflectionUtil
    {
        public static AccessAttributeEnum GetAccessAttributeEnum(Type type)
        {
            if (type.IsPublic) return AccessAttributeEnum.Public;
            if (type.IsNotPublic) return AccessAttributeEnum.Private;
            return AccessAttributeEnum.Private;
        }

        public static AccessAttributeEnum GetAccessAttributeEnum(FieldInfo SharpField)
        {
            AccessAttributeEnum AccessAttribute = AccessAttributeEnum.Private;
            if (SharpField.IsPublic)
            {
                AccessAttribute = AccessAttributeEnum.Public;
            }
            else if (SharpField.IsPrivate)
            {
                AccessAttribute = AccessAttributeEnum.Private;
            }
            else if (SharpField.IsFamily)
            {
                AccessAttribute = AccessAttributeEnum.Internal;
            }
            else if (SharpField.IsFamilyOrAssembly)
            {
                AccessAttribute = AccessAttributeEnum.Protected;
            }
            else
            {
                AccessAttribute = AccessAttributeEnum.Private;
            }
            return AccessAttribute;
        }

        public static AccessAttributeEnum GetAccessAttributeEnum(PropertyInfo SharpProperty)
        {
            AccessAttributeEnum getAae = ReflectionUtil.GetAccessAttributeEnum(SharpProperty.GetGetMethod());
            AccessAttributeEnum setAae = ReflectionUtil.GetAccessAttributeEnum(SharpProperty.GetSetMethod());
            var AccessAttribute = AccessAttributeEnum.Private;
            if (getAae == AccessAttributeEnum.Public || setAae == AccessAttributeEnum.Public)
            {
                AccessAttribute = AccessAttributeEnum.Public;
            }
            else if (getAae == AccessAttributeEnum.Private && setAae == AccessAttributeEnum.Private)
            {
                AccessAttribute = AccessAttributeEnum.Private;
            }
            else if (getAae == AccessAttributeEnum.Internal && setAae == AccessAttributeEnum.Internal)
            {
                AccessAttribute = AccessAttributeEnum.Internal;
            }
            else if (getAae == AccessAttributeEnum.Protected && setAae == AccessAttributeEnum.Protected)
            {
                AccessAttribute = AccessAttributeEnum.Protected;
            }
            //else
            //{
            //    AccessAttribute = AccessAttributeEnum.Private;
            //}
            return AccessAttribute;
        }

        public static AccessAttributeEnum GetAccessAttributeEnum(MethodInfo method)
        {
            if (method == null) return AccessAttributeEnum.Private;
            if (method.IsPublic)
            {
                return AccessAttributeEnum.Public;
            }
            else if (method.IsPrivate)
            {
                return AccessAttributeEnum.Private;
            }
            else if (method.IsFamily)
            {
                return AccessAttributeEnum.Internal;
            }
            else if (method.IsFamilyOrAssembly)
            {
                return AccessAttributeEnum.Protected;
            }
            else
            {
                return AccessAttributeEnum.Private;
            }
        }

        static Type[] NumberTypes = new Type[] { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) };

        static int indexOfNumber(Type type)
        {
            for(int i=0;i<NumberTypes.Length;i++)
            {
                if (NumberTypes[i] == type)
                    return i;
            }
            return -1;
        }

        public static bool IsStruct(Type atype)
        {
            if (!atype.IsValueType) return false;
            if (atype == typeof(bool) || atype == typeof(byte) || atype == typeof(char) || atype == typeof(short) || atype == typeof(int) || atype == typeof(long)
                  || atype == typeof(float) || atype == typeof(double) || atype == typeof(decimal))
                return false;
            return true;
        }

        public static bool IsNumberType(Type type)
        {
            //return type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(long) || type == typeof(byte) || type == typeof(short);
            return indexOfNumber(type) != -1;
        }

        public static bool MoreEqNumberType(Type typea,Type typeb)
        {
            int indexa = indexOfNumber(typea);
            int indexb = indexOfNumber(typeb);
            return indexa >= indexb;
        }

        public static bool IsDeclare(Type type, FieldInfo property)
        {
            if (property.DeclaringType == type)
            {
                return true;
            }
            return false;
        }

        public static bool IsDeclare(Type type,PropertyInfo property)
        {
            if (property.DeclaringType == type)
             {
                 return true;
             }
             return false;
        }

        public static bool IsDeclare(Type type, MethodInfo method)
        {
            if (method.DeclaringType == type)
            {
                return true;
            }
            return false;
        }

        public static bool IsExtends(Type subType, Type baseType)
        {
            if (subType==null)
            {
                return true;
            }
            if (subType == baseType)
            {
                return true;
            }
            //Console.WriteLine(subType.FullName);
            //Console.WriteLine(baseType.FullName);
            if (subType.IsSubclassOf(baseType))
            {
                return true;
            }
            if (baseType.IsGenericType)
            {
                Type temp = GenericUtil.GetMakeGenericType(subType, baseType);
                if (temp.IsGenericType && temp.GetGenericTypeDefinition() == baseType)
                 {
                     return true;
                 }
            }
            else
            {
                if(baseType== typeof(object))
                {
                    return true;
                }
                else if (baseType == typeof(float) && subType == typeof(int))
                {
                    return true;
                }
                //if (subType.IsAssignableFrom(baseType))
                //{
                //    return true;
                //}
            }
            return false;
        }

        public static bool IsStatic(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        public static bool IsStatic(PropertyInfo property)
        {
            MethodInfo method = null;
            method = property.GetGetMethod();
            if (method != null && method.IsStatic) return true;
            method = property.GetSetMethod();
            if (method != null && method.IsStatic) return true;
            return false;
        }

        public static bool IsPublic(PropertyInfo property)
        {
            MethodInfo method = null;
            method = property.GetGetMethod();
            if (method != null && method.IsPublic) return true;
            method = property.GetSetMethod();
            if (method != null && method.IsPublic) return true;
            return false;
        }

        public static bool IsProtected(PropertyInfo property)
        {
            MethodInfo method = null;
            method = property.GetGetMethod();
            if (method != null && method.IsFamily) return true;
            method = property.GetSetMethod();
            if (method != null && method.IsFamily) return true;
            return false;
        }

        public static object NewInstance(Type type, params object[] args)
        {
            //List<object> argList = new List<object>();
            object obj = Activator.CreateInstance(type, args);
            if (obj == null)
            {
                throw new ZyyRTException("无法创建实例" + type.FullName);
            }
            return obj;
        }

        public static MethodInfo GetMethod(Type type, string methodName)
        {
            MethodInfo[] methods = type.GetMethods(
                BindingFlags.Public         //公共成员  
                | BindingFlags.Static        //为了获取返回值，必须指定 BindingFlags.Instance 或 BindingFlags.Static。  
                | BindingFlags.NonPublic     //非公共成员（即私有成员和受保护的成员）  
                | BindingFlags.Instance      //为了获取返回值，必须指定 BindingFlags.Instance 或 BindingFlags.Static。  
                //| BindingFlags.DeclaredOnly
                 );
            foreach (var method in methods)
            {
                if (method.Name == methodName)
                {
                    return method;
                }
            }
            return null;
        }

    }
}
