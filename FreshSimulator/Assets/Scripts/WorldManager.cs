using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] int Width;
    [SerializeField] int Height;
    [SerializeField] public int Buffer;

    private int _HalfWidth;
    private int _HalfHeight;

    [SerializeField] int SputterCount;
    [SerializeField] int ProduceCount;

    [SerializeField] GameObject Fridge;
    [SerializeField] GameObject SputterHeatPrefab;
    [SerializeField] GameObject SputterColdPrefab;

    public GameObject[] ProducePrefabs;

    private void Awake()
    {
        _HalfWidth = Width / 2;
        _HalfHeight = Height / 2;

        Fridge.GetComponent<FridgeTemperature>().SetDimensions(Width, Height, Buffer);

        SpawnTempSprites();

        Fridge.GetComponent<FridgeTemperature>().UpdateTempSpriteChildren();

        SpawnProduce();
    }

    private void SpawnProduce()
    {
        float MinX = -_HalfWidth + Buffer;
        float MaxX = _HalfWidth - Buffer;
        float MinY = -_HalfHeight + Buffer;
        float MaxY = _HalfHeight - Buffer;

        foreach (GameObject produce in ProducePrefabs)
        {
            GameObject sputter = Instantiate(produce, Fridge.transform);

            sputter.transform.localPosition = new Vector3(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY), -0.5f);
        }
    }

    private void SpawnTempSprites()
    {
        for (int i = 0; i < SputterCount; i++)
        {
            GameObject sputter = Instantiate(SputterHeatPrefab, Fridge.transform);
        }

        for (int i = 0; i < SputterCount; i++)
        {
            GameObject sputter = Instantiate(SputterColdPrefab, Fridge.transform);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
