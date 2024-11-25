using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 2f;
    public float attackCooldown = 3f; // Cooldown entre ataques
    public int attackDamage = 10;
    public LayerMask playerLayer;

    private Transform player;
    private bool canAttack = true;
    private Rigidbody2D rb;
    private Vector2 movement;

    public bool stunned;
    public int health;
    public float chaseDistance;

    public Animator animator;

    // Lista de prefabs de ingredientes para dropar
    public List<GameObject> ingredientPrefabs;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null && !stunned)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Verifica se o jogador está dentro do alcance de perseguição
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

                    if (canAttack)
                    {
                        PerformAttack();
                    }
                }
            }
            else
            {
                // Para o movimento se o jogador estiver fora do alcance
                movement = Vector2.zero;
                animator.SetBool("Walk", false);
            }
        }
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    private void PerformAttack()
    {
        canAttack = false;
        animator.SetTrigger("Attack"); // Ativa a animação de ataque
        PlayerMovement.instance.TakeDamage(attackDamage);

        // Inicia o cooldown do ataque
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);

        // Garante que o inimigo volte para Idle após o ataque
        animator.SetBool("Attack", false);
        canAttack = true;
    }

    public void Stun(float stunnedTimer)
    {
        print("stunado lixo");
        stunned = true;
        StartCoroutine(StunWait(stunnedTimer));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }

    public IEnumerator StunWait(float time)
    {
        movement = Vector2.zero;
        yield return new WaitForSeconds(time);
        stunned = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " recebeu " + damage + " de dano. Vida restante: " + health);
        Stun(0.36f);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " morreu!");

        // Dropa um ingrediente aleatório ao morrer
        if (ingredientPrefabs.Count > 0)
        {
            Instantiate(ingredientPrefabs[0], transform.position, Quaternion.identity);
        }

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
