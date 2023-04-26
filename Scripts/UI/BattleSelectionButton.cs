using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSelectionButton : MonoBehaviour
{
    [SerializeField] BattleInfoSO battleInfoSO;
    BattleSelector battleSelector;
    [SerializeField] Image battleIcon;

    void Awake()
    {
        battleIcon.sprite = battleInfoSO.Icon;
        battleSelector = transform.root.GetComponent<BattleSelector>();
    }

    public void SelectBattle()
    {
        battleSelector.SelectBattle(battleInfoSO);
    }
}
