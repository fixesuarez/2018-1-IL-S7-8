using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Work.Tests
{
    static class MyLinq
    {
        public static IEnumerable<TResult> Select<T, TResult>( this IEnumerable<T> input, Func<T, TResult> f )
        {
            foreach( var x in input ) yield return f( x );
        }

        public static IEnumerable<T> Where<T>( this IEnumerable<T> input, Func<T, bool> predicate )
        {
            foreach( var l in input )
            {
                if( predicate( l ) ) yield return l;
            }
        }

        public static TResult Aggregate<T, TResult>( this IEnumerable<T> input, TResult seed, Func<TResult, T, TResult> aggregator )
        {
            foreach( var x in input )
            {
                seed = aggregator( seed, x );
            }
            return seed;
        }

        public static int Count<T>( this IEnumerable<T> input )
        {
            if( input is ICollection<T> c ) return c.Count;
            if( input is IReadOnlyCollection<T> rc ) return rc.Count;
            int count = 0;
            foreach( var v in input ) ++count;
            return count;
        }

        // IEnumerable<T> ==> IEnumerable<TResult>
        //  T ==> IEnumerable<TResult>
        public static IEnumerable<TResult> SelectMany<T, TResult>( this IEnumerable<T> input, Func<T, IEnumerable<TResult>> selector )
        {
            foreach( var papa in input )
            {
                foreach( var fiston in selector( papa ) ) yield return fiston;
            }
        }

        public static IEnumerable<long> ThatAreNotMultipleOf( this IEnumerable<long> input, IEnumerable<long> divisors )
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

    }

    [TestFixture]
    public class EnumerableGameTests
    {

        long Multiply( IEnumerable<long> input )
        {
            long result = 1;
            foreach( var x in input )
            {
                result = result * x;
            }
            return result;
        }

        IEnumerable<long> GetNumbers( long start, long count )
        {
            for( long i = 0; i < count; ++i ) yield return start + i;
        }


        IEnumerable<string> NumbersToString( IEnumerable<long> input )
        {
            foreach( var n in input ) yield return n.ToString();
        }

        [Test]
        public void generating_numbers()
        {
            var r = MyLinq.ThatAreNotMultipleOf( GetNumbers( 0, 100 ), new long[] { 2, 3 } );
            foreach( var v in r )
            {
                Console.WriteLine( v );
            }
        }

        class Filter
        {
            readonly List<string> _source;
            public Filter( List<string> s )
            {
                _source = s;
            }

            public IEnumerable<string> KeepOnlyStringLongerThan2AndSortThem()
            {
                return _source.Where( s => s.Length > 2 ).OrderBy( s => s );
            }
        }

        [Test]
        public void fuck_fx()
        {
            var list = new List<string>();
            var f = new Filter( list );
            var filtered = f.KeepOnlyStringLongerThan2AndSortThem();

            list.Add( "Toto" );
            list.Add( "1" );
            list.Add( "Tata" );

            CollectionAssert.AreEqual( filtered, new[] { "Tata", "Toto" } );

            list.Add( "Tutu" );

            CollectionAssert.AreEqual( filtered, new[] { "Tata", "Toto", "Tutu" } );
        }

        [Test]
        public void selecting_numbers()
        {
            var r = GetNumbers( 0, 1000 )
                    .ThatAreNotMultipleOf( new long[] { 2, 3 } )
                    .Where( v => (v%13) != 0 )
                    .Where( v => v.ToString().Contains( '8' ) )
                    .Select( v => v.ToString() );

            var r2 = GetNumbers( 0, 1000 ).ThatAreNotMultipleOf( new long[] { 2, 3 } );

            double mult = MyLinq.Aggregate( r2, 1.0, ( acc, value ) => acc * value );

            // Static method.
            var wStatic = MyLinq.Where( r2, FilterThisNumberStatic );
            // Instance method (this is captured).
            var wInstance = MyLinq.Where( r2, FilterThisNumber );

            bool hasAtLeastOne1 = wStatic.Any();
            bool hasAtLeastOne2 = wInstance.Any();


            // Anonymous function.
            int numberOfCallsToDelegate = 0;
            var wDelegate = MyLinq.Where( r2, delegate ( long x ) {
                numberOfCallsToDelegate++;
                return (x % 13) != 0;
            } );
            var wLambda = MyLinq.Where( r2, x => (x % 13) != 0 );

            // Lambda function.


            foreach( var v in wDelegate )
            {
                Console.WriteLine( v );
            }
            Console.WriteLine( _numberOfCalls );
        }

        int _numberOfCalls;

        bool FilterThisNumber( long x )
        {
            ++_numberOfCalls;
            return (x % 13) != 0;
        }
        static bool FilterThisNumberStatic( long x )
        {
            return (x % 13) != 0;
        }

        static string GetThisFilePath( [CallerFilePath]string p = null ) => p;

        public void enumerating_files()
        {
            var dir = Path.GetDirectoryName( Path.GetDirectoryName( GetThisFilePath() ) );

            IEnumerable<string> childDirs = Directory.EnumerateDirectories( dir );

        }
    }


}
