using Script.Data;

namespace Script.Interface
{
    public interface ILogSink
    {
        //in传一个只读引用
        void Emit(in LogEntry entry);
    }
}