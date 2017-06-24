using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZMethodArg : ZArgBase
    {
        public string ArgName { get; protected set; }

        public abstract string ToZCode();
    }
}
