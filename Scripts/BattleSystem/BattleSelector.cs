using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelector : MonoBehaviour
{
    BattleInfoSO selectedBattleInfoSO;

    public void SelectBattle(BattleInfoSO battleInfoSO)
    {
        selectedBattleInfoSO = battleInfoSO;
    }

    public void StartBattle()
    {
        SceneLoader.Instance.LoadScene(selectedBattleInfoSO.SceneName);
    }

    public void LoadMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
