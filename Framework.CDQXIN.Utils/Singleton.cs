using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public sealed class Singleton<T> where T : new()
    {
        private static T _instance = new T();

        private static readonly object lockHelper = new object();

        private Singleton()
        { }

        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (lockHelper)
                    {
                        if (null == _instance)
                        {
                            _instance = new T();
                        }
                    }
                }

                return _instance;
            }
        }

        public void SetInstance(T value)
        {
            _instance = value;
        }
    }
}
