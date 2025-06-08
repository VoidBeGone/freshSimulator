using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    [Header("Fridge Dimensions")]
    [SerializeField] public int Width;
    [SerializeField] public int Height;
    [SerializeField] public int Buffer;

    private int _HalfWidth;
    private int _HalfHeight;

    [Header("Entity Count")]
    [SerializeField] int SputterCount;
    [SerializeField] int ProduceCount;

    [Header("Prefabs and Objects")]
    [SerializeField] GameObject UIHolder;
    [SerializeField] GameObject Fridge;
    [SerializeField] GameObject SputterHeatPrefab;
    [SerializeField] GameObject SputterColdPrefab;
    [SerializeField] GameObject HeartPrefab;

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
            GameObject sputter = Instantiate(produce, Fridge.transform);

            sputter.transform.localPosition = new Vector3(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY), -0.5f);
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
        {
            GameObject sputter = Instantiate(SputterHeatPrefab, Fridge.transform);
        }

        for (int i = 0; i < SputterCount; i++)
        {
            GameObject sputter = Instantiate(SputterColdPrefab, Fridge.transform);
        }
    }

    private void UpdateCountDown()
    {
        RemainingTime = RoundTime - Time.time;

        if (RemainingTime < 0)
            SceneManager.LoadScene("Game Win");

        int mins = (int)(RemainingTime / 60f);
        int seconds = (int)(RemainingTime % 60f);

        if (seconds < 10)
            CountDownTimer.text = $"{mins}:0{seconds}";
        else
            CountDownTimer.text = $"{mins}:{seconds}";
    }

    private int count = 0;

    private void FixedUpdate()
    {
        UpdateCountDown();

        count++;

        if (count > 600)
        {
            //PlayerDamaged();
            count = 0;
        }

    }

    public void PlayerDamaged()
    {
        PlayerHP--;

        GameObject.Destroy(Hearts[PlayerHP]);

        if (PlayerHP == 0)
        {
            SceneManager.LoadScene("Game Over");
            //Game Over
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
