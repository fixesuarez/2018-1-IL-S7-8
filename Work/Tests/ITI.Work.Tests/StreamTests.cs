using FluentAssertions;
using NUnit.Framework;
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


    }
}
