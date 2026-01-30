using System;
using System.Text;
using MLogger.Runtime.Data;
using MLogger.Runtime.Interface;
using Debug = UnityEngine.Debug;

namespace MLogger.Runtime.Sink
{
    /// <summary>
    /// 0GC 控制台输出
    /// </summary>
    public sealed class UnityConsoleSink : ILogSink
    {
        //保证每个线程都有一个sb,每个线程都会单独访问一边sb去初始化
        private StringBuilder _log;
        private StringBuilder log => _log ??= new StringBuilder();

        public void Open()
        {
            Core.MLogger.AddSink(this);
        }

        public void Close()
        {
            log.Clear();
            Core.MLogger.RemoveSink(this);
        }

        public void Emit(in LogEntry entry)
        {
            var level = entry.Level;
            var category = entry.Category;
            var message =  entry.Message;
            var obj = entry.Context;

            //输出
            var setting = Core.MLogger.setting;
            
            //展示等级/分类样式
            var colorType = setting.IsShowLevel() ? setting.GetColorText(level) : setting.GetColorText(category);
            
            //输出字符串
            log.Clear();
            log.Append(colorType);
            log.Append("[");
            log.Append(level);
            log.Append("] (");
            log.Append(category);
            log.Append("): ");
            log.Append(message);
            log.Append("</color>");
            
            message = log.ToString();

            //低级别
            if ((level & (LogLevel.Trace | LogLevel.Debug | LogLevel.Info)) != 0) Debug.Log(message, obj);
            //中级别
            if ((level & LogLevel.Warning) != 0) Debug.LogWarning(message, obj);
            //高级别
            if ((level & (LogLevel.Error | LogLevel.Fatal)) != 0) Debug.LogError(message, obj);
        }
    }
}