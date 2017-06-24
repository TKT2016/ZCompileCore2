using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZMethodGenericArg : ZMethodArg
    {
        public ZType ArgBaseZType { get; protected set; }
        //public string ArgName { get; protected set; }

        public ZMethodGenericArg(string argName, ZType argBaseType)
        {
            ArgName = argName;
            ArgBaseZType = argBaseType;
        }

        public override string ToZCode()
        {
            if(ArgBaseZType.SharpType!= typeof(object))
                return string.Format("类型:{0}", ArgBaseZType.ZName);
            else
                return string.Format("{0}类型:{1}", ArgBaseZType.ZName, ArgName);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", ArgBaseZType.ZName, ArgName);
        }
    }
}
