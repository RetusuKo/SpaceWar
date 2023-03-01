using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static void PauseGame(bool pause)
    {
        Time.timeScale = !pause ? 1.0f : 0.0f;
        PlayerInfo.CanMove = !pause;
    }    
}
