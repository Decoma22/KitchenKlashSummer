using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour, IInteractable
{
    private Player player;
    private Transform snapPoint;
    private Rigidbody rb;
    private Transform newParent;
    private Transform originParent;
   

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        rb = GetComponent<Rigidbody>();
        originParent = transform.parent;
        newParent = player.holdPoint;
    }

    public void Interact()
    {
        print("picked up");
        HoldItem(snapPoint);
    }

    private void HoldItem(Transform snapPoint)
    {
        snapPoint = player.holdPoint;
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.SetParent(newParent, false);
    }

    public void DropItem()
    {
        print("drop");
        rb.useGravity = true;
        rb.isKinematic = false;
        transform.SetParent(originParent, true);
    }
}
