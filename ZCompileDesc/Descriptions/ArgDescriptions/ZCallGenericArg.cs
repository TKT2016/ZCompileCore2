using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCallGenericArg : ZCallArg
    {
        public ZType BaseZType { get; protected set; }
        //public object Data { get; set; }

        public ZCallGenericArg(ZType argZType)
        {
            BaseZType = argZType;
        }

        public override bool Compare(ZMethodArg arg)
        {
            return false;
        }
    }
}
