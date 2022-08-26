using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Seasail.Extensions
{
    public static partial class TNHExtension
    {
        public static int IndexOf(this byte[] sourceArray, byte[] subArray)
        {
            if (sourceArray == null || subArray == null) return -1;
            if (sourceArray.Length < subArray.Length) return -1;
            for(int i = 0;i<sourceArray.Length-subArray.Length;i++)
            {
                for(int j = 0;j<subArray.Length;j++)
                {
                    if (sourceArray[i + j] != subArray[j])
                        break;
                    return i;
                }
            }
            return -1;
        }
    }
}
