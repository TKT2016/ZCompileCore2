using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;

namespace ZCompileCore.AST
{
    public class PackageNameAST : UnitBase
    {
        public List<Token> Tokens { get; protected set; }
        public string PackageFullName { get; protected set; }
        public ZPackageDesc ZPackage { get; protected set; }

        public PackageNameAST()
        {
            Tokens = new List<Token>();
        }

        public void Add(Token token)
        {
            Tokens.Add(token);
        }

        //public void Analy(ContextFile fileContext, List<ZType> addedZtypes)
        //{
        //    PackageFullName = string.Join("/", Tokens.Select(p => p.GetText()));
        //    LoadPackage(context, context.ProjectContext.RefAssemblys, addedZtypes);
        //}

        public void Analy(ContextFile fileContext)
        {
            this.FileContext = fileContext;
            PackageFullName = string.Join("/", Tokens.Select(p => p.GetText()));
            LoadPackage(fileContext.ImportContext.ImportPackageDescList , fileContext.ProjectContext.AssemblyDescDictionary);
        }

        private bool LoadPackage(ZPackageDescList addTo, Dictionary<Assembly, ZAssemblyDesc> addFrom)
        {
            if(addTo.Contains(this.PackageFullName))
            {
                ErrorE(this.Position, "开发包'{0}'已经导入", PackageFullName);
                return false;
            }
            ZPackageDesc packageDesc = SearchZPackageDesc(this.PackageFullName, addFrom);
            if (packageDesc == null)
            {
                ErrorE(this.Position, "不存在'{0}'开发包", PackageFullName);
                return false;
            }
            else
            {
                addTo.Add(packageDesc);
            }
            return true;
        }

        private ZPackageDesc SearchZPackageDesc(string packageName, Dictionary<Assembly, ZAssemblyDesc> dict)
        {
            foreach (ZAssemblyDesc assemblyDesc in dict.Values)
            {
                ZPackageDesc packageDesc = assemblyDesc.SearhcZPackageDesc(packageName);
                if (packageDesc != null)
                    return packageDesc;
            }
            return null;
        }

        /*
        private void LoadPackage(ContextFile context, List<Assembly> asmlist, List<ZType> addedZtypes)
        {
            int addCount = 0;
            //PackageContext packageContext = new PackageContext();
            ContextImport importContext2 = context.ImportContext;
            foreach (Assembly asm in asmlist)
            {
                var refTypes = asm.GetTypes();
                foreach (var type in refTypes)
                {
                    if (type.IsPublic && type.Namespace == PackageFullName)
                    {
                        IZDescType gcl = ZTypeCache.CreateZDescType(type);
                        if (gcl != null)
                        {
                            addCount++;
                            AddIZDescType(importContext2,gcl, addedZtypes);
                        }
                    }
                }
            }

            if (addCount == 0)
            {
                errorf(this.Position, "开发包'{0}'内没有类型", PackageFullName);
            }
        }

        private void AddIZDescType(ContextImport importContext2 ,IZDescType gcl, List<ZType> addedZtypes)
        {
            if (gcl is ZDimType)
            {
                importContext2.AddDim(gcl as ZDimType);
            }
            else if (gcl is ZEnumType)
            {
                importContext2.AddZEnum(gcl as ZEnumType);
            }
            else if (gcl is ZClassType)
            {
                ZClassType zct = gcl as ZClassType;
                //if(zct.ZName=="异常")
                //{
                //    Console.WriteLine("SectionImport.PackageName " + zct.ZName);
                //}
                importContext2.AddZClassType(zct);
                BuildZClassWord(importContext2, zct, addedZtypes);
            }
        }

        //Dictionary<ZType, ZType> addedZtypes = new Dictionary<ZType, ZType>();
        
        private void BuildZClassWord(ContextImport importContext2, ZClassType ztype,List<ZType> addedZtypes)
        {
            ZClassType temp = ztype;
            while(temp!=null)
            {
                if (addedZtypes.IndexOf(temp)!=-1) return;
                BuildZClassWord(importContext2, temp);
                addedZtypes.Add(temp);
                temp = temp.BaseZType;
            }
        }

        private void BuildZClassWord(ContextImport importContext2, ZClassType zclass)
        {
            WordKind nameKind = (zclass.SharpType.IsGenericType) ? WordKind.GenericClassName : WordKind.TypeName;
            WordInfo info = new WordInfo(zclass.ZName, nameKind);
            importContext2.AddNameWord(info);

            ZMemberInfo[] zmembers = zclass.ZMembers;
            //ZClassType zclass = ztype as ZClassType;
            //ZPropertyInfo[] piarr = zclass.GetDeclareZProperties();
            foreach (var item in zmembers)
            {
                if(item.AccessAttribute== AccessAttributeEnum.Public)
                {
                    string[] znames = item.ZNames;
                    foreach(var zname in znames)
                    {
                        WordInfo info2 = new WordInfo(zname, WordKind.MemberName);
                        importContext2.AddMemberWord(info2);
                    }
                }
            }

            ZMethodInfo[] zmethods = zclass.ZMethods;
            foreach (var item in zmethods)
            {
                if (item.AccessAttribute == AccessAttributeEnum.Public)
                {
                    ZMethodDesc[] zproces = item.ZDesces;
                    foreach (ZMethodDesc zdesc in zproces)
                    {
                        //WordInfo info2 = new WordInfo(zname, WordKind.PropertyName);
                        //importContext2.AddMemberWord(info2);
                        BuildProcWord(importContext2, zdesc);
                    }
                }
            }
        }

        private void BuildProcWord(ContextImport importContext2, ZMethodDesc proc)
        {
            foreach (object part in proc.Parts)
            {
                if (part is string)
                {
                    WordInfo info2 = new WordInfo(part as string, WordKind.ProcNamePart);
                    importContext2.AddMemberWord(info2);
                }
            }
        }
         * */

        public virtual CodePosition Position
        {
            get
            {
                return this.Tokens[0].Position;
            }
        }

        public override string ToString()
        {
            return string.Join("/", Tokens.Select(p => p.GetText()));
        }
    }
}

