using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;

namespace ZCompileDesc.Utils
{
    public class ZCodeParser
    {

        public ZCodeParser(Type type, MethodInfo method)
        {
            argTypeDict = new Dictionary<string, Type>();
             Init( type,  method);
        }

        #region 初始化类型参数
        private Dictionary<string, Type> argTypeDict;
        private void Init(Type type, MethodInfo method)
        {
            if (type.IsGenericType)
            {
                Type parentType = type.GetGenericTypeDefinition();
                Type[] subTypes = GenericUtil.GetInstanceGenriceType(type, parentType);
                Type[] gengeParams = parentType.GetGenericArguments();
                for (int i = 0; i < gengeParams.Length; i++)
                {
                    AddType(gengeParams[i].Name, subTypes[i]);
                }
            }
            ParameterInfo[] paramArray = method.GetParameters();
            foreach (var param in paramArray)
            {
                if (!param.ParameterType.IsGenericType)
                    AddType(param.ParameterType);
            }

            foreach (var param in paramArray)
            {
                if (param.ParameterType.IsGenericType)
                {
                    var ptype = param.ParameterType;
                    Type parentType = ptype.GetGenericTypeDefinition();
                    Type[] subTypes = GenericUtil.GetInstanceGenriceType(ptype, parentType);
                    Type[] gengeParams = parentType.GetGenericArguments();
                    //Type[] arr2 = new Type[gengeParams.Length];
                    string[] subNames = new string[gengeParams.Length];
                    for (int i = 0; i < gengeParams.Length; i++)
                    {
                        //subNames[i] = gengeParams[i].Name;
                        //AddType(gengeParams[i].Name, subTypes[i]);
                        string gengeParamName = gengeParams[i].Name;
                        subNames[i] = gengeParamName;
                        //arr2[i] = ArgTypeDict[gengeParamName];
                    }
                    Type newType = ptype;// ptype.MakeGenericType(arr2);
                    string newTypeName = GenericUtil.GetGenericTypeShortName(ptype) + "<" + string.Join(",", subNames) + ">";
                    AddType(newTypeName, newType);
                }
            }
        }

        private void AddType(Type type)
        {
            string name = type.Name;
            if (argTypeDict.ContainsKey(name) == false)
                argTypeDict.Add(name, type);
        }

        private void AddType(string name, Type type)
        {
            if (argTypeDict.ContainsKey(name) == false)
                argTypeDict.Add(name, type);
        }
       #endregion

        int i = 0;
        ZMethodDesc desc = null;
        string Code = null;
        char ch
        {
            get
            {
                if (i > Code.Length - 1) return '\0';
                return Code[i];
            }
        }

        public ZMethodDesc Parser(string code)
        {
            i = 0;
            desc = new ZMethodDesc();
            Code = code;
            while (i < Code.Length)
            {
                if(ch=='(')
                {
                    parseBracket();
                }
                else
                {
                    parseText();
                }
            }
            return desc;
        }

        void parseText()
        {
            StringBuilder buff = new StringBuilder();
            for (;  i < Code.Length&&ch != '(' ; i++)
            {
                buff.Append(ch);
            }
            desc.Add(buff.ToString());
        }

        void parseBracket()
        {
            i++;
            List<ZMethodArg> bracketargs = new List<ZMethodArg>();
            //Dictionary<string, TKTProcArg> dict = new Dictionary<string, TKTProcArg>();
            for (; i < Code.Length; i++ )
            {
                ZMethodArg arg = parseArg();
                if (arg != null)
                {
                    bracketargs.Add(arg);
                }
                //dict.Add(arg.ArgName, arg);
                if(ch=='\0')
                {
                    break;
                }
                if (ch == ')')
                {
                    i++;
                    break;
                } 
            }
            //TKTProcBracket bracket = new TKTProcBracket(bracketargs,dict);
            desc.Add(bracketargs.ToArray());
        }

        private ZMethodArg parseArg()
        {
            string argTypeName = parseIdent();
            movenext();
            string argname = parseIdent();
            //string realArgName = argname;
            if (string.IsNullOrEmpty(argname)) return null;
            //ZMethodArg arg = null;
            if (argTypeDict.ContainsKey(argTypeName))
            {
                ZMethodGenericArg genericArg = new ZMethodGenericArg(argTypeName, ZTypeManager.ZOBJECT);
                return genericArg;
            }
            else
            {
                var ztype = GetZTypeByTypeName(argTypeName);
                if (ztype != null)
                {
                    ZMethodNormalArg zarg = new ZMethodNormalArg(argname, ztype);
                    return zarg;
                }
            }
            throw new ZyyRTException("没有导入'" + argTypeName + "'类型");
            //return arg;
        }

        private ZType GetZTypeByTypeName(string typeName)
        {
            //if (typeName == "Func<bool>")
            //{
            //    arg = new ZMethodArg(typeName, ZTypeCache.GetByZType(typeof(Func<bool>)));
            //}
            //else if (argtypename == "Action")
            //{
            //    arg = new ZMethodArg(realArgName, ZTypeCache.GetByZType(typeof(Action)));
            //}
            //else
            //{
          
                var ztypes = ZTypeManager.GetByMarkName(typeName);
                if (ztypes.Length == 0)
                {
                    ztypes = ZTypeManager.GetBySharpName(typeName);
                }
                if (ztypes.Length > 0)
                {
                    ZType ztype = ztypes[0] as ZType;
                    //arg = new ZMethodArg(typeName, ztype);
                    return ztype;
                }
                else
                {

                }
                return null;
            
        }

        string parseIdent()
        {
            StringBuilder buff = new StringBuilder();
            for (; i < Code.Length; i++)
            {
                if (ch == ':'||ch == ')'||ch == ',')
                {
                    break;
                }
                else
                {
                    buff.Append(ch);
                }
            }
            return buff.ToString();
        }

        void movenext()
        {
            i++;
        }
    }
}
