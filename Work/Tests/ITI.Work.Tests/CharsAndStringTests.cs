using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Work.Tests
{
    [TestFixture]
    class CharsAndStringTests
    {
        [Test]
        public void chars_decomposition()
        {
            string s = "Noël";
            s.Length.Should().Be( 4 );
            var sD = s.Normalize( NormalizationForm.FormD );
            sD.Length.Should().Be( 5 );
        }

        [Test]
        public void removing_diacritics()
        {
            string s = "NoëléàùçÄÎµ";
            RemoveDiacriticsNaïve( s ).Should().Be( "NoeleaucAIµ" );
        }


        class A
        {
            readonly public int ToTo;
        }

        [Test]
        public void coalescence_and_nullable_demo()
        {
            A a = null;
            int? t = a?.ToTo;

            Nullable<int> t2 = a?.ToTo;

            int theValue = t.HasValue ? t.Value : -3712;
            int theValue2 = t != null ? t.Value : -3712;

            int tWithDefault = a?.ToTo ?? -3712;
        }

        [Test]
        public void testing_arabic_where_vwels_are_not_Diacritics()
        {
            string s = "الغوطة الشرق";
            var sR = RemoveDiacriticsNaïve( s );
            sR.Should().Be( s );
            sR.Should().NotBeSameAs( s );
        }

        [Test]
        public void naive_and_better_RemoveDiacritics()
        {
            string s = "kjhsdjfh 71652";
            var sRN = RemoveDiacriticsNaïve( s );
            sRN.Should().Be( s );
            sRN.Should().NotBeSameAs( s );

            var sB = RemoveDiacriticsBetter( s );
            sB.Should().BeSameAs( s );

            RemoveDiacriticsBetter( "â ë l" ).Should().Be( "a e l" );
            RemoveDiacriticsBetter( "---ù" ).Should().Be( "---u" );
            RemoveDiacriticsBetter( "ï" ).Should().Be( "i" );
            RemoveDiacriticsBetter( "" ).Should().BeSameAs( "" );
            RemoveDiacriticsBetter( "ÂÎ" ).Should().Be( "AI" );
            RemoveDiacriticsBetter( "A" ).Should().BeSameAs( "A" );
            RemoveDiacriticsBetter( "AB" ).Should().BeSameAs( "AB" );
        }

        static string RemoveDiacriticsBetter( string s )
        {
            var sD = s.Normalize( NormalizationForm.FormD );
            StringBuilder b = null;
            for( int i = 0; i < sD.Length; ++i )
            {
                if( Char.GetUnicodeCategory( sD, i ) == UnicodeCategory.NonSpacingMark )
                {
                    if( b == null ) b = new StringBuilder( sD, 0, i, s.Length );
                }
                else b?.Append( sD[i] );
            }
            return b != null ? b.ToString() : s;
        }


        static string RemoveDiacriticsNaïve( string s )
        {
            var sD = s.Normalize( NormalizationForm.FormD );
            StringBuilder b = new StringBuilder();
            foreach( var c in sD )
            {
                if( Char.GetUnicodeCategory( c ) != UnicodeCategory.NonSpacingMark )
                {
                    b.Append( c );
                }
            }
            return b.ToString();
        }

        [Test]
        public void seeing_encoding()
        {
            var greek = Encoding.GetEncoding( 737 );
            var noelInUTF8 = Encoding.UTF8.GetBytes( "Noël" );

            var received = noelInUTF8;
            string shouldBeNoel = Encoding.UTF7.GetString( received );

            var noelInGreek = greek.GetBytes( "Noël" );
            var noelInUTF7 = Encoding.UTF7.GetBytes( "Noël" );
            var noelInUTF32 = Encoding.UTF32.GetBytes( "Noël" );
        }


    }
}
