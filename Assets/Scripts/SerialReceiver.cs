using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace Bno055
{
    public class SerialReceiver : MonoBehaviour, IDataReceiver
    {
        public event DataReceivedEventHandler OnDataReceived;
        public bool IsRunning { get { return isRunning; } }

        public string portName = "COM6";
        public int baudRate = 115200;
        public byte[] Delimiter = new byte[] { 10 };

        private SerialPort serialPort;
        private Thread thread;
        private bool isRunning = false;
        private byte[] buffer = new byte[512];
        private byte[] tmp = new byte[512];
        
        private int byteCounter;

        void Awake()
        {
            Open();
        }

        void OnDestroy()
        {
            Close();
        }

        private bool Open()
        {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            var portExists = File.Exists(portName);
            if (!portExists)
            {
                Debug.LogWarning(string.Format("Port {0} does not exist.", portName));
                return false;
            }
#endif

            bool openSuccess = false;
            serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Debug.Log("Opening serial port...");
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                    serialPort.ReadTimeout = 1;
#else
                    serialPort.ReadTimeout = 1000;
#endif
                    serialPort.WriteTimeout = 1000;
                    serialPort.Open();
                    openSuccess = true;
                    break;
                }
                catch (IOException ex)
                {
                    Debug.LogWarning("error:" + ex.ToString());
                }
                Thread.Sleep(1000);
            }

            if (!openSuccess)
            {
                return false;
            }

            Debug.Log("Opened");

            isRunning = true;
            byteCounter = 0;

            thread = new Thread(Read);
            thread.Start();
            return true;
        }

        private void Close()
        {
            isRunning = false;

            if (thread != null && thread.IsAlive)
            {
                thread.Join();
            }

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
            }
        }

        private void Read()
        {
            while (isRunning && serialPort != null && serialPort.IsOpen)
            {
                try
                {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                    try
                    {
                        while (true)
                        {
                            var b = (byte)serialPort.ReadByte();
                            buffer[byteCounter++] = b;
                        }
                    }
                    catch (System.TimeoutException e)
                    {
                        // ignore
                    }
#else
                    if (serialPort.BytesToRead > 0)
                    {
                        //message_ = serialPort_.ReadTo(Encoding.ASCII.GetString());
                        int n = serialPort.Read(tmp, 0, serialPort.BytesToRead);
                        Array.Copy(tmp, 0, buffer, byteCounter, byteCounter + n);
                        byteCounter += n;
#endif
                    while (true)
                    {
                        var index = Utils.FindArray(buffer, byteCounter, Delimiter);
                        if (index >= 0)
                        {
                            var data = new byte[index];
                            Array.Copy(buffer, data, index);

                            // splice buffer and decrement byte counter.
                            Array.Copy(buffer, index + Delimiter.Length, buffer, 0, byteCounter - index - Delimiter.Length);
                            byteCounter -= (index + Delimiter.Length);

                            var stringData = System.Text.Encoding.ASCII.GetString(data);

                            if (OnDataReceived != null)
                            {
                                OnDataReceived(stringData);
                            }

                        }
                        else
                        {
                            break;
                        }
                    }
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
#else
                }
#endif
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message + ":" + e.StackTrace);
                }
            }
        }

        public void Write(string message)
        {
            try
            {
                serialPort.Write(message);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
}
