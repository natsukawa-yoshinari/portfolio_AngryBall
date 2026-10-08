using UnityEngine;

public class InPortal : MonoBehaviour
{
    [SerializeField]
    private Transform _outPortal;

    private const int LAYER_PLAYER = 7;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LAYER_PLAYER) return;
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        Vector2 velocity = rb.linearVelocity;

        collision.transform.position = _outPortal.position;
        rb.linearVelocity = velocity;
    }
}
