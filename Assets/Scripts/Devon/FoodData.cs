using UnityEngine;

[CreateAssetMenu(fileName = "NewFoodData", menuName = "Food System/Food Data")]
public class FoodData : ScriptableObject
{
    public string foodID;
    public string displayName;
}