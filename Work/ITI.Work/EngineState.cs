using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Work
{
    public enum EngineState : byte
    {
        IsBrake = 1 << 0,
        IsClutch = 1 << 1,
        IsRunning = 1 << 2,
        SpeedMask = (1 << 3 | 1 << 4 | 1 << 5),
        SpeedShift = 3
    }
}
