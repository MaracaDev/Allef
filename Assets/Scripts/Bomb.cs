using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explodeDelay = 1f;
    public float explosionRadius = 3f; // Ajuste conforme necessário
    public int explosionDamage = 2;
    public LayerMask enemyLayer;

    void Start()
    {
        Invoke("Explode", explodeDelay);
    }

    void Explode()
    {
        // Efeito visual da explosão (opcional)
        // Por exemplo, instanciar uma animação ou partículas

        // Detecta inimigos na área de explosão
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(explosionDamage);
            }
            PoisonEnemy poisonScript = enemy.GetComponent<PoisonEnemy>();
            if (poisonScript != null)
            {
                poisonScript.TakeDamage(explosionDamage);
            }
            BossController bossScript = enemy.GetComponent<BossController>();
            if (bossScript != null)
            {
                bossScript.TakeDamage(explosionDamage);
            }
            EnemyRange rangeScript = enemy.GetComponent<EnemyRange>();
            if (rangeScript != null)
            {
                rangeScript.TakeDamage(explosionDamage);
            }
        }

        // Destroi a bomba após a explosão
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha o raio de explosão no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
