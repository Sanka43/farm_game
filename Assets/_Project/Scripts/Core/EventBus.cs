using System;

namespace Meadowbrook.Core
{
    /// <summary>Typed publish/subscribe bus. Events are structs, so publishing does not allocate.</summary>
    public static class EventBus
    {
        static class Channel<T> where T : struct
        {
            public static Action<T> Handlers;
        }

        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            Channel<T>.Handlers += handler;
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            Channel<T>.Handlers -= handler;
        }

        public static void Publish<T>(T evt) where T : struct
        {
            Channel<T>.Handlers?.Invoke(evt);
        }
    }
}
