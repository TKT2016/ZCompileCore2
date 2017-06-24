using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLangRT.Attributes
{
    public class ZInstanceAttribute :Attribute// ZClassAttribute, IZTag
    {
        //public bool IsStaticClass { get; private set; }
        public Type SharpType { get; private set; }

        public ZInstanceAttribute( )
        {
            //IsStaticClass = false;
        }

        public ZInstanceAttribute(Type forType)
        {
            //IsStaticClass = false;
            SharpType = forType;
        }
    }
}
