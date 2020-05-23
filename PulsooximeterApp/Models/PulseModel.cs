using System;
using System.Collections.Generic;
using System.Text;

namespace PulsooximeterApp.Models
{
    class PulseModel
    {
        public int Heartrate { get; set; }
        public int SpO2 { get; set; }
        public DateTime BeatLast { get; set; }
        public DateTime BeatExec { get; set; }
    }
}
