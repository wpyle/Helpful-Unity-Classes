/*
 * =========================================================
 *     Exposes methods as buttons in the Unity Inspector.
 *     Does not work with methods without default params.
 * 
 *     William Pyle 2020 (wpyle.com)
 * =========================================================
 */

using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// Add to methods to expose a button in the editor, that when pressed, executes the method. 
/// WARNING: Does not work on methods that require params
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute
{
    public string ButtonText { get; } = null;
    public int SpaceBefore { get; }
    public Color Color { get; } = GUI.backgroundColor;

    /// <summary>
    ///  Button with custom text and set color.  NOTE: Only mark one color property as true. Later ones listed will overwrite previous ones.
    /// </summary>
    public EditorButtonAttribute(string buttonText, int spaceBefore = 10, bool white = false, bool cyan = false, bool blue = false,
        bool yellow = false, bool green = false, bool magenta = false, bool red = false, bool gray = false, bool black = false)
    {
        this.ButtonText = buttonText;
        this.SpaceBefore = spaceBefore;

        if (white) Color = Color.white;
        if (cyan) Color = Color.cyan;
        if (blue) Color = Color.blue;
        if (yellow) Color = Color.yellow;
        if (green) Color = Color.green;
        if (magenta) Color = Color.magenta;
        if (red) Color = Color.red;
        if (gray) Color = Color.gray;
        if (black) Color = Color.black;
    }
    /// <summary>
    /// Button with text as method name and set color.
    /// </summary>
    public EditorButtonAttribute(int spaceBefore = 10, bool white = false, bool cyan = false, bool blue = false,
        bool yellow = false, bool green = false, bool magenta = false, bool red = false, bool gray = false, bool black = false)
    {
        this.SpaceBefore = spaceBefore;

        if (white) Color = Color.white;
        if (cyan) Color = Color.cyan;
        if (blue) Color = Color.blue;
        if (yellow) Color = Color.yellow;
        if (green) Color = Color.green;
        if (magenta) Color = Color.magenta;
        if (red) Color = Color.red;
        if (gray) Color = Color.gray;
        if (black) Color = Color.black;
    }
    /// <summary>
    /// Button with custom text and custom color. No alpha.
    /// </summary>
    public EditorButtonAttribute(string buttonText, float colorR, float colorG, float colorB, int spaceBefore = 10)
    {
        this.ButtonText = buttonText;
        this.SpaceBefore = spaceBefore;
        this.Color = new Color(colorR, colorG, colorB);
    }
    /// <summary>
    /// Button with text as method name and custom color. No alpha.
    /// </summary>
    public EditorButtonAttribute(float colorR, float colorG, float colorB, int spaceBefore = 10)
    {
        this.SpaceBefore = spaceBefore;
        this.Color = new Color(colorR, colorG, colorB);
    }
    /// <summary>
    /// Button with custom text and custom color. With alpha.
    /// </summary>
    public EditorButtonAttribute(string buttonText, float colorR, float colorG, float colorB, float colorA, int spaceBefore = 10)
    {
        this.ButtonText = buttonText;
        this.SpaceBefore = spaceBefore;
        this.Color = new Color(colorR, colorG, colorB, colorA);
    }
    /// <summary>
    /// Button with text as method name and custom color. With alpha.
    /// </summary>
    public EditorButtonAttribute(float colorR, float colorG, float colorB, float colorA, int spaceBefore = 10)
    {
        this.SpaceBefore = spaceBefore;
        this.Color = new Color(colorR, colorG, colorB, colorA);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class EditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var mono = target as MonoBehaviour;

        if (mono == null) return;
        
        var methods = mono.GetType()
            .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                        BindingFlags.NonPublic)
            .Where(o => Attribute.IsDefined(o, typeof(EditorButtonAttribute)));

        foreach (var memberInfo in methods)
        {
            var attr = memberInfo.GetCustomAttribute(typeof(EditorButtonAttribute)) as EditorButtonAttribute;

            string buttonText = attr.ButtonText != null
                ? buttonText = attr.ButtonText
                : buttonText = memberInfo.Name;

            GUILayout.Space(attr.SpaceBefore);

            var defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = attr.Color;
            if (GUILayout.Button(buttonText))
            {
                var method = memberInfo as MethodInfo;

                var parameters = method?.GetParameters();
                var newCollection = new List<object>();
                foreach(var param in parameters)
                {
                    if (!param.HasDefaultValue)
                    {
                        Debug.LogError("EditorButtonAttribute only works on methods that contain exclusively parameters with default values." +
                                       " Parameter '" + param.Name + "' does not have a default value.");
                        return;
                    }
                    var newParam = param.DefaultValue;
                    newCollection.Add(newParam);
                }

                var objArry = newCollection.ToArray<object>();
               
                method?.Invoke(mono, objArry);
            }
            GUI.backgroundColor = defaultColor;
        }
    }
}
#endif