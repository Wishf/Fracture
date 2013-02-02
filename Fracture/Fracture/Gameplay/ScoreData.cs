using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fracture
{
    public class ScoreData
    {
        public int Score { get; protected set; }
        public int FreeEscapees { get; protected set; }
        public int CaughtEscapees { get; protected set; }

        public ScoreData()
        {
            Score = 100;
            FreeEscapees = CaughtEscapees = 0;
        }

        public void AddFreeEscapee()
        {
            Score -= 20;
            FreeEscapees++;
        }

        public void AddCaughtEscapee()
        {
            Score += 10;
            CaughtEscapees++;
        }
    }
}
