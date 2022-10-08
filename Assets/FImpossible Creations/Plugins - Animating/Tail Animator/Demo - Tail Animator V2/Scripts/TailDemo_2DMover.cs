using UnityEngine;

namespace FIMSpace
{
    public class TailDemo_2DMover : MonoBehaviour
    {
        public float MovementSpeed = 4;
        [Range(0f, 1f)]
        public float SmoothRotation = 0f;
        public float JumpPower = 12f;

        public bool DoubleJump = true;

        [Tooltip("Use keyboard keys movement implementation for quick debugging?")]
        public bool WSADMovement = true;

        [Range(0f, 0.5f)]
        [Tooltip("How slow accelerate/decelerate should be")]
        public float accelerationTime = 0.1f;

        protected float moveSpeed = 5f;
        protected float rotateSpeed = 10f;

        protected Vector3 smoothedAcceleration;

        private Vector3 moveDir = Vector3.zero;
        protected Quaternion targetRot;
        protected Vector3 veloHelper = Vector3.zero;
        protected bool isGrounded = false;

        protected float triggerJumping = 0f;

        private Rigidbody2D rigbody;
        private Collider2D charCollider;
        private int jumps = 0;



        void Start()
        {
            rigbody = GetComponentInChildren<Rigidbody2D>();
            charCollider = GetComponentInChildren<Collider2D>();
            targetRot = transform.rotation;
        }

        void Update()
        {
            moveSpeed = MovementSpeed;
            moveDir = Vector3.zero;

            if (WSADMovement)
            {
                // Move left / right
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    moveDir = Vector3.right;
                    targetRot = Quaternion.Euler(0f, 180f, 0f);
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    moveDir = Vector3.right;
                    targetRot = Quaternion.Euler(0f, 0f, 0f);
                }

                // Debug sprint
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) moveSpeed *= 1.5f;

                // Triggering jump to be executed in next fixed update
                if (jumps < (DoubleJump ? 2 : 1))
                {
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
                        triggerJumping = JumpPower;
                }
            }

            // Calculating smooth acceleration value to be used in next fixed update frame
            smoothedAcceleration = Vector3.SmoothDamp(smoothedAcceleration, moveDir, ref veloHelper, accelerationTime, Mathf.Infinity, Time.deltaTime);

            // Calculating smooth rotation
            if (SmoothRotation > 0f)
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * (1.1f - SmoothRotation) * 60f);
            else
                transform.rotation = targetRot;
        }



        protected virtual void FixedUpdate()
        {
            Vector3 velocityMemory = rigbody.velocity;

            // Supporting model scale and moving in forward direction with computed acceleration
            Vector3 targetVelo = transform.TransformVector(smoothedAcceleration) * moveSpeed;

            if (triggerJumping != 0f)
            {
                targetVelo.y = triggerJumping;
                rigbody.MovePosition(rigbody.position + Vector2.up * triggerJumping * 0.05f);
                triggerJumping = 0f;
                jumps++;
                isGrounded = false;
            }
            else targetVelo.y = velocityMemory.y; // Not changing velocity in y only accelerate in x and z


            // Applying sliding material to mover when there is desired movement
            if (!isGrounded || targetVelo.sqrMagnitude > moveSpeed * 0.2f)
                charCollider.sharedMaterial = FEngineering.PMSliding2D;
            else // When stopped we preventing from sliding on walls
                charCollider.sharedMaterial = FEngineering.PMFrict2D;


            rigbody.velocity = targetVelo;
        }




        /// <summary>
        /// Checking grounded state
        /// </summary>
        protected virtual void OnCollisionEnter2D(UnityEngine.Collision2D collision)
        {
            if (!isGrounded)
                if (collision != null)
                    if (collision.contacts.Length > 0)
                    {
                        for (int i = 0; i < collision.contacts.Length; i++)
                        {
                            float dot = Vector2.Dot(rigbody.transform.up, collision.contacts[i].normal);
                            // Dot == 1 -> Standing on flat surface
                            // Dot == 0 -> pushing against vertical wall
                            // Dot == 0.5 -> standing on 45 degrees wall

                            if (dot > 0.25f) // Standing max on 22,5 degrees wall
                            {
                                OnGrounded();
                                return;
                            }
                        }
                    }
        }



        /// <summary>
        /// To be overrided for particel effects / animations
        /// </summary>
        protected virtual void OnGrounded()
        {
            isGrounded = true;
            jumps = 0;
        }



    }
}