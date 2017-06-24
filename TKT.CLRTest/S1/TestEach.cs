using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;
using Z语言系统;

namespace TKT.CLRTest.S1
{
    public static class TestEach
    {
        public static void 启动()
        {
            列表<string> L = new 列表<string>();
            L.Add("AA");
            L.Add("BB");
            L.Add("CC");
            int i = 1;
            int count = L.Count;
            while (Calculater.LEInt(i,count))
            {
                Console.Write(L[i]);
                i++;
            }
            Console.ReadKey();
        }

      
    }
}
