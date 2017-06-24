using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class ContextImport
    {
        //public string PackageName { get; private set; }
        //public List<ZEnumType> ZEnumList { get; private set; }
        //public Dictionary<string, ZClassType> ImportNormalZTypes { get; private set; }
        //public Dictionary<string, ZClassType> ImportGenericZTypes { get; private set; }
        //public Dictionary<string, ZDimType> ImportDimTypes { get; private set; }
        //public Dictionary<string, ZEnumType> ImportEnumDimTypes { get; private set; }
        //public List<ZDimType> ImportDims { get; private set; }
        public WordDictionary DictName { get; private set; }
        //public WordDictionary DictMember { get; private set; }
        public ZPackageDescList ImportPackageDescList { get; protected set; }

        public ContextImport()
        {
            //ImportNormalZTypes = new Dictionary<string, ZClassType>();
            //ImportGenericZTypes = new Dictionary<string, ZClassType>();
            //ImportDimTypes = new Dictionary<string, ZDimType>();
            //ImportDims = new List<ZDimType>();
            DictName = new WordDictionary("导入类表");
            //DictMember = new WordDictionary("导入成员表");
            //ZEnumList = new List<ZEnumType>();

            ImportPackageDescList = new ZPackageDescList();
        }

        //public void AddZClassType(ZClassType ztype)
        //{
        //    if (ztype.SharpType.IsGenericType)
        //    {
        //        ImportGenericZTypes.Add(ztype.ZName, ztype);
        //    }
        //    else
        //    {
        //        ImportNormalZTypes.Add(ztype.ZName, ztype);
        //    }
        //}

        //public void AddZEnum(ZEnumType zenum)
        //{
        //    ZEnumList.Add(zenum);
        //}

        //public void AddDim(ZDimType dimType)
        //{
        //    //ImportDimTypes.Add(dimType.ZName, dimType);
        //    ImportDims.Add(dimType);
        //}

        public void AddNameWord(WordInfo word)
        {
            DictName.Add(word);
        }

        //public void AddMemberWord(WordInfo word)
        //{
        //    DictMember.Add(word);
        //}

        //public IZDescType[] SearchZDescType(string zname)
        //{
        //    List<IZDescType> list = new List<IZDescType>();
        //    if (ImportNormalZTypes.ContainsKey(zname))
        //    {
        //        list.Add(ImportNormalZTypes[zname]);
        //    }
        //    if (ImportGenericZTypes.ContainsKey(zname))
        //    {
        //        list.Add(ImportGenericZTypes[zname]);
        //    }
        //    return list.ToArray();
        //}
    }
}
