using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCallValueArg : ZCallArg
    {
        public ZType ValueZType { get; protected set; }
        //public object Data { get; set; }

        public ZCallValueArg(ZType argZType)
        {
            ValueZType = argZType;
        }

        public override bool Compare(ZMethodArg arg)
        {
            if (arg is ZMethodGenericArg) return false;
            else if (arg is ZMethodNormalArg)
            {
                ZMethodNormalArg znarg = (arg as ZMethodNormalArg);
                if (znarg.ArgZType.SharpType == ZLambda.ActionType) return true;
                else if (znarg.ArgZType.SharpType == ZLambda.CondtionType) return ValueZType == ZTypeManager.ZBOOL;
                else
                {
                    return ReflectionUtil.IsExtends(ValueZType.SharpType, znarg.ArgZType.SharpType);
                }
            }
            else
            {
                throw new ZyyRTException();
            }
        }
    }
}
