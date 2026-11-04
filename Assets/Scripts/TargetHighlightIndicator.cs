using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHighlightIndicator : MonoBehaviour
{
    private CameraSwitch cameraSwitch;
    private Player player;
    private Workstation shownWorkstation;

    // Start is called before the first frame update
    void Start()
    {
        cameraSwitch = FindFirstObjectByType<CameraSwitch>();
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraSwitch == null || player == null) return;

        Workstation current = cameraSwitch.IsUsingWorkstationCamera ? cameraSwitch.CurrentWorkstation : null;
        if (shownWorkstation != null && shownWorkstation != current)
        {
            HideAll(shownWorkstation);
        }
        shownWorkstation = current;
        if (current == null) return;

        Ingredients carried = player.HeldIngredient;
        if (carried == null) carried = current.GetHoveringIngredient();

        foreach (Snappoint snap in current.SnapPoints)
        {
            if (snap == null) continue;
            snap.SetTargetVisible(carried != null && snap.CanAccept(carried));
        }
    }

    private void HideAll(Workstation workstation)
    {
        foreach (Snappoint snap in workstation.SnapPoints)
        {
            if (snap != null) snap.SetTargetVisible(false);
        }
    }
}
