using System;

namespace MyCoreLib.BaseAsync.EAP
{
    public static class EventManager<THandler, TArgs>
        where TArgs : EventArgs
    {
        public static THandler CreateEventHandler()
        {

            return default(THandler);
        }
    }


}
