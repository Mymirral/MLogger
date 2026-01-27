using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Script
{
    public class MLoggerEditorSetting : EditorWindow
    {
        
        MLoggerSetting setting;
    
        [MenuItem("Tools/Logger Output Setting")]
        static void OpenSettingWindow()
        {
            GetWindow<MLoggerEditorSetting>("Logger Output Setting");
        }

        private void OnEnable()
        {
            FindAsset();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            DrawSettingField();
            DrawFindAssetBtn();
            EditorGUILayout.EndVertical();
            
            DrawAssetField();
            DrawRefreshBtn();
        }
        private void DrawSettingField()
        {
            EditorGUILayout.ObjectField("配置文件",setting, typeof(MLoggerSetting), false);
        }

        private void DrawFindAssetBtn()
        {
            GUILayout.Space(10);
            //绘制按钮
            if (GUILayout.Button("查找MLogger配置文件"))
            {
                FindAsset();
            }
        }

        private void FindAsset()
        {
            var guids = AssetDatabase.FindAssets("t:MLoggerSetting");

            if (guids.Length == 0)
            {
                Debug.LogError(" 未找到 MLoggerSetting");
                return;
            }

            if (guids.Length > 1)
            {
                Debug.LogError(" 找到多个 MLoggerSetting，请保证唯一");
                return;
            }

            //根据路径绑定对象
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var setting = AssetDatabase.LoadAssetAtPath<MLoggerSetting>(path);

            this.setting = setting;
            MLogger.BindSetting(setting);
        }

        Editor editor;
        private void DrawAssetField()
        {
            GUILayout.Space(20);
            if (setting)
            {
                GUIStyle centeredStyle = new GUIStyle(GUI.skin.label); 
                centeredStyle.alignment = TextAnchor.MiddleCenter;
                centeredStyle.fontSize = 16; 
                centeredStyle.fontStyle = FontStyle.Bold;
                
                GUILayout.Label("输出配置" , centeredStyle, GUILayout.Height(30));
                GUILayout.Space(10);
                
                //编辑器为空/编辑器的绘制对象不为配置
                if (!editor || editor.target != setting)
                {
                    editor = Editor.CreateEditor(setting);
                }
                
                editor.OnInspectorGUI();
            }
        }
        
        private void DrawRefreshBtn()
        {
            GUILayout.Space(20);

            if (GUILayout.Button("刷新"))
            {
                setting.Init();
            }
        }
    }
}