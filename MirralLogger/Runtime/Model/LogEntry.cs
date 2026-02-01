using UnityEngine;

namespace MirralLogger.Runtime.Model
{
    // 使用Struct，保证无GC
    public readonly struct LogEntry
    {
        //不允许二次修改，避免GC
        public readonly string Message;
        public readonly LogLevel Level;
        public readonly LogCategory Category;
        public readonly Object Context;

        public LogEntry(string message, LogLevel level, LogCategory category, Object context)
        {
            Message = message;
            Level = level;
            Category = category;
            Context = context;
        }
    }
}