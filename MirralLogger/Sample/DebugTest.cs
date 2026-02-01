using System;
using System.Linq;
using MirralLogger.Runtime.Core;
using MirralLogger.Runtime.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MirralLogger
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
            if(!inputField) MLogger.Log("Component missing",LogLevel.Warning,LogCategory.None,this);
            
            if(!levelDropdown) MLogger.Log("Component missing",LogLevel.Warning,LogCategory.None,this);
            else
            {
                levelDropdown.ClearOptions();
                levelDropdown.AddOptions(Enum.GetNames(typeof(LogLevel)).ToList());
                levelDropdown.onValueChanged.AddListener(val => level = (LogLevel)(1 << val));
            }
            
            if(!categoryDropdown) MLogger.Log("Component missing",LogLevel.Warning,LogCategory.None,this);
            else
            {
                categoryDropdown.ClearOptions();
                categoryDropdown.AddOptions(Enum.GetNames(typeof(LogCategory)).ToList());
                categoryDropdown.onValueChanged.AddListener(val => category = (LogCategory)(val == 0 ? 0 : 1 << val));
            }

            if (!button)
            {
                MLogger.Log("Component missing",LogLevel.Warning,LogCategory.None,this);
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