
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using PulsooximeterApp.Droid.BlueTooth;
using Java.IO;
using Java.Util;
using Application = Xamarin.Forms.Application;
using PulsooximeterApp;

[assembly: Xamarin.Forms.Dependency(typeof(Bth))]
namespace PulsooximeterApp.Droid.BlueTooth
{
    class Bth : IBth
    {
        private CancellationTokenSource _ct { get; set; }
        private EventHandler<string> onReceive;

        public string MessageToSend { get; set; }
        public string LastReceivedData { get; set; }
        BluetoothSocket bthSocket = null;
        public bool IsConnected() { return bthSocket != null ? bthSocket.IsConnected : false; }
        public string ConnectedDevice() { return bthSocket != null ? bthSocket.RemoteDevice.Name : null; }


        public Bth()
        {
            _ct = new CancellationTokenSource();
        }

        public void Connect(string name)
        {
            if (IsConnected())
                Disconnect();
            Task.Run(async () => ConnectDevice(name));
        }
        public void Disconnect()
        {
            if (_ct != null)
            {
                System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
                _ct.Cancel();
            }
        }
        public void Send(string message)
        {
            if (MessageToSend == null)
                MessageToSend = message;
        }

        public void AttachDelegate(System.EventHandler<string> Deleg)
        {
            onReceive += Deleg;
        }

        private async Task ConnectDevice(string name)
        {
            BluetoothDevice device = null;
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;

            while (_ct.IsCancellationRequested == false)
            {
                try
                {
                    Thread.Sleep(250);

                    adapter = BluetoothAdapter.DefaultAdapter;

                    if (adapter == null)
                        System.Diagnostics.Debug.WriteLine("No Bluetooth adapter found.");
                    else
                        System.Diagnostics.Debug.WriteLine("Adapter found!!");

                    if (!adapter.IsEnabled)
                        System.Diagnostics.Debug.WriteLine("Bluetooth adapter is not enabled.");
                    else
                        System.Diagnostics.Debug.WriteLine("Adapter enabled!");

                    System.Diagnostics.Debug.WriteLine("Try to connect to " + name);

                    foreach (var bd in adapter.BondedDevices)
                    {
                        System.Diagnostics.Debug.WriteLine("Paired devices found: " + bd.Name.ToUpper());
                        if (bd.Name.ToUpper().IndexOf(name.ToUpper()) >= 0)
                        {
                            System.Diagnostics.Debug.WriteLine("Found " + bd.Name + ". Try to connect with it!");
                            device = bd;
                            break;
                        }
                    }

                    if (device == null)
                        System.Diagnostics.Debug.WriteLine("Named device not found.");
                    else
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                        bthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                        if (bthSocket != null)
                        {
                            await bthSocket.ConnectAsync();

                            var mReader = new InputStreamReader(bthSocket.InputStream);
                            var buffer = new BufferedReader(mReader);

                            if (bthSocket.IsConnected)
                            {
                                System.Diagnostics.Debug.WriteLine("Connected!");
                                while (_ct.IsCancellationRequested == false)
                                {
                                    if (MessageToSend != null)
                                    {
                                        var chars = MessageToSend.ToCharArray();
                                        var bytes = new List<byte>();
                                        foreach (var character in chars)
                                        {
                                            bytes.Add((byte)character);
                                        }

                                        await bthSocket.OutputStream.WriteAsync(bytes.ToArray(), 0, bytes.Count);
                                        MessageToSend = null;
                                    }

                                    while(buffer.Ready())
                                    {
                                        // I read...
                                        char[] chr = new char[100];
                                        LastReceivedData = buffer.ReadLine();
                                        onReceive?.Invoke(this, LastReceivedData);
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (bthSocket != null)
                        bthSocket.Close();
                    device = null;
                    adapter = null;
                }
            }
        }
        public List<string> PairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            List<string> devices = new List<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }

    }
}