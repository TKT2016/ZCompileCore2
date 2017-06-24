using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Words
{
    public class WordData
    {
        public WordKind WKind { get; set; }
        public Object Data { get; set; }

        public WordData(WordKind kind, Object data)
        {
            WKind = kind;
            Data = data;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}",WKind,Data);
        }
    }
}
