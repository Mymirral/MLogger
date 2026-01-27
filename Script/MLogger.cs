using System;
using System.Diagnostics;
using System.Text;
using Script;
using Script.Data;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class MLogger
{
    public static MLoggerSetting setting;

    //保证每个线程都有一个sb,每个线程都会单独访问一边sb去初始化
    [ThreadStatic] private static StringBuilder _log;
    private static StringBuilder log => _log ??= new StringBuilder();

    public static void BindSetting(MLoggerSetting asset) => setting = asset;
    
    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(string message,LogLevel level = LogLevel.Trace, LogCategory category = LogCategory.None, Object obj = null)
    {
        if (!setting)
        {
            Debug.LogError("[MLogger] 未配置，请配置LogSetting，并放置在Resources文件夹下");
            return;
        }
        
        //展示等级/分类样式
        bool showLevel = setting.type == LoggerType.Level;

        //是否能输出
        bool canLog = LogFilter(level) && LogFilter(category);
        if (!canLog) return;
        
        //输出
        var colorType = showLevel ? GetColor(level) : GetColor(category);
        
        message = LogMessage(message,setting.levelName[level],setting.categoryName[category],colorType);
        
        //低级别
        if((level & (LogLevel.Trace | LogLevel.Debug | LogLevel.Info)) != 0) Debug.Log(message,obj);
        //中级别
        if((level & LogLevel.Warning) != 0) Debug.LogWarning(message,obj);
        //高级别
        if((level & (LogLevel.Error | LogLevel.Fatal)) != 0) Debug.LogError(message,obj);
    }
    private static bool LogFilter(LogLevel level)
    {
        LogLevel levelFilter = setting.LogLevel;
        return (level & levelFilter) != 0;
    }
    private static bool LogFilter(LogCategory category)
    {
        if(category == LogCategory.None) return true;
        LogCategory categoryFilter = setting.LogCategory;
        return (category & categoryFilter) != 0;
    }
    private static string GetColor(LogLevel level)
    {
        if(!setting.levelStyle.TryGetValue(level, out var color)) color = MLoggerSetting.DefaultStyle;
        return color;
    }
    private static string GetColor(LogCategory category)
    {
        //字典中找得到,按照字典配置,否则为白色
        if(!setting.categoryStyle.TryGetValue(category, out var color)) color = MLoggerSetting.DefaultStyle;
        return color;
    }
    private static string LogMessage(string message, string level,string category, string color)
    {
        log.Clear();
        log.Append(color);
        log.Append("[");
        log.Append(level);
        log.Append("] ");
        log.Append("(");
        log.Append(category);
        log.Append("): ");
        log.Append(message);
        log.Append("</color>");
        
        return log.ToString();
    }
}