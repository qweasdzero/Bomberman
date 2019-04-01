using UnityEditor;
using UnityEngine;

namespace SG1.Editor
{
    [CustomPropertyDrawer(typeof(InspectorReadOnly))]
    public class InspectorReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (InspectorReadOnly) attribute;
            switch (attr.Mode)
            {
                case InspectorDiplayMode.AlwaysEnabled:
                    GUI.enabled = false;
                    EditorGUI.PropertyField(position, property, label, true);
                    GUI.enabled = true;
                    break;
                case InspectorDiplayMode.DisabledInPlayMode when !Application.isPlaying:
                    GUI.enabled = false;
                    EditorGUI.PropertyField(position, property, label, true);
                    GUI.enabled = true;
                    break;
                case InspectorDiplayMode.DisabledInPlayMode:
                    EditorGUI.PropertyField(position, property, label, true);
                    break;
                case InspectorDiplayMode.EnabledInPlayMode when Application.isPlaying:
                    GUI.enabled = false;
                    EditorGUI.PropertyField(position, property, label, true);
                    GUI.enabled = true;
                    break;
                case InspectorDiplayMode.EnabledInPlayMode:
                    EditorGUI.PropertyField(position, property, label, true);
                    break;
            }
        }
    }
}