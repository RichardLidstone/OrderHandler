using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OrderHandler
{
    public abstract class Common
    {
        public static ConcurrentDictionary<Type, int> nextId = new ConcurrentDictionary<Type, int>();

        public Common()
        {
            /*
            if (!nextId.ContainsKey(GetType()))
                nextId.[GetType()] = 1;

            id = nextId[GetType()]++;
            /*/
            id = nextId.GetOrAdd(GetType(), type => nextId.ContainsKey(type) ? nextId[type] : 1);
            nextId[GetType()]++;
            //*/
        }

        public int id { get; private set; }

        public string timestamp => DateTime.Now.ToString("HH:mm.ss tt").ToLower();
    }
}
