using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bno055
{
	public delegate void DataReceivedEventHandler(string data);
	
    public interface IDataReceiver
    {

        bool IsRunning
        {
            get;
        }

        event DataReceivedEventHandler OnDataReceived;
    }
}