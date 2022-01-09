using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHitPauseManager : MonoBehaviour
{
    bool Is_Pausing = false;

    void Start()
    {
        Time.timeScale = 1f; // Sometime when player dead will make timeScale stuck between 0 and 1.
    }

    public void HitPauseStopTime(float stoptime,float restoretime)
    {
        if (Is_Pausing) return;
        StartCoroutine(StartPausing(stoptime,restoretime));
    }

    IEnumerator StartPausing(float stoptime, float restoretime)
    {
        Time.timeScale = 0f;
        Is_Pausing = true;
        yield return new WaitForSecondsRealtime(stoptime);

        if (restoretime == 0f) Time.timeScale = 1f; // Just in case restoretime is 0, which will cause mistake.

        while ( Time.timeScale < 1 )
        {
            Debug.Log("TimeScale:" + Time.timeScale);
            Time.timeScale = Mathf.Clamp(Time.timeScale + 1 / restoretime * Time.unscaledDeltaTime, 0f, 1f);
            yield return null;
        }
        Is_Pausing = false;
    }
}
