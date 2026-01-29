using System;

namespace MLogger.Runtime.Data
{
    [Flags]
    public enum LogLevel
    {
        Trace = 1 << 0,
        Debug = 1 << 1,
        Info =  1 << 2,
        Warning =  1 << 3,
        Error = 1 << 4,
        Fatal =  1 << 5,
        
        All = Trace | Debug | Info | Warning | Error | Fatal
    }
}