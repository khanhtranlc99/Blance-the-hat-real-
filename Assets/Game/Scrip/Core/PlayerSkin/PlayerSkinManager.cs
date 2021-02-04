using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] private SkeletonMecanim charSke;
    private const String charPrefix = "char";

    private void Awake()
    {
        SkinAsset.OnChanged += OnSkinChange;
    }

    private void OnSkinChange(SkinData current, List<SkinData> list)
    {
        SetSkin(current.index);
    }

    // private void OnEnable()
    // {
    //     GameStateManager.OnStateChanged += OnStateChanged;
    // }
    //
    // private void OnStateChanged(GameState current, GameState last, object data)
    // {
    //     if (current == GameState.Init)
    //     {
    //         SetSkin(DataManager.CurrentSkin.index);
    //     }
    // }

    private void GetSkinNames()
    {
        var listOfSkins = charSke.SkeletonDataAsset.GetSkeletonData(false).Skins;
        foreach (var skin in listOfSkins)
        {
            Debug.Log(skin.Name);
        }
    }

    private void SetSkin(int index)
    {
        var numOfSkin = charSke.SkeletonDataAsset.GetSkeletonData(false).Skins;
        if (index >= numOfSkin.Count) return;
        // charSke.Skeleton.SetSkin($"{charPrefix}{index + 1}");

        var ske = charSke.Skeleton;
        var newSkin = new Spine.Skin("new_skin");
        newSkin.AddSkin(ske.Data.FindSkin($"{charPrefix}{index + 1}"));
        ske.SetSkin(newSkin);
        ske.SetToSetupPose();
    }
}