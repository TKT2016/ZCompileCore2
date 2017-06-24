using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    internal static class ProcDescHelper
    {
        public static ZConstructorDesc CreateZConstructorDesc(ConstructorInfo ci)
        {
            List<ZMethodNormalArg> args = new List<ZMethodNormalArg>();
            foreach (ParameterInfo param in ci.GetParameters())
            {
                ZMethodNormalArg arg = new ZMethodNormalArg(param.Name, ZTypeManager.GetBySharpType(param.ParameterType) as ZType);
                args.Add(arg);
            }
            ZConstructorDesc desc = new ZConstructorDesc(args);
            desc.Constructor=ci;
            return desc;
        }
        /*
        public static ZProcDesc CreateProcDesc(ZMethodInfo exMethod)
        {
            var method = exMethod.Method;
            ZProcDesc desc = new ZProcDesc();
            desc.Add(method.Name);
            if(method.IsGenericMethod)
            {
                foreach (Type paramType in method.GetGenericArguments())
                {
                    ZMethodArg arg = new ZMethodArg(ZTypeCache.GetByZType(paramType));
                    desc.Add(arg);
                }
            }
            if (method.GetParameters().Length > 0)
            {
                List<ZMethodArg> args = new List<ZMethodArg>();
                foreach (ParameterInfo param in method.GetParameters())
                {
                    ZMethodArg arg = new ZMethodArg(param.Name, ZTypeCache.GetByZType(param.ParameterType));
                    args.Add(arg);
                }
                desc.Add(args.ToArray());
            }
            desc.ExMethod = exMethod;
            return desc;
        }*/

        //public static Tuple<string, ZType[]> CreateMethodDesc(ZProcDesc procDesc)
        //{
        //    if (procDesc.Parts.Count > 0 && procDesc.Parts[0] is string)
        //    {
        //        string methodName = procDesc.Parts[0] as string;
        //        if (methodName == null) return null;
        //        ZType[] types = procDesc.GetArgTypes().ToArray();
        //        return new Tuple<string, ZType[]>(methodName, types);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static MethodInfo SearchMethod(Type type,ZProcDesc procDesc)
        //{
        //    foreach(var method in type.GetMethods())
        //    {
        //        ZMethodInfo exMethod = new ZMethodInfo(method, method.DeclaringType == type);
        //        ZProcDesc methodDesc = CreateProcDesc(exMethod);
        //        if (methodDesc.Eq(procDesc))
        //            return method;
        //    }
        //    return null;
        //}
    }
}
