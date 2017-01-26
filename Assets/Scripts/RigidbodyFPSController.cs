using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class RigidbodyFPSController : MonoBehaviour {

    public float speed = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    private bool grounded = false;

    void Awake() {
        //GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    void FixedUpdate() {
        if (grounded) {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = Vector3.zero;
            /*if (Physics.gravity.x != 0) {
                targetVelocity = new Vector3(0, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            } else if (Physics.gravity.y != 0) {
                targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            } else if (Physics.gravity.z != 0) {
                targetVelocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            }*/
            targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            if (Physics.gravity.x != 0) {
                velocityChange.x = 0;
                velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            } else if (Physics.gravity.y != 0) {
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            } else if (Physics.gravity.z != 0) {
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = 0;
            }
            
            //velocityChange.y = 0;
            GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            /*if (canJump && Input.GetButton("Jump")) {
                GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            }*/
        }

        // We apply gravity manually for more tuning control
        GetComponent<Rigidbody>().AddForce(new Vector3(Physics.gravity.x * GetComponent<Rigidbody>().mass, Physics.gravity.y * GetComponent<Rigidbody>().mass, Physics.gravity.z * GetComponent<Rigidbody>().mass));

        grounded = false;
    }

    void OnCollisionStay() {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed() {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return 0;//Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}