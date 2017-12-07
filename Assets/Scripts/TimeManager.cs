using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public static void ChangeTimeFactor(float timeFactor)
    {
        Time.timeScale = timeFactor;
    }

    public static void RestoreTimeFactor()
    {
        Time.timeScale = 1f;
    }

    public static void StopTime()
    {
        Time.timeScale = 0f;
    }

}
