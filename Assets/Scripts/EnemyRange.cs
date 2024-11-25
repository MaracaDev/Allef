using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    public float attackInterval = 3f;     // Intervalo de tempo entre ataques
    public float attackRange = 5f;        // Distância de alcance do ataque
    public int attackDamage = 10;         // Dano do ataque
    public LayerMask playerLayer;         // Layer do jogador para detecção
    public GameObject projectilePrefab;   // Prefab do projétil
    public float projectileSpeed = 5f;    // Velocidade do projétil

    private Transform player;             // Referência ao transform do jogador
    private bool canAttack = true;        // Controle de cooldown para o ataque
    private float AtackTimer;             //Cooldown attack counter
    private float interpolation = 2f;

    public bool stunned;
    public bool chase = false;
    public int health;

    // Lista de prefabs de ingredientes para dropar
    public List<GameObject> ingredientPrefabs;

    void Start()
    {
        // Encontra o jogador pela tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Calcula a distância entre o inimigo e o jogador
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Se o jogador estiver dentro do alcance de ataque
            if (distanceToPlayer <= attackRange)
            {
                AtackTimer += Time.deltaTime;

                if (AtackTimer > interpolation)
                {
                    PerformAttack();
                    AtackTimer = 0f;
                }
            }
        }
    }

    // Função para realizar o ataque
    private void PerformAttack()
    {
        // Verifica se o jogador está dentro da distância de ataque
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Inimigo disparou um projétil!");

            // Calcula a direção para o projétil
            Vector2 direction = (player.position - transform.position).normalized;

            // Cria o projétil e define sua posição inicial
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Destroy(projectile.gameObject, 2f);
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }
        else
        {
            Debug.Log("O jogador está fora do alcance de ataque.");
        }
    }

    

    // Função para desenhar gizmos (opcional, para visualizar a área de ataque no editor)
    private void OnDrawGizmosSelected()
    {
        // Desenha o círculo de alcance do ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " recebeu " + damage + " de dano. Vida restante: " + health);
        
        if (health <= 0)
        {
            Instantiate(ingredientPrefabs[0], transform.position, Quaternion.identity);
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " morreu!");
        Destroy(gameObject);
    }

   

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Magic")
        {
            TakeDamage(5);
        }
    }
}
