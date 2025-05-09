using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


//addressable≥°æ∞–≈œ¢
[CreateAssetMenu(menuName = "GameScene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType;
    public AssetReference sceneReference;
}
