using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using MLogger.Runtime.Data;
using MLogger.Runtime.Interface;
using UnityEngine;

namespace MLogger.Runtime.Sink
{
    public class LogFileSink : ILogSink, IDisposable
    {
        private static readonly Queue<LogEntry> _logQueue;
        private static string path => Path.Combine(Application.persistentDataPath,"MLogger", "MLogger.log");
        
        private static readonly StringBuilder logBuilder = new();
        
        //文件流
        private static readonly FileStream fileStream = new(path,FileMode.Append,FileAccess.Write,FileShare.Read,4096);
        private static readonly StreamWriter streamWriter = new(fileStream,Encoding.UTF8,4096) {AutoFlush = false};

        public void Emit(in LogEntry log)
        {
            //只管入队
            _logQueue.Enqueue(log);
        }

        public Thread WriteLoop()
        {
            var setting = Core.MLogger.setting;

            while (true)
            {
                while (_logQueue.TryDequeue(out var log))
                {
                    var message = log.Message;
                    var level = log.Level;
                    var category = log.Category;
            
                    //展示等级/分类样式
                    var colorType = setting.IsShowLevel() ? setting.GetColorText(level) : setting.GetColorText(category);

                    logBuilder.Clear();
                
                    logBuilder.Append(colorType);
                    logBuilder.Append("[");
                    logBuilder.Append(level);
                    logBuilder.Append("] ");
                    logBuilder.Append("(");
                    logBuilder.Append(category);
                    logBuilder.Append(") ");
                    logBuilder.Append(message);
                    logBuilder.Append("</color>");
            
                    logBuilder.Append('\n');  //换行
                
                    //写入streamWriter
                    streamWriter.Write(logBuilder.ToString());
                }
                
                //10ms写入一次
                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}