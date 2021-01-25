#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// This is sample, DO NOT MODIFY IT -> Duplicate then change namespace new game ex: Yogame.DancingBall
/// </summary>
[CustomEditor(typeof(ItemsAsset))]
public class ItemsAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemsAsset myScript = (ItemsAsset)target;

        if (GUILayout.Button("Order by Index"))
        {
            myScript.OderByIndex();
        }

        if (GUILayout.Button("Update Index"))
        {
            myScript.UpdateIndex();
        }

        if (GUILayout.Button("Update Cost"))
        {
            myScript.UpdateIndex();
            myScript.OderByIndex();

            myScript.UpdateCost();
            AssetDatabase.SaveAssets();
        }


        DrawDefaultInspector();

        if (GUILayout.Button("Update Id by Name"))
        {
            if (EditorUtility.DisplayDialog(
                "Update Id by Name",
                "Do you want update id by name!?",
                "Update",
                "Cancel"))
            {
                myScript.UpdateIdByName();
            }
        }

        if (GUILayout.Button("Reset Data"))
        {
            myScript.ResetData();
            AssetDatabase.SaveAssets();
        }
    }
}
#endif