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
    public class ZConstructorInfo //: ZMemberInfo
    {
        public ConstructorInfo Constructor { get; private set; }
        //public bool IsStatic { get; protected set; }

        public ZConstructorDesc ZDesc { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }
        //protected ProcDescCodeParser parser = new ProcDescCodeParser();

        public ZConstructorInfo(ConstructorInfo constructorInfo)
        {
            Constructor = constructorInfo;
            Init();
        }

        protected void Init()
        {
            //RetZType = ZTypeCache.GetBySharpType(SharpMethod.ReturnType) as ZType;
            //IsStatic = SharpMethod.IsStatic;
            //ZDesces = GetProcDesc(MarkMethod, SharpMethod);
            ZDesc = ProcDescHelper.CreateZConstructorDesc(Constructor);
            AccessAttribute = GetAccessAttributeEnum(Constructor);
        }

        public virtual bool HasZConstructorDesc(ZNewDesc procDesc)
        {
            return procDesc.Compare(ZDesc);
        }

        internal static AccessAttributeEnum GetAccessAttributeEnum(ConstructorInfo constructor)
        {
            if (constructor == null) return AccessAttributeEnum.Private;
            if (constructor.IsPublic)
            {
                return AccessAttributeEnum.Public;
            }
            else if (constructor.IsPrivate)
            {
                return AccessAttributeEnum.Private;
            }
            else if (constructor.IsFamily)
            {
                return AccessAttributeEnum.Internal;
            }
            else if (constructor.IsFamilyOrAssembly)
            {
                return AccessAttributeEnum.Protected;
            }
            else
            {
                return AccessAttributeEnum.Private;
            }
        }
    }
}
