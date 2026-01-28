using System.Collections.Generic;
using System.Diagnostics;
using Script.Data;
using Script.Interface;
using Script.Sink;
using UnityEngine;

public static class MLogger
{
    //静态构造，保证初始化时注册控制台输出
    static MLogger()
    {
        var console = new UnityConsoleSink();
        AddSink(console);
    }
    
    //静态加只读，保证只初始化一次，0GC
    private static readonly List<ILogSink> Sinks = new();

    /// <summary>
    /// 注册Log接收对象，这些对象会输出所有数据
    /// </summary>
    /// <param name="sink">接收者</param>
    public static void AddSink(ILogSink sink)
    {
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
    
    // 通用Log，输出到多渠道
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(string msg, LogLevel level, LogCategory category,Object context)
    {
        var Log = new LogEntry(msg, level, category, context);

        foreach (var sink in Sinks)
        {
            sink.Emit(Log);
        }
    }
}