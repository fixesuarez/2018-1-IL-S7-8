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
            while ( !IsEnd && Char.IsWhiteSpace(Head)) Forward();

            return _current = TokenType.EOS;
        }

    }
}
