﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;

namespace ZCompileDesc.Utils
{
    public static class ZTypeListHelper
    {
        public static bool Contains(string zname, params IZTypeDictionary[] dictList)
        {
            foreach (var item in dictList)
            {
                if (item.ContainsZType( zname))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Contains(string zname, params ZType[] dictList)
        {
            foreach (var item in dictList)
            {
                if (item.ZName==zname)//(text))
                {
                    return true;
                }
            }
            return false;
        }

        public static ZType[] Search(string zname, params ZType[] dictList)
        {
            List<ZType> words = new List<ZType>();
            foreach (ZType item in dictList)
            {
                if (item.ZName == zname)//(text))
                {
                    words.Add(item);
                }
            }
            return words.ToArray();
        }

        public static ZType[] Search(string zname, params IZTypeDictionary[] dictList)
        {
            List<ZType> words = new List<ZType>();
            foreach (IZTypeDictionary item in dictList)
            {
                words.AddRange(item.SearchZType(zname));
            }
            return words.ToArray();
        }
    }
}
