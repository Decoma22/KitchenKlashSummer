using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredientDefinition", menuName = "Cooking/Ingredient Definition")]
public class IngredientDefinition : ScriptableObject
{
    public string displayName;

    [Header("What this ingredient is scored on")]
    public bool tracksCook = true;
    public bool tracksRest;
    public bool tracksWash;
    public bool tracksChop;

    [Header("Cooking")]
    public float suggestedCookSeconds = 20f;
    public float goodTolerane = 0.25f;
    public float overcookRange = 0.5f;
    [Header("Resting after Cooking")]
    public float maxRestMultiplier = 3f;

    public float GoodStart => suggestedCookSeconds * (1f - goodTolerane);
    public float GoodEnd => suggestedCookSeconds * (1f + goodTolerane);
    public float OvercookSeconds => suggestedCookSeconds * overcookRange;
    public float MaxRestSeconds => suggestedCookSeconds * maxRestMultiplier;

    private void OnValidate()
    {
        suggestedCookSeconds = Mathf.Max(1f, suggestedCookSeconds);
        goodTolerane = Mathf.Clamp(goodTolerane, 0.05f, 0.9f);
        overcookRange = Mathf.Max(0.1f, overcookRange);
    }

}
