using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatePopup : MonoBehaviour
{

    public void Open()
    {
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
    }

    public void Rate()
    {
        Time.timeScale = 1;

#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + Application.identifier);
#endif

    }

    public void Later()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }
}
