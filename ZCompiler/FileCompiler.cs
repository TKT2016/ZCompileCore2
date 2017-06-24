using System;
using System.IO;
using System.Reflection.Emit;
using ZCompileCore.Engines;
using ZCompileCore.Reports;
using ZCompileKit.Infoes;
using ZLangRT;

namespace ZCompiler
{
    public class FileCompiler
    {
        //public const string ZExt = ".zyy";
        private ZProjectModel projectModel;
        //private ZFileModel classModel;
        private FileInfo srcFileInfo;

        public ProjectCompileResult CompileResult { get; private set;}

        public FileCompiler( )
        {

        }

        public ProjectCompileResult Compile(string srcPath)
        {
            InitFile(srcPath);
            ZProjectEngine builder2 = new ZProjectEngine();
            ProjectCompileResult result = builder2.Compile(projectModel);
            CompileResult= result;
            return result;
        }

        private void InitFile(string srcPath)
        {
            srcFileInfo = new FileInfo(srcPath);
            projectModel = new ZProjectModel();
            //classModel = new ZFileModel();
            projectModel.ProjectFileInfo = new ZCompileFileInfo(true, srcPath, null, null); //zlogoFileInfo;
            projectModel.ProjectRootDirectoryInfo = srcFileInfo.Directory;
            projectModel.BinaryFileKind = PEFileKinds.ConsoleApplication;
            projectModel.BinarySaveDirectoryInfo = srcFileInfo.Directory;
            projectModel.ProjectPackageName = "ZLangSingleFile";
            projectModel.EntryClassName = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            projectModel.BinaryFileNameNoEx = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            projectModel.NeedSave = true;
            projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefPackage("Z操作系统", "Z文件系统", "Z互联网", "Z绘图", "Z桌面控件");
            //classModel.SourceFileInfo = srcFileInfo;
            ZFileModel classModel = new ZFileModel(new ZCompileFileInfo(false, srcPath, null, null));
            projectModel.AddClass(classModel);
        }
    }
}
