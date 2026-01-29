using System;
using System.Collections.Generic;
using MLogger.Runtime.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MLogger.Runtime.Config
{
    [CreateAssetMenu(fileName = "MLoggerSetting", menuName = "MirralDevTool/MLoggerSetting")]
    public class MLoggerSetting : ScriptableObject
    {

        #region 输出设置
        [LabelText("输出Log等级")] public LogLevel LogLevel = LogLevel.Trace;

        [LabelText("输出Log分类")] public LogCategory LogCategory = LogCategory.None;
        #endregion

        #region 样式

        [Space, Title("样式", null, TitleAlignments.Centered)] 
        
        [LabelText("等级与颜色")]
        public List<LogLevelStyle> logLevelStyle = new();

        private readonly Dictionary<LogLevel, string> levelStyleColorText = new();
        private readonly Dictionary<LogLevel, Color> levelStyleColor = new();
        public readonly Dictionary<LogLevel, string> levelName = new();
        
        [LabelText("分类与颜色")]
        public List<LogCategoryStyle> logCategoryStyle = new();

        private readonly Dictionary<LogCategory, string> categoryStyleColorText = new();
        private readonly Dictionary<LogCategory, Color> categoryStyleColor = new();
        public readonly Dictionary<LogCategory, string> categoryName = new();

        private static readonly string DefaultColorStyleText = "<color=#FFFFFF}>";
        private static readonly Color DefaultColorStyle = Color.white;
        #endregion

        [Space, Title("显示样式", null, TitleAlignments.Centered)]
        
        public LoggerType type;

        public bool showInGameScene;
        
        private void OnEnable()
        {
            Init();
        }

        #region 初始化
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
                AddSettingLevelStyle();
            }

            #endregion

            #region 分类配置

            //如果没有配置，添加默认配置
            if (logCategoryStyle.Count == 0)
            {
                AddDefaultLogCategoryStyle();
            }
            else
            {
                AddSettingLogCategoryStyle();
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
                levelStyleColor.TryAdd(level, Color.green);
            }
        }

        void AddSettingLevelStyle()
        {
            foreach (var style in logLevelStyle)
            {
                levelStyleColorText.TryAdd(style.level, ColorText(style.colors));
                levelStyleColor.TryAdd(style.level, style.colors);
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
                categoryStyleColor.TryAdd(category, Color.green);
            }
        }

        void AddSettingLogCategoryStyle()
        {
            foreach (var style in logCategoryStyle)
            {
                categoryStyleColorText.TryAdd(style.category, ColorText(style.colors));
                categoryStyleColor.TryAdd(style.category, style.colors);
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

        #endregion
        
        #region 查询方法

        public bool IsShowLevel() => type == LoggerType.Level;
        
        /// <summary>
        /// 筛选设置的可输出等级
        /// </summary>
        /// <param name="level">等级</param>
        /// <returns>是否可输出</returns>
        public bool LogFilter(LogLevel level)
        {
            LogLevel levelFilter = LogLevel;
            return (level & levelFilter) != 0;
        }
        
        /// <summary>
        /// 筛选可输出分类，类别为None，不参与输出
        /// </summary>
        /// <param name="category">类别</param>
        /// <returns>是否可输出</returns>
        public bool LogFilter(LogCategory category)
        {
            if (category == LogCategory.None) return true;
            LogCategory categoryFilter = LogCategory;
            return (category & categoryFilter) != 0;
        }
        
        /// <summary>
        /// 获取输出等级的颜色字符串
        /// </summary>
        /// <param name="level">等级</param>
        /// <returns>颜色字符串</returns>
        public string GetColorText(LogLevel level)
        {
            var color = levelStyleColorText.GetValueOrDefault(level, DefaultColorStyleText);
            return color;
        }
        
        /// <summary>
        /// 获取输出分类的颜色字符串
        /// </summary>
        /// <param name="category">类别</param>
        /// <returns>颜色字符串</returns>
        public string GetColorText(LogCategory category)
        {
            //字典中找得到,按照字典配置,否则为白色
            var color = categoryStyleColorText.GetValueOrDefault(category, DefaultColorStyleText);
            return color;
        }
        
        /// <summary>
        /// 获取输出等级的颜色
        /// </summary>
        /// <param name="level">等级</param>
        /// <returns>颜色</returns>
        public Color GetColor(LogLevel level)
        {
            var color = levelStyleColor.GetValueOrDefault(level, DefaultColorStyle);
            return color;
        }

        /// <summary>
        /// 获取输出分类的颜色
        /// </summary>
        /// <param name="category">分类</param>
        /// <returns>颜色</returns>
        public Color GetColor(LogCategory category)
        {
            var color = categoryStyleColor.GetValueOrDefault(category, DefaultColorStyle);
            return color;
        }

        #endregion
        
        private string ColorText(Color color) => $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>";
    }
}