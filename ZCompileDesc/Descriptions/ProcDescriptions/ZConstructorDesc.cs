using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions 
{
    public class ZConstructorDesc //: ZMethodDesc
    {
        public List<ZMethodNormalArg> Args { get; private set; }
        public ConstructorInfo Constructor { get; set; }

        public ZConstructorDesc()
        {
            //Parts = new List<object>();
        }

        public ZConstructorDesc(IEnumerable<ZMethodNormalArg> args)
        {
            //Parts = new List<object>();
            //Add(args);
            //Bracket = new TKTProcBracket(args);
            Args = args.ToList();
        }

        public void Add(params  ZMethodNormalArg[] args)
        {
            Args.AddRange(args);
        }

        //public bool Eq(ZConstructorDesc another)
        //{
        //    if (this == another) return true;
        //    if (this.Args.Count != another.Args.Count) return false;
        //    for (int i = 0; i < this.Args.Count;i++ )
        //    {
        //        if (this.Args[i].(another.Args[i]) == false)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //    //return EqBrackets(another);
        //}

        public Type[] GetArgTypes()
        {
            return Args.Select(P => P.ArgZType.SharpType).ToArray();
        }

        public override string ToString()
        {
            List<string> list = new List<string>();
            if(this.Constructor!=null)
            {
                list.Add(this.Constructor.DeclaringType.Name);
            }
            string argsText = string.Join(",",Args.ToString());
            list.Add(argsText);
            return string.Join("", list);
        }
    }
}
