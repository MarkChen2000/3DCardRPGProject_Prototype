using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHitPauseManager : MonoBehaviour
{
    public float RestorTimeScaleSpeed = 10f;

    bool Is_Pausing = false;
    //bool Is_Restoring = false;

    private void Start()
    {
        Time.timeScale = 1f; // Sometime when player dead will make timeScale stuck between 0 and 1.
    }

    public void HitPauseStopTime(float stopduration)
    {
        if (Is_Pausing) return;
        StartCoroutine(PausingTimeScale(stopduration));
        //Is_Restoring = true;
    }

    private void Update()
    {
        /*if (Is_Restoring)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += RestorTimeScaleSpeed * Time.deltaTime;
            }
            else
            {
                Time.timeScale = 1f;
                Is_Restoring = false;
            }
        }*/
    }

    IEnumerator PausingTimeScale(float stopduration)
    {
        Time.timeScale = 0f;
        Is_Pausing = true;
        while (Time.timeScale < 1f)
        {
            Time.timeScale += 1 / stopduration * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            yield return null;
        }
        //yield return new WaitForSecondsRealtime(duration);
        Is_Pausing = false;
        Time.timeScale = 1f;
    }

}
