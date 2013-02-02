using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fracture
{
    public interface IRoundManager
    {
        Round Current { get; }
        Round Next { get; set; }

        int RemainingParticipants { get; }
        int ParticipantsInPlay { get; }
        int TimeOnRound { get; }
        int NextWaveTimer { get; }

        event EventHandler RoundChanged;
    }
}
