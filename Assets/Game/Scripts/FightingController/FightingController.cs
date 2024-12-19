using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator;

    [Header("Player Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamages = 5;
    public string[] attackAnimations =
    {
        "Attack1Animation",
        "Attack2Animation",
        "Attack3Animation",
        "Attack4Animation"
    };
    public float dodgeDistance = 2f;
    public float attackRadius = 2.2f;
    public Transform[] opponents;
    private float lastAttackTime;

    [Header("Ultimate Attack")]
    public float ultimateMaxCharge = 100f; // Maximum charge required for ultimate
    public float ultimateCharge = 0f;      // Current charge level
    public float ultimateChargePerAttack = 10f; // Charge gained per successful attack
    public float ultimateDamageMultiplier = 2f; // Damage multiplier for ultimate attack
    public string[] ultimateAnimations = { "UltimateAttack1", "UltimateAttack2", "UltimateAttack3", "UltimateAttack4", "UltimateAttack5", "UltimateAttack6" };
    public float ultimateCooldown;     // Cooldown after using ultimate
    private bool isUltimateReady = false;
    private bool isUltimateOnCooldown = false;
    public UltimateBar ultimateBar;

    [Header("Effects and Sound")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public ParticleSystem attack4Effect;

    public AudioClip[] hitSounds;

    [Header("Dodge Settings")]
    public float dodgeCooldown = 1f;   // Cooldown duration in seconds
    private float lastDodgeTime = -Mathf.Infinity; // Tracks the last time dodge was performed

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.GiveFullHealth(currentHealth);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        performMovement();
        PerformDodgeFront();

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("Fire1PK"))
        {
            PerformAttack(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown("Fire2PK"))
        {
            PerformAttack(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("Fire3PK"))
        {
            PerformAttack(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetButtonDown("Fire4PK"))
        {
            PerformAttack(3);
        }

        // Handle ultimate attack
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Fire5PK"))
        {
            PerformUltimateAttack();
        }
    }

    void performMovement()
    {
        float horizontalInput = Input.GetAxis("Vertical");
        float verticalInput = Input.GetAxis("Horizontal");

        // pakai -verticalInput karena tekken mundurnya kesamping bukan kebawah
        Vector3 movement = new Vector3(verticalInput, 0f, horizontalInput);

        // normalize movement agar kecepatan diagonal tetap sama dengan kecepatan horizontal/vertikal
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement); // arah hadap karakter
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // gerakkan karakter sesuai arah hadap
            if (horizontalInput > 0)
            {
                animator.SetBool("Walking", true);
            }
            else if (horizontalInput < 0)
            {
                animator.SetBool("Walking", true);
            }
            else if (verticalInput != 0)
            {
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }

    void PerformAttack(int attackIndex)
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.Play(attackAnimations[attackIndex]);

            Debug.Log("Performed attack " + (attackIndex + 1) + " dealing " + attackDamages + " damage");

            lastAttackTime = Time.time;

            bool hitSuccessful = false; // Flag to check if any opponent was hit

            // Loop through each opponent
            foreach (Transform opponent in opponents)
            {
                if (Vector3.Distance(transform.position, opponent.position) <= attackRadius)
                {
                    hitSuccessful = true;

                    // Apply damage directly to the opponent
                    OpponentAI opponentAI = opponent.GetComponent<OpponentAI>();
                    if (opponentAI != null)
                    {
                        opponentAI.TakeDamage(attackDamages); // Reduce health immediately
                    }
                }
            }

            // Only charge the ultimate if at least one opponent was hit
            if (hitSuccessful)
            {
                IncreaseUltimateCharge();
            }
            else
            {
                Debug.Log("Attack missed. No ultimate charge gained.");
            }
        }
        else
        {
            Debug.Log("Cannot perform attack yet. Cooldown time remaining.");
        }
    }

    void PerformDodgeFront()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire6PK"))
        {
            if (Time.time - lastDodgeTime >= dodgeCooldown) // Check if cooldown has passed
            {
                lastDodgeTime = Time.time; // Update last dodge time

                animator.Play("DodgeFrontAnimation");

                Vector3 dodgeDirection = transform.forward * dodgeDistance;
                characterController.Move(dodgeDirection);

                Debug.Log("Player dodged!");
            }
            else
            {
                Debug.Log("Dodge on cooldown. Time remaining: " + (dodgeCooldown - (Time.time - lastDodgeTime)).ToString("F2") + " seconds");
            }
        }
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

    // Skill untuk ultimate attack
    void IncreaseUltimateCharge()
    {
        if (!isUltimateOnCooldown)
        {
            // Validate ultimateChargePerAttack
            if (ultimateChargePerAttack <= 0)
            {
                Debug.LogWarning("Ultimate Charge Per Attack is not set properly!");
                return;
            }

            ultimateCharge += ultimateChargePerAttack;

            // Clamp the ultimateCharge to ensure it doesn't exceed the max
            ultimateCharge = Mathf.Clamp(ultimateCharge, 0, ultimateMaxCharge);

            // Normalize the charge value to fit the range [0, 1]
            float normalizedCharge = ultimateCharge / ultimateMaxCharge;

            // Update the Ultimate Bar UI with the normalized value
            ultimateBar.SetCharge(normalizedCharge);

            if (ultimateCharge >= ultimateMaxCharge)
            {
                isUltimateReady = true;
                Debug.Log("Ultimate Ready!");
                // Update UI to reflect ultimate readiness
                ultimateBar.UltimateReady(ultimateMaxCharge);
            }
        }
    }


    void PerformUltimateAttack()
    {
        if (isUltimateReady && !isUltimateOnCooldown)
        {
            StartCoroutine(ExecuteUltimate());
        }
        else if (isUltimateOnCooldown)
        {
            Debug.Log("Ultimate is on cooldown.");
        }
        else
        {
            Debug.Log("Ultimate not ready yet.");
        }
    }

    IEnumerator ExecuteUltimate()
    {
        isUltimateOnCooldown = true;
        isUltimateReady = false;
        ultimateCharge = 0f;

        // Reset UI ultimate charge bar
        ultimateBar.ResetCharge(0);

        Debug.Log("Executing Ultimate Attack!");

        foreach (string anim in ultimateAnimations)
        {
            // Play the ultimate animation
            animator.Play(anim);

            // Wait for the animation to finish
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        }

        Debug.Log("Ultimate Attack finished!");

        // Start cooldown timer
        yield return StartCoroutine(StartUltimateCooldown());
    }

    public void TriggerUltimateDamage()
    {
        int ultimateDamage = attackDamages * (int)ultimateDamageMultiplier;

        foreach (Transform opponent in opponents)
        {
            if (Vector3.Distance(transform.position, opponent.position) <= attackRadius)
            {
                OpponentAI opponentAI = opponent.GetComponent<OpponentAI>();
                if (opponentAI != null)
                {
                    opponentAI.TakeDamage(ultimateDamage); // Apply ultimate damage
                    opponentAI.StunOpponent(3f); // Stun the opponent for 2 seconds
                }
            }
        }

        Debug.Log("Ultimate damage applied to all opponents!");
    }

    IEnumerator StartUltimateCooldown()
    {
        Debug.Log("Ultimate is on cooldown.");
        // Optionally, update UI to show cooldown timer

        yield return new WaitForSeconds(ultimateCooldown);
        isUltimateOnCooldown = false;
        Debug.Log("Ultimate is ready again.");
    }

    // Optional: Regenerate ultimate over time
    void RegenerateUltimate()
    {
        if (!isUltimateReady && !isUltimateOnCooldown)
        {
            ultimateCharge += Time.deltaTime * 10f; // Adjust regeneration rate
            ultimateCharge = Mathf.Clamp(ultimateCharge, 0, ultimateMaxCharge);

            if (ultimateCharge >= ultimateMaxCharge)
            {
                isUltimateReady = true;
                Debug.Log("Ultimate Ready!");
            }
        }
    }

    void Die()
    {
        Debug.Log("Player died.");
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
