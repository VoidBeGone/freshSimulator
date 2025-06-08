using UnityEngine;

public class FridgeTemperature : MonoBehaviour
{
    [SerializeField] Camera Camera;

    [SerializeField] int Width;
    [SerializeField] int Height;
    [SerializeField] public int Buffer;

    [SerializeField] int SputterCount;

    [SerializeField] GameObject SputterHeatPrefab;
    [SerializeField] GameObject SputterColdPrefab;

    public GameObject[] ProducePrefabs;

    [SerializeField] float AvgTemp;

    public float[,] TempData;

    private SpriteRenderer _SpriteRenderer;
    private Texture2D _Texture;

    private int _HalfWidth;
    private int _HalfHeight;

    private Transform[] Children;
    
    private void Awake()
    {
        // Get half Dimensions
        _HalfWidth = Width / 2;
        _HalfHeight = Height / 2;

        //Get the Sprite Renderer
        _SpriteRenderer = GetComponent<SpriteRenderer>();

        // Create temp data
        TempData = new float[Width, Height];

        // Create a new Empty Texture
        _Texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
        _Texture.filterMode = FilterMode.Point;
        _Texture.wrapMode = TextureWrapMode.Clamp;

        //Set Initial Values for Texture and TempData
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _Texture.SetPixel(x, y, new Color(0, 0, 0, 0.3f));
                TempData[x, y] = 0.5f;
            }
        }

        //Apply the texture
        _Texture.Apply();

        // Create new Sprite with the Texture
        Sprite sprite = Sprite.Create(_Texture, new Rect(0, 0, Width, Height), new Vector2(0.5f, 0.5f), 1);
        _SpriteRenderer.sprite = sprite;

        //Fit the Camera to the Size of the Texture with buffer
        float orthoSizeHeight = _SpriteRenderer.bounds.size.y / 2f;
        Camera.orthographicSize = orthoSizeHeight * 1.1f;

        //Spawn the Temperature Sprites
        SpawnTempSprites();
    }

    private void SpawnTempSprites()
    {
        for (int i = 0; i < SputterCount; i++)
        {
            GameObject sputter = Instantiate(SputterHeatPrefab, transform);
        }

        for (int i = 0; i < SputterCount; i++)
        {
            GameObject sputter = Instantiate(SputterColdPrefab, transform);
        }

        Children = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            Children[i] = child;
        }

        foreach (GameObject produce in ProducePrefabs)
        {
            GameObject sputter = Instantiate(produce, transform);
        }
    }

    private void SpawnProduce ()
    {

    }

    private void CalculateAvgTemp()
    {
        float sum = 0;

        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                sum += TempData[x, y];

        AvgTemp = sum / TempData.Length;
    }

    private void FixedUpdate()
    {
        SetSpriteTemp();

        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                UpdateTemperature(x, y);

        CalculateAvgTemp();

        _Texture.Apply();
    }

    public float GetTempMultiplier(TempEmittingSprite sprite)
    {
        if (sprite.Temperature < 0.5f && AvgTemp > 0.6f)
            return 0.8f;

        if (sprite.Temperature > 0.5f && AvgTemp < 0.4f)
            return 1.2f;

        return 1f;
    }

    private void SetSpriteTemp()
    {
        foreach (Transform child in Children)
        {
            TempEmittingSprite sprite = child.GetComponent<TempEmittingSprite>();

            if (sprite == null)
                continue;

            float multiplier = GetTempMultiplier(sprite);

            Vector3 childPos = child.position;

            int x = (int)childPos.x + _HalfWidth;
            int y = (int)childPos.y + _HalfHeight;

            float temp = sprite.Temperature * multiplier;

            for (int w = -sprite.TempSize; w < sprite.TempSize + 1; w++)
                for (int h = -sprite.TempSize; h < sprite.TempSize + 1; h++)
                    TempData[x + w, y + h] = temp;
        }
    }

    public enum HeatDiffusionCase
    {
        Middle,
        Top,
        Bottom,
        Right,
        Left,
        BottomLeft,
        BottomRight,
        TopRight,
        TopLeft
    }

    public HeatDiffusionCase GetDiffusionCase(int width, int height)
    {
        int leftSide = this.Width - 1;
        int topSide = this.Height - 1;

        if (width == 0 && height == 0)
            return HeatDiffusionCase.BottomLeft;
        else if (width == leftSide && height == 0)
            return HeatDiffusionCase.BottomRight;
        else if (width == 0 && height == topSide)
            return HeatDiffusionCase.TopLeft;
        else if (width == leftSide && height == topSide)
            return HeatDiffusionCase.TopRight;
        else if (width == 0)
            return HeatDiffusionCase.Left;
        else if (width == leftSide)
            return HeatDiffusionCase.Right;
        else if (height == topSide)
            return HeatDiffusionCase.Top;
        else if (height == 0)
            return HeatDiffusionCase.Bottom;
        else
            return HeatDiffusionCase.Middle;
    }

    private void UpdateTemperature(int width, int height)
    {
        float val = 0;

        HeatDiffusionCase heatCase = GetDiffusionCase(width, height);

        switch (heatCase)
        {
            case HeatDiffusionCase.BottomLeft:
                val = 2 * TempData[width + 1, height] + 2 * TempData[width, height + 1];
                break;
            case HeatDiffusionCase.TopLeft:
                val = 2 * TempData[width + 1, height] + 2 * TempData[width, height - 1];
                break;
            case HeatDiffusionCase.BottomRight:
                val = 2 * TempData[width - 1, height] + 2 * TempData[width, height + 1];
                break;
            case HeatDiffusionCase.TopRight:
                val = 2 * TempData[width - 1, height] + 2 * TempData[width, height - 1];
                break;
            case HeatDiffusionCase.Left:
                val = 2 * TempData[width + 1, height] + TempData[width, height - 1] + TempData[width, height + 1];
                break;
            case HeatDiffusionCase.Right:
                val = 2 * TempData[width - 1, height] + TempData[width, height - 1] + TempData[width, height + 1];
                break;
            case HeatDiffusionCase.Bottom:
                val = TempData[width - 1, height] + TempData[width + 1, height] + 2 * TempData[width, height + 1];
                break;
            case HeatDiffusionCase.Top:
                val = TempData[width - 1, height] + TempData[width + 1, height] + 2 * TempData[width, height - 1];
                break;
            case HeatDiffusionCase.Middle:
                val = (TempData[width - 1, height] + TempData[width + 1, height] + TempData[width, height - 1] + TempData[width, height + 1]);
                break;

        }

        TempData[width, height] = Mathf.Clamp01(val / 4);

        _Texture.SetPixel(width, height, GetColor(TempData[width, height]));

        //if (TempData[width, height] > 0.5f)
        //    _Texture.SetPixel(width, height, new Color((TempData[width, height] * 2) - 0.5f, 0, 0, 0.3f));
        //else
        //    _Texture.SetPixel(width, height, new Color(0, 0, 1 - (TempData[width, height] * 2), 0.3f));
    }

    private Color GetColor(float temp)
    {
        float distFromCenter = Mathf.Abs(temp - 0.5f); // 0 when temp = 0.5
        float maxDist = 0.2f; // max possible distance from 0.5
        float t = distFromCenter / maxDist; // 0 at center, 1 at edges

        // More green at center (t = 0), more red at edges (t = 1)
        float red = t;
        float green = 1 - t;

        return new Color(red, green, 0, 0.3f);
    }
}
