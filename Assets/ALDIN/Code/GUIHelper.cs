using UnityEditor;
using UnityEngine;
using System;

public static class GUIHelper
{
    public static void GroupedBoxFielWithLabel(Action action, string groupLabel, GUIStyle style, GUIStyle style2)
    {
        if (action != null)
        {
            EditorGUILayout.BeginVertical(style2);
            action();
            EditorGUILayout.EndVertical();

            Rect rt = GUILayoutUtility.GetLastRect();
            GUI.Label(new Rect(new Vector2(rt.xMin + 5, rt.yMin - 5), new Vector2(100, 15)), new GUIContent(groupLabel), style);
        }
    }

    public static void GroupedBoxVertical(Action action)
    {
        if (action != null)
        {
            EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.box));
            action();
            EditorGUILayout.EndVertical();
        }
    }

    public static void GroupedBoxHorizontal(Action action)
    {
        if (action != null)
        {
            EditorGUILayout.BeginHorizontal(new GUIStyle(GUI.skin.box));
            action();
            EditorGUILayout.EndHorizontal();
        }
    }

    public static void BoolFieldWithLabel(SerializedProperty property, string label)
    {
        EditorGUILayout.BeginHorizontal();
        property.boolValue = EditorGUILayout.Toggle(label, property.boolValue);
        EditorGUILayout.EndHorizontal();
    }

    public static void FloatRangeFieldWithLabel(SerializedProperty property, string label, float minRange, float maxRange)
    {
        EditorGUILayout.BeginHorizontal();
        property.floatValue = EditorGUILayout.Slider(label, property.floatValue, minRange, maxRange);
        EditorGUILayout.EndHorizontal();
    }

    public static void VectorFieldWithLabel(SerializedProperty property, string label)
    {
        EditorGUILayout.BeginHorizontal();
        property.vector3Value = EditorGUILayout.Vector3Field(label, property.vector3Value);
        EditorGUILayout.EndHorizontal();
    }

    public static void ColorFieldWithLabel(SerializedProperty property, string label)
    {
        EditorGUILayout.BeginHorizontal();
        property.colorValue = EditorGUILayout.ColorField(label, property.colorValue);
        EditorGUILayout.EndHorizontal();
    }

    public static void PropertyFieldWithLabel(SerializedProperty property, string label)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(property, new GUIContent(label));
        EditorGUILayout.EndHorizontal();
    }
}