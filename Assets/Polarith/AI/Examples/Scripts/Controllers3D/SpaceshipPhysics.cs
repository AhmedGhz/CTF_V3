using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This component is a physics class to enable movement in a gravity-free space. This class processes the steering
    /// values from the attached <see cref="Package.SpaceshipController"/> and applies them to the connected <see
    /// cref="Rigidbody"/>. Therefore, it uses the force (which may be with you) and angle values from the controller
    /// and applies them as force and relative torque to the given rigidbody. It requires a <see
    /// cref="Package.SpaceshipController"/> and a rigidbody. Note that the controller is able to move along all three
    /// dimensions.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Character/Spaceship Physics")]
    [HelpURL("http://docs.polarith.com/ai/component-aimp-spaceshipcontroller.html")]
    [RequireComponent(typeof(SpaceshipController))]
    public sealed class SpaceshipPhysics : MonoBehaviour
    {
        #region Fields =================================================================================================

        [Tooltip("Affects how strong and, thus, how fast rotations are applied to the spaceship.")]
        [SerializeField]
        private float torque = 25.0f;

        [Tooltip("Defines how much a translation force is applied to the spaceship.")]
        [SerializeField]
        private float speed = 1000.0f;

        [Tooltip("The 'Spaceship Controller' component that is used to calculate force and rotation values.")]
        [SerializeField]
        private SpaceshipController spaceshipController;

        private Vector3 eulerAngleVelocity;
        private Vector3 translation;
        private Rigidbody body;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Affects how strong and, thus, how fast rotations are applied to the spaceship.
        /// </summary>
        public float Torque
        {
            get { return torque; }
            set { torque = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Defines how much a translation force is applied to the spaceship.
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The <see cref="Package.SpaceshipController"/> that is used to calculate force and rotation values.
        /// </summary>
        public SpaceshipController SpaceshipController
        {
            get { return spaceshipController; }
            set { spaceshipController = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void Start()
        {
            SpaceshipController = GetComponent<SpaceshipController>();
            body = SpaceshipController.Body;
            body.maxAngularVelocity = 4;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            eulerAngleVelocity = new Vector3(-SpaceshipController.Pitch,
                                              SpaceshipController.Yaw,
                                              SpaceshipController.Roll);
            translation = transform.right * SpaceshipController.Force.x
                        + transform.up * SpaceshipController.Force.y
                        + transform.forward * SpaceshipController.Force.z;

            SpaceshipController.Body.AddRelativeTorque(eulerAngleVelocity * Torque * body.mass);
            SpaceshipController.Body.AddForce(translation * Speed * body.mass);
        }

        #endregion // Methods
    } // class SpaceshipPhysics
} // namespace Polarith.AI.Package
