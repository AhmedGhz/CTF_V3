using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This component is a simple physics class that simulates some aspects of an aircraft. Its central goal is to
    /// process the control values from the <see cref="Package.AircraftController"/> and apply these forces and angles
    /// to the rigidbody. Additionally to the forces from the controller, lift is calculated based on the movement in
    /// the XZ-plane. If the aircraft moves faster than the <see cref="MinAirborneVelocity"/>, lift is strong enough to
    /// compensate gravity and keep the airplane in the air.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Character/Aircraft Physics")]
    [HelpURL("http://docs.polarith.com/ai/component-aimp-aircraftcontroller.html")]
    [RequireComponent(typeof(AircraftController))]
    public sealed class AircraftPhysics : MonoBehaviour
    {
        #region Fields =================================================================================================

        [Tooltip("Affects how strong and, thus, how fast rotations are applied to the aircraft.")]
        [SerializeField]
        private float torque = 1.0f;

        [Tooltip("Defines how strong a translation force is applied to the aircraft.")]
        [SerializeField]
        private float speed = 50.0f;

        [Tooltip("Minimum velocity that is required for staying airborne. Lower than this, lift cannot compensate " +
            "gravity.")]
        [SerializeField]
        private float minAirborneVelocity = 15.0f;

        [Tooltip("The 'Aircraft Controller' component that is used to calculate force and rotation values.")]
        [SerializeField]
        private AircraftController aircraftController;

        private Vector3 force;
        private Vector3 radialForce;
        private Rigidbody body;
        private Vector3 eulerAngleVelocity;
        private Vector3 translation;
        private float liftForAirborne;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Affects how strong and, thus, how fast rotations are applied to the aircraft.
        /// </summary>
        public float Torque
        {
            get { return torque; }
            set { torque = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Defines how strong a translation force is applied to the aircraft.
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Minimum velocity that is required for staying airborne. Lower than this, lift cannot compensate gravity.
        /// </summary>
        public float MinAirborneVelocity
        {
            get { return minAirborneVelocity; }
            set { minAirborneVelocity = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The <see cref="Package.AircraftController"/> component that is used to calculate force and rotation values.
        /// </summary>
        public AircraftController AircraftController
        {
            get { return aircraftController; }
            set { aircraftController = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void Start()
        {
            AircraftController = GetComponent<AircraftController>();
            body = AircraftController.Body;
            body.maxAngularVelocity = 4;
            liftForAirborne = body.mass * -Physics.gravity.y;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            if (MinAirborneVelocity <= 0)
                MinAirborneVelocity = 0.01f;

            eulerAngleVelocity = new Vector3(-AircraftController.Pitch,
                                              AircraftController.Yaw,
                                              AircraftController.Roll);
            eulerAngleVelocity = eulerAngleVelocity
                * Mathf.Clamp(body.velocity.magnitude / MinAirborneVelocity, 0, 1.0f);
            translation = transform.forward * AircraftController.Force.z;

            float lift = CalculateLift(new Vector2(body.velocity.x, body.velocity.z).magnitude);

            radialForce = eulerAngleVelocity * Torque * body.mass;
            force = translation * Speed * body.mass + lift * transform.up;

            body.AddRelativeTorque(radialForce);
            body.AddForce(force);
        }

        //--------------------------------------------------------------------------------------------------------------

        private float CalculateLift(float velocity)
        {
            float liftFactor = (velocity / MinAirborneVelocity);
            liftFactor = Mathf.Clamp(liftFactor, 0, 1.1f);
            return liftForAirborne * liftFactor;
        }

        #endregion // Methods
    } // class AircraftPhysics
} // namespace Polarith.AI.Package
