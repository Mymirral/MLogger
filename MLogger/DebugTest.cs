using MLogger.Runtime.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MLogger
{
    public class DebugTest : MonoBehaviour
    {
        public LogLevel level = LogLevel.Info;
        public LogCategory category;
        
        [Button("测试输出")]
        public void DebugCategory(string msg)
        {
            Runtime.Core.MLogger.Log(msg,level,category,this);
        }
    }
}