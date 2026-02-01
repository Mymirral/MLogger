using MirralLogger.Runtime.Config;
using UnityEditor;
using UnityEngine;

namespace MirralLogger.Editor
{
    public class MLoggerEditorSetting : EditorWindow
    {
        MLoggerSetting setting;
        
        GameObject debugPanel;

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

            DrawGameDebugCanvas();
        }

        #region Draw
        
        private void DrawSettingField()
        {
            EditorGUILayout.ObjectField("Config Assets", setting, typeof(MLoggerSetting), false);
        }

        private void DrawFindAssetBtn()
        {
            GUILayout.Space(10);
            //绘制按钮
            if (GUILayout.Button("Find MLogger Config Asset"))
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

            Runtime.Core.MLogger.BindSetting(setting);
        }

        UnityEditor.Editor editor;

        private void DrawAssetField()
        {
            GUILayout.Space(20);
            if (setting)
            {
                GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
                centeredStyle.alignment = TextAnchor.MiddleCenter;
                centeredStyle.fontSize = 16;
                centeredStyle.fontStyle = FontStyle.Bold;

                GUILayout.Label("OutputConfig", centeredStyle, GUILayout.Height(30));
                GUILayout.Space(10);

                //编辑器为空/编辑器的绘制对象不为配置
                if (!editor || editor.target != setting)
                {
                    editor = UnityEditor.Editor.CreateEditor(setting);
                }

                editor.OnInspectorGUI();
            }
        }

        private void DrawRefreshBtn()
        {
            GUILayout.Space(20);

            if (GUILayout.Button("Refresh"))
            {
                setting.Init();
            }
        }

        private void DrawGameDebugCanvas()
        {
            GUILayout.Space(20);
            if (GUILayout.Button("Display/Hide GameDebug Panel"))
            {
                if(!debugPanel)
                {
                    //Try to Find Debug Panel
                    debugPanel = GameObject.Find("MDebugPanel");
                    if (!debugPanel)
                    {
                        var panel = Instantiate(setting.debugCanvas.gameObject);
                        panel.name = "MDebugPanel";
                        return;
                    }
                }
                debugPanel.SetActive(!debugPanel.activeSelf);
            }
        }
        
        #endregion
        
    }
}