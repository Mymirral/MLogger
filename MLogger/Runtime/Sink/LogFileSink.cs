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
    public class LogFileSink : ILogSink, IDisposable
    {
        private readonly ConcurrentQueue<LogEntry> _logQueue = new();

        private StringBuilder _log;
        private StringBuilder logBuilder => _log ??= new();
        
        private Thread thread;
        
        //文件流
        private FileStream fileStream;
        private StreamWriter streamWriter;
        
        //保证线程内部读取最新值
        private volatile bool running = false;

        private int flushCount = 10;
        private int flushCounter = 0;
        
        public void Open()
        {
            if (running) return;
            
            Core.MLogger.AddSink(this);
            
            string path = Path.Combine(Application.persistentDataPath,"MLogger", "MLogger.log");

            //后台线程，用于写入
            thread = new Thread(WriteFileLoop) { IsBackground = true };
            thread.Start();
            
            fileStream =  new FileStream(path,FileMode.Append,FileAccess.Write,FileShare.Read);
            streamWriter = new StreamWriter(fileStream,Encoding.UTF8,4096);
            
            running = true;
        }

        public void Close()
        {
            // 结束线程
            running = false;
            thread.Join();

            // 清除sb
            logBuilder.Clear();
            
            //释放
            Dispose();
        }
        
        public void Emit(in LogEntry log)
        {
            //只管入队
            _logQueue.Enqueue(log);
        }

        private void WriteFileLoop()
        {
            while (running)
            {
                while (_logQueue.TryDequeue(out var log))
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
            
                    logBuilder.Append('\n');  //换行
                
                    //写入streamWriter(内存)
                    streamWriter.Write(logBuilder.ToString());
                    
                    flushCounter++;

                    if (flushCounter == flushCount)
                    {
                        flushCounter = 0;
                        streamWriter.Flush();
                    }
                }
                
                //10ms写入一次
                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            streamWriter?.Flush();
            streamWriter?.Dispose();
            fileStream?.Dispose();
        }
    }
}