using System;

namespace CT.Tokenizer
{
    public class Tokenizer
    {
        readonly string _text;
        int _headPos;
        TokenType _current;
        int _currentInteger;

        public Tokenizer( string text )
        {
            _text = text;
            Next();
        }


        bool IsEnd => _headPos >= _text.Length;

        char Head => _text[_headPos];

        char Forward() => _text[_headPos++];

        public TokenType Current => _current;

        public int IntegerValue => _currentInteger;

        public TokenType Next()
        {
            // Skipping white space.
            while ( !IsEnd && Char.IsWhiteSpace(Head)) Forward();
            if( IsEnd ) return _current = TokenType.EOS;

            // Handling terminals.
            char c = Forward();
            switch( c )
            {
                case '*': return _current = TokenType.Mult;
                case '/': return _current = TokenType.Div;
                case '-': return _current = TokenType.Minus;
                case '+': return _current = TokenType.Plus;
                case '(': return _current = TokenType.OpenPar;
                case ')': return _current = TokenType.ClosePar;
            }
            // Handling non-terminals.
            int v = c - '0';
            if (v > 0 && v <= 9)
            {
               while (!IsEnd)
                {
                    int d = Head - '0';
                    if (d >= 0 && d <= 9)
                    {
                        v = v * 10 + d;
                        Forward();
                    }
                    else break;
                }
                _currentInteger = v;
                return _current = TokenType.Integer;
            }
            return _current = TokenType.Error;
        }

    }
}
