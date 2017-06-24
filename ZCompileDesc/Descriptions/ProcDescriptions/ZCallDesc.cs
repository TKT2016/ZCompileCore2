using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;

namespace ZCompileDesc.Descriptions
{
    public class ZCallDesc : ZMethodDescBase
    {
        public ZMethodInfo ZMethod { get; set; }
        //public List<object> Parts { get; protected set; }
        public List<ZCallArg> Args { get; protected set; }

        public ZCallDesc()
        {
            Parts = new List<object>();
            Args = new List<ZCallArg>();
        }

        public void Add(params object[] objs)
        {
            foreach(var obj in objs)
            {
                AddOne(obj);
            }
        }

        private  void AddOne(object obj)
        {
            if(obj is string)
            {
                Parts.Add(obj);
            }
            else  if (obj is ZCallArg)
            {
                Parts.Add(obj);
                Args.Add(obj as ZCallArg);
            }
            else
            {
                throw new ZyyRTException();
            }
        }
        
        //public void SetArg(int k,ZMethodArg newArg)
        //{
        //    Args[k] = newArg;
        //    int j = 0;
        //    int i = 0;
        //    for (; i < this.Parts.Count;)
        //    {
        //        object thisPart = this.Parts[i];
        //        if (thisPart is ZMethodArg)
        //        {
        //            if(j==k)
        //            {
        //                Parts[i] = newArg;
        //                break;
        //            }
        //            j++;
        //        }
        //        i++;
        //    }      
        //}

        public bool Compare(ZMethodDesc another)
        {
            //if (this == another) return true;
            return EqParts(another);//&& EqBrackets(another);
        }

        private bool EqParts(ZMethodDesc another)
        {
            if (this.Parts.Count != another.Parts.Count) return false;
            if (this.Args.Count != another.Args.Count) return false;

            for (int i = 0; i < this.Parts.Count;i++ )
            {
                object thisPart = this.Parts[i];
                object anotherPart = another.Parts[i];

                if (thisPart is string)
                {
                    if (!(anotherPart is string)) return false;

                    if (((thisPart as string) != anotherPart as string))
                    {
                        return false;
                    }
                }
                else if (thisPart is ZCallArg)
                {
                    if (!(anotherPart is ZMethodArg)) return false;

                    if (!((thisPart as ZCallArg).Compare(anotherPart as ZMethodArg)))
                    {
                        return false;
                    }
                }
                /*else if (thisPart is TKTProcBracket)
                {
                    if (!(anotherPart is TKTProcBracket)) return false;

                    if (!((thisPart as TKTProcBracket).Eq(anotherPart as TKTProcBracket)))
                    {
                        return false;
                    }
                }*/
            }
            return true;
        }
        /*
        bool EqBrackets(TKTProcDesc another)
        {
            if (this.Bracket == null && another.Bracket == null) return true;
            if (this.Bracket != null && another.Bracket != null) return this.Bracket.Eq(another.Bracket);
            return false;
        }*/

        public bool HasSubject()
        {
            return (Parts.Count > 0 && Parts[0] is ZCallArg);
        }

        //public ZMethodArg GetSubjectArg()
        //{
        //    if (!HasSubject()) return null;
        //    return (Parts[0] as ZMethodArg);
        //}

        public ZCallDesc CreateTail()
        {
            ZCallDesc tailDesc = new ZCallDesc();
            List<object> list = this.Parts;
            for (int i = 1; i < list.Count;i++ )
            {
                object item = list[i];
                if (item is string)
                {
                    tailDesc.Add(item as string);
                }
                else if (item is ZCallArg)
                {
                    tailDesc.Add(item as ZCallArg);
                }
            }
            return tailDesc;
        }

        public int Count
        {
            get
            {
                return Parts.Count;
            }
        }

        public override string ToString()
        {
             List<string> list = new List<string>();
            for (int i = 0; i < this.Parts.Count; i++)
            {
                if (this.Parts[i] is string)
                {
                    list.Add(this.Parts[i] as string);
                }
                else if (this.Parts[i] is ZCallValueArg)
                {
                    list.Add("(" + (this.Parts[i] as ZCallValueArg).ValueZType.ZName + ")");
                }
                else if (this.Parts[i] is ZCallGenericArg)
                {
                    list.Add("(" + (this.Parts[i] as ZCallGenericArg).BaseZType.ZName+"类型" + ")");
                }
                else
                {
                    throw new ZyyRTException();
                }
                //else if (this.Parts[i] is TKTProcBracket)
                //{
                //    list.Add((this.Parts[i] as TKTProcBracket).ToString() );
                //}
            }
            return string.Join("", list);
        }

        //public string ToZCode()
        //{
        //    List<string> list = new List<string>();
        //    for (int i = 0; i < this.Parts.Count; i++)
        //    {
        //        if (this.Parts[i] is string)
        //        {
        //            list.Add(this.Parts[i] as string);
        //        }
        //        else if (this.Parts[i] is ZMethodArg)
        //        {
        //            list.Add("(" + (this.Parts[i] as ZMethodArg).ToZCode() + ")");
        //        }
        //        //else if (this.Parts[i] is TKTProcBracket)
        //        //{
        //        //    list.Add((this.Parts[i] as TKTProcBracket).ToString() );
        //        //}
        //    }
        //    return string.Join("", list);
        //}

        //public string ToMethodName()
        //{
        //    List<string> list = new List<string>();
        //    for (int i = 0; i < this.Parts.Count; i++)
        //    {
        //        if (this.Parts[i] is string)
        //        {
        //            list.Add(this.Parts[i] as string);
        //        }
        //        //else if (this.Parts[i] is ZProcArg)
        //        //{
        //        //    list.Add("(" + (this.Parts[i] as ZProcArg).ToZCode() + ")");
        //        //}
        //        //else if (this.Parts[i] is TKTProcBracket)
        //        //{
        //        //    list.Add((this.Parts[i] as TKTProcBracket).ToString() );
        //        //}
        //    }
        //    return string.Join("", list);
        //}
    }
}
