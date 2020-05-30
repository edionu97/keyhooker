using System;

namespace DataStreams.Hooker.Hooker
{
    public interface IHooker<out T>
    {
        /// <summary>
        /// Starts the hooker
        /// <param name="onHookedAction">a callback, representing the hook handler</param>
        /// </summary>
        void Start(Action<T> onHookedAction = null);
    }
}
