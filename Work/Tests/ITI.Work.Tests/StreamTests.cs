using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace ITI.Work.Tests
{
    // Comment: طة 
    [TestFixture]
    public class StreamTests
    {

        static string ThisFilePath( [CallerFilePath]string p = null ) => p;

        [Test]
        public void saving_and_reading_this_file_and_it_must_be_the_same()
        {
            byte[] content = File.ReadAllBytes( ThisFilePath() );

            using( var m = new MemoryStream() )
            {
                m.Write( content, 0, content.Length );
                m.Position = 0;

                var newContent = new byte[7657645];
                int nbRead = m.Read( newContent, 0, newContent.Length );

                nbRead.Should().Be( content.Length );
            }

        }

        [Test]
        public void saving_this_file_with_compression()
        {
            byte[] content = File.ReadAllBytes( ThisFilePath() );

            using( var m = new MemoryStream() )
            using( var c = new GZipStream( m, CompressionMode.Compress ) )
            {
                c.Write( content, 0, content.Length );
                c.Flush();
                m.Position.Should().BeLessThan( content.Length );
            }

        }

        [TestCase( "a password" )]
        [TestCase( "another one" )]
        public void Krabouille_stream_basically_works( string password )
        {
            byte[] data = new byte[] { 0, 1, 2, 3, 4 };

            byte[] written = Write( data, password );
            written.Should().NotBeEquivalentTo( data );

            byte[] readBack = Read( written, password );
            readBack.Should().BeEquivalentTo( data );

            byte[] readFailed = Read( written, "!" + password );
            readFailed.Should().NotBeEquivalentTo( data );
        }

        [TestCase( "p1", 3 )]
        [TestCase( "p2", 73 )]
        [TestCase( "p3", 273 )]
        public void Krabouille_stream_on_file( string pwd, int bufferSize )
        {
            string original = ThisFilePath();
            string krabouilled = $"{original}.{pwd}.krab";
            string readBack = krabouilled + ".txt";

            // original => krabouilled (.krab)
            using( var o = new FileStream( original, FileMode.Open, FileAccess.Read, FileShare.None ) )
            using( var target = new FileStream( krabouilled, FileMode.Create, FileAccess.Write, FileShare.None ) )
            using( var k = new KrabouilleStream( target, KrabouilleMode.Krabouille, pwd ) )
            {
                o.CopyTo( k, bufferSize );
            }

            File.ReadAllBytes( original ).Should().NotBeEquivalentTo( File.ReadAllBytes( krabouilled ) );

            // krabouilled => readBack (.txt)
            using( var source = new FileStream( krabouilled, FileMode.Open, FileAccess.Read ) )
            using( var target = new FileStream( readBack, FileMode.Create, FileAccess.Write ) )
            using( var uk = new KrabouilleStream( source, KrabouilleMode.Unkrabouille, pwd ) )
            {
                uk.CopyTo( target, bufferSize + 1 );
            }

            File.ReadAllBytes( original ).Should().BeEquivalentTo( File.ReadAllBytes( readBack ) );
        }


        [Test]
        public void Krabouille_stream_is_password_dependent()
        {
            byte[] content = File.ReadAllBytes( ThisFilePath() );

            byte[] f = Write( content, "first" );
            byte[] s = Write( content, "second" );

            f.Should().NotBeEquivalentTo( content );
            s.Should().NotBeEquivalentTo( content );
            f.Should().NotBeEquivalentTo( s );

            byte[] fBack = Read( f, "first" );
            byte[] sBack = Read( s, "second" );

            fBack.Should().BeEquivalentTo( content );
            sBack.Should().BeEquivalentTo( content );
        }

        [Test]
        public void a_good_Krabouille_stream_is_non_deterministic()
        {
            byte[] content = File.ReadAllBytes( ThisFilePath() );

            byte[] f = Write( content, "a password" );
            byte[] s = Write( content, "a password" );

            f.Should().NotBeEquivalentTo( content );
            s.Should().NotBeEquivalentTo( content );
            f.Should().NotBeEquivalentTo( s );

            byte[] fBack = Read( f, "a password" );
            byte[] sBack = Read( s, "a password" );

            fBack.Should().BeEquivalentTo( content );
            sBack.Should().BeEquivalentTo( content );
        }

        byte[] Write( byte[] data, string password )
        {
            using( var m = new MemoryStream() )
            {
                using( var k = new KrabouilleStream( m, KrabouilleMode.Krabouille, password ) )
                {
                    k.Write( data, 0, data.Length );
                    k.Flush();
                }
                return m.ToArray();
            }
        }

        byte[] Read( byte[] data, string password )
        {
            List<byte> result = new List<byte>();
            using( var m = new MemoryStream( data ) )
            {
                byte[] buffer = new byte[128];
                using( var uk = new KrabouilleStream( m, KrabouilleMode.Unkrabouille, password ) )
                {
                    int nbRead;
                    while( (nbRead = uk.Read( buffer, 0, buffer.Length )) > 0 )
                    {
                        for( int i = 0; i < nbRead; ++i ) result.Add( buffer[i] );
                    }

                }
                return result.ToArray();
            }
        }
    }

}
