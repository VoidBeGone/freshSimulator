using UnityEngine;

public class Produce : MonoBehaviour
{
    public ProduceData data;
    private float currentHealth;
    private SpriteRenderer spriteRenderer;
    private FridgeTemperature fridge;
    private WorldManager world;

    void Start()
    {
        currentHealth = data.maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        fridge = GetComponentInParent<FridgeTemperature>();
        world = fridge?.GetComponent<WorldManager>();

        if (fridge == null || world == null)
        {
            Debug.LogError("FridgeTemperature or WorldManager not found on parent.");
        }

        UpdateVisuals();
    }

    void Update()
    {
        if (fridge == null || world == null)
            return;

        float currentTemp = GetCurrentTempFromGrid();
        Vector2 range = data.idealTempRange;

        if (currentTemp < range.x || currentTemp > range.y)
        {
            currentHealth -= data.freshnessDecayPerSecond * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, data.maxHealth);
            UpdateVisuals();
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
        float percent = currentHealth / data.maxHealth;
        spriteRenderer.sprite = percent > 0.5f ? data.freshSprite : data.rottenSprite;
    }

    public bool IsSpoiled() => currentHealth <= 0;

    public void ResetFreshness() => currentHealth = data.maxHealth;
}
