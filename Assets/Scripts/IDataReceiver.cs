using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DataReceivedEventHandler(string data);

namespace Bno055
{
    public interface IDataReceiver
    {

        bool IsRunning
        {
            get;
        }

        event DataReceivedEventHandler OnDataReceived;
    }
}