using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    // Status
    public int health;

    // Velocidade de movimento e dash
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 1f;

    // Cooldowns
    public float ability1Cooldown = 3f;
    public float ability2Cooldown = 5f;
    public float ability3Cooldown = 7f;

    // Detec��o de ataque
    public float attackRadius = 2f;  // Tamanho do c�rculo de detec��o
    public LayerMask enemyLayer;      // Define a layer dos inimigos
    public float attackOffsetDistance = 1.5f;  // Dist�ncia � frente do jogador onde o ataque ocorre
    public float knockbackForce = 5f; // For�a do empurr�o no inimigo

    // Refer�ncias
    private Rigidbody2D rb;
    private Vector2 movementInput;
    public VariableJoystick variableJoystick;
    private SpriteRenderer spriteRenderer; // Refer�ncia ao SpriteRenderer

    // Desacelera��o
    public float smoothTime = 0.1f;
    private Vector2 currentVelocity;

    // Dash control
    private bool isDashing = false;
    private bool canDash = true;

    // Controle de habilidades
    private bool canUseAbility1 = true;
    private bool canUseAbility2 = true;
    private bool canUseAbility3 = true;

    // Dire��o de movimento para ataque
    private Vector2 lastDirection;

    // Vari�vel para exibir gizmo durante o ataque
    private bool showAttackGizmo = false;
    private float gizmoDisplayTime = 0.5f;  // Tempo que o gizmo ficar� vis�vel
    private float gizmoTimer;

    // Posi��o do gizmo de ataque
    private Vector2 attackPosition;

    // Refer�ncias para habilidades
    public GameObject magicProjectilePrefab;
    public Transform magicSpawnPoint;

    public GameObject shieldPrefab;
    public float shieldLife;

    public GameObject bombPrefab;
    public Transform bombSpawnPoint;

    public CinemachineVirtualCamera cvc;

    public Animator animator;

    //Valores de status
    public int dano = 2;

    // Cooldown para ataque
    public float attackCooldown = 1f; // Tempo de cooldown do ataque
    private bool canAttack = true;

    public static PlayerMovement instance;

    void Start()
    {
        instance = this;
        Setup();
        
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            // Captura a dire��o do joystick
            //movementInput = variableJoystick.Direction;

            // Captura a entrada do teclado para movimento
            Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movementInput = variableJoystick.Direction + keyboardInput.normalized;

            if (movementInput != Vector2.zero)
            {
                animator.SetBool("Moving", true);
                // Atualiza a �ltima dire��o baseada no movimento
                lastDirection = movementInput.normalized;

                // Atualiza o "flip" do sprite dependendo da dire��o
                FlipSpriteBasedOnDirection(lastDirection);
            }
            else
            {
                animator.SetBool("Moving", false);
            }

            // Movimenta o jogador com desacelera��o
            Vector2 targetVelocity = movementInput * moveSpeed;
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
            
        }
    }

    public void Setup()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obt�m o SpriteRenderer
        variableJoystick = FindObjectOfType<VariableJoystick>();
        cvc = FindObjectOfType<CinemachineVirtualCamera>();
        cvc.LookAt = this.transform;
        cvc.Follow = this.transform;

    }

    void Update()
    {
        // Atualiza o temporizador do gizmo de ataque
        if (showAttackGizmo)
        {
            gizmoTimer -= Time.deltaTime;
            if (gizmoTimer <= 0)
            {
                showAttackGizmo = false;
            }
        }

        // Captura a entrada do teclado para movimento
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementInput = variableJoystick.Direction + keyboardInput.normalized;

        if (movementInput != Vector2.zero)
        {
            animator.SetBool("Moving", true);
            lastDirection = movementInput.normalized;
            FlipSpriteBasedOnDirection(lastDirection);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        // Input de teclado para dash (Shift Esquerdo)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Dash();
        }

        // Input de teclado para ataque (F)
        if (Input.GetKeyDown(KeyCode.F) && canAttack)
        {
            Attack();
        }

        // Input de teclado para habilidades (E, R, T)
        if (Input.GetKeyDown(KeyCode.E) && canUseAbility1)
        {
            UseAbility1();
        }
        if (Input.GetKeyDown(KeyCode.R) && canUseAbility2)
        {
            UseAbility2();
        }
        if (Input.GetKeyDown(KeyCode.T) && canUseAbility3)
        {
            UseAbility3();
        }
    }


    // Fun��o de dash/rolamento
    public void Dash()
    {
        if (canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        animator.SetBool("Rolling", true);
        canDash = false;
        isDashing = true;

        // Captura a dire��o atual do jogador para o dash
        Vector2 dashDirection = variableJoystick.Direction;

        // Se n�o houver entrada de movimento, continua na �ltima dire��o
        if (dashDirection == Vector2.zero)
        {
            dashDirection = lastDirection; // Continua na �ltima dire��o registrada
        }

        // Aplica a velocidade de dash
        rb.velocity = dashDirection * dashSpeed;

        // Espera o tempo do dash
        yield return new WaitForSeconds(dashDuration);

        // Retorna ao movimento normal
        isDashing = false;
        animator.SetBool("Rolling", false);
        // Espera pelo cooldown antes de poder dashar novamente
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    // Fun��o de ataque com detec��o em �rea � frente do jogador
    // Fun��o de ataque com detec��o em �rea � frente do jogador
    public void Attack()
    {
        if (canAttack)
        {
            StartCoroutine(AttackCooldown());
            animator.SetBool("Atack", true);

            // Calcula a posi��o do ataque � frente do jogador com base na dire��o
            attackPosition = (Vector2)transform.position + lastDirection * attackOffsetDistance;

            // Detecta todos os inimigos dentro de um c�rculo � frente do jogador
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRadius, enemyLayer);

            if (hitEnemies.Length > 0)
            {
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("Inimigo atingido: " + enemy.name);

                    // Aplica o empurr�o (knockback) ao inimigo
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

                    if (enemyRb != null)
                    {
                        Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                        enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                    }

                    // Tenta aplicar dano no inimigo, verificando qual script de inimigo ele possui
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    PoisonEnemy poisonEnemyScript = enemy.GetComponent<PoisonEnemy>();
                    EnemyRange enemyRangeScript = enemy.GetComponent<EnemyRange>();
                    BossController bossControllerScript = enemy.GetComponent<BossController>();

                    if (enemyScript != null)
                    {
                        enemyScript.TakeDamage(dano);
                        if (enemyScript.health <= 0)
                        {
                            Destroy(enemy.gameObject);
                        }
                    }
                    else if (poisonEnemyScript != null)
                    {
                        poisonEnemyScript.TakeDamage(dano);
                        if (poisonEnemyScript.health <= 0)
                        {
                            Destroy(enemy.gameObject);
                        }
                    }
                    else if (enemyRangeScript != null)
                    {
                        enemyRangeScript.TakeDamage(dano);
                        if (enemyRangeScript.health <= 0)
                        {
                            Destroy(enemy.gameObject);
                        }
                    }

                    else if (bossControllerScript != null)
                    {
                        bossControllerScript.TakeDamage(dano);
                        print("bati no boss");
                    }
                }
            }
        }
    }



    // Fun��es das habilidades com cooldown
    public void UseAbility1()
    {
        AudioControler.Instance.PlaySound(0);
        if (canUseAbility1)
        {
            Debug.Log("Habilidade 1 (Disparo M�gico) ativada!");
            StartCoroutine(Ability1Cooldown());
            InstantiateMagicProjectile();
        }
    }

    public void UseAbility2()
    {
        AudioControler.Instance.PlaySound(1);
        if (canUseAbility2)
        {
            Debug.Log("Habilidade 2 (Escudo Poderoso) ativada!");
            StartCoroutine(Ability2Cooldown());
            ActivateShield();
        }
    }

    public void UseAbility3()
    {
        AudioControler.Instance.PlaySound(2);
        if (canUseAbility3)
        {
            Debug.Log("Habilidade 3 (�rea Explosiva) ativada!");
            StartCoroutine(Ability3Cooldown());
            ThrowBomb();
        }
    }

    // Corrotinas para os cooldowns das habilidades
    private IEnumerator Ability1Cooldown()
    {
        canUseAbility1 = false;
        yield return new WaitForSeconds(2f); // Countdown espec�fico para Disparo M�gico
        yield return new WaitForSeconds(ability1Cooldown - 2f);
        canUseAbility1 = true;
    }

    private IEnumerator Ability2Cooldown()
    {
        canUseAbility2 = false;
        yield return new WaitForSeconds(7f); // Countdown ap�s o escudo quebrar
        canUseAbility2 = true;
    }

    private IEnumerator Ability3Cooldown()
    {
        canUseAbility3 = false;
        yield return new WaitForSeconds(4f); // Countdown espec�fico para �rea Explosiva
        yield return new WaitForSeconds(ability3Cooldown - 4f);
        canUseAbility3 = true;
    }

    // Fun��o para mudar a dire��o do sprite
    private void FlipSpriteBasedOnDirection(Vector2 direction)
    {
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true; // Olha para a direita
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = false; // Olha para a esquerda
        }
    }

    // Fun��o de debug para mostrar o c�rculo de ataque no editor
    private void OnDrawGizmos()
    {
        // Se showAttackGizmo for verdadeiro, desenha o c�rculo de ataque � frente do jogador
        if (showAttackGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition, attackRadius);
        }
    }

    public void TakeDamage(int damage)
    {
        if (shieldLife <= 0)
        {
            shieldPrefab.SetActive(false);
            health -= damage;
            Debug.Log("Jogador tomou " + damage + " de dano. Vida restante: " + health);
            PlayerUI.instance.UpdateHealthUI();
        }

        else
        {
            shieldLife--;

        }
        
        if (health <= 0)
        {
            Die();
            // Implementar a l�gica de morte do jogador
        }
    }

    private void Die()
    {
        // Ativa a anima��o de morte
        animator.SetBool("Die", true);

        // Espera 1 segundo para retornar ao menu
        StartCoroutine(WaitAndReturnToMainMenu());
    }

    // Corrotina para esperar 1 segundo e depois carregar o menu principal
    private IEnumerator WaitAndReturnToMainMenu()
    {
        // Espera 1 segundo para a anima��o de morte ser conclu�da
        yield return new WaitForSeconds(1f);
        // Ativa a anima��o de morte
        animator.speed = 0.0000000001f;
        // Carrega a cena "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }

    // M�todos para habilidades

    private void InstantiateMagicProjectile()
    {
        Vector2 spawnPosition = magicSpawnPoint != null ? magicSpawnPoint.position : (Vector2)transform.position;
        GameObject projectile = Instantiate(magicProjectilePrefab, spawnPosition, Quaternion.identity);
        MagicProjectile magicScript = projectile.GetComponent<MagicProjectile>();

        if (magicScript != null)
        {
            Vector2 direction = lastDirection != Vector2.zero ? lastDirection : Vector2.right;
            magicScript.Initialize(direction, this);
        }
    }

    private void ActivateShield()
    {
        shieldPrefab.SetActive(true);
        shieldPrefab.transform.SetParent(transform); // Faz o shieldObj ser filho do objeto onde o script est� anexado
        shieldLife = 3;
    }

    private void ThrowBomb()
    {
        Vector2 spawnPosition = bombSpawnPoint != null ? bombSpawnPoint.position : (Vector2)transform.position + lastDirection;
        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
    }

    // Callback para quando o escudo � quebrado
    public void OnShieldBroken()
    {
        StartCoroutine(Ability2Cooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.01f);
        animator.SetBool("Atack", false);
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "EnemyShot")
        {
            print("levei um tiro");
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

}
