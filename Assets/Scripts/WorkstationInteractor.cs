using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkstationInteractor : MonoBehaviour
{
    [SerializeField] private int ingredientMouseButton = 1;
    [SerializeField] private float maxDistance = 100f;

    private CameraSwitch cameraSwitch;
    private Player player;
    private readonly RaycastHit[] hits = new RaycastHit[16];

    void Start()
    {
        cameraSwitch = FindFirstObjectByType<CameraSwitch>();
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        if (cameraSwitch == null || player == null) return;
        if (!cameraSwitch.IsUsingWorkstationCamera) return;
        if (!Input.GetMouseButtonDown(ingredientMouseButton)) return;

        Workstation workstation = cameraSwitch.CurrentWorkstation;
        Camera cam = cameraSwitch.CurrentWorkstationCamera;
        if (workstation == null || cam == null) return;

        FindTarget(cam, out Snappoint snap, out Ingredients clicked);

        Ingredients hovering = workstation.GetHoveringIngredient();
        if (hovering != null)
        {
            HandleHovering(hovering, snap, clicked);
        }
        else if(player.HeldIngredient != null)
        {
            HandleHeld(player.HeldIngredient, snap);
        }
        else
        {
            HandleEmptyHands(snap);
        }
    }

    private void HandleHovering(Ingredients hovering, Snappoint snap, Ingredients clicked)
    {
        if (clicked == hovering || (snap != null && snap == hovering.CurrentSnapPoint))
        {
            hovering.TakeFromHover();
            return;
        }
        if (snap != null && snap.CanAccept(hovering))
        {
            hovering.PlaceAt(snap);
            return;
        }
        hovering.CancelHover();
    }

    private void HandleHeld(Ingredients held, Snappoint snap)
    {
        if (snap != null && snap.CanAccept(held))
        {
            held.PlaceAt(snap);
        }
    }

    private void HandleEmptyHands(Snappoint snap)
    {
        if (snap == null) return;
        Ingredients occupant = snap.Occupant;
        if(occupant != null && occupant.CurrentState == Ingredients.IngredientState.Placed)
        {
            occupant.LiftToHover();
        }
    }

    private void FindTarget(Camera cam, out Snappoint snap, out Ingredients ingredient)
    {
        snap = null;
        ingredient = null;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        int count = Physics.RaycastNonAlloc(ray, hits, maxDistance, ~0, QueryTriggerInteraction.Collide);

        float best = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.distance >= best) continue;

            Snappoint s = hit.collider.GetComponentInParent<Snappoint>();
            Ingredients ing = hit.collider.GetComponent<Ingredients>();
            if (ing != null && ing.CurrentState != Ingredients.IngredientState.Hovering) ing = null;
            if (s == null && ing == null) continue;

            best = hit.distance;
            snap = s;
            ingredient = ing;
        }
    }
}
