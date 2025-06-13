using UnityEngine;

[CreateAssetMenu(fileName = "ProduceData", menuName = "FreshSimulator/ProduceData")]
public class ProduceData : MonoBehaviour
{
    public string produceName;
    public Vector2 idealTempRange; // (minTemp, maxTemp)
    public int maxHealth = 100;
    protected float freshnessDecayPerSecond = 4f;

    public Sprite freshSprite;
    public Sprite rottenSprite;

    public GameObject freshPrefab;
    public GameObject rottenPrefab;
}
