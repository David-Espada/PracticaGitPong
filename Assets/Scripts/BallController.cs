using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] private float startSpeed = 6f;
    [SerializeField] private float speedIncrease = 0.5f;
    [SerializeField] private float maxSpeed = 15f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Rebote perfecto
        var col = GetComponent<CircleCollider2D>();
        var mat = new PhysicsMaterial2D("BallMat") { bounciness = 1f, friction = 0f };
        col.sharedMaterial = mat;
    }

    private void Start() => Launch();

    private void Launch()
    {
        float dirX = Random.value < 0.5f ? -1f : 1f;
        float dirY = Random.Range(-0.5f, 0.5f);
        rb.velocity = new Vector2(dirX, dirY).normalized * startSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Paddle"))
        {
            float newSpeed = Mathf.Min(rb.velocity.magnitude + speedIncrease, maxSpeed);
            rb.velocity = rb.velocity.normalized * newSpeed;
        }
        else
        {
            // Mantener velocidad estable tras rebotar paredes
            rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, startSpeed, maxSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GoalRight"))
        {
            GameManager.Instance.Padde2Scored();   // suma derecha
            ResetBall(-1);                          // vuelve hacia la izquierda
        }
        else if (other.CompareTag("GoalLeft"))
        {
            GameManager.Instance.Padde1Scored();   // suma izquierda
            ResetBall(1);                           // vuelve hacia la derecha
        }
    }

    private void ResetBall(int dirX)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.position = Vector2.zero;

        Vector2 dir = new Vector2(dirX, Random.Range(-0.5f, 0.5f)).normalized;
        rb.velocity = dir * startSpeed;
    }
}
