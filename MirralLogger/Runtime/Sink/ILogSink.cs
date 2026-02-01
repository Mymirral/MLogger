using MirralLogger.Runtime.Model;

namespace MirralLogger.Runtime.Sink
{
    public interface ILogSink
    {
        public void Open();
        
        public void Close();
        
        //in传一个只读引用
        void Emit(in LogEntry entry);
    }
}