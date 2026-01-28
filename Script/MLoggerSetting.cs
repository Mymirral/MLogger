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

        [LabelText("输出Log分类")] public LogCategory LogCategory = LogCategory.None;

        #region 样式

        [Space, Title("样式", null, TitleAlignments.Centered)] 
        
        [LabelText("等级与颜色")]
        public List<LogLevelStyle> logLevelStyle = new();
        public readonly Dictionary<LogLevel, string> levelStyleColorText = new();
        public readonly Dictionary<LogLevel, Color> levelStyleColor = new();
        public readonly Dictionary<LogLevel, string> levelName = new();
        
        [LabelText("分类与颜色")]
        public List<LogCategoryStyle> logCategoryStyle = new();
        public readonly Dictionary<LogCategory, string> categoryStyleColorText = new();
        public readonly Dictionary<LogCategory, Color> categoryStyleColor = new();
        public readonly Dictionary<LogCategory, string> categoryName = new();
        
        public static readonly string DefaultStyle = "<color=#FFFFFF}>";
        #endregion

        [Space, Title("显示样式", null, TitleAlignments.Centered)]
        
        public LoggerType type;

        public bool showInGameScene;
        
        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            levelStyleColorText.Clear();
            categoryStyleColorText.Clear();

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
                    levelStyleColorText.TryAdd(style.level, ColorText(style.colors));
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
                    categoryStyleColorText.TryAdd(style.category, ColorText(style.colors));
                }
            }

            #endregion
            
            //字典生成
            levelName.Clear();
            categoryName.Clear();
            
            InitLevelNameDictionary();
            InitCategoryNameDictionary();
        }

        /// <summary>
        /// 配置默认等级样式
        /// </summary>
        void AddDefaultLevelStyle()
        {
            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
            {
                levelStyleColorText.TryAdd(level, ColorText(Color.green));
            }
        }

        /// <summary>
        /// 配置默认分类样式
        /// </summary>
        void AddDefaultLogCategoryStyle()
        {
            foreach (LogCategory category in Enum.GetValues(typeof(LogCategory)))
            {
                categoryStyleColorText.TryAdd(category, ColorText(Color.green));
            }
        }

        //建立查表信息，避免GC
        void InitLevelNameDictionary()
        {
            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
            {
                levelName.TryAdd(level,level.ToString());
            }
        }

        void InitCategoryNameDictionary()
        {
            foreach (LogCategory category in Enum.GetValues(typeof(LogCategory)))
            {
                categoryName.TryAdd(category, category.ToString());
            }
        }

        private string ColorText(Color color) => $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>";
    }
}