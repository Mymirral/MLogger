using MirralLogger.Runtime.Sink;
using UnityEngine;

namespace MirralLogger.Runtime.Core
{
    public static class MLoggerBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            var logfileSink = new LogFileSink();
            logfileSink.Open();
            
            // 监听退出
            Application.quitting += MLogger.Close;
        }
    }
}