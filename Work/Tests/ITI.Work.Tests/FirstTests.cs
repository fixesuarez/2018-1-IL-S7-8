using NUnit.Framework;
using FluentAssertions;
using System;

namespace ITI.Work.Tests
{
    [TestFixture]
    public class FirstTests
    {
        /// <summary>
        /// This test is extraordinary
        /// </summary>
        [Test]
        public void yes_we_can()
        {
            var y = new YesWeCan();
            Assert.DoesNotThrow( () => y.Work(), "There is no exception when we work." );
            y.Invoking( sut => sut.Work() ).ShouldNotThrow( "There is no exception when we work." );
        }

        /// <summary>
        /// This one too
        /// </summary>
        [Test]
        public void an_integer_can_overflow()
        {
            Action ThisThrow = () =>
            {
                checked
                {
                    int i = int.MaxValue + -2;
                    i++;
                    i++;
                    i++;
                }
            };
            ThisThrow.ShouldThrow<OverflowException>();
        }

        [Test]
        public void playing_with_bitflags()
        {
            EngineState state = EngineState.IsRunning
                                | EngineState.IsClutch;

            bool isRunning = state.CheckRunning();

            isRunning.Should().BeTrue();

            state = state.StopEngine();

            state.CheckRunning().Should().BeFalse();

            state.GetSpeed().Should().Be( 0 );
            state = state.SetSpeed( 3 );
            state.GetSpeed().Should().Be( 3 );
        }
    }

}
