using System;
using System.Collections.Generic;

namespace Meadowbrook.Core
{
    /// <summary>Minimal service registry. Systems register interfaces; consumers fetch them.</summary>
    public static class ServiceLocator
    {
        static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            Services[typeof(T)] = service;
        }

        public static T Get<T>() where T : class
        {
            if (Services.TryGetValue(typeof(T), out var service))
                return (T)service;
            throw new InvalidOperationException("Service not registered: " + typeof(T).Name);
        }

        public static bool TryGet<T>(out T service) where T : class
        {
            if (Services.TryGetValue(typeof(T), out var obj))
            {
                service = (T)obj;
                return true;
            }
            service = null;
            return false;
        }

        public static void Clear()
        {
            Services.Clear();
        }
    }
}
