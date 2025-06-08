using UnityEngine;

public class Produce : ProduceData
{
    public float currentHealth;
    private SpriteRenderer spriteRenderer;
    private FridgeTemperature fridge;
    private WorldManager world;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        fridge = GetComponentInParent<FridgeTemperature>();
        world = GameObject.Find("World").GetComponent<WorldManager>();

        if (fridge == null || world == null)
        {
            Debug.LogError("FridgeTemperature or WorldManager not found on parent.");
        }

        UpdateVisuals();
    }

    void Update()
    {
        if (fridge == null || world == null)
        {
            Debug.Log("Returning");
        }

        float currentTemp = GetCurrentTempFromGrid();
        //Debug.Log(currentTemp);
        Vector2 range = idealTempRange;

        if (currentTemp < range.x || currentTemp > range.y)
        {
            currentHealth -=freshnessDecayPerSecond * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateVisuals();
        } 

        if (currentHealth < 0)
        {
            world.PlayerDamaged();
            GameObject.Destroy(this.gameObject);
        }
            
    }

    float GetCurrentTempFromGrid()
    {
        Vector3 localPos = transform.localPosition;
        int x = Mathf.RoundToInt(localPos.x + world.Width / 2f);
        int y = Mathf.RoundToInt(localPos.y + world.Height / 2f);

        x = Mathf.Clamp(x, 0, fridge.TempData.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, fridge.TempData.GetLength(1) - 1);

        return fridge.TempData[x, y];
    }

    void UpdateVisuals()
    {
        float percent = currentHealth / maxHealth;
        spriteRenderer.sprite = percent > 0.5f ? freshSprite : rottenSprite;
    }

    public bool IsSpoiled() => currentHealth <= 0;

    public void ResetFreshness() => currentHealth =maxHealth;
}
