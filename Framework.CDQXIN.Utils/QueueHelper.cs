using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public class QueueHelper
    {
        //加锁  防止高并发
        private static object myLock = new object();

        public static readonly QueueHelper newsIndex = new QueueHelper();

        public void StartNewThread()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(QueueToIndex));
        }


        //请求队列
        public static Queue<object> newsQueue = new Queue<object>();

        /// <summary>
        /// 新增下载信息
        /// </summary>
        /// <param name="books"></param>
        public static void Add(object dto)
        {
            lock (myLock)
            {
                newsQueue.Enqueue(dto);
            }
        }

        //定义一个线程 将队列中的数据取出来 插入索引库中
        private static void QueueToIndex(object para)
        {
            while (true)
            {
                if (newsQueue.Count > 0)
                {
                    object model = null;
                    lock (myLock)
                    {
                        model = newsQueue.Dequeue();
                    }
                    if (model == null)
                    {
                        continue;
                    }
                    WriteLog(model);
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLog(Object obj)
        {
            try
            {
                //具体方法
            }
            catch { }
        }
    }
}
