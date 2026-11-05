using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookTint : MonoBehaviour
{
    [SerializeField] private Color rawColor = new Color(1f, 0.6f, 0.65f);
    [SerializeField] private Color cookedColor = new Color(0.85f, 0.65f, 0.4f);
    [SerializeField] private Color burntColor = new Color(0.2f, 0.12f, 0.08f);

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private FoodProgress progress;
    private Renderer[] renderers;
    private MaterialPropertyBlock block;
    private float lastSeconds = -1f;

    void Start()
    {
        progress = GetComponent<FoodProgress>();
        renderers = GetComponentsInChildren<Renderer>();
        block = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (progress == null || progress.Definition == null) return;
        float t = progress.Record.cookSeconds;
        if (Mathf.Approximately(t, lastSeconds)) return;
        lastSeconds = t;
        SetTint(Evaluate(t));
    }

    private Color Evaluate(float t)
    {
        IngredientDefinition d = progress.Definition;
        if (t  < d.GoodStart) return Color.Lerp(rawColor, cookedColor, t / d.GoodStart);
        if (t <= d.GoodEnd) return cookedColor;
        return Color.Lerp(cookedColor, burntColor, (t - d.GoodEnd) / d.OvercookSeconds);
    }

    private void SetTint(Color c)
    {
        foreach (Renderer r in renderers)
        {
            r.GetPropertyBlock(block);
            block.SetColor(BaseColorId, c); ;
            r.SetPropertyBlock(block);
        }
    }
}
