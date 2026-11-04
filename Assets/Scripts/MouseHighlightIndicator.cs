using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHighlightIndicator : MonoBehaviour
{
    [SerializeField] private Material glowMaterial;
    [SerializeField] private float maxDistance = 100f;

    private CameraSwitch cameraSwitch;
    private Transform highlighted;
    private readonly Dictionary<Renderer, Material[]> originals = new Dictionary<Renderer, Material[]>();

    void Start()
    {
        cameraSwitch = FindFirstObjectByType<CameraSwitch>();
    }

    void Update()
    {
        if (highlighted != null && Input.GetMouseButton(0)) return;
        Transform target = null;
        if (cameraSwitch != null && cameraSwitch.IsUsingWorkstationCamera)
        {
            target = FindTool(cameraSwitch.CurrentWorkstation, cameraSwitch.CurrentWorkstationCamera);
        }

        if (target == highlighted) return;

        ClearHighlight();
        highlighted = target;
        if (highlighted != null) ApplyHighlight(highlighted);
    }

    private void OnDisable()
    {
        ClearHighlight();
        highlighted = null;
    }

    private Transform FindTool(Workstation workstation, Camera cam)
    {
        if (workstation == null || cam == null || !workstation.MinigameActive) return null;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance, ~0, QueryTriggerInteraction.Ignore)) return null;
        if (IsPartOf(hit.transform, workstation.Pan)) return workstation.Pan;
        if (IsPartOf(hit.transform, workstation.Knife)) return workstation.Knife;
        return null;
    }

    private bool IsPartOf(Transform hit, Transform tool)
    {
        return tool != null && (hit == tool || hit.IsChildOf(tool));
    }

    private void ApplyHighlight(Transform tool)
    {
        if (glowMaterial == null) return;

        foreach (Renderer rend in tool.GetComponentsInChildren<Renderer>())
        {
            if (originals.ContainsKey(rend)) continue;

            Material[] orig = rend.sharedMaterials;
            originals[rend] = orig;

            Material[] withGlow = new Material[orig.Length + 1];
            orig.CopyTo(withGlow, 0);
            withGlow[orig.Length] = glowMaterial;
            rend.sharedMaterials = withGlow;
        }
    }

    private void ClearHighlight()
    {
        foreach (var pair in originals)
        {
            if (pair.Key != null) pair.Key.sharedMaterials = pair.Value;
        }
        originals.Clear();
    }
}
