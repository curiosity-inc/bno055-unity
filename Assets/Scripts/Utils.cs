using System;
using UnityEngine;

namespace Bno055
{
    public class Utils
    {
        public static float[] Strings2Floats(string[] strings, int offset, int length)
        {
            var res = new float[length];
            for (var i = 0; i < length; i++)
            {
                if (i + offset >= strings.Length)
                {
                    res[i] = float.NaN;
                }
                else if (!float.TryParse(strings[i + offset], out res[i]))
                {
                    res[i] = float.NaN;
                }
            }
            return res;
        }

        public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
        {
            // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
            Quaternion q = new Quaternion();
            q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
            q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
            q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
            q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
            q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
            q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
            q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
            return q;
        }

        // https://stackoverflow.com/questions/3940194/find-an-array-inside-another-larger-array
        public static int FindArray(byte[] largeArray, int length, byte[] subArray)
        {

            /* If any of the arrays is empty then not found */
            if (length == 0 || subArray.Length == 0)
            {
                return -1;
            }

            /* If subarray is larger than large array then not found */
            if (subArray.Length > length)
            {
                return -1;
            }

            for (int i = 0; i < length; i++)
            {
                /* Check if the next element of large array is the same as the first element of subarray */
                if (largeArray[i] == subArray[0])
                {

                    bool subArrayFound = true;
                    for (int j = 0; j < subArray.Length; j++)
                    {
                        /* If outside of large array or elements not equal then leave the loop */
                        if (length <= i + j || subArray[j] != largeArray[i + j])
                        {
                            subArrayFound = false;
                            break;
                        }
                    }

                    /* Sub array found - return its index */
                    if (subArrayFound)
                    {
                        return i;
                    }

                }
            }

            /* Return default value */
            return -1;
        }
    }
}
