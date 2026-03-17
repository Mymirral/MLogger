using System;
using System.Collections;
using System.Linq;
using MirralLogger.Runtime.Core;
using MirralLogger.Runtime.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


    public class DebugTest : MonoBehaviour
    {
        public LogLevel level = LogLevel.Info;
        public LogCategory category;
        
        public TMP_InputField  inputField;
        public Button button;
        public TMP_Dropdown levelDropdown;
        public TMP_Dropdown categoryDropdown;
        public Toggle toggle;

        private string msg;

        private bool isContinuous;

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

            if (!toggle)
            {
                MLogger.Log("Component missing",LogLevel.Warning,LogCategory.None,this);
            }
            else
            {
                toggle.onValueChanged.AddListener(val => isContinuous = val);
            }
        }

        public void DebugCategory(string msg)
        {
            if (isContinuous)
            {
                StartCoroutine(DebugLoop(msg));
                return;
            }
            MLogger.Log(msg, level, category, this);
        }

        private IEnumerator DebugLoop(string msg)
        {
            while (isContinuous)
            {
                MLogger.Log(msg, level, category, this);
                yield return null;
            }
        }
    }
