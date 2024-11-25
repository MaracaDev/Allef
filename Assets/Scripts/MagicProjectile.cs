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

        // Verifica a dist�ncia percorrida
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
        // Verifica se colidiu com uma parede (assumindo que paredes est�o em uma layer espec�fica)
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha a trajet�ria da esfera para visualiza��o no editor
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * maxDistance));
    }
}
