using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class CreateData : Editor
{
    [MenuItem("Data/CreateKindInfo")]
    public static void CreateKindInfo()
    {
        string kindPath = "Assets/Resources/kind.asset";
        KindInfo obj = ScriptableObject.CreateInstance<KindInfo>();
        AssetDatabase.CreateAsset(obj, kindPath);
        EditorUtility.SetDirty(obj);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeObject = obj;
    }
}
