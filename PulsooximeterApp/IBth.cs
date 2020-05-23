using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PulsooximeterApp
{ 
    public interface IBth
    {
        void Send(string message);
        string Read();
        void Disconnect();
        void Connect(string name);
        List<string> PairedDevices();
    }
}
