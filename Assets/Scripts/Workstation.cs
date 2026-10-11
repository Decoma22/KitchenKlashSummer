using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Workstation : MonoBehaviour, IInteractable
{
    public Camera workstationCamera;
    [SerializeField] private Transform snapPoint;
    [SerializeField] private GameHandler.MiniType miniType;
    [SerializeField] public Transform pan;
    [SerializeField] public Transform knife;

    private CameraSwitch cameraSwitch;
    private Player player;
    private MinigameManager minigameManager;

    public GameHandler.MiniType MiniType => miniType;
    public Transform Pan => pan;
    public Transform Knife => knife;
  
    //TO-DO: Just a reminder that the coroutines in here need to be removed once manager works.
  
    private void Start()
    {
        cameraSwitch = FindFirstObjectByType<CameraSwitch>();
        player = FindFirstObjectByType<Player>();
        minigameManager = FindFirstObjectByType<MinigameManager>();

    }

    public void EnterWorkstation()
    {
        print("DEBUG: WORKSTATION INTERACTED");
        cameraSwitch.ShowWorkstationCamera(workstationCamera);
        player.Snap_Movement(snapPoint);
        minigameManager.Minigame_Switch(this);
        //minigameManager.Minigame_Switch(cameraSwitch.currentIndex);
    }

    public void Interact()
    {
        EnterWorkstation();
    }
    
    
}
