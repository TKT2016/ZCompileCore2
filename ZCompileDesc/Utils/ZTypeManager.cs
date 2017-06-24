using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using Z语言系统;

namespace ZCompileDesc.Descriptions
{
    public static class ZTypeManager
    {
        //static Dictionary<Type, IZDescType> sharpCache = new Dictionary<Type, IZDescType>();
        //static Dictionary<Type, IZDescType> ztypeCache = new Dictionary<Type, IZDescType>();
        #region 核心类型
        static ZTypeCache Cache = new ZTypeCache();
        public static ZClassType ZVOID { get; private set; }
        public static ZClassType ZOBJECT { get; private set; }
        public static ZClassType ZINT { get; private set; }
        public static ZClassType ZFLOAT { get; private set; }
        public static ZClassType ZLIST { get; private set; }
        public static ZClassType ZBOOL { get; private set; }
        public static ZClassType ZSTRING { get; private set; }

        public static ZClassType ZACTION { get; private set; }
        public static ZClassType ZCONDITION { get; private set; }
        public static ZClassType ZDATETIME { get; private set; }
        #endregion

        static ZTypeManager()
        {
            ZOBJECT = CreateZTypeImp(typeof(事物)) as ZClassType;
            Cache.AddCache(ZOBJECT);
            Cache.AddCache(ZOBJECT,"object");

            ZVOID = CreateZTypeImp(typeof(VOID)) as ZClassType;
            Cache.AddCache(ZVOID);
            Cache.AddCache(ZVOID,"void");

            ZBOOL = CreateZTypeImp(typeof(判断符)) as ZClassType;
            Cache.AddCache(ZBOOL);
            Cache.AddCache(ZBOOL,"bool");

            ZINT = CreateZTypeImp(typeof(整数)) as ZClassType;
            Cache.AddCache(ZINT);
            Cache.AddCache(ZINT,"int");

            ZFLOAT = CreateZTypeImp(typeof(浮点数)) as ZClassType;
            Cache.AddCache(ZFLOAT);
            Cache.AddCache(ZFLOAT,"float");

            ZSTRING = CreateZTypeImp(typeof(文本)) as ZClassType;
            Cache.AddCache(ZSTRING);
            Cache.AddCache(ZSTRING,"string");

            ZACTION = CreateZTypeImp(typeof(可运行语句)) as ZClassType;
            Cache.AddCache(ZACTION);

            ZCONDITION = CreateZTypeImp(typeof(可运行条件)) as ZClassType;
            Cache.AddCache(ZCONDITION);

            ZDATETIME = CreateZTypeImp(typeof(时间日期)) as ZClassType;
            Cache.AddCache(ZDATETIME);

            ZLIST = CreateZTypeImp(typeof(列表<>)) as ZClassType;
            Cache.AddCache(ZLIST);
        }
      
        public static ZType RegNewGenericType(Type newType)
        {
            ZType ztype = CreateZTypeImp(newType) as ZType;
            return ztype;
        }

        public static IZDescType[] GetBySharpName(string sharpName)
        {
            if (Cache.SNameCache.ContainsKey(sharpName)) return new IZDescType[] { Cache.SNameCache[sharpName] };
            else return new IZDescType[] { };
            //if (sharpName == "void") return new ZType[]{ ZVOID};
            //else if (sharpName == "object") return new ZType[] { ZOBJECT };
            //else if (sharpName == "int") return new ZType[] { ZINT };
            //else if (sharpName == "float") return new ZType[] { ZFLOAT };
            //else if (sharpName == "bool") return new ZType[] { ZBOOL };
            //else if (sharpName == "string") return new ZType[] { ZSTRING };

            //List<ZType> list = new List<ZType>();
            //foreach (var item in sharpCache.Values)
            //{
            //    if (item.SharpType.Name == sharpName)
            //    {
            //        list.Add(item as ZType);
            //    }
            //}
            //return list.ToArray();
        }

        public static IZDescType[] GetByMarkName(string zname)
        {
            if (Cache.ZNameCache.ContainsKey(zname)) return new IZDescType[] { Cache.ZNameCache[zname] };
            else return new IZDescType[] { };
            //if (zname == "VOID") return new ZType[] { ZVOID };
            //else if (zname == "事物") return new ZType[] { ZOBJECT };
            //else if (zname == "整数") return new ZType[] { ZINT };
            //else if (zname == "浮点数") return new ZType[] { ZFLOAT };
            //else if (zname == "判断符") return new ZType[] { ZBOOL };
            //else if (zname == "文本") return new ZType[] { ZSTRING };

            //List<ZType> list = new List<ZType>();
            //foreach(var item in sharpCache.Values)
            //{
            //    if(item.ZName==zname)
            //    {
            //        list.Add(item as ZType);
            //    }
            //}
            //return list.ToArray();
        }

        public static IZDescType GetBySharpType(Type type)
        {
            if (type == null) return null;
            if (Cache.SharpCache.ContainsKey(type)) return Cache.SharpCache[type];
            else return null;

            //if (sharpCache.ContainsKey(type))
            //{
            //    return sharpCache[type] as ZClassType;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public static IZDescType GetByMarkType(Type type)
        {
            if (type == null) return null;
            if (Cache.MarkCache.ContainsKey(type)) return Cache.MarkCache[type];
            IZDescType descType = CreateZTypeImp(type);
            if(descType!=null)
            {
                Cache.AddCache(descType);
            }
            return descType;
            //var zdesctype =  CreateZDescType(type);
            //if (zdesctype == null) return null;
            //return zdesctype as ZType;
        }

        //public static IZDescType CreateZDescType(Type type)
        //{
        //    if (ztypeCache.ContainsKey(type))
        //    {
        //        return ztypeCache[type];
        //    }
        //    else
        //    {
        //        IZDescType ztype = CreateZTypeImp(type);
        //        if (ztype != null)
        //        {
        //            if (!sharpCache.ContainsKey(ztype.SharpType))
        //                sharpCache.Add(ztype.SharpType, ztype);
        //            if (!ztypeCache.ContainsKey(type))
        //                ztypeCache.Add(type, ztype);
        //        }
        //        return ztype;
        //    }
        //}

        private static IZDescType CreateZTypeImp(Type type)
        {
            if(AttributeUtil.HasAttribute<ZDimAttribute>(type))
            {
                ZDimType zdim = new ZDimType(type);
                return zdim;
            }
            else if (AttributeUtil.HasAttribute<ZEnumAttribute>(type))
            {
                ZEnumType zenum = new ZEnumType(type);
                return zenum;
            }
            else if (AttributeUtil.HasAttribute<ZStaticAttribute>(type))
            {
                ZStaticAttribute zAttr = AttributeUtil.GetAttribute<ZStaticAttribute>(type);
                Type sharpType = (zAttr.SharpType == null ? type : zAttr.SharpType);
                ZClassType zclass = new ZClassType(type, sharpType, true);
                return zclass;
            }
            else if (AttributeUtil.HasAttribute<ZInstanceAttribute>(type))
            {
                ZInstanceAttribute zAttr = AttributeUtil.GetAttribute<ZInstanceAttribute>(type);
                Type sharpType = (zAttr.SharpType == null ? type : zAttr.SharpType);
                ZClassType zclass = new ZClassType(type, sharpType, false);
                return zclass;
            }
            return null;
        }

        private class ZTypeCache
        {
            public Dictionary<Type, IZDescType> MarkCache = new Dictionary<Type, IZDescType>();
            public Dictionary<Type, IZDescType> SharpCache = new Dictionary<Type, IZDescType>();
            public Dictionary<string, IZDescType> ZNameCache = new Dictionary<string, IZDescType>();
            public Dictionary<string, IZDescType> SNameCache = new Dictionary<string, IZDescType>();

            public void AddCache(IZDescType descType)
            {
                try
                {
                    if (!MarkCache.ContainsKey(descType.MarkType))
                        MarkCache.Add(descType.MarkType, descType);
                    if (!SharpCache.ContainsKey(descType.SharpType))
                        SharpCache.Add(descType.SharpType, descType);
                    if (!ZNameCache.ContainsKey(descType.ZName))
                        ZNameCache.Add(descType.ZName, descType);
                    if (!SNameCache.ContainsKey(descType.SharpType.Name))
                        SNameCache.Add(descType.SharpType.Name, descType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ZTypeManager.Cache.AddCache(IZDescType):" + descType +":"+ ex.Message);
                }
            }

            public void AddCache(IZDescType descType,string sname)
            {
                try
                {
                    if (!MarkCache.ContainsKey(descType.MarkType))
                        MarkCache.Add(descType.MarkType, descType);
                    if (!SharpCache.ContainsKey(descType.SharpType))
                        SharpCache.Add(descType.SharpType, descType);
                    if (!ZNameCache.ContainsKey(descType.ZName))
                        ZNameCache.Add(descType.ZName, descType);
                    if (!SNameCache.ContainsKey(sname))
                        SNameCache.Add(sname, descType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ZTypeManager.Cache.AddCache(IZDescType):" + sname + ":" + ex.Message);
                }
            }

            public bool Contains(Type type)
            {
                return MarkCache.ContainsKey(type) || SharpCache.ContainsKey(type);
            }

            public bool Contains(string typeName)
            {
                return ZNameCache.ContainsKey(typeName) || SNameCache.ContainsKey(typeName);
            }

            public IZDescType Get(Type type)
            {
                if (MarkCache.ContainsKey(type))
                    return MarkCache[type];
                else if (SharpCache.ContainsKey(type))
                    return SharpCache[type];
                else
                    return null;
            }

            public IZDescType Get(string typeName)
            {
                if (ZNameCache.ContainsKey(typeName))
                    return ZNameCache[typeName];
                else if (SNameCache.ContainsKey(typeName))
                    return SNameCache[typeName];
                else
                    return null;
            }

            public void Clear()
            {
                MarkCache.Clear();
                SharpCache.Clear();
                ZNameCache.Clear();
                SNameCache.Clear();
            }


        }

       
    }
}
