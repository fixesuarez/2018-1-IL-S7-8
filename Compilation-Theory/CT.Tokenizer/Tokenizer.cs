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

        void Forward() => _headPos++;

        public TokenType Current => _current;

        public int IntegerValue => _currentInteger;

        public TokenType Next()
        {
            while( !IsEnd && Char.IsWhiteSpace( Head ) ) Forward();
            if( IsEnd ) return _current = TokenType.EndOfStream;

            switch( Forward() )
            {
                case '(': return _current = TokenType.OpenPar;
                case ')': return _current = TokenType.ClosePar;
                case '*': return _current = TokenType.Mult;
                case '/': return _current = TokenType.Div;
                case '+': return _current = TokenType.Plus;
                case '-': return _current = TokenType.Minus;
            }
            return _current = TokenType.Error;
        }

    }
}
