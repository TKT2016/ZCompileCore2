using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ZCompileCore;
using ZCompileCore.Engines;
using ZCompileCore.Reports;
using ZCompileKit;
using ZCompileKit.Infoes;

namespace ZCompiler
{
    public class ProjectCompiler
    {
        //string projectFilePath;
        public ProjectCompileResult Compile(FileInfo projectFileInfo)
        {
            //Messager.Clear();
            //this.projectFilePath = projectFilePath;
            ZProjectModel projectModel = ReadModel(projectFileInfo);
            ZProjectEngine builder2 = new ZProjectEngine();
            ProjectCompileResult result = builder2.Compile(projectModel);
            return result;
        }

        private ZProjectModel ReadModel(FileInfo projectFileInfo)//(string projectFilePath)
        {
            //FileInfo projFileInfo = new FileInfo(projectFilePath);
            string[] lines = File.ReadAllLines(projectFileInfo.FullName);
            ZProjectModel projectModel = ParseProjectFile(lines, projectFileInfo.Directory.FullName);
            //classModel = new ZyyFileModel();
            //projectModel.ProjectRootDirectoryInfo = srcFileInfo.Directory;
            //projectModel.BinaryFileKind = PEFileKinds.ConsoleApplication;
            //projectModel.BinarySaveDirectoryInfo = srcFileInfo.Directory;
            //projectModel.ProjectPackageName = "ZLangSingleFile";
            //projectModel.EntryClassName = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            //projectModel.BinaryFileNameNoEx = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            //projectModel.NeedSave = true;
            //projectModel.AddRefPackage("Z语言系统");
            //projectModel.AddRefPackage("Z操作系统", "Z文件系统", "Z互联网", "Z绘图", "Z桌面控件");
            //classModel.SourceFileInfo = srcFileInfo;
            //projectModel.AddClass(classModel);

            return projectModel;
        }

        //public ProjectCompileResult CompileProject(string projfile)
        //{
        //    ProjectCompileResult result = new ProjectCompileResult();
        //    string[] lines = File.ReadAllLines(projfile);
        //    FileInfo projectFileInfo = new FileInfo(projfile);
        //    string folder = projectFileInfo.Directory.FullName;
        //    string binPath = Compile(lines, folder, result, projfile);
        //    result.BinFilePath = binPath;
        //    return result;
        //}

        public ZProjectModel ParseProjectFile(string[] lines, string folderPath)
        {
            //ProjectContext context = new ProjectContext();
            ZProjectModel projectModel = new ZProjectModel();
            projectModel.NeedSave = true;

            projectModel.AddRefPackage("Z语言系统");
            //context.AddPackage("TKT系统");
            //context.AddPackage("TKT绘图");
            //context.AddPackage("TKT桌面控件");
            //context.AddPackage("TKT文件系统");
            //context.AddPackage("TKT互联网");
            //context.AddPackage("TKT操作系统");
            //context.AddPackage("TKT多媒体");
           
            //projectModel.Analy();

            for (int i = 0; i < lines.Length; i++)
            {
                string code = lines[i];
                if (string.IsNullOrEmpty(code))
                {
                    continue;
                }
                else if (code.StartsWith("//"))
                {
                    continue;
                }
                else if (code.StartsWith("包名称:"))
                {
                    string name = code.Substring(4);
                    projectModel.ProjectPackageName = name;
                    projectModel.BinaryFileNameNoEx = name;
                }
                else if (code.StartsWith("生成类型:"))
                {
                    string lx = code.Substring(5);
                    PEFileKinds fileKind = PEFileKinds.ConsoleApplication;
                    if (lx == "开发包")
                    {
                        fileKind = PEFileKinds.Dll;
                    }
                    else if (lx == "控制台程序")
                    {
                        fileKind = PEFileKinds.ConsoleApplication;
                    }
                    else if (lx == "桌面程序")
                    {
                        fileKind = PEFileKinds.WindowApplication;
                    }
                    projectModel.BinaryFileKind = fileKind;
                    //GenerateProject(projectModel);
                }
                else if (code.StartsWith("编译:"))
                {
                    string src = code.Substring(3);
                    string srcPath = Path.Combine(folderPath, src);
                    //var srcFileInfo = new FileInfo(srcPath);

                    ZFileModel classModel = new ZFileModel(new ZCompileFileInfo(false, srcPath, null, null));// new ZFileModel();
                    //classModel.SourceFileInfo = srcFileInfo;
                    projectModel.AddClass(classModel);
                }
                else if (code.StartsWith("设置启动:"))
                {
                    string name = code.Substring(5);
                    projectModel.EntryClassName = name;

                    //Type type = projectModel.GetProjectType(name);
                    //if (type == null)
                    //{
                    //    errorf(result, file, i + 1, 6, "类型'{0}'不存在", name);
                    //    continue;
                    //}
                    //MethodInfo main = type.GetMethod("启动");
                    //if (main == null)
                    //{
                    //    errorf(result, file, i + 1, 6, "类型'{0}'不存在'启动'过程", name);
                    //    continue;
                    //}
                    //if (!main.IsStatic)
                    //{
                    //    errorf(result, file, i + 1, 6, "'{0}'不是唯一类型，不能作为启动入口", name);
                    //    continue;
                    //}
                    //projectModel.EmitContext.AssemblyBuilder.SetEntryPoint(main, projectModel.PEKind);
                }
                else
                {
                    throw new CompileException("无法识别项目编译指令:" + code);
                    //errorf(result, file, i + 1, 1, "无法识别项目编译指令:'{0}'", code);
                    //continue;
                    //return null;
                }
            }
            return projectModel;
            //if (result.HasError() == false)
            //{
            //    string binFileName = projectModel.BinFileName;
            //    string binFilePath = Path.Combine(folderPath, binFileName);
            //    //try
            //    //{
            //        projectModel.EmitContext.AssemblyBuilder.Save(binFileName); 
            //        string str = System.AppDomain.CurrentDomain.BaseDirectory;
            //        if (File.Exists(binFilePath))
            //        {
            //            File.Delete(binFilePath);
            //        }
            //        File.Move(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, binFileName), binFilePath);
            //        deletePDB(binFileName);
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    //Console.WriteLine(ex.Message);
            //    //}
            //    return binFilePath;
            //}
            //return null;
        }

        //void errorf( ProjectCompileResult result,string file,int line,int col,string formatstring,params string[] args)
        //{
        //    CompileMessage msg = new CompileMessage() { FileName = file, Line = line, Col = col, Text = string.Format(formatstring, args) };
        //    result.Errors.Add(msg);
        //}

    }
}
