using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Reports;
using ZLangRT;

namespace ZCompiler
{
    public class CompileProgram
    {
        static void Main(string[] args)
        {
            CompileCmdModel cmdm = ParseArgs(args);
            if(cmdm==null)
            {
                Console.WriteLine("命令行参数不正确");
                return;
            }
            Compile(cmdm);
            if(cmdm.IsReadKey)
            {
                Console.ReadKey();
            }
        }

        static ProjectCompileResult Compile(CompileCmdModel model)
        {
            ProjectCompileResult result = null;
            if(model.IsCompileProject)
            {
                result = CompileFile(model.SrcFile);
            }
            else
            {
                result = CompileProject(model.SrcFile);
            }
            if(model.IsShowError)
            {
                if(result.HasError())
                {
                    ShowErrors(result);
                }
            }
            if(model.IsRun)
            {
                if (result.HasError()==false)
                {
                    Run(result);
                }
            }
            return result;
        }

        static CompileCmdModel ParseArgs(string[] args)
        {
            if(args.Length==1)
            {
                CompileCmdModel model = new CompileCmdModel();
                model.SrcFile = args[0];
                model.IsCompileProject = model.SrcFile.ToLower().EndsWith(Const.FileExt);
                model.IsRun = true;
                model.IsShowError = true;
                model.IsReadKey = true;
                return model;
            }
            else if(args.Length==5)
            {
                CompileCmdModel model = new CompileCmdModel();
                model.SrcFile = args[0];
                model.IsCompileProject =(args[1]=="1"|| args[1].ToLower()=="true");
                model.IsRun = (args[2] == "1" || args[1].ToLower() == "true");
                model.IsShowError = (args[3] == "1" || args[1].ToLower() == "true");
                model.IsReadKey = (args[4] == "1" || args[1].ToLower() == "true");
                return model;
            }
            else
            {
                return null;
            }
        }

        static ProjectCompileResult CompileFile(string srcFile)
        {
            //FileInfo srcFileInfo = new FileInfo(srcFile);
            FileCompiler compiler = new FileCompiler();
            ProjectCompileResult result = compiler.Compile(srcFile);
            return result;
        }

        static ProjectCompileResult CompileProject(string srcFile)
        {
            FileInfo srcFileInfo = new FileInfo(srcFile);
            ProjectCompiler compiler = new ProjectCompiler();
            ProjectCompileResult result = compiler.Compile(srcFileInfo);
            return result;
        }

        /*
        static void Main3(string[] args)
        {
            string srcFile ="";
            srcFile = "sample2c/test.zyy";
            //srcFile = "sample/对应表例子.zyy";
            //srcFile = "sample/你好.zyy";
            //srcFile = "sample/列表例子.zyy";
            //srcFile = "sample2c/sample1.zyy";
            //srcFile = "sample2c/sample2.zyy";
            srcFile = "sample2c/TestLanmbda1.zyy";

            if (args.Length > 0)
            {
                srcFile = args[0];
            }
            FileInfo srcFileInfo = new FileInfo(srcFile);
            //ZSingleCompiler compiler = new ZSingleCompiler(srcFileInfo);
            var compiler = new FileCompiler(srcFileInfo);
            compiler.Compile();
            if (compiler.CompileResult.HasError())
            {
                ShowErrors(compiler.CompileResult);
                Console.ReadKey();
            }
            else
            {
                compiler.Run();
            }
        }
        */
        public static void ShowErrors(ProjectCompileResult compileResult)
        {
            StringBuilder buffBuilder = new StringBuilder();
            //buffBuilder.AppendFormat("文件'{0}'有以下错误:\n", srcFile);
            foreach (CompileMessage compileMessage in compileResult.Errors.ValuesToList())
            {
                if (compileMessage.Line > 0 || compileMessage.Col > 0)
                {
                    buffBuilder.AppendFormat(" {2} 第{0}行,第{1}列", compileMessage.Line, compileMessage.Col, compileMessage.SourceFileInfo.ZFileName);
                }
                buffBuilder.AppendFormat("错误:{0}\n", compileMessage.Text);
            }
            Console.WriteLine(buffBuilder.ToString());
            Console.ReadKey();
        }
       
        public static void Run(ProjectCompileResult result)
        {
            Type type = result.EntrtyZType.SharpType;
            Invoker.Call(type, "启动");
        }

       
    }
}
