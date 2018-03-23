using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Work.Tests
{

    [TestFixture]
    public class ReflectionTests
    {
        [Test]
        public void simple_reflection_demo()
        {
            Type tFromTypeOf = typeof( YesWeCan );
            // 
            object o = new YesWeCan();
            Type tFromObject = o.GetType();

            tFromObject.Name.Should().Be( "YesWeCan" );
            tFromObject.FullName.Should().Be( "ITI.Work.YesWeCan" );
            FieldInfo[] fields = tFromObject.GetFields(
                                    BindingFlags.NonPublic
                                    | BindingFlags.Public
                                    | BindingFlags.Instance
                                    | BindingFlags.Static
                                    );

            MethodInfo mWork = tFromObject.GetMethod( "Work" );
            mWork.Invoke( o, new object[] { "Julie" } );

        }
    }
}
