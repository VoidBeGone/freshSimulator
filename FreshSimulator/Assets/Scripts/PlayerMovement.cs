using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 6f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private bool isGrounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>(); 
        body.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics2D.BoxCast(
            boxCollider.bounds.center,
            new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y),
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );

        Debug.Log("Grounded: " + isGrounded);

        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jump);
        }
    }
}