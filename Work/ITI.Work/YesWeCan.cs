using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Work
{
    public class YesWeCan
    {
        readonly string _name;
        EngineState _state;
        int _x;
        int _y;

        public YesWeCan()
        {
            _name = Guid.NewGuid().ToString( "N" );
        }

        public void Work( string name = "Unknown" )
        {
            Console.WriteLine( "Hello " + name );
            _state = _state.StartEngine();
        }
    }
}
