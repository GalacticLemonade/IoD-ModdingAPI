using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoDModdingAPI
{
    public static class Events
    {
        /// <summary>
        /// Called after
        /// </summary>
        public readonly static Event OnApplicationLateStart = new(true);
    }
}
