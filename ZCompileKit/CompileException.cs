using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileKit
{
    public class CompileException:Exception
    {
        public CompileException( )
            : base("编译错误")
        {

        }

        public CompileException(string message):base(message)
        {

        }
    }
}
