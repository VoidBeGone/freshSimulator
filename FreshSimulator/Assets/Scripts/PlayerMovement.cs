using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 6f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Text healthText;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private bool isGrounded;

    private bool isGrabing;

    private float grabCoolDownTime;

    private GameObject grabbedFruit;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        body.freezeRotation = true;
        isGrabing = false;
        grabCoolDownTime = 0;
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

        //Debug.Log("Grounded: " + isGrounded);

        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jump);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isGrabing && Time.time > grabCoolDownTime)
            {
                grabbedFruit.transform.parent = GameObject.Find("Fridge").transform;

                grabbedFruit.GetComponent<Rigidbody2D>().simulated = true;

                isGrabing = false;

                grabCoolDownTime = Time.time + 1f;
            }
        }

        if (isGrabing)
        {
            healthText.text = grabbedFruit.GetComponent<Produce>().currentHealth.ToString();
        }
        else
        {
            healthText.text = "0";
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log($"Colliding with something : {collision.gameObject.layer}");

        if (collision.gameObject.layer == 7)
        {
            healthText.text = collision.gameObject.GetComponent<Produce>().currentHealth.ToString();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!isGrabing && Time.time > grabCoolDownTime)
                {
                    collision.gameObject.transform.parent = this.transform;

                    collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;

                    isGrabing = true;

                    grabCoolDownTime = Time.time + 1f;

                    grabbedFruit = collision.gameObject;

                }
            }
        }

    }
}