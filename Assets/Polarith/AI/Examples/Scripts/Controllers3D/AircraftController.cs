using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This component is a physics-based controller that is designed to steer a fixed-wing aircraft based on the AI´s
    /// decision. The purpose of this controller is to mimic a behaviour that keeps the limitations of a fixed-wing
    /// aircraft in mind. Therefore, the controller will use yaw for small turns. For larger turns, the aircraft will
    /// roll on its side and change the direction by the pitch angle.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Character/Aircraft Controller")]
    [HelpURL("http://docs.polarith.com/ai/component-aimp-aircraftcontroller.html")]
    [DisallowMultipleComponent]
    public sealed class AircraftController : RpyController
    {
        #region Fields =================================================================================================

        private const float rollThreshold = 45.0f;
        private const float direction2Yaw = 25.0f; // empirical value for conversion
        private float currentRoll;

        [SerializeField]
        [Tooltip("If set equal to or greater than 0, the evaluated AI decision value scales the translation force")]
        [Move.TargetObjective(true)]
        private int objectiveAsSpeed = -1;

        #endregion // Fields

        #region Properties =============================================================================================

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
        /// The pitch command is calculated based on the difference between the <see cref="Transform.forward"/> and the
        /// <see cref="Context.DecidedDirection"/> in the local YZ-plane.
        /// </summary>
        protected override void CalculatePitch()
        {
            pitch = -SignedAngle(transform.forward,
                        Vector3.ProjectOnPlane(Context.DecidedDirection, transform.right),
                        transform.right);
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// As long as the roll value is below a certain threshold it will keep the airplane horizontal. If the value is
        /// above the threshold it is calculated based on the angular difference between <see cref="Transform.forward"/>
        /// and the <see cref="AIMContext.LocalDecidedDirection"/> in the XZ-plane.
        /// </summary>
        protected override void CalculateRoll()
        {
            currentRoll = SignedAngle(
                transform.right, Vector3.ProjectOnPlane(transform.right, Vector3.up), transform.forward);
            if (transform.up.y < 0)
                currentRoll += Mathf.Sign(currentRoll) * 90;

            Vector3 projection = Vector3.ProjectOnPlane(Context.LocalDecidedDirection, Vector3.up);
            roll = -Mathf.Atan2(projection.x, projection.z) * Mathf.Rad2Deg;
            if (Mathf.Abs(roll) < rollThreshold)
                roll = currentRoll;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculates the yaw value based on the X-value of the <see cref="AIMContext.LocalDecidedDirection"/>.
        /// </summary>
        protected override void CalculateYaw()
        {
            yaw = Context.LocalDecidedDirection.x * direction2Yaw;
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

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The yaw and pitch values are clamped to limit them. The roll command is limited in a way that the current
        /// roll value shall not exceed 90 degrees.
        /// </summary>
        protected override void LimitControls()
        {
            if (Mathf.Abs(currentRoll + roll) > rollLimit)
                roll = Mathf.Sign(roll) * rollLimit - currentRoll;

            pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);
            yaw = Mathf.Clamp(yaw, -yawLimit, yawLimit);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            rollLimit = 90.0f;
            pitchLimit = 90.0f;
            yawLimit = 40.0f;
        }

        #endregion // Methods
    } // class AircraftController
} // namespace Polarith.AI.Package
