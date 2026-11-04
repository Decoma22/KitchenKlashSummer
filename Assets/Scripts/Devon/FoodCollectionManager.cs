using System.Collections.Generic;
using UnityEngine;

public class FoodCollectionManager : MonoBehaviour
{
    public static FoodCollectionManager Instance;

    [Header("Collected Food - Live Debug Lists")]
    [SerializeField] private List<string> collectedIDs = new List<string>();
    [SerializeField] private List<string> collectedFoodNames = new List<string>();

    private HashSet<string> collectedIDSet = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate FoodCollectionManager found.", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Rebuild the duplicate-checking set from the Inspector list.
        collectedIDSet = new HashSet<string>(collectedIDs);
    }

    public bool RegisterFood(FoodData foodData)
    {
        if (foodData == null)
        {
            Debug.LogWarning("Food collection failed: FoodData is missing.", this);
            return false;
        }

        if (string.IsNullOrWhiteSpace(foodData.foodID))
        {
            Debug.LogWarning(
                "Food collection failed: Food ID is empty for " +
                foodData.displayName,
                this
            );
            return false;
        }

        if (collectedIDSet.Contains(foodData.foodID))
        {
            Debug.Log(
                "Food already collected: " + foodData.foodID,
                this
            );
            return false;
        }

        collectedIDSet.Add(foodData.foodID);
        collectedIDs.Add(foodData.foodID);
        collectedFoodNames.Add(foodData.displayName);

        Debug.Log(
            "FOOD COLLECTED: " + foodData.displayName +
            " | ID: " + foodData.foodID,
            this
        );

        Debug.Log(
            "Total unique food items collected: " + collectedIDs.Count,
            this
        );

        return true;
    }

    public bool HasCollected(string foodID)
    {
        return collectedIDSet.Contains(foodID);
    }
}