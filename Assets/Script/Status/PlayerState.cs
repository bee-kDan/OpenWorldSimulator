using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // Turn Player State in to Singleton
    public static PlayerState Instance { get; set; }

    // ------ Player HP ------- //

    public float currentHealth;
    public float maxHealth;

    float distanceTravlled = 0;
    Vector3 lastPotion;

    public GameObject playerBody;

    // ------ Player Calories ------- //

    public float currentCalories;
    public float maxCalories;

    // ------ Player Hydration ------- //

    public float currentHydrationPercent;
    public float maxHydrationPercent;

    public bool isHydrated;


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        lastPotion = playerBody.transform.position;

        StartCoroutine(decreaseHydation());
    }

    IEnumerator decreaseHydation()
    {
        while (!isHydrated)
        {
            setHydration(currentHydrationPercent - 1);
            yield return new WaitForSeconds(5);
        }
    }

    void Update()
    {
        distanceTravlled += Vector3.Distance(playerBody.transform.position, lastPotion);
        lastPotion = playerBody.transform.position;

        if (distanceTravlled >= 5)
        {
            distanceTravlled = 0;
            setCalories(currentCalories - 1);
        }
    }

    public void setHealth(float heathToSet)
    {
        currentHealth = Mathf.Clamp(heathToSet, 0f, maxHealth);

    }

    public void setCalories(float caloriesToSet)
    {
        currentCalories = Mathf.Clamp(caloriesToSet, 0f, maxCalories);
    }

    public void setHydration(float hydrationPercentToSet)
    {
        currentHydrationPercent = Mathf.Clamp(hydrationPercentToSet, 0f, maxHydrationPercent);
    }

    //private void Die()
    //{
    //    Debug.Log("Player died");
    //    // TODO: xử lý chết (game over, respawn...)
    //}
}