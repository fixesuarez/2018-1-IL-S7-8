using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Work.Tests
{
    [TestFixture]
    public class EnumerableGameTests
    {

        IEnumerable<long> GetNumbers( long start, long count )
        {
            for( long i = 0; i < count; ++i ) yield return start + i;
        }

        IEnumerable<long> ThatAreNotMultipleOf(
            IEnumerable<long> input,
            IEnumerable<long> divisors )
        {
            foreach( var v in input )
            {
                bool isGood = true;
                foreach( var d in divisors )
                {
                    if( (v % d) == 0 )
                    {
                        isGood = false;
                        break;
                    }
                }
                if( isGood ) yield return v;
            }
        }

        IEnumerable<string> NumbersToString( IEnumerable<long> input )
        {
            foreach( var n in input ) yield return n.ToString();
        }

        [Test]
        public void generating_numbers()
        {
            var r = ThatAreNotMultipleOf( GetNumbers( 0, 100 ), new long[] { 2, 3 } );
            foreach( var v in r )
            {
                Console.WriteLine( v );
            }
        }
    }
}
