using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour, IInteractable
{
    public enum IngredientState
    {
        World,
        Held,
        Placed,
        Hovering
    }

    public IngredientState CurrentState { get; private set; } = IngredientState.World;
    public Snappoint CurrentSnapPoint { get; internal set; }

    private Player player;
    private Rigidbody rb;
    private Transform newParent;
    private Transform originParent;
    private Collider[] ingredientColliders;
    private bool[] originalColliderStates;
    private bool originalUseGravity;
    private bool originalIsKinematic;
    //hard code patch for instructions TO-DO: Replace with event system
    [SerializeField] Canvas worldSpaceCanvas;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        rb = GetComponent<Rigidbody>();
        originParent = transform.parent;
        newParent = player.holdPoint;

        ingredientColliders = GetComponentsInChildren<Collider>();
        originalColliderStates = new bool[ingredientColliders.Length];
        for (int i = 0; i < ingredientColliders.Length; i++)
        {
            originalColliderStates[i] = ingredientColliders[i].enabled;
        }
        if (rb != null)
        {
            originalUseGravity = rb.useGravity;
            originalIsKinematic = rb.isKinematic;
        }
    }

    public void Interact()
    {
        if (CurrentState != IngredientState.World) return;
            print("picked up");
            HoldItem();
            print(transform.position);

            //TO-DO: Replace with event system
            if (worldSpaceCanvas != null)  worldSpaceCanvas.enabled = false;
    }

    private bool HoldItem()
    {
        if (!player.SetHeldIngredient(this)) return false;
        if (CurrentSnapPoint != null) CurrentSnapPoint.Release(this);

        print("picked");
        transform.SetParent(newParent, true);
        DisablePhysics();
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        CurrentState = IngredientState.Held;
        return true;
    }

    public void DropItem()
    {
        print("trydrop");
       if (CurrentState != IngredientState.Held) return;

        print("drop");
        RestorePhysics();
        transform.SetParent(originParent, true);
        player.ClearHeldIngredient(this);
        CurrentState = IngredientState.World;
    }

    public bool PlaceAt(Snappoint target)
    {
        if (target == null) return false;
        if (CurrentState != IngredientState.Held && CurrentState != IngredientState.Hovering) return false;

        Snappoint previous = CurrentSnapPoint;
        if (previous != null) previous.Release(this);

        if (!target.Claim(this))
        {
            if (previous != null) previous.Claim(this);
            return false;
        }
        player.ClearHeldIngredient(this);

        DisablePhysics();
        transform.SetParent(target.transform, true);
        transform.SetPositionAndRotation(target.PlacePoint.position, target.PlacePoint.rotation);
        CurrentState = IngredientState.Placed;
        return true;
    }

    public bool LiftToHover()
    {
        if (CurrentState != IngredientState.Placed || CurrentSnapPoint == null) return false;

        RestoreColliders();
        transform.position = CurrentSnapPoint.HoverPosition;
        CurrentState = IngredientState.Hovering;
        return true;
    }

    public bool TakeFromHover()
    {
        if (CurrentState != IngredientState.Hovering) return false;
        return HoldItem();
    }

    public void CancelHover()
    {
        if (CurrentState != IngredientState.Hovering || CurrentSnapPoint == null) return;

        DisablePhysics();
        transform.SetPositionAndRotation(CurrentSnapPoint.PlacePoint.position, CurrentSnapPoint.PlacePoint.rotation);
        CurrentState = IngredientState.Placed;
    }

    private void DisablePhysics()
    {
        SetColliders(false);
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void RestorePhysics()
    {
        RestoreColliders();
        if (rb != null)
        {
            rb.useGravity = originalUseGravity;
            rb.isKinematic = originalIsKinematic;
        }
    }

    private void SetColliders(bool enabled)
    {
        print(ingredientColliders);
        foreach (Collider c in ingredientColliders)
        {
            if (c != null) c.enabled = enabled;
        }
    }

    private void RestoreColliders()
    {
        for (int i = 0; i < ingredientColliders.Length; i++)
        {
            if (ingredientColliders[i] != null) ingredientColliders[i].enabled = originalColliderStates[i];
        }
    }
}
