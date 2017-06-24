using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileKit.Collections;
using ZCompileDesc.Descriptions;
using ZCompileKit.Infoes;

namespace ZCompileCore.Reports
{
    public class ProjectCompileResult
    {
        public string BinaryFilePath { get; set; }

        public KeyValuesDictionary<ZCompileFileInfo, CompileMessage> Errors { get; private set; }
        public KeyValuesDictionary<ZCompileFileInfo, CompileMessage> Warnings { get; private set; }
        public List<IZDescType> CompiledTypes { get; private set; }
        public ZType EntrtyZType { get; set; }

        public ProjectCompileResult()
        {
            //Errors = new List<CompileMessage>();
            //Warnings = new List<CompileMessage>();
            Errors = new KeyValuesDictionary<ZCompileFileInfo, CompileMessage>();
            Warnings = new KeyValuesDictionary<ZCompileFileInfo, CompileMessage>();
            CompiledTypes = new List<IZDescType>();
        }

        public bool HasError()
        {
            return this.Errors.Count>0;
        }

        //public void AddCompiledType(params IZDescType[] descTypes)
        //{
        //    CompiledTypes.AddRange(descTypes);
        //}

        public IZDescType GetCompiledType(string name)
        {
            foreach (var type in CompiledTypes)
            {
                if (type.ZName == name)
                    return type;
            }
            return null;
        }
    }
}
