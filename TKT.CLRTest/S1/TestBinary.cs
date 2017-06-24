using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;
using Z语言系统;

namespace TKT.CLRTest.S1
{
   public static  class TestBinary
    {
       [STAThread]
       public static void 启动()
       {
           控制台.打印(Calculater.AddInt(100, 20));
       }
    }
}
