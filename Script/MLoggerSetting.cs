using System;
using System.Collections.Generic;
using Script.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script
{
    [CreateAssetMenu(fileName = "MLoggerSetting", menuName = "MirralDevTool/MLoggerSetting")]
    public class MLoggerSetting : ScriptableObject
    {
        [LabelText("输出Log等级")] public LogLevel LogLevel = LogLevel.Trace;
        
        [LabelText("输出Log分类")]
        public LogCategory LogCategory = LogCategory.None;

        #region 样式
        
        [Space,Title("样式",null,TitleAlignments.Centered)]
        [LabelText("等级与颜色")]
        public List<LogLevelStyle> logLevelStyle = new();
        public Dictionary<LogLevel, Color> levelStyle = new();
        
        [LabelText("分类与颜色")]
        public List<LogCategoryStyle> logCategoryStyle = new();
        public Dictionary<LogCategory, Color> categoryStyle = new();
        
        #endregion


        [Space, Title("显示样式", null, TitleAlignments.Centered)]
        public LoggerType type;

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            levelStyle.Clear();
            categoryStyle.Clear();
            
            #region 等级配置

            //如果没有配置，添加默认配置
            if (logLevelStyle.Count == 0)
            {
                AddDefaultLevelStyle();
            }
            else
            {
                foreach (var style in logLevelStyle)
                {
                    levelStyle.TryAdd(style.level, style.colors);
                }
            }

            #endregion

            #region 分类配置
            
            if (logCategoryStyle.Count == 0)
            {
                AddDefaultLogCategoryStyle();
            }
            else
            {
                foreach (var style in logCategoryStyle)
                {
                    categoryStyle.TryAdd(style.category, style.colors);
                }
            }
            #endregion
        }

        /// <summary>
        /// 配置默认等级样式
        /// </summary>
        void AddDefaultLevelStyle()
        {
            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
            {
                levelStyle.TryAdd(level, Color.green);
            }
        }

        /// <summary>
        /// 配置默认分类样式
        /// </summary>
        void AddDefaultLogCategoryStyle()
        {
            foreach (LogCategory category in Enum.GetValues(typeof(LogCategory)))
            {
                categoryStyle.TryAdd(category, Color.green);
            }
        }
    }
}