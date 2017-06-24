using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z桌面控件
{
    [ZStatic]
    public class 控件管理器 
    {
        [ZCode("初始化()")]
        public static void 初始化()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        [ZCode("启动(Form:form)")]
        public static void 启动(Form form)
        {
            //Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory); 
            Application.Run(form);
        }

    }
}
