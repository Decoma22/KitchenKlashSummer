using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snappoint : MonoBehaviour
{
    [SerializeField] private Transform placePoint;
    [SerializeField] private GameObject targetMarker;
    [SerializeField] private float hoverHeight = 0.15f;
    [SerializeField] private Burner burner;
    public Ingredients Occupant {  get; private set; }
    public bool IsFree => Occupant == null;
    public Transform PlacePoint => placePoint != null ? placePoint : transform;
    public Vector3 HoverPosition => PlacePoint.position + Vector3.up * hoverHeight;
    public bool IsHeated => burner != null && burner.IsOn;

    private Collider[] clickColliders;
    private bool markerVisible;

    private void Awake()
    {
        if (targetMarker != null) targetMarker.SetActive(false);
    }
    
    public bool CanAccept(Ingredients ingredient)
    {
        return ingredient != null && Occupant == null;
    }

    public bool Claim(Ingredients ingredient)
    {
        if (!CanAccept(ingredient)) return false;
        Occupant = ingredient;
        ingredient.CurrentSnapPoint = this;
        return true;
    }

    public void Release(Ingredients ingredient)
    {
        if (Occupant != ingredient) return;
        Occupant = null;
        if (ingredient != null && ingredient.CurrentSnapPoint == this)
        {
            ingredient.CurrentSnapPoint = null;
        }
    }

    public void SetTargetVisible(bool visible)
    {
        if (targetMarker == null || markerVisible == visible) return;
        markerVisible = visible;
        targetMarker.SetActive(visible);
    }

    public void SetClickable(bool clickable)
    {
        if (clickColliders == null) clickColliders = GetComponents<Collider>();

        foreach (Collider c in clickColliders)
        {
            if (c != null) c.enabled = clickable;
        }
    }
}
