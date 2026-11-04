using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour, IInteractable
{
    private Player player;
    private Transform snapPoint;
    private Rigidbody rb;
    private BoxCollider boxc;
    private BoxCollider BoxCollider;
    private Transform newParent;
    private Transform originParent;
    //hard code patch for instructions TO-DO: Replace with event system
    [SerializeField] Canvas worldSpaceCanvas;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        rb = GetComponent<Rigidbody>();
        boxc = GetComponent<BoxCollider>();
        BoxCollider = GetComponent<BoxCollider>();
        originParent = transform.parent;
        newParent = player.holdPoint;
    }

    public void Interact()
    {
        if (player.isHolding)
        {
            print("one only");
        }
        else
        {
            print("picked up");
            HoldItem(snapPoint);
            print(transform.position);

            //TO-DO: Replace with event system
            worldSpaceCanvas.enabled = false;
        }
    }

    private void HoldItem(Transform snapPoint)
    {
        print("picked");
        transform.SetParent(newParent, false);
        boxc.enabled = false;
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        player.isHolding = true;
    }

    public void DropItem()
    {
        print("drop");
        boxc.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;
        transform.SetParent(originParent, true);
        player.isHolding = false;
    }
}
