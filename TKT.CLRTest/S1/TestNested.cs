using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z语言系统;

namespace TKT.CLRTest.S1
{
    public static class TestNested
    {
        public static void 启动()
        {
            Nested1 n1 = new Nested1();
            补语控制.执行_次(() => n1.CALL(), 3);
        }

        class Nested1
        {
            public void CALL()
            {
                控制台.打印("你好");
            }
        }
    }
}
