using System;
using System.Linq;
using MirralLoggerSystem.Runtime.Core;
using MirralLoggerSystem.Runtime.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MirralLoggerSystem
{
    public class DebugTest : MonoBehaviour
    {
        public LogLevel level = LogLevel.Info;
        public LogCategory category;
        
        public TMP_InputField  inputField;
        public Button button;
        public TMP_Dropdown levelDropdown;
        public TMP_Dropdown categoryDropdown;

        private string msg;

        private void Start()
        {
            if(!inputField) MLogger.Log("组件缺失",LogLevel.Warning,LogCategory.None,this);
            
            if(!levelDropdown) MLogger.Log("组件缺失",LogLevel.Warning,LogCategory.None,this);
            else
            {
                levelDropdown.AddOptions(Enum.GetNames(typeof(LogLevel)).ToList());
                levelDropdown.onValueChanged.AddListener(val => level = (LogLevel)((val+1) * Mathf.Sqrt(2f)));
            }
            
            if(!categoryDropdown) MLogger.Log("组件缺失",LogLevel.Warning,LogCategory.None,this);
            else
            {
                categoryDropdown.AddOptions(Enum.GetNames(typeof(LogCategory)).ToList());
                categoryDropdown.onValueChanged.AddListener(val => category = (LogCategory)((val) * Mathf.Sqrt(2f)));
            }

            if (!button)
            {
                MLogger.Log("组件缺失",LogLevel.Warning,LogCategory.None,this);
            }
            else
            {
                button.onClick.AddListener(() => {
                    msg =  inputField.text;
                    DebugCategory(msg);
                });
            }
        }

        public void DebugCategory(string msg)
        {
            MLogger.Log(msg, level, category, this);
        }
    }
}