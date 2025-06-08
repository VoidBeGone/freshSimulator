using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    [Header("Fridge Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] public int Buffer;

    public int Width { get; private set; }
    public int Height { get; private set; }

    private int _HalfWidth;
    private int _HalfHeight;

    [Header("Spawn Settings")]
    [SerializeField] private int SputterCount;
    [SerializeField] private int ProduceCount;

    [SerializeField] private GameObject Fridge;
    [SerializeField] private GameObject SputterHeatPrefab;
    [SerializeField] private GameObject SputterColdPrefab;

    [SerializeField] GameObject[] ProducePrefabs;

    [Header("Game Time")]
    [SerializeField] int RoundTime;
    [SerializeField] Text CountDownTimer;
    private float RemainingTime;

    [Header("HP")]
    [SerializeField] int PlayerHP;
    private GameObject[] Hearts;



    private void Awake()
    {
        // Copy serialized values to public properties
        Width = width;
        Height = height;

        _HalfWidth = Width / 2;
        _HalfHeight = Height / 2;

        Fridge.GetComponent<FridgeTemperature>().SetDimensions(Width, Height, Buffer);

        SpawnTempSprites();
        Fridge.GetComponent<FridgeTemperature>().UpdateTempSpriteChildren();

        SpawnProduce();
        SpawnHearts();
    }

    private void SpawnProduce()
    {
        float MinX = -_HalfWidth + Buffer;
        float MaxX = _HalfWidth - Buffer;
        float MinY = -_HalfHeight + Buffer;
        float MaxY = _HalfHeight - Buffer;

        foreach (GameObject produce in ProducePrefabs)
        {
            GameObject instance = Instantiate(produce, Fridge.transform);
            instance.transform.localPosition = new Vector3(
                Random.Range(MinX, MaxX),
                Random.Range(MinY, MaxY),
                -0.5f
            );
        }
    }

    private void SpawnHearts()
    {
        Hearts = new GameObject[PlayerHP];

        for (int i = 0; i < PlayerHP; i++)
            Hearts[i] = Instantiate(HeartPrefab, UIHolder.transform);
    }

    private void SpawnTempSprites()
    {
        for (int i = 0; i < SputterCount; i++)
            Instantiate(SputterHeatPrefab, Fridge.transform);

        for (int i = 0; i < SputterCount; i++)
            Instantiate(SputterColdPrefab, Fridge.transform);
    }

    void Start() { }

    void Update() { }
}
