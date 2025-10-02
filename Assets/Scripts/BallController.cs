using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class SideEvent : UnityEvent<string> { }

public class BallController : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] private float startSpeed = 6f;
    [SerializeField] private float speedIncrease = 0.5f;
    [SerializeField] private float maxSpeed = 15f;

    [Header("Eventos")]
    public SideEvent OnGoal; // Dispara "Left" o "Right"

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Físicas: rebote perfecto
        PhysicsMaterial2D mat = new PhysicsMaterial2D("BallMat");
        mat.bounciness = 1f;
        mat.friction = 0f;
        GetComponent<CircleCollider2D>().sharedMaterial = mat;
    }

    private void Start()
    {
        Launch();
    }

    private void Launch()
    {
        // Dirección inicial aleatoria izquierda o derecha
        float dirX = Random.value < 0.5f ? -1f : 1f;
        float dirY = Random.Range(-0.5f, 0.5f);
        rb.velocity = new Vector2(dirX, dirY).normalized * startSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Paddle"))
        {
            // Aumentar un poco la velocidad en cada rebote con paleta
            float newSpeed = Mathf.Min(rb.velocity.magnitude + speedIncrease, maxSpeed);
            rb.velocity = rb.velocity.normalized * newSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GoalLeft"))
        {
            OnGoal?.Invoke("Right"); // Gana el jugador derecho
            ResetBall(1);
        }
        else if (other.CompareTag("GoalRight"))
        {
            OnGoal?.Invoke("Left");  // Gana el jugador izquierdo
            ResetBall(-1);
        }
    }

    private void ResetBall(int dirX)
    {
        rb.position = Vector2.zero;
        Vector2 dir = new Vector2(dirX, Random.Range(-0.5f, 0.5f)).normalized;
        rb.velocity = dir * startSpeed;
    }
}
