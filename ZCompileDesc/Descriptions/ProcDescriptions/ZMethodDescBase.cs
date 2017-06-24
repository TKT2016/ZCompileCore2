using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZMethodDescBase // : ZMemberInfo
    {
        public List<object> Parts { get; protected set; }
       
    }
}
