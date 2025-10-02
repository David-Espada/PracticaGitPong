using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [Header("Controles")]
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;

    [Header("Movimiento (unidades/seg)")]
    [SerializeField] private float speed = 70f;

    [Header("Límites (mundo)")]
    [SerializeField] private float topClamp = 4.3f;
    [SerializeField] private float bottomClamp = -4.3f;

    private Rigidbody2D rb;
    private float inputY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.isKinematic = true;
        rb.gravityScale = 0f;
        rb.drag = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        inputY = 0f;
        if (Input.GetKey(upKey)) inputY += 1f;
        if (Input.GetKey(downKey)) inputY -= 1f;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(inputY) > 0f)
        {
            Vector2 pos = rb.position + Vector2.up * (inputY * speed * Time.fixedDeltaTime);
            pos.y = Mathf.Clamp(pos.y, bottomClamp, topClamp);
            rb.MovePosition(pos);
        }
    }
    public void SetKeys(KeyCode up, KeyCode down) { upKey = up; downKey = down; }
}
