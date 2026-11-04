using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    private MinigameManager manager;

    void Start()
    {
        manager = FindAnyObjectByType<MinigameManager>();
    }

    void UpdateScore()
    {
        
    }
}
