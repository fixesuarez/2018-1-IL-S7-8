using FluentAssertions;
using System;
using Xunit;

namespace CT.Tokenizer.Tests
{
    public class SimpleTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("  \r\n\t ")]
        public void WhiteSpace_are_skipped( string text )
        {
            var t = new Tokenizer(text);
            t.Current.Should().Be(TokenType.EOS);
        }

        [Fact]
        public void simple_language_terminals()
        {
            var t = new Tokenizer(" + -    *    (/) " );
            t.Current.Should().Be(TokenType.Plus);

            t.Next().Should().Be(TokenType.Minus);
            t.Current.Should().Be(TokenType.Minus);

            t.Next().Should().Be(TokenType.Mult);
            t.Current.Should().Be(TokenType.Mult);

            t.Next().Should().Be(TokenType.OpenPar);
            t.Current.Should().Be(TokenType.OpenPar);

            t.Next().Should().Be(TokenType.Div);
            t.Current.Should().Be(TokenType.Div);

            t.Next().Should().Be(TokenType.ClosePar);
            t.Current.Should().Be(TokenType.ClosePar);

            t.Next().Should().Be(TokenType.EOS);
            t.Current.Should().Be(TokenType.EOS);
        }
    }
}
