using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PaddleController2D : MonoBehaviour
{
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public float speed = 10f;
    public float padding = 0.3f;

    Rigidbody2D rb;
    float halfHeight = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        var sr = GetComponent<SpriteRenderer>();
        if (sr) halfHeight = sr.bounds.extents.y;
    }

    void Update()
    {
        float dir = (Input.GetKey(upKey) ? 1f : 0f) - (Input.GetKey(downKey) ? 1f : 0f);
        rb.velocity = new Vector2(0f, dir * speed);

        // Clamp dentro de cámara
        float top = Camera.main.orthographicSize - padding - halfHeight;
        float bottom = -Camera.main.orthographicSize + padding + halfHeight;
        var p = transform.position;
        p.y = Mathf.Clamp(p.y, bottom, top);
        transform.position = p;
    }
}
