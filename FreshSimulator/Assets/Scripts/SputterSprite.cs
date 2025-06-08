using UnityEngine;

public class SputterSprite : TempEmittingSprite
{
    [SerializeField] bool IsCold;

    private float Offset;

    public Vector3 MovementDirection;
    public float Speed;
    public float Mag;

    private void Awake()
    {
        GetParentSize();

        this.TempSize = 1;

        IsActive = true;

        this.Offset = Random.Range(0, 100000f);

        if (IsCold)
            Temperature = Random.Range(0.15f, 0.3f);
        else
            Temperature = Random.Range(0.7f, 0.85f);

        MovementDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

        transform.position = new Vector3(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY), -1);

        Speed = Random.Range(0.5f, 0.9f);
    }

    private void FixedUpdate()
    {
        float x = Mathf.PerlinNoise(Time.time * Speed, Offset) - 0.5f;
        float y = Mathf.PerlinNoise(Offset, Time.time * Speed) - 0.5f;

        MovementDirection += new Vector3(x, y, 0) * Mag;
        Vector3 futurePosition = transform.position + MovementDirection * Speed;

        if (MovementDirection.magnitude > 1)
            MovementDirection.Normalize();

        // Bounce and clamp
        if (futurePosition.x < MinX || futurePosition.x > MaxX)
        {
            MovementDirection.x *= -1;
            futurePosition.x = Mathf.Clamp(futurePosition.x, MinX, MaxX);
            Offset = Random.Range(0, 100000f);
        }
        if (futurePosition.y < MinY || futurePosition.y > MaxY)
        {
            MovementDirection.y *= -1;
            futurePosition.y = Mathf.Clamp(futurePosition.y, MinY, MaxY);
            Offset = Random.Range(0, 100000f);
        }

        transform.position = futurePosition;
    }
}
