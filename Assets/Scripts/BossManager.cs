using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform[] movePoints;
    public float moveInterval = 5f;
    public float moveSpeed = 2f;

    public int maxHealth = 100;
    public int currentHealth;

    public float chargeRange = 5f;
    public int chargeDamage = 15;
    public float chargeSpeed = 5f;
    public float chargeCooldown = 8f;
    private bool isCharging = false;
    private float chargeTimer = 0f;

    public float invisibilityDuration = 15f;
    private bool isInvisible = false;

    public GameObject deadlyAreaPrefab;
    public float deadlyAreaDuration = 10f;
    public float deadlyAreaInterval = 3f;
    public float deadlyAreaCooldown = 20f;
    private bool isDeadlyAreaActive = false;

    private Transform player;
    private int targetPointIndex = 0;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(MoveToPoints());
    }

    private void Update()
    {
        HandleChargeAttack();
        HandleInvisibility();
        HandleDeadlyAreas();

        // Chamar habilidade aleatória com a tecla 'R' (ou configure um evento específico)
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseRandomAbility();
        }
    }

    private IEnumerator MoveToPoints()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            Vector3 targetPosition = movePoints[targetPointIndex].position;
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f && !isCharging)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            targetPointIndex = (targetPointIndex + 1) % movePoints.Length;
        }
    }

    private void HandleChargeAttack()
    {
        if (chargeTimer > 0)
        {
            chargeTimer -= Time.deltaTime;
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= chargeRange && !isCharging)
        {
            StartCoroutine(ChargeAttack());
            chargeTimer = chargeCooldown;
        }
    }

    private IEnumerator ChargeAttack()
    {
        isCharging = true;
        Vector3 direction = (player.position - transform.position).normalized;

        float chargeTime = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < chargeTime)
        {
            transform.position += direction * chargeSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isCharging = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(chargeDamage);
            Debug.Log("Boss causou dano ao avançar!");
        }
    }

    private void HandleInvisibility()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isInvisible)
        {
            StartCoroutine(GoInvisible());
        }
    }

    private IEnumerator GoInvisible()
    {
        isInvisible = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(invisibilityDuration);

        isInvisible = false;
        spriteRenderer.enabled = true;
    }

    private void HandleDeadlyAreas()
    {
        if (!isDeadlyAreaActive && Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(ActivateDeadlyAreas());
        }
    }

    private IEnumerator ActivateDeadlyAreas()
    {
        isDeadlyAreaActive = true;
        float elapsed = 0f;

        while (elapsed < deadlyAreaDuration)
        {
            yield return new WaitForSeconds(deadlyAreaInterval);

            GameObject area = Instantiate(deadlyAreaPrefab, player.position, Quaternion.identity);
            StartCoroutine(HandleDeadlyAreaDamage(area));
            elapsed += deadlyAreaInterval;
        }

        isDeadlyAreaActive = false;
        yield return new WaitForSeconds(deadlyAreaCooldown);
    }

    private IEnumerator HandleDeadlyAreaDamage(GameObject area)
    {
        yield return new WaitForSeconds(deadlyAreaDuration);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(area.transform.position, 1f);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerMovement>().TakeDamage(2);
                Debug.Log("Área mortal causou dano ao jogador!");
            }
        }

        Destroy(area);
    }

    private void UseRandomAbility()
    {
        int randomAbility = Random.Range(0, 3); // Ajuste o intervalo com base na quantidade de habilidades

        switch (randomAbility)
        {
            case 0:
                Debug.Log("Boss usou ataque de carga!");
                if (!isCharging)
                    StartCoroutine(ChargeAttack());
                break;
            case 1:
                Debug.Log("Boss ficou invisível!");
                if (!isInvisible)
                    StartCoroutine(GoInvisible());
                break;
            case 2:
                Debug.Log("Boss ativou áreas mortais!");
                if (!isDeadlyAreaActive)
                    StartCoroutine(ActivateDeadlyAreas());
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Boss recebeu {damage} de dano. Vida atual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        LoadScene.instance.LoaderScene("MainMenu");
        Debug.Log("Boss morreu!");
        Destroy(gameObject);
    }
}
