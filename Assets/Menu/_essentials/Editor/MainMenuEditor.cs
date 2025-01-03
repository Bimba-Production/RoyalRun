using UnityEditor;
using UnityEngine;

namespace _Scripts
{
    [CustomEditor(typeof(MainMenuManager))]
    public class MainMenuEditor : Editor
    {
        private SerializedProperty mainColor;

        SerializedObject soTarget;
        MainMenuManager mainMenuManager;
        Texture2D texturePanel1;

        private void OnEnable()
        {
            mainMenuManager = (MainMenuManager)target;
            soTarget = new SerializedObject(target);

            texturePanel1 = Resources.Load<Texture2D>("img/InspectorBanner1");

            mainColor = soTarget.FindProperty("mainColor");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.DrawPreviewTexture(new Rect(18, 10, 520, 80), texturePanel1, mat: null, ScaleMode.ScaleToFit);
            EditorGUILayout.Space(91);

            base.OnInspectorGUI();
            mainMenuManager.UIEditorUpdate();
        }
    }
}
