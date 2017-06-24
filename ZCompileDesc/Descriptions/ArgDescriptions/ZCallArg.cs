using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZCallArg : ZArgBase
    {
        public object Data { get; set; }
        public abstract bool Compare(ZMethodArg arg);

        //public ZType ValueZType { get; protected set; }

        //public ZCallArg(ZType argZType)
        //{
        //    ValueZType = argZType;
        //}
        

        //public bool Compare(ZMethodArg arg)
        //{
        //    if (arg is ZGenericArg) return false;
        //    else if (arg is ZNormalArg)
        //    {
        //        ZNormalArg znarg = (arg as ZNormalArg);
        //        if (znarg.ArgZType.SharpType == ZLambda.ActionType) return true;
        //        else if (znarg.ArgZType.SharpType == ZLambda.CondtionType) return ValueZType == ZTypeCache.ZBOOL;
        //        else
        //        {
        //            return ReflectionUtil.IsExtends(ValueZType.SharpType, znarg.ArgZType.SharpType);
        //        }
        //    }
        //    else
        //    {
        //        throw new ZyyRTException();
        //    }
        //}
    }
}
