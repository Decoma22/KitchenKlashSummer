using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookState
{
    Undercooked,
    Good,
    Overcooked
}
[System.Serializable]
public class IngredientRecord
{
    [Range(0f, 1f)] public float cleanliness = 0f;
    [Range(0f, 1f)] public float chopAccuracy = 0f;

    public float cookSeconds;
    public float restSeconds;
    public IngredientRecord Clone()
    {
        return (IngredientRecord)MemberwiseClone();
    }

}
