using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using MLogger.Runtime.Data;
using MLogger.Runtime.Interface;
using UnityEngine;

namespace MLogger.Runtime.Sink
{
    //runtime Only
    public class LogFileSink : ILogSink
    {
        //环形数组
        private LogEntry[] logs;
        private int logCapacity = 1024;
        private long logReadIndex;
        private long logWriteIndex;
        
        private StringBuilder _log;
        private StringBuilder logBuilder => _log ??= new(256);

        //线程
        private Thread thread;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        //文件流
        private FileStream fileStream;
        private StreamWriter streamWriter;

        //保证线程内部读取最新值
        private volatile bool running;

        //flush策略
        private int flushCount = 10;
        private int flushCounter;

        public void Open()
        {
            if (running) return;

            //加入接收者队列
            Core.MLogger.AddSink(this);

            logs = new LogEntry[logCapacity];

            string fileName = $"MLogger_{DateTime.Now:yyyy-MM-dd_HH-mm}.log";
            string path = Path.Combine(Application.persistentDataPath, "MLogger",fileName);
            string dir = Path.GetDirectoryName(path);

            if(!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            streamWriter = new StreamWriter(fileStream, Encoding.UTF8, 4096);

            //后台线程，用于写入
            running = true;
            
            thread = new Thread(WriteFileLoop) { IsBackground = true };
            thread.Start();
        }

        public void Close()
        {
            // 结束线程
            running = false;
            thread.Join();
            
            //取消接收
            Core.MLogger.RemoveSink(this);

            // 清除sb
            logBuilder.Clear();
            
            //如果队列里还有剩余日志，直接输出
            while (logReadIndex < logWriteIndex)
            {
                var log = logs[++logReadIndex % logCapacity];
                streamWriter.WriteLine(WriteMessage(log));
            }

            //释放
            streamWriter?.Flush();
            streamWriter?.Dispose();
            fileStream?.Dispose();
        }

        public void Emit(in LogEntry log)
        {
            //防止未启动线程调用
            if (!running) return;
            
            //索引从0开始
            var index = Interlocked.Increment(ref logWriteIndex) - 1;
            logs[index % logCapacity] = log;
            
            //写追上读，抛弃最老的
            var r = Volatile.Read(ref logReadIndex);
            if (index - r >= logCapacity)
            {
                Interlocked.Increment(ref logReadIndex);
            }
            
            //放行
            semaphoreSlim.Release();
        }

        private void WriteFileLoop()
        {
            while (running)
            {
                semaphoreSlim.Wait();
                
                var index = Interlocked.Increment(ref logReadIndex) - 1;
                var log = logs[index % logCapacity];
                
                streamWriter.WriteLine(WriteMessage(log));

                Interlocked.Increment(ref flushCounter);

                if (flushCounter == flushCount)
                {
                    flushCounter = 0;
                    streamWriter.Flush();
                }
            }
        }

        private string WriteMessage(LogEntry log)
        {
            var message = log.Message;
            var level = log.Level;
            var category = log.Category;

            //展示等级/分类样式
            logBuilder.Clear();

            logBuilder.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
            logBuilder.Append("[");
            logBuilder.Append(level);
            logBuilder.Append("] ");
            logBuilder.Append("(");
            logBuilder.Append(category);
            logBuilder.Append(") ");
            logBuilder.Append(message);

            logBuilder.Append('\n'); //换行

            return logBuilder.ToString();
        }
    }
}