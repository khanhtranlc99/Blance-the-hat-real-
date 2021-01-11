#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// This is sample, DO NOT MODIFY IT -> Duplicate then change namespace new game ex: Yogame.DancingBall
/// </summary>
[CustomEditor(typeof(StagesAsset))]
public class StagesAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StagesAsset myScript = (StagesAsset)target;

        if (GUILayout.Button("Order by Index"))
        {
            myScript.OderByIndex();
        }
        if (GUILayout.Button("Add 100 Levels"))
        {
            myScript.AddLevel(100);
        }

        if (GUILayout.Button("Update Index"))
        {
            myScript.UpdateIndex();
        }

        if (GUILayout.Button("Update Cost"))
        {
            myScript.UpdateIndex();
            myScript.OderByIndex();

            for (int i = 0; i < myScript.list.Count; i++)
            {
                //if (i < 3)
                    myScript.list[i].unlockType = UnlockType.Star;
                //else if (i % 3 == 2)
                //    myScript.list[i].unlockType = UnlockType.Star;
                //else if (i % 3 == 1)
                //    myScript.list[i].unlockType = UnlockType.Ads;
                //else
                //    myScript.list[i].unlockType = UnlockType.Gold;
                myScript.list[i].unlockBy = UnlockType.None;
            }

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
