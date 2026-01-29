using System;
using UnityEngine;

namespace MLogger.Runtime.Data
{
    [Serializable]
    public class LogLevelStyle
    {
        public LogLevel level;
        public Color colors = Color.green;
    }
    
    
    [Serializable]
    public class LogCategoryStyle
    {
        public LogCategory category;
        public Color colors = Color.green;
    }
}