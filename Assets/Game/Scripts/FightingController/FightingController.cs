using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        performMovement();
    }

    void performMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // pakai -verticalInput karena tekken mundurnya kesamping bukan kebawah
        Vector3 movement = new Vector3(-verticalInput, 0f, horizontalInput);

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
}
