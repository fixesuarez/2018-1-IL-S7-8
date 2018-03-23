using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Work
{
    static public class EngineStateExtension
    {
        public static bool CheckRunning( this EngineState s )
        {
            return (s & EngineState.IsRunning) != 0;
        }

        public static EngineState StopEngine( this EngineState s )
        {
            return s & ~EngineState.IsRunning;
        }

        public static EngineState StartEngine( this EngineState s )
        {
            return s | EngineState.IsRunning;
        }

        public static int GetSpeed( this EngineState s )
        {
            return (int)(s & EngineState.SpeedMask)
                            >> (int)EngineState.SpeedShift;
        }

        public static EngineState SetSpeed( this EngineState s, int speed )
        {
            if( speed < 0 || speed > 7 ) throw new ArgumentException( "Speed must be between 0 and 7." );
            speed <<= (int)EngineState.SpeedShift;
            s &= ~EngineState.SpeedMask;
            return (EngineState)((int)s | speed);
        }

    }
}
