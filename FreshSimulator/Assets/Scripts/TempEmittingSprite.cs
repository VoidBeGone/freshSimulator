using UnityEngine;

public class TempEmittingSprite : MonoBehaviour
{
    /// <summary>
    /// Flag indicating that the Sprite is Emitting it's Temperature
    /// </summary>
    public bool IsActive;


    /// <summary>
    /// Temperature the Sprite is Emitting
    /// </summary>
    public float Temperature;


    /// <summary>
    /// Size of the Emitted Temperature Zone
    /// </summary>
    public int TempSize;


    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;

    public int ParentSizeHalfX;
    public int ParentSizeHalfY;

    protected void GetParentSize()
    {
        Vector3 ParentSize = transform.parent.GetComponent<SpriteRenderer>().bounds.size;

        int buffer = transform.parent.GetComponent<FridgeTemperature>().Buffer;

        MinX = -ParentSize.x / 2 + buffer;
        MaxX = ParentSize.x / 2 - buffer;
        MinY = -ParentSize.y / 2 + buffer;
        MaxY = ParentSize.y / 2 - buffer;
    }
}
