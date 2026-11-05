using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : MonoBehaviour
{
    [SerializeField] private Burner burner;

    [SerializeField] private Transform knobVisual;
    [SerializeField] private Vector3 turnAxis = Vector3.up;
    [SerializeField] private float onAngle = 90f;

    private Quaternion offRotation;

    private void Start()
    {
        if (knobVisual != null) offRotation = knobVisual.localRotation;
        RefreshVisual();
    }

    public void Click()
    {
        if (burner == null)
        {
            Debug.LogWarning(name + ": knob has no assigned burner", this);
            return;
        }

        burner.Toggle();
        RefreshVisual();
    }

    private void RefreshVisual()
    {
        if (knobVisual == null || burner == null) return;
        float angle = burner.IsOn ? onAngle : 0f;
        knobVisual.localRotation = offRotation * Quaternion.AngleAxis(angle, turnAxis);
    }
}
