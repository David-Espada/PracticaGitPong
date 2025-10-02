using UnityEngine;
using UnityEngine.Events;

public enum Side { Left, Right }  // Enum para saber quién anotó

[System.Serializable]
public class SideEvent : UnityEvent<Side> { }

public class BallController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float startSpeed = 6f;
    [SerializeField] private float paddleHitSpeedBoost = 0.6f;
    [SerializeField] private float maxSpeed = 14f;

    [Header("Eventos")]
    public SideEvent OnGoal;  // Invoca Side.Left o Side.Right (quien anota)

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sharedMaterial = MakePerfectBounce(); // Rebote perfecto
    }

    private void OnEnable()
    {
        Launch();
    }

    private void Launch()
    {
        // Dirección inicial aleatoria hacia izquierda o derecha
        float dirX = Random.value < 0.5f ? -1f : 1f;
        float dirY = Random.Range(-0.5f, 0.5f);

        Vector2 dir = new Vector2(dirX, dirY).normalized;
        rb.velocity = dir * startSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Paddle"))
        {
            // Aumenta un poco la velocidad
            float newSpeed = Mathf.Min(rb.velocity.magnitude + paddleHitSpeedBoost, maxSpeed);

            // Ajusta trayectoria según dónde golpeó en la pala
            float y = (transform.position.y - col.transform.position.y);
            Vector2 dir = new Vector2(Mathf.Sign(rb.velocity.x), y).normalized;

            rb.velocity = dir * newSpeed;
        }
        else
        {
            // Normaliza velocidad tras rebote contra paredes
            rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, startSpeed, maxSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GoalLeft"))
        {
            OnGoal?.Invoke(Side.Right);  // Anota la derecha
            ResetBall(1);
        }
        else if (other.CompareTag("GoalRight"))
        {
            OnGoal?.Invoke(Side.Left);   // Anota la izquierda
            ResetBall(-1);
        }
    }

    private void ResetBall(int dirX)
    {
        rb.position = Vector2.zero;
        Vector2 dir = new Vector2(dirX, Random.Range(-0.4f, 0.4f)).normalized;
        rb.velocity = dir * startSpeed;
    }

    private static PhysicsMaterial2D MakePerfectBounce()
    {
        var mat = new PhysicsMaterial2D("BallMat");
        mat.bounciness = 1f;
        mat.friction = 0f;
        return mat;
    }
}
