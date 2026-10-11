using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    private Workstation workstation;
    private CameraSwitch cameraSwitch;
    private MinigameSaute minigameSaute;
    private MinigameChop minigameChop;
    private Coroutine currentMinigame;

    private void Start()
    {
        minigameSaute = GetComponent<MinigameSaute>();
        minigameChop = GetComponent<MinigameChop>();
    }

    public void Minigame_Switch(Workstation workstation)
    {
        StopMinigame();

        switch (workstation.MiniType)
        {
            case GameHandler.MiniType.Saute:
                currentMinigame = StartCoroutine(minigameSaute.Mini_Pan(workstation.pan));
                break;
            case GameHandler.MiniType.Chop:
                currentMinigame = StartCoroutine(minigameChop.Mini_Chop(workstation.knife));
                break;
           
        }
    }

    public void StopMinigame()
    {
        
        if (currentMinigame != null)
        {
            StopCoroutine(currentMinigame);
            currentMinigame = null;
        }
        print(" Minigame stopped");
    }

   


    public void Mini_Pot()
    {
        Debug.Log("MiniPotRun");
    }

    //temporary: 
  
}
