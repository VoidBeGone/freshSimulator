using UnityEngine;
using UnityEngine.Rendering;

public class FridgeTemperature : MonoBehaviour
{
    [SerializeField] Camera Camera;

    [SerializeField] int Width;
    [SerializeField] int Height;
    [SerializeField] int Buffer;

    [SerializeField] int SputterCount;

    [SerializeField] GameObject SputterHeatPrefab;
    [SerializeField] GameObject SputterColdPrefab;

    private SpriteRenderer _SpriteRenderer;
    private Texture2D _Texture;

    private void Awake()
    {

        _SpriteRenderer = GetComponent<SpriteRenderer>();

        // Create temp data
        // tempData = new float[width, height];

        // Make a texture
        _Texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
        _Texture.filterMode = FilterMode.Point;
        _Texture.wrapMode = TextureWrapMode.Clamp;

        // Apply blank pixels
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                _Texture.SetPixel(x, y, new Color(0, 0, 0, 0.3f));

        /*for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                tempData[x, y] = 0.5f;

                if (x < Buffer || x > this.Width - Buffer)
                    continue;

                if (y < Buffer || y > this.Height - Buffer)
                    continue;

                if (y % 10 == 0)
                {
                    float randTemp = 0.5f + Random.Range(-0.3f, 0.3f);

                    for (int i = -2; i < 2; i++)
                        for (int j = -2; j < 2; j++)
                            tempData[x + i, y + j] = randTemp;

                }
            }*/


        _Texture.Apply();

        // Create sprite from texture
        Sprite sprite = Sprite.Create(_Texture, new Rect(0, 0, Width, Height), new Vector2(0.5f, 0.5f), 1);
        _SpriteRenderer.sprite = sprite;

        //Fit Camera to height of the fridge (+ 5% buffer)
        float orthoSizeHeight = _SpriteRenderer.bounds.size.y / 2f;
        Camera.orthographicSize = orthoSizeHeight * 1.05f;

        for (int i = 0; i < SputterCount; i ++)
        {
            GameObject sputter = Instantiate(SputterHeatPrefab, transform);
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
