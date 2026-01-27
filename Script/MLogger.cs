using System.Text;
using Script;
using Script.Data;
using UnityEngine;

public static class MLogger
{
    public static MLoggerSetting setting;

    static readonly StringBuilder log = new();

    public static void BindSetting(MLoggerSetting asset) => setting = asset;
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
        
        message = LogMessage(message,level.ToString(),category.ToString(),colorType);
        Debug.Log(message,obj);
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
        if(setting.levelStyle.TryGetValue(level, out var color)) color = Color.white;
        return ColorText(color);
    }
    private static string GetColor(LogCategory category)
    {
        if(!setting.categoryStyle.TryGetValue(category, out var color)) color = Color.white;
        return ColorText(color);
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
    private static string ColorText(Color color) => $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>";
}