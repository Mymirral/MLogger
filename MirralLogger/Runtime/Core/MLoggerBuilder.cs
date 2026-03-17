using MirralLogger.Runtime.Config;
using MirralLogger.Runtime.Sink;

namespace MirralLogger.Runtime.Core
{
    public static class MLoggerBuilder
    {
        public static void BindSetting(MLoggerSetting setting)
        {
            MLogger.BindSetting(setting);
        }
        
        public static void AddSink(ILogSink sink)
        {
            MLogger.AddSink(sink);
        }
        
        public static void RemoveSink(ILogSink sink)
        {
            MLogger.RemoveSink(sink);
        }
        
        public static void Close()
        {
            MLogger.Close();
        }
    }
}