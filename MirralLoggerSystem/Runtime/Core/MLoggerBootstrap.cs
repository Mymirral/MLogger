using MirralLoggerSystem.Runtime.Sink;
using UnityEngine;

namespace MirralLoggerSystem.Runtime.Core
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