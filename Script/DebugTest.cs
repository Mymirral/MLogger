using Script.Data;
using Sirenix.OdinInspector;
using UnityEditor.Scripting;
using UnityEngine;

namespace Script
{
    public class DebugTest : MonoBehaviour
    {
        public LogLevel level = LogLevel.Info;
        public LogCategory category;
        
        [Button("测试输出")]
        public void DebugCategory(string msg)
        {
            MLogger.Log(msg, level, category,this);
        }
    }
}