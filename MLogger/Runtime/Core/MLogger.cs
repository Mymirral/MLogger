using System.Collections.Generic;
using System.Diagnostics;
using MLogger.Runtime.Config;
using MLogger.Runtime.Data;
using MLogger.Runtime.Interface;
using MLogger.Runtime.Sink;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MLogger.Runtime.Core
{
    public static class MLogger
    {
        //静态构造，保证初始化时注册控制台输出
        static MLogger()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            setting ??= Resources.Load<MLoggerSetting>("MLoggerSetting");
#endif
            
            //控制台界面输出
            var consoleSink = new UnityConsoleSink();
            consoleSink.Open();
        }
    
        //静态加只读，保证只初始化一次
        private static readonly List<ILogSink> Sinks = new();
    
        //只有一份配置
        public static MLoggerSetting setting;
    
        public static void BindSetting(MLoggerSetting asset) => setting = asset;

        #region 注册

        /// <summary>
        /// 注册Log接收对象，这些对象会输出所有数据
        /// </summary>
        /// <param name="sink">接收者</param>
        public static void AddSink(ILogSink sink)
        {
            if(sink == null) return;
            if (!Sinks.Contains(sink)) Sinks.Add(sink);
        }

        /// <summary>
        /// 清除Log接收对象
        /// </summary>
        /// <param name="sink">接收者</param>
        public static void RemoveSink(ILogSink sink)
        {
            Sinks.Remove(sink);
        }

        public static void Close()
        {
            //逆序解绑
            for (var i = Sinks.Count - 1; i >= 0 ; i--)
            {
                Sinks[i].Close();
            }
        }
        
        #endregion
       
        // 通用Log，输出到多渠道
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string msg, LogLevel level, LogCategory category,Object context)
        {
            if (!setting)
            {
                Debug.LogError("[UnityConsoleSink] 未配置，请配置LogSetting，并放置在Resources文件夹下");
                return;
            }

            //是否能输出
            bool canLog = setting.LogFilter(level) && setting.LogFilter(category);
            if (!canLog) return;
        
            var Log = new LogEntry(msg, level, category, context);

            foreach (var sink in Sinks)
            {
                sink.Emit(Log);
            }
        }
    }
}