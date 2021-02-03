using UnityEngine;

[CreateAssetMenu(fileName = "SkinsAsset", menuName = "DataAsset/SkinsAsset")]
public class SkinAsset : BaseAsset<SkinData>
{

}

[System.Serializable]
public class SkinData : SaveData
{
    [Header("GameData")]
    public Sprite thumbnail;
}
