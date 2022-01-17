using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightLightController : MonoBehaviour
{
    public float DayNightTurningSpeed = 10f;
    //Transform Trans_DirectionLight_Day;
    //Transform Trans_DirectionLight_Night;

    void Awake()
    {
        //Trans_DirectionLight_Day = transform.GetChild(0);
        //Trans_DirectionLight_Night = transform.GetChild(1);
    }

    void FixedUpdate()
    {
        transform.Rotate(DayNightTurningSpeed * Time.fixedDeltaTime,0f, 0f);
    }
}
