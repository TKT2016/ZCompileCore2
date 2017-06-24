using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class ProcContextCollection
    {
        public ContextClass ClassContext { get; set; }
        public List<ContextProc> ProcContextList { get; private set; }

        public ProcContextCollection()
        {
            ProcContextList = new List<ContextProc>();
        }

        List<IWordDictionary> _WordCollection;
        public List<IWordDictionary> GetWordCollection()
        {
            if (_WordCollection == null)
            {
                _WordCollection = new IWordDictionaryList();
                foreach (var item in ProcContextList)
                {
                    _WordCollection.Add(item.ProcNameWordDictionary);
                }
            }
            return _WordCollection;
        }

        public void Add(ContextProc procContext)
        {
            ProcContextList.Add(procContext);
        }

        public ZMethodDesc[] SearchProc(ZCallDesc procDesc)
        {
            List<ZMethodDesc> data = new List<ZMethodDesc>();
            foreach (var context in ProcContextList)
            {
                if (!context.IsConstructor && procDesc.Compare(context.ProcDesc)) 
                {
                    data.Add(context.ProcDesc);
                }
            }
            return data.ToArray();
        }
    }
}
