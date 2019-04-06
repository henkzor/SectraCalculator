using System;
using System.Collections.Generic;
using System.Text;

namespace SectraCalculator
{
    class Query
    {

        //Variable variable = new Variable(); 


        public string VariableName { get; set; }

        public Register Register { get; set; }
        public string Operation { get; set; }
        public int Value { get; set; }
    }
}
