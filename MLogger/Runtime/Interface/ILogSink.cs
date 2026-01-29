using MLogger.Runtime.Data;

namespace MLogger.Runtime.Interface
{
    public interface ILogSink
    {
        //in传一个只读引用
        void Emit(in LogEntry entry);
    }
}