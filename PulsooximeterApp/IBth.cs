using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PulsooximeterApp
{ 
    public interface IBth
    {
        void AttachDelegate(System.EventHandler<string> onReceive);
        void Send(string message);
        void Disconnect();
        void Connect(string name);
        bool IsConnected();
        string ConnectedDevice();
        List<string> PairedDevices();
    }
}
