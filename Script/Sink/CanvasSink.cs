using System;
using System.Text;
using Script.Data;
using Script.Interface;
using TMPro;
using UnityEngine;

namespace Script.Sink
{
    public class CanvasSink : MonoBehaviour, ILogSink
    {
        [SerializeField] private TMP_Text canvasTexts;
        
        //保证多线程，每个线程都会自己新建一个builder,并只会用自己的builder
        private static readonly StringBuilder msgBuilder  =  new();

        private void OnEnable()
        {
            //添加接收者
            MLogger.AddSink(this);
        }
        private void OnDisable()
        {
            //移除接收
            MLogger.RemoveSink(this);
        }
        public void Emit(in LogEntry entry)
        {
            var setting = MLogger.setting;
            
            var message = entry.Message;
            var level = entry.Level;
            var category = entry.Category;
            
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
            
            //展示等级/分类样式
            canvasTexts.text = msgBuilder.ToString();
        }

        public void Clear()
        {
            msgBuilder.Clear();
            canvasTexts.text = string.Empty;
        }
    }
}