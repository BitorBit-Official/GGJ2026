using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] LayerMask wallAndPlayerMask;
    [SerializeField] float speed;
    GameObject playerObject;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Try to find the player
        if (!collision.CompareTag("Player")) return;
        if (collision.GetComponent<Player>().isMasked) return;

        Vector2 origin = transform.position;
        Vector2 target = collision.transform.position;
        Vector2 direction = target - origin;
        float distance = direction.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction.normalized,
            distance,
            wallAndPlayerMask
        );

        if (hit && hit.collider.CompareTag("Player") && playerObject == null)
        {
            playerObject = hit.collider.gameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player player))
        {
            player.LoseLife();
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (playerObject != null)
        {
            rb.position = Vector2.MoveTowards(rb.position, playerObject.GetComponent<Rigidbody2D>().position, speed);
        }
    }
}
