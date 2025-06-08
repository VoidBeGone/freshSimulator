using UnityEngine;

[CreateAssetMenu(fileName = "ProduceData", menuName = "FreshSimulator/ProduceData")]
public class ProduceData : ScriptableObject
{
    public string produceName;
    public Vector2 idealTempRange; // (minTemp, maxTemp)
    public int maxHealth = 100;
    public float freshnessDecayPerSecond = 2f;

    public Sprite freshSprite;
    public Sprite rottenSprite;

    public GameObject freshPrefab;
    public GameObject rottenPrefab;
}
