using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bno055
{
    public class Tracker : MonoBehaviour
    {
        public bool IsDebug = false;
        public Vector3 Acceleration;

        private IDataReceiver DataReceiver;
        private float roll, pitch, yaw;
        private Quaternion rotation;
        
        void Awake()
        {
            DataReceiver = GetComponentInChildren<IDataReceiver>();
            DataReceiver.OnDataReceived += DataReceiver_OnDataReceived;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // var q = Quaternion.Euler(pitch, yaw, roll);
            transform.rotation = rotation;
            Debug.Log(Acceleration.magnitude);
        }

        void DataReceiver_OnDataReceived(string incoming)
        {
            if (IsDebug)
            {
                Debug.Log(incoming);
            }

            if ((incoming.Length > 8))
            {
                var list = incoming.Split(' ');
                if (list[0].Equals("Orientation:"))
                {
                    var x = float.Parse(list[1]);
                    var y = float.Parse(list[2]); 
                    var z = float.Parse(list[3]);
                    var w = float.Parse(list[4]);
                    rotation.Set(-y, -z, x, w);
                }
                else if (list[0].Equals("Accel:"))
                {
                    var x = float.Parse(list[1]);
                    var y = float.Parse(list[2]); 
                    var z = float.Parse(list[3]);
                    Acceleration.Set(x, y, z);
                }
                else if (list[0].Equals("Calibration:"))
                {
                    int sysCal = int.Parse(list[1]);
                    int gyroCal = int.Parse(list[2]);
                    int accelCal = int.Parse(list[3]);
                    int magCal = int.Parse(list[4]);
                    Debug.Log("Calibration: Sys=" + sysCal + " Gyro=" + gyroCal + " Accel=" + accelCal + " Mag=" + magCal);
                }
            }
        }
    }
}
