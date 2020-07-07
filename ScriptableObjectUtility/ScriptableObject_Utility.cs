/*
 * =========================================================
 *     Class to contain utility functions pertaining to
 *     ScriptableObject Asset Managment.
 * 
 *     William Pyle 2020 (wpyle.com)
 * =========================================================
 */

using System.Collections.Generic;
using UnityEditor;

public static class ScriptableObject_Utility
{
    /// <summary>
    /// Finds all scriptable objects of type in project folder and returns them as a List of ScriptableObjects
    /// </summary>
    /// <typeparam name="T">Type to search for</typeparam>
    /// <typeparam name="folderPath">A specific folder to look in</typeparam>
    /// <returns>List of scriptable objects as passed in type T</returns>
    public static List<T> GetAllAssetsOfType<T>(string folderPath = "Assets") where T : UnityEngine.ScriptableObject
    {
        List<T> scriptableObjects = new List<T>();

        string[] p = { folderPath };
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)), p);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T so = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;

            if (so != null) scriptableObjects.Add(so);
        }

        return scriptableObjects;
    }
}