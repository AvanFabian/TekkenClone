using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    [Header("Opponent Movement")]
    public float movementSpeed = 0f;
    public float rotationSpeed = 10f;
    public CharacterController characterController;
    public Animator animator;

    [Header("Opponent Fight")]
    public float attackCooldown = 0f;
    public int attackDamages = 0;
    public string[] attackAnimations =
    {
        "Attack1Animation",
        "Attack2Animation",
        "Attack3Animation",
        "Attack4Animation"
    };
    public float dodgeDistance = 2f;
    public int attackCount = 0;
    public int randomNumber;
    public float attackRadius = 2f;
    public FightingController[] fightingController;
    public Transform[] players;
    public bool isTakingDamage;
    private float lastAttackTime;

    [Header("Effects and Sound")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public ParticleSystem attack4Effect;

    public AudioClip[] hitSounds;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.GiveFullHealth(currentHealth);
        createRandomNumber();

        // Fetch difficulty settings from DifficultyManager
        ApplyDifficultySettings();  
    }

    void Update()
    {
        // initialize for loop yang mengambil semua elemen dari Array fightingController
        for (int i = 0; i < fightingController.Length; i++)
        {
            if (players[i].gameObject.activeSelf && Vector3.Distance(transform.position, players[i].position) <= attackRadius)
            {
                animator.SetBool("Walking", false);

                if (Time.time - lastAttackTime > attackCooldown)
                {
                    int randomAttackIndex = Random.Range(0, attackAnimations.Length);

                    if (!isTakingDamage)
                    {
                        PerformAttack(randomAttackIndex);
                    }
                    // Play hit/damage animation on player
                    fightingController[i].StartCoroutine(fightingController[i].PlayHitDamageAnimation(attackDamages));
                }
            }
            else
            {
                // jika objek players/karakter aktif, maka...
                if (players[i].gameObject.activeSelf)
                {
                    // maka bergerak menuju player
                    Vector3 direction = (players[i].position - transform.position).normalized;
                    characterController.Move(direction * movementSpeed * Time.deltaTime);

                    // maka berotasi menuju player
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );
                    // maka aktifkan animasi berjalan
                    animator.SetBool("Walking", true);
                }
            }
        }
    }

    void ApplyDifficultySettings()
    {
        if (DifficultyManager.Instance != null)
        {
            switch (DifficultyManager.Instance.currentDifficulty)
            {
                case DifficultyManager.Difficulty.Easy:
                    attackCooldown = DifficultyManager.Instance.easyAttackCooldown;
                    attackDamages = DifficultyManager.Instance.easyAttackDamage;
                    movementSpeed = DifficultyManager.Instance.easyMovementSpeed;
                    break;

                case DifficultyManager.Difficulty.Medium:
                    attackCooldown = DifficultyManager.Instance.mediumAttackCooldown;
                    attackDamages = DifficultyManager.Instance.mediumAttackDamage;
                    movementSpeed = DifficultyManager.Instance.mediumMovementSpeed;
                    break;

                case DifficultyManager.Difficulty.Hard:
                    attackCooldown = DifficultyManager.Instance.hardAttackCooldown;
                    attackDamages = DifficultyManager.Instance.hardAttackDamage;
                    movementSpeed = DifficultyManager.Instance.hardMovementSpeed;
                    break;
            }

            Debug.Log("Difficulty Applied: " + DifficultyManager.Instance.currentDifficulty);
        }
    }

    void PerformAttack(int attackIndex)
    {
        animator.Play(attackAnimations[attackIndex]);

        int damage = attackDamages;
        Debug.Log("Performed attack" + (attackIndex + 1) + " dealing " + damage + "damage");

        lastAttackTime = Time.time;
    }

    void PerformDodgeFront()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("DodgeFrontAnimation");

            Vector3 dodgeDirection = -transform.forward * dodgeDistance;

            characterController.SimpleMove(dodgeDirection);
        }
    }

    void createRandomNumber()
    {
        randomNumber = Random.Range(1, 5);
    }

    public IEnumerator PlayHitDamageAnimation(int takeDamage)
    {
        yield return new WaitForSeconds(0.5f);

        // Play random hit sound
        if(hitSounds != null && hitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randomIndex], transform.position);
        }
        // decrease health
        currentHealth -= takeDamage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }

        animator.Play("HitDamageAnimation");
    }

    void Die()
    {
        Debug.Log("Opponent died.");
    }

    public void Attack1Effect()
    {
        attack1Effect.Play();
    }

    public void Attack2Effect()
    {
        attack2Effect.Play();
    }

    public void Attack3Effect()
    {
        attack3Effect.Play();
    }

    public void Attack4Effect()
    {
        attack4Effect.Play();
    }
}
