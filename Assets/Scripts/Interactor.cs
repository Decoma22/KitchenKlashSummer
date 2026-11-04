using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public Material glowMaterial;
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private List<Renderer> toClear = new List<Renderer>();

    private void Update()
    {
        // print("DEBUG: E was pressed");
        GameObject glowTarget = null;
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitinfo, InteractRange))
            {
                if (hitinfo.collider.gameObject.TryGetComponent(out IInteractable _) && hitinfo.collider.gameObject.TryGetComponent(out Ingredients _))
            {
                glowTarget = hitinfo.collider.gameObject;
                AddGlow(glowTarget);
            }
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                //  print("DEBUG: ray hit: " + hitinfo.collider.gameObject.name);
                if (hitinfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                   print("DEBUG:Interactablefound");
                }
                else
                {
                 //   print("DEBUG: Ray hit but not interactable");
                }
            }
        }
        RemoveGlow(glowTarget);
    }

    private void AddGlow(GameObject target)
    {
        if (glowMaterial == null) return;
        foreach (Renderer rend in target.GetComponentsInChildren<Renderer>())
        {
            if (originalMaterials.ContainsKey(rend)) continue;
            Material[] orig = rend.sharedMaterials;
            originalMaterials[rend] = orig;
            Material[] withGlow = new Material[orig.Length * 2];
            orig.CopyTo(withGlow, 0);
            for (int i = orig.Length; i < withGlow.Length; i++)
            {
                withGlow[i] = glowMaterial;
            }
            rend.sharedMaterials = withGlow;
        }
        
    }
    private void RemoveGlow(GameObject target)
    {
       if (originalMaterials.Count ==  0) return;
        toClear.Clear();
        foreach (var pair in originalMaterials)
        {
            if (pair.Key == null || target == null || !pair.Key.transform.IsChildOf(target.transform))
                toClear.Add(pair.Key);
        }

        foreach (Renderer rend in toClear)
        {
            if (rend != null) rend.sharedMaterials = originalMaterials[rend];
             originalMaterials.Remove(rend);
         }
    }
}
