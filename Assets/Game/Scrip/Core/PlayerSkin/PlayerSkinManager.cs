using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] private SkeletonMecanim charSke;
    private const String charPrefix = "char";

    private void OnEnable()
    {
        GameStateManager.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameState current, GameState last, object data)
    {
        if (current == GameState.Init)
        {
            var charSkinIndex = DataManager.gameData.user.skinIndex;
            SetSkin(charSkinIndex);
        }
    }

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
        if (index > numOfSkin.Count) return;
        charSke.Skeleton.SetSkin($"{charPrefix}{index}");
    }
    
    
}
