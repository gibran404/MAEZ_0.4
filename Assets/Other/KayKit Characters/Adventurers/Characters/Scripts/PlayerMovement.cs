using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    [SerializeField] private float baseSpeed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float sprintMultiplier = 1.5f;

    private float speedBoost = 1f;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // anim booleans: Walking, Running
    }

    void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedBoost = sprintMultiplier;
            anim.SetBool("Running", true);
        }
        else
        {
            speedBoost = 1f;
            anim.SetBool("Running", false);
        }

        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        if (move != Vector3.zero)
        {
            transform.forward = move; // Make the character look in the direction of movement
        }

        controller.Move(move * baseSpeed * speedBoost * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
