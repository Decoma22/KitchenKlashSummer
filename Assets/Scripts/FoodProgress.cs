using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProgress : MonoBehaviour
{
    [SerializeField] private IngredientDefinition definition;
    [SerializeField] private IngredientRecord record = new IngredientRecord();

    [Header("Display only")]
    [SerializeField] private CookState shownState;
    [SerializeField] private float shownCookPercent;
    private Ingredients ingredient;
    private CookState lastLoggedState;
    public IngredientDefinition Definition => definition;
    public IngredientRecord Record => record;
    public bool TracksCook => definition != null && definition.tracksCook;

    private void Start()
    {
        Debug.Log("FoodProgressSTART on " + name + " (id " + GetInstanceID() + ")", this);
        ingredient = GetComponent<Ingredients>();
        if (definition == null) Debug.LogWarning(name + ": FoodProgress has no IngredientDefinition assigned", this);
        RefreshDisplay();
    }

    private void Update()
    {
        if (ingredient == null) return;

        bool heated = ingredient.CurrentState == Ingredients.IngredientState.Placed && ingredient.CurrentSnapPoint != null && ingredient.CurrentSnapPoint.IsHeated;

        if (heated) AddCookSeconds(Time.deltaTime);
        if (State != lastLoggedState)
        {
            lastLoggedState = State;
            Debug.Log(name + "is now" + State + " (" + record.cookSeconds.ToString("F1") + "s)", this);
        }
    }
    private float lastSeconds;
    private void LateUpdate()
    {
        if (record.cookSeconds < lastSeconds)
        {
            Debug.LogWarning("Dropped on '" + name +"' (id "+ GetInstanceID() + "): "+ lastSeconds +"->"+ record.cookSeconds, this);
        }
        lastSeconds = record.cookSeconds;
    }
    public CookState State
    {
        get
        {
            if (definition == null) return CookState.Undercooked;
            float t = record.cookSeconds;
            if (t < definition.GoodStart) return CookState.Undercooked;
            if (t <= definition.GoodEnd) return CookState.Good;
            if (t > definition.GoodEnd) return CookState.Overcooked;
            return CookState.Undercooked;
        }
    }

    public float CookScore
    {
        get
        {
            if (definition == null) return 0f;
            float t = record.cookSeconds;
            if (t < definition.GoodStart) return Mathf.Clamp01(t / definition.GoodStart);
            if (t <= definition.GoodEnd) return 1f;
            return Mathf.Clamp01(1f - (t - definition.GoodEnd) / definition.OvercookSeconds);
        }
    }

    public void AddCookSeconds(float seconds)
    {
        if (!TracksCook) return;
        record.cookSeconds += seconds;
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        shownState = State;
        shownCookPercent = CookScore * 100f;
    }

    [ContextMenu("Test: add 5 cook Seconds")]
    private void TestAddFive()
    {
        AddCookSeconds(5f);
        Debug.Log(name + ": " + record.cookSeconds + "s ->" + State + " (" + Mathf.RoundToInt(CookScore * 100f) + "%)", this);
    }
}
