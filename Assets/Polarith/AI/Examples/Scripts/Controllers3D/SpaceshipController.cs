using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This component is a physics-based controller that is designed to steer a spaceship in a gravity-free space. It
    /// transforms the AI´s decision into a 6-DoF command that can be used by the <see cref="SpaceshipPhysics"/> class.
    /// The calculation of the <see cref="RpyController.Roll"/>, <see cref="RpyController.Pitch"/>, <see
    /// cref="RpyController.Yaw"/> works in a way that the spaceship will turn towards the AI´s current decision. Also,
    /// the roll angle will be calculated in a way that the ship will try to be parallel to a plane defined by the <see
    /// cref="UpVector"/>. This can also be used to make it fly upside down or to simulate gravity towards a particular
    /// object as, for instance, a planet.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Character/Spaceship Controller")]
    [HelpURL("http://docs.polarith.com/ai/component-aimp-spaceshipcontroller.html")]
    [DisallowMultipleComponent]
    public sealed class SpaceshipController : RpyController
    {
        #region Fields =================================================================================================

        private Vector3 upVector = Vector3.up;

        [SerializeField]
        [Tooltip("If set equal to or greater than 0, the evaluated AI decision value scales the translation force")]
        [Move.TargetObjective(true)]
        private int objectiveAsSpeed = -1;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// This vector defines where up is. Typically this will be <see cref="Vector3.up"/>. An application can be to
        /// keep your vehicle horizontal to some other object.
        /// </summary>
        public Vector3 UpVector
        {
            get { return upVector; }
            set { upVector = value; }
        }

        /// <summary>
        /// If set equal to or greater than 0, the evaluated AI decision value scales the translation force.
        /// </summary>
        public int ObjectiveAsSpeed
        {
            get
            {
                return objectiveAsSpeed;
            }
            set
            {
                if (context.ObjectiveCount > value)
                    objectiveAsSpeed = value;
                else
                    Debug.LogWarning("Index out of bounds! Maximum objective index: " + (Context.ObjectiveCount - 1));
            }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        /// <summary>
        /// This function calculates the pitch angle in degrees based on the <see
        /// cref="AI.Move.AIMContext.LocalDecidedDirection"/>.
        /// </summary>
        protected override void CalculatePitch()
        {
            pitch = Mathf.Atan2(Context.LocalDecidedDirection.y, Context.LocalDecidedDirection.z) * Mathf.Rad2Deg;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This function calculates the pitch angle in degree based on the forward direction and the <see
        /// cref="UpVector"/>.
        /// </summary>
        protected override void CalculateRoll()
        {
            float angleToUp = SignedAngle(transform.up, upVector, transform.forward);

            Vector3 unrotatedLocalXAxis = Quaternion.AngleAxis(angleToUp, transform.forward) * transform.right;
            Vector3 unrotatedLocalYAxis = Quaternion.AngleAxis(90, transform.forward) * unrotatedLocalXAxis;

            roll = Vector3.Angle(unrotatedLocalXAxis, transform.right);

            float dotProduct = Vector3.Dot(transform.right, unrotatedLocalYAxis);

            if (dotProduct < 0)
                roll = 360 - roll;

            if (roll > 180)
                roll = 360 - roll;
            else if (roll < 180)
                roll = -roll;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// This function calculates the yaw angle in degrees based on the <see
        /// cref="AI.Move.AIMContext.LocalDecidedDirection"/>.
        /// </summary>
        protected override void CalculateYaw()
        {
            yaw = Mathf.Atan2(Context.LocalDecidedDirection.x, Context.LocalDecidedDirection.z) * Mathf.Rad2Deg;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculates the force for each axis (X, Y, Z) that has to be applied to the agent based on the <see
        /// cref="AIMContext.DecidedValues"/>.
        /// </summary>
        protected override void CalculateForce()
        {
            force = new Vector3(0.0f, 0.0f, objectiveAsSpeed >= 0 ? Context.DecidedValues[objectiveAsSpeed] : 1);
        }

        #endregion // Methods
    } // class SpaceShipController
} // namespace Polarith.AI.Package
