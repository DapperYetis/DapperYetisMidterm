using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingLogic : MonoBehaviour
{
    public void StopPause()
    {
        UIManager.instance.StartsPlaying();
    }

    public void AllowPause()
    {
        UIManager.instance.StopsPlaying();
    }
}
