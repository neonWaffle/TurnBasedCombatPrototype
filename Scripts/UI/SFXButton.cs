using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXButton : MonoBehaviour
{
    public void Click()
    {
        AudioManager.Instance.PlayUISFX();
    }
}
