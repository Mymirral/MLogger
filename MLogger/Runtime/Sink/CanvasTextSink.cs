using System;
using System.Collections.Generic;
using System.Text;
using MLogger.Runtime.Data;
using MLogger.Runtime.Interface;
using TMPro;
using UnityEngine;

namespace MLogger.Runtime.Sink
{
    public class CanvasTextSink : MonoBehaviour, ILogSink
    {
        [SerializeField] private int maxLogCount = 10;
        [SerializeField] private TMP_Text canvasTexts;
        
        private static readonly StringBuilder msgBuilder  =  new(2048);
        private static readonly Queue<LogEntry> logQueue = new();
        
        private void OnEnable()
        {
            //添加接收者
            Core.MLogger.AddSink(this);
        }
        private void OnDisable()
        {
            //移除接收
            Core.MLogger.RemoveSink(this);
        }
        public void Emit(in LogEntry entry)
        {
            var setting = Core.MLogger.setting;
            
            //日志流输出，大于最大数量，删除最早的一条log
            while(logQueue.Count >= maxLogCount) logQueue.Dequeue();
            logQueue.Enqueue(entry);
            
            //展示等级/分类样式

            //清除stringBuilder
            msgBuilder.Clear();
            foreach (var log in logQueue)
            {
                var message = log.Message;
                var level = log.Level;
                var category = log.Category;
            
                //展示等级/分类样式
                var colorType = setting.IsShowLevel() ? setting.GetColorText(level) : setting.GetColorText(category);
            
                msgBuilder.Append(colorType);
                msgBuilder.Append("[");
                msgBuilder.Append(level);
                msgBuilder.Append("] ");
                msgBuilder.Append("(");
                msgBuilder.Append(category);
                msgBuilder.Append(") ");
                msgBuilder.Append(message);
                msgBuilder.Append("</color>");
            
                msgBuilder.Append('\n');  //换行
            }
            
            canvasTexts.text = msgBuilder.ToString();
        }
        
        public void Clear()
        {
            logQueue.Clear();
            msgBuilder.Clear();
            canvasTexts.text = string.Empty;
        }
    }
}