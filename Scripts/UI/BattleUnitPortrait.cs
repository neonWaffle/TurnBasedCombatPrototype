using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnitPortrait : MonoBehaviour
{
    [SerializeField] Image unitIcon;
    [SerializeField] Vector3 currentScale = new Vector3(1.2f, 1.2f, 1.2f);
    Vector3 initialScale = Vector3.one;

    public void Set(BattleUnit unit, bool isUnitsTurn)
    {
        transform.localScale = isUnitsTurn ? currentScale : initialScale;
        unitIcon.sprite = unit.Data.Icon;
    }
}
