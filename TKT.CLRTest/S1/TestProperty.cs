using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKT.CLRTest.S1
{
    class TestProperty
    {
        public void Fn1(TestPropertyClass2 c)
        {
            c.C1.Name = "CCC";
        }
    }

    class TestPropertyClass1
    {
        public string Name { get; set; }
    }

    class TestPropertyClass2
    {
        public TestPropertyClass1 C1 { get; set; }
    }
}
