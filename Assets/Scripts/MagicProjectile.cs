using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 100f;
    private Vector2 direction;
    private Vector2 startPosition;
    private PlayerMovement player;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Verifica a distância percorrida
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector2 dir, PlayerMovement playerMovement)
    {
        direction = dir.normalized;
        startPosition = transform.position;
        player = playerMovement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se colidiu com um inimigo
        if (((1 << collision.gameObject.layer) & player.enemyLayer) != 0)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            Destroy(gameObject);
        }
        // Verifica se colidiu com uma parede (assumindo que paredes estão em uma layer específica)
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha a trajetória da esfera para visualização no editor
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * maxDistance));
    }
}
