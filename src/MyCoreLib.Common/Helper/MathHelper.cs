
using System;
using System.Collections.Generic;

namespace MyCoreLib.Common.Helper
{
    public static class MathHelper
    {
        public static T Sum<T>(Func<IEnumerable<T>, T> operation, IEnumerable<T> objects)
        {
            return operation(objects);
        }
    }
}
