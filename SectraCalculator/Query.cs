using System;
using System.Collections.Generic;
using System.Text;

namespace SectraCalculator
{
    class Query
    {
        //The register in which the result of the operation is stored
        public Register Register { get; set; }
        //A string to determine what mathematical or other (print/quit) operation
        //should be used with this query
        public string Operation { get; set; }
        //If the value is an integer it is stored here while ValueRegister is ""
        public int ValueInt { get; set; } = 0;
        //If the value is a register, its name is stored here while ValueInt is 0
        public string ValueRegister { get; set; } = "";
        //A bool to tell the logic in the Program file if it should be evaluated lazily or not
        public bool IsValueARegister { get; set; }
    }
}
