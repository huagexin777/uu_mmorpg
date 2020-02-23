using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework.Interface
{
    public interface ICache
    {
        int Count
        {
            get;
        }

        void Clear();

        bool Contains(string key);

        T Get<T>(string key);

        bool TryGet<T>(string key, out T value);

        void Set<T>(string key, T value);

        void Set<T>(string key, T value, DateTime absoluteExpiration);

        void Set<T>(string key, T value, TimeSpan slidingExpiration);

        void Set<T>(string key, T value, int seconds);

        void Remove(string key);
    }
}
