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
        }

        IEnumerable<long> GetNumbersThatAreNotMultipleOf(
            long start,
            long count,
            IEnumerable<long> divisors )
        {
        }

        [Test]
        public void generating_numbers()
        {

        }
    }
}
