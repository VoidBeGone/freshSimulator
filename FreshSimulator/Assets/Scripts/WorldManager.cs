using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class WorldManager : MonoBehaviour
{
    [SerializeField] int SputterCount;
    [SerializeField] int ProduceCount;

    [SerializeField] GameObject Fridge;
    [SerializeField] GameObject SputterHeatPrefab;
    [SerializeField] GameObject SputterColdPrefab;

    public GameObject[] ProducePrefabs;

    private void Awake()
    {
        
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
