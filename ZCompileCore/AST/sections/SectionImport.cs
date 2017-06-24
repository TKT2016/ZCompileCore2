using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class SectionImport : SectionBase
    {
        public Token KeyToken;
        public List<PackageNameAST> Packages = new List<PackageNameAST>();
        List<ZType> addedZtypes = new List<ZType>();
        public void Analy()//(ContextFile fileContext)
        {
            addedZtypes = new List<ZType>();
            //this.fileContext = fileContext;
            foreach (PackageNameAST item in this.Packages)
            {
                item.FileContext = this.FileContext;
                //item.Analy(this.FileContext, addedZtypes);
                item.Analy(this.FileContext);
            }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(KeyToken.GetText());
            buf.Append(":");
            List<string> tempList = new List<string>();
            foreach (var item in this.Packages)
            {
                buf.Append(item.ToString());
            }
            buf.Append(string.Join(",", tempList));
            buf.AppendLine(";");
            return buf.ToString();
        }

    }
}
