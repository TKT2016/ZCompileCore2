using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Engines;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileCore.Symbols;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class ContextFile
    {
        public ContextProject ProjectContext { get; set; }
        public ContextUse UseContext { get; private set; }

        //public CompiledContext CompiledContext { get; set; }
        public ContextImport ImportContext;
        public ZFileModel FileModel;
        public ContextClass ClassContext { get; set; }

        public ZDimType EmitedZDim;
        public ZType EmitedZClass;
        public ISymbolTable SymbolTable { get; set; }


        public ContextFile(ContextProject projectContext,ZFileModel fileModel)
        {
            ProjectContext = projectContext;
            FileModel = fileModel;

            //CompiledContext = new CompiledContext();
            UseContext = new ContextUse();
            //this.ImportCollectionContext = new ImportCollectionContext();
            //this.ImportCollectionContext.SetFileContext(this);
            this.UseContext.SetFileContext(this);
            //PreDimCollectionContext = new PreDimCollectionContext();
            ImportContext = new ContextImport();
            ClassContext = new ContextClass(this);

            SymbolTable = UseContext.SymbolTable;
        }

        public ZMethodInfo[] SearchUseProc(ZCallDesc procDesc)
        {
            return UseContext.SearchProc(procDesc);
        }

        public IZDescType[] SearchZDescType(string zname)
        {
            return this.ImportContext.ImportPackageDescList.SearchZDescType(zname);
        }

        #region error 

        private void Error(int line ,int col,string message)
        {
            var file = this.FileModel.ZFileInfo;
            CompileMessage cmsg = new CompileMessage(file, line, col, message);
            this.ProjectContext.CompileResult.Errors.Add(file, cmsg);
            CompileConsole.Error("文件" + file.ZFileName + " 第" + line + "行,第" + col + "列错误:" + message);
        }

        public void Errorf(int line, int col, string messagef, params object[] args)
        {
            string msg = string.Format(messagef, args);
            Error(line,col,msg);
        }

        public void Errorf(CodePosition position, string messagef, params object[] args)
        {
            string msg = string.Format(messagef, args);
            Error(position.Line, position.Col, msg);
        }

        #endregion
    }
}
