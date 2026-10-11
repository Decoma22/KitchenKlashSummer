using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
           // print("DEBUG: E was pressed");
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitinfo, InteractRange))
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
            else
            {
              //  print("DEBUG: hit nothin");
            }
        }
    }
}
