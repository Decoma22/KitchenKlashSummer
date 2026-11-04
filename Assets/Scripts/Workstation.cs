using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Workstation : MonoBehaviour, IInteractable
{
    public Camera workstationCamera;
    [FormerlySerializedAs("snapPoint")]
    [SerializeField] private Transform playerStandPoint;
    [SerializeField] private Snappoint[] snapPoints;
    [SerializeField] private GameHandler.MiniType miniType;
    [SerializeField] public Transform pan;
    [SerializeField] public Transform knife;
    [SerializeField] public Transform plate;
    [SerializeField] public Transform ingredient;
    [SerializeField] public Canvas screenCanvas;

    private CameraSwitch cameraSwitch;
    private Player player;
    private MinigameManager minigameManager;
    private TextMeshPro display;

    public GameHandler.MiniType MiniType => miniType;
    public Transform Pan => pan;
    public Transform Knife => knife;
    public Transform Plate => plate;
    public IReadOnlyList<Snappoint> SnapPoints => snapPoints;
    public bool MinigameActive { get; private set; }

    //TO-DO: Just a reminder that the coroutines in here need to be removed once manager works.

    private void Awake()
    {
        if (snapPoints == null || snapPoints.Length == 0)
        {
            snapPoints = GetComponentsInChildren<Snappoint>(true);
        }
    }

    private void Start()
    {
        cameraSwitch = FindFirstObjectByType<CameraSwitch>();
        player = FindFirstObjectByType<Player>();
        minigameManager = FindFirstObjectByType<MinigameManager>();
      //  display = screenCanvas.GetComponentsInChildren<TextMeshPro>();
    }

    public void EnterWorkstation()
    {
        print("DEBUG: WORKSTATION INTERACTED");
        cameraSwitch.ShowWorkstationCamera(workstationCamera);
        player.Snap_Movement(playerStandPoint);
        SetPlacementActive(true);
        RefreshMinigame();
        
        //minigameManager.Minigame_Switch(cameraSwitch.currentIndex);
    }

    public void Interact()
    {
        EnterWorkstation();
    }
    
    public void LeaveWorkstation()
    {
        CancelHover();
        SetPlacementActive(false);
        MinigameActive = false;
    }

    public void RefreshMinigame()
    {
        if (minigameManager == null) return;

        if (MeetsRequirements())
        {
            minigameManager.Minigame_Switch(this);
            MinigameActive = true;
        }
    }

    private bool MeetsRequirements()
    {
        switch (miniType)
        {
            case GameHandler.MiniType.Saute:
                return pan != null && pan.gameObject.activeInHierarchy;
            case GameHandler.MiniType.Chop:
                return true;
            case GameHandler.MiniType.Plate:
                return true;
            default: 
                return false;
        }
    }

    public Ingredients GetHoveringIngredient()
    {
        return FindIngredientInState(Ingredients.IngredientState.Hovering);
    }

    public void CancelHover()
    {
        Ingredients hovering = GetHoveringIngredient();
        if (hovering != null) hovering.CancelHover();
    }
    public void SetPlacementActive(bool active)
    {
        if (snapPoints == null) return;
        foreach (Snappoint snap in snapPoints)
        {
            if (snap == null) continue;
            snap.SetClickable(active);
            if (!active) snap.SetTargetVisible(false);
        }
    }

    private Ingredients FindIngredientInState(Ingredients.IngredientState state)
    {
        if (snapPoints == null) return null;

        foreach (Snappoint snap in snapPoints)
        {
            if (snap == null) continue;

            Ingredients occupant = snap.Occupant;
            if (occupant != null && occupant.CurrentState == state) return occupant;
        }
        return null;
    }
    
}
