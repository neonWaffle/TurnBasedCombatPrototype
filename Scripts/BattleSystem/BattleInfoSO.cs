using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleInfo", menuName = "ScriptableObjects/BattleInfo")]
public class BattleInfoSO : ScriptableObject
{
    [SerializeField] string sceneName;
    public string SceneName => sceneName;
    [SerializeField] Sprite icon;
    public Sprite Icon => icon;
}
