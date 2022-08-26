using System;
using System.Collections.Generic;
using System.Text;

namespace Seasail.Extensions
{
    public static partial class TNHExtension
    {

        public static object CreateInstance(this Type type)
        {
            if (type == null) return null;
            try
            {
                var constructor = type.GetConstructor(new Type[0]);
                return constructor.Invoke(new Type[0]);
            }
            catch
            {
                return null;
            }
        }
    }
}
