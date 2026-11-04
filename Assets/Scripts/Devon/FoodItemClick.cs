using UnityEngine;

public class FoodItemClick : MonoBehaviour
{
    private FoodIdentity foodIdentity;

    private void Awake()
    {
        foodIdentity = GetComponent<FoodIdentity>();

        if (foodIdentity == null)
        {
            foodIdentity = GetComponentInParent<FoodIdentity>();
        }

        if (foodIdentity == null)
        {
            Debug.LogWarning(
                "FoodItemClick: No FoodIdentity found.",
                this
            );
        }
    }

    private void OnMouseDown()
    {
        CollectFood();
    }

    public void CollectFood()
    {
        if (foodIdentity == null || foodIdentity.foodData == null)
        {
            Debug.LogWarning(
                "Food collection failed: FoodIdentity or FoodData is missing.",
                this
            );
            return;
        }

        if (FoodCollectionManager.Instance == null)
        {
            Debug.LogWarning(
                "Food collection failed: No FoodCollectionManager exists.",
                this
            );
            return;
        }

        bool registered = FoodCollectionManager.Instance.RegisterFood(
            foodIdentity.foodData
        );

        if (registered)
        {
            Debug.Log(
                "Deactivating food object: " + gameObject.name,
                this
            );

            gameObject.SetActive(false);
        }
    }
}