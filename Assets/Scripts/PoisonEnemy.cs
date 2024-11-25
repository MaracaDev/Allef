using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PoisonEnemy : MonoBehaviour
{
    public float attackRange = 1.5f;      // Dist�ncia de alcance do ataque
    public int attackDamage = 5;         // Dano do ataque direto
    public int poisonDamage = 2;         // Dano por segundo do veneno
    public float poisonDuration = 4f;    // Dura��o do envenenamento
    public LayerMask playerLayer;        // Layer do jogador para detec��o
    public float attackCooldown = 3f;    // Intervalo entre ataques
    public float moveSpeed = 2f;         // Velocidade de movimento
    public float stopDistance = 1.5f;    // Dist�ncia para parar ao alcan�ar o jogador
    public int health = 20;              // Vida do inimigo
    public float stunDuration = 0.36f;   // Dura��o do atordoamento
    public float chaseDistance = 10f;    // Dist�ncia m�xima para perseguir o jogador

    private Transform player;            // Refer�ncia ao transform do jogador
    private float attackTimer;           // Cron�metro para cooldown do ataque
    private Rigidbody2D rb;              // Refer�ncia ao Rigidbody2D do inimigo
    private Vector2 movement;            // Vetor de movimenta��o
    public Animator animator;
    private bool stunned = false;        // Estado de atordoamento

    // Lista de prefabs de ingredientes para dropar
    public List<GameObject> ingredientPrefabs;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player != null && !stunned)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Perseguir jogador se estiver dentro da dist�ncia de persegui��o
            if (distanceToPlayer <= chaseDistance)
            {
                if (distanceToPlayer > stopDistance)
                {
                    Vector2 direction = (player.position - transform.position).normalized;
                    movement = direction;
                }
                else
                {
                    movement = Vector2.zero;

                    // Ataca o jogador se estiver ao alcance e fora do cooldown
                    if (distanceToPlayer <= attackRange && attackTimer <= 0)
                    {
                        PerformPoisonAttack();
                        attackTimer = attackCooldown;
                    }
                }
            }
            else
            {
                movement = Vector2.zero; // Para de se mover se o jogador estiver fora do alcance de persegui��o
            }

            // Reduz o cooldown ao longo do tempo
            attackTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (!stunned && movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    private void PerformPoisonAttack()
    {
        Debug.Log("Inimigo aplicou ataque de veneno!");
        animator.SetTrigger("Attack"); // Ativa a anima��o de ataque
        PlayerMovement.instance.TakeDamage(attackDamage); // Aplica o dano direto
        StartCoroutine(ApplyPoisonDamage(PlayerMovement.instance)); // Inicia o dano por veneno
        
    }

    private IEnumerator ApplyPoisonDamage(PlayerMovement player)
    {
        float elapsedTime = 0f;

        // Aplica dano por veneno a cada segundo durante a dura��o do veneno
        while (elapsedTime < poisonDuration)
        {
            player.TakeDamage(poisonDamage);  // Aplica dano de veneno
            Debug.Log("Jogador envenenado: recebeu " + poisonDamage + " de dano de veneno");
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f); // Espera entre os danos de veneno
        }

        Debug.Log("Efeito de veneno terminou.");
    }

    // Fun��o para atordoar o inimigo por um tempo
    public void Stun(float stunTime)
    {
        stunned = true;
        movement = Vector2.zero; // Para o movimento enquanto atordoado
        StartCoroutine(StunCoroutine(stunTime));
    }

    private IEnumerator StunCoroutine(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }

    // Fun��o para receber dano e verificar se o inimigo deve morrer
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " recebeu " + damage + " de dano. Vida restante: " + health);
        Stun(stunDuration);
        if (health <= 0)
        {
            Instantiate(ingredientPrefabs[0], transform.position, Quaternion.identity);
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " morreu!");
        Destroy(gameObject); // Destr�i o inimigo
    }

    // Fun��o para desenhar o alcance de ataque no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    // Detecta colis�o com objetos de magia para aplicar dano
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Magic")
        {
            TakeDamage(5);
        }
    }
}
