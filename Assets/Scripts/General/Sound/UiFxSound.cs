using System.Collections;
using System.Collections.Generic;
using General.Sound;
using UnityEngine;

public class UiFxSound : MonoBehaviour
{
    public void PlayUIPop()
    {
        AudioManager.Instance.PlayOneShot("UiTap");
    }
}
