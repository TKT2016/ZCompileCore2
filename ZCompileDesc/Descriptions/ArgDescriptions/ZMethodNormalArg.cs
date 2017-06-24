using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZMethodNormalArg : ZMethodArg
    {
        public ZType ArgZType { get; protected set; }
       // public string ArgName { get; protected set; }

        public ZMethodNormalArg(string argName, ZType argType)
        {
            ArgZType = argType;
            ArgName = argName;
        }

        public override string ToZCode()
        {
            return string.Format("{0}:{1}", ArgZType.ZName, ArgName);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", ArgZType.ZName, ArgName);
        }
    }
}
