using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

public class Movements : MonoBehaviour {

    private Animator animator;
    // basic motion variables
    private float speed;
    private float side;
    private float run;
    private bool sneak;
    private float normalizedSpeed;
    // jumping variables
    private bool isJumping;
    private const float JUMP_VELOCITY = 7f;
    private float JUMP_ROTATION_SPEED = 2f;
    private float NEAR_GROUND_MARKER = 1.2f;
    // rotation variables
    private float mouseX;
    private Rigidbody rigidbody;
    private Weapons weaponsScript;
    private float TURN_SMOOTHING_TIMER = 0.2f;
    private float turnSmoothVelocity;

    public bool getSneak {
        get { return sneak; }
    }

    public float getRun {
        get { return run; }
    }

    public bool getGroundedMarker {
        get { return isGrounded(); }
    }

    // Use this for initialization
    private void Start() {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        weaponsScript = GetComponent<Weapons>();
    }

    // Update is called once per frame
    private void Update() {
        speed = Input.GetAxisRaw("Vertical");
        side = Input.GetAxisRaw("Horizontal");
        run = Input.GetButton("Run") && isGrounded() ? 1 : 0;
        sneak = isGrounded() ? Input.GetButton("Sneak") : false;
        mouseX += Input.GetAxis("Mouse X");

        speed += speed > 0 ? run : speed < 0 ? -run : 0;
        side += side > 0 ? run : side < 0 ? -run : 0;

        if (weaponsScript.FightingMode) {
            transform.rotation = Quaternion.Euler(0, mouseX, 0);

            if (!isGrounded()) {
                transform.Translate(
                    side * JUMP_ROTATION_SPEED * Time.deltaTime,
                    0,
                    speed * JUMP_ROTATION_SPEED * Time.deltaTime
                );
            }

        } else {
            Vector2 inputDir = new Vector2(side, speed);
            Vector3 globalInputRelativeToCamera = Camera.main.transform.TransformDirection(inputDir.x, 0, inputDir.y);
            Vector3 globalDirectionRelativeToCamera = globalInputRelativeToCamera.normalized;

            if (inputDir != Vector2.zero) {
                float newRotation = Mathf.Atan2(globalDirectionRelativeToCamera.x, globalDirectionRelativeToCamera.z) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, newRotation, ref turnSmoothVelocity, TURN_SMOOTHING_TIMER);
            }

            if (isGrounded())
                normalizedSpeed = globalDirectionRelativeToCamera.magnitude + run;

            if (!isGrounded())
                transform.position += new Vector3(
                    globalInputRelativeToCamera.x * JUMP_ROTATION_SPEED * Time.deltaTime,
                    0,
                    globalInputRelativeToCamera.z * JUMP_ROTATION_SPEED * Time.deltaTime
                );
        }
    }

    private void LateUpdate() {
        animator.SetFloat("Speed", speed);
        animator.SetFloat("NormalizedSpeed", normalizedSpeed);
        animator.SetFloat("Side", side);
        animator.SetBool("Sneak", sneak);
        animator.SetBool("Jump", isJumping);
        animator.SetBool("Falling", isFalling());
        animator.SetBool("Grounded", isGrounded());
        animator.SetBool("NearGround", nearGround());

        if (isGrounded() && Input.GetButton("Jump")) {
            startJumping();
            this.isJumping = true;
        }

        if (isFalling() && nearGround())
            Invoke("UpdateJumpChecker", 0.7f);
    }

    private void UpdateJumpChecker() {
        this.isJumping = false;
    }

    // Check if character on ground now
    private bool isGrounded() {
        RaycastHit hit;
        return Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), Vector3.down, out hit, 0.2f);
    }

    // Check if character near ground while falling
    private bool nearGround() {
        RaycastHit hit;
        return (!isGrounded()) && Physics.Raycast(transform.position, Vector3.down, out hit, NEAR_GROUND_MARKER);
    }

    // Falling Logic
    private bool isFalling() {
        return (!isGrounded() && rigidbody.velocity.y < 0);
    }

    // Jump Logic
    private void startJumping() {
        rigidbody.velocity = new Vector3(side, JUMP_VELOCITY, speed);
    }
}
