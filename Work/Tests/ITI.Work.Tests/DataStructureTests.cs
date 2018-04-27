using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Work.Tests
{
    [TestFixture]
    public class DataStructureTests
    {
        [Test]
        public void our_generic_list_supports_foreach()
        {
            var myList = new ITIList<int>();
            myList.Add( 3712 );

            foreach( var i in myList )
            {
                Console.WriteLine( i );
            }

            ///
            {
                using( IEnumerator<int> e = myList.GetEnumerator() )
                {
                    while( e.MoveNext() )
                    {
                        Console.WriteLine( e.Current );
                    }
                }
            }

            {
                IEnumerator<int> e = myList.GetEnumerator();
                try
                {
                    while( e.MoveNext() )
                    {
                        Console.WriteLine( e.Current );
                    }
                }
                finally
                {
                    e.Dispose();
                }
            }

        }

        [Test]
        public void how_list_works()
        {
            var myList = new ITIListInt();
            myList.Add( 3712 );
            myList[0].Should().Be( 3712 );
            myList[0] = -67;
            myList[0].Should().Be( -67 );
        }

        [Test]
        public void how_generic_list_works()
        {
            var myList = new ITIList<int>();
            myList.Add( 3712 );
            myList[0].Should().Be( 3712 );
            myList[0] = -67;
            myList[0].Should().Be( -67 );
        }

        [Test]
        public void list_is_extensible()
        {
            var myList = new ITIListInt();
            var r = new Random( 876 );
            for( int i = 0; i < 1233; ++i ) myList.Add( r.Next() );

            var rCheck = new Random( 876 );
            for( int i = 0; i < 1233; ++i )
            {
                myList[i].Should().Be( rCheck.Next() );
            }
        }

        [TestCase( 10 )]
        [TestCase( 2873 )]
        public void list_supports_insert_at( int listSize )
        {
            var myList = new ITIListInt();
            var r = new Random( 87623 );
            for( int i = 0; i < listSize; ++i )
            {
                myList.InsertAt( r.Next() % (myList.Count + 1), i + 1 );
            }

            var checkList = new int[ listSize ];

            for( int i = 0; i < listSize; ++i )
            {
                int theI = myList[i];
                theI.Should().BeGreaterThan( 0 );
                checkList[theI - 1].Should().Be( 0 );
                checkList[theI - 1] = 1;
            }
            for( int i = 0; i < checkList.Length; ++i )
            {
                checkList[i].Should().Be( 1 );
            }
        }

        [Test]
        public void list_supports_remove_at_and_index_of()
        {
            var myList = new ITIListInt();
            myList.Add( 1 );
            myList.Add( 2 );
            myList.Add( -5 );
            myList.Add( -8 );
            myList.Add( 10 );
            myList.Add( 12 );

            myList.Count.Should().Be( 6 );
            int idx = myList.IndexOf( -5 );
            idx.Should().Be( 2 );
            myList.RemoveAt( idx );
            myList[2].Should().Be( -8 );
            myList.Count.Should().Be( 5 );

            idx = myList.IndexOf( 12 );
            idx.Should().Be( 4 );
            myList.RemoveAt( idx );
            myList.Count.Should().Be( 4 );

            idx = myList.IndexOf( 1 );
            myList.RemoveAt( idx );
            myList.Count.Should().Be( 3 );

            myList[0].Should().Be( 2 );
            myList[1].Should().Be( -8 );
            myList[2].Should().Be( 10 );
        }


        [Test]
        public void dictionary_at_work()
        {
            var d = new ITIDictionary<string, int>();
            d.Add( "One", 1 );
            d["Two"] = 2;

            d.Invoking( sut => sut.Add( "One", 1 ) )
                .ShouldThrow<InvalidOperationException>();

            d["One"].Should().Be( 1 );
            d["Two"].Should().Be( 2 );

            d["Two"] = 2000;

            d.Count.Should().Be( 2 );

            int count = 0;
            foreach( KeyValuePair<string,int> kv in d )
            {
                Assert.That(  ++count <= d.Count
                                &&
                              ((kv.Key == "One") == (kv.Value == 1)
                                 ||
                               (kv.Key == "Two" ) == (kv.Value == 2000)) );
            }

            d.Remove( "One" );
            d.Count.Should().Be( 1 );

            d.Remove( "Two" );
            d.Count.Should().Be( 0 );

            foreach( KeyValuePair<string, int> kv in d )
            {
                Assert.Fail( "No way to be here!" );
            }
        }
    }
}
