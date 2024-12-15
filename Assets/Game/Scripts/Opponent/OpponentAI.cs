using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    [Header("Opponent Movement")]
    public float movementSpeed = 2f;
    public float rotationSpeed = 10f;
    public CharacterController characterController;
    public Animator animator;

    [Header("Opponent Fight")]
    public float attackCooldown = 1f;
    public int attackDamages = 2;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    public float attackRadius = 2.0f;

    [Header("Dodge Settings")]
    public float dodgeCooldown = 5f;
    private float lastDodgeTime;

    [Header("Special Attack")]
    public float specialAttackCooldown = 10f;
    private float lastSpecialAttackTime;

    [Header("Retreat Settings")]
    public float retreatDuration = 1.5f;
    public float retreatSpeedMultiplier = 2.0f;
    private float retreatStartTime;
    private bool isRetreating = false;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    [Header("Effects and Sounds")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public ParticleSystem attack4Effect;
    public AudioClip[] hitSounds;

    private float lastAttackTime;
    private Transform player;
    private bool isTakingDamage;

    private enum OpponentState { Idle, Chase, Attack, Dodge, Retreat }
    private OpponentState currentState = OpponentState.Chase;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.GiveFullHealth(currentHealth);

        StartCoroutine(FindActivePlayer());
        ApplyDifficultySettings();
    }

    IEnumerator FindActivePlayer()
    {
        while (player == null)
        {
            foreach (Transform child in GameObject.Find("PlayerCharacters").transform)
            {
                if (child.gameObject.activeSelf && child.CompareTag("Player"))
                {
                    player = child;
                    break;
                }
            }
            if (player == null)
            {
                Debug.Log("Waiting for active player...");
                yield return null;
            }
        }

        Debug.Log("Active Player Found: " + player.name);
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        switch (currentState)
        {
            case OpponentState.Chase:
                ChasePlayer();
                break;
            case OpponentState.Attack:
                PerformAttack();
                break;
            case OpponentState.Dodge:
                PerformDodge();
                break;
            case OpponentState.Retreat:
                Retreat();
                break;
        }

        UpdateBehaviorBasedOnHealth();
    }

    void ApplyDifficultySettings()
    {
        if (DifficultyManager.Instance != null)
        {
            switch (DifficultyManager.Instance.currentDifficulty)
            {
                case DifficultyManager.Difficulty.Easy:
                    attackCooldown = 2f;
                    attackDamages = 3;
                    movementSpeed = 1.5f;
                    break;
                case DifficultyManager.Difficulty.Medium:
                    attackCooldown = 1.5f;
                    attackDamages = 5;
                    movementSpeed = 2.5f;
                    break;
                case DifficultyManager.Difficulty.Hard:
                    attackCooldown = 1f;
                    attackDamages = 8;
                    movementSpeed = 3.5f;
                    break;
            }
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRadius)
        {
            currentState = OpponentState.Attack;
        }
        else
        {
            Vector3 direction = (player.position - transform.position).normalized;
            characterController.Move(direction * movementSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("Walking", true);
        }
    }

    void PerformAttack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRadius)
            {
                int attackIndex = Random.Range(0, attackAnimations.Length);
                animator.Play(attackAnimations[attackIndex]);
                TriggerAttackEffect(attackIndex);

                Debug.Log("Opponent performed attack: " + attackAnimations[attackIndex]);

                player.GetComponent<FightingController>().StartCoroutine(
                    player.GetComponent<FightingController>().PlayHitDamageAnimation(attackDamages));

                lastAttackTime = Time.time;
            }
        }

        // Check if special attack can occur
        if (Time.time - lastSpecialAttackTime > specialAttackCooldown)
        {
            PerformSpecialAttack();
        }
    }

    void TriggerAttackEffect(int attackIndex)
    {
        switch (attackIndex)
        {
            case 0:
                if (attack1Effect != null) attack1Effect.Play();
                break;
            case 1:
                if (attack2Effect != null) attack2Effect.Play();
                break;
            case 2:
                if (attack3Effect != null) attack3Effect.Play();
                break;
            case 3:
                if (attack4Effect != null) attack4Effect.Play();
                break;
        }
    }

    void PerformSpecialAttack()
    {
        animator.Play("SpecialAttackAnimation");
        Debug.Log("Opponent performed a SPECIAL ATTACK!");

        if (Vector3.Distance(transform.position, player.position) <= attackRadius)
        {
            player.GetComponent<FightingController>().StartCoroutine(
                player.GetComponent<FightingController>().PlayHitDamageAnimation(attackDamages * 2));
        }

        lastSpecialAttackTime = Time.time;
    }

    void PerformDodge()
    {
        animator.Play("DodgeFrontAnimation");

        Vector3 dodgeDirection = -transform.forward * dodgeDistance;
        characterController.Move(dodgeDirection);

        Debug.Log("Opponent dodged!");

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRadius)
        {
            currentState = OpponentState.Attack;
        }
        else
        {
            currentState = OpponentState.Chase;
        }
    }

    void Retreat()
    {
        if (Time.time - retreatStartTime < retreatDuration)
        {
            Vector3 retreatDirection = -transform.forward * movementSpeed * retreatSpeedMultiplier * Time.deltaTime;
            characterController.Move(retreatDirection);

            animator.SetBool("Walking", true);
        }
        else
        {
            isRetreating = false;
            animator.SetBool("Walking", false);
            currentState = OpponentState.Chase;
        }
    }

    void UpdateBehaviorBasedOnHealth()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentHealth <= maxHealth * 0.2f && !isRetreating)
        {
            StartRetreat();
        }
        else if (distanceToPlayer <= attackRadius)
        {
            currentState = OpponentState.Attack;
        }
        else if (Time.time - lastDodgeTime > dodgeCooldown)
        {
            if (Random.value < 0.1f)
            {
                currentState = OpponentState.Dodge;
                lastDodgeTime = Time.time;
            }
        }
        else
        {
            currentState = OpponentState.Chase;
        }
    }

    void StartRetreat()
    {
        isRetreating = true;
        retreatStartTime = Time.time;
        currentState = OpponentState.Retreat;
    }

    public void TakeDamage(int damage)
    {
        // Reduce health immediately
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        Debug.Log("Opponent took " + damage + " damage. Remaining health: " + currentHealth);

        // Play hit animation
        StartCoroutine(PlayHitDamageAnimation());

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public IEnumerator PlayHitDamageAnimation()
    {
        yield return new WaitForSeconds(0.1f); // Optional delay before playing animation

        // Play hit animation
        animator.Play("HitDamageAnimation");

        // Optionally, play hit sound
        if (hitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randomIndex], transform.position);
        }
    }

    void Die()
    {
        // animator.Play("DeathAnimation");
        Debug.Log("Opponent died.");
    }
}