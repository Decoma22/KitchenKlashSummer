using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [SerializeField] private Renderer burnerRenderer;
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material onMaterial;
    [SerializeField] private bool startsOn = false;

    public bool IsOn { get; private set; }

    private void Awake()
    {
        SetOn(startsOn);
    }

    public void Toggle()
    {
        SetOn(!IsOn);
    }

    public void SetOn(bool on)
    {
        IsOn = on;

        if (burnerRenderer != null)
        {
            burnerRenderer.sharedMaterial = on ? onMaterial : offMaterial;
        }

        Debug.Log(name + "burner" + (on ? "ON" : "OFF"), this);
    }
}
