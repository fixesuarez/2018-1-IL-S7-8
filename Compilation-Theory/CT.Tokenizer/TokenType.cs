using System;
using System.Collections.Generic;
using System.Text;

namespace CT.Tokenizer
{
    public enum TokenType
    {
        Error = -1,
        EndOfStream = 0,
        Integer,
        Mult,
        Div,
        Plus,
        Minus,
        OpenPar,
        ClosePar
    }
}
