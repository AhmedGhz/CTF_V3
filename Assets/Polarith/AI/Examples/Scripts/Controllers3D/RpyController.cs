using Polarith.AI.Move;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// An abstract class for a set of example controllers that can use the <see cref="AIMSpatialSensor"/>. The goal of
    /// all derivative classes is to transform the decision made by <see cref="AIMContext"/> into meaningful roll, pitch
    /// and yaw values. Therefore, the general workflow is the following: At first, you may pre-process some data. Then,
    /// the derived controller calculates rotation (roll, pitch, yaw) and translation (Vector3) forces. Finally, an
    /// optional PID controller can optimize the rotation values. Note that the output of the PID controller can be
    /// limited.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    /// <remarks>
    /// The derived controllers will only provide the steering commands. The actual physics will be handled by a
    /// separate physics class.
    /// </remarks>
    public abstract class RpyController : MonoBehaviour
    {
        #region Fields =================================================================================================

        [SerializeField]
        [Tooltip("Rigidbody that is used by the physics component to move the agent.")]
        protected Rigidbody body;

        [SerializeField]
        [Tooltip("'AIM Context' component for providing decision information that will be transformed into " +
            "roll, pitch and yaw.")]
        protected AIMContext context;

        [SerializeField]
        [Tooltip("If true, roll, pitch and yaw will be determined by a PID controller. Additionally, the PID " +
            "parameters will be enabled.")]
        protected bool usePidController;

        protected Vector3 force;
        protected float pitch = 0.0f;
        protected float pitchLimit = 180.0f;
        protected float roll = 0.0f;
        protected float rollLimit = 180.0f;
        protected float yaw = 0.0f;
        protected float yawLimit = 180.0f;

        [SerializeField]
        protected PidController pitchController;

        [SerializeField]
        protected PidController rollController;

        [SerializeField]
        protected PidController yawController;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Rigidbody that is used by the physics component to move the agent.
        /// </summary>
        public Rigidbody Body
        {
            get { return body; }
            set { body = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// <see cref="AIMContext"/> component for providing decision information that will be transformed into roll,
        /// pitch and yaw.
        /// </summary>
        public AIMContext Context
        {
            get { return context; }
            set { context = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// If true, roll, pitch and yaw will be determined by a PID controller. Additionally, the PID parameters will
        /// be enabled.
        /// </summary>
        public bool UsePidController
        {
            get { return usePidController; }
            set { usePidController = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Returns the 3-dimensional force value that the controller calculated.
        /// </summary>
        public Vector3 Force
        {
            get { return force; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Returns the pitch value that the controller calculated.
        /// </summary>
        public float Pitch
        {
            get { return pitch; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Returns the roll value that the controller calculated.
        /// </summary>
        public float Roll
        {
            get { return roll; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Returns the yaw value that the controller calculated.
        /// </summary>
        public float Yaw
        {
            get { return yaw; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void FixedUpdate()
        {
            PreprocessData();

            CalculateRoll();
            CalculatePitch();
            CalculateYaw();
            CalculateForce();

            if (UsePidController)
                ApplyPIDControl();

            LimitControls();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void ApplyPIDControl()
        {
            roll = rollController.GetOutput(roll);
            pitch = pitchController.GetOutput(pitch);
            yaw = yawController.GetOutput(yaw);
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculates the Force for each axis (x, y, z) that has to be applied to the agent. For example, the force can
        /// be based on the <see cref="AIMContext.DecidedValues"/>.
        /// </summary>
        protected abstract void CalculateForce();

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculate the pitch value from <see cref="AIMContext.LocalDecidedDirection"/> and assign it to <see
        /// cref="pitch"/>.
        /// </summary>
        protected abstract void CalculatePitch();

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculate the roll value from <see cref="AIMContext.LocalDecidedDirection"/> and assign it to <see
        /// cref="roll"/>.
        /// </summary>
        protected abstract void CalculateRoll();

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculate the yaw value from <see cref="AIMContext.LocalDecidedDirection"/> and assign it to <see
        /// cref="yaw"/>.
        /// </summary>
        protected abstract void CalculateYaw();

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Limit angular steering values after applying them to the PID-controller.
        /// </summary>
        protected virtual void LimitControls()
        { }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Pre-process some data, e.g. averaging decisions before the main processing.
        /// </summary>
        protected virtual void PreprocessData()
        { }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Compute signed angle between two vectors.
        /// </summary>
        protected float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
        {
            float angle = Vector3.Angle(from, to);
            Vector3 perpVect = Vector3.Cross(axis, from);
            angle *= Mathf.Sign(Vector3.Dot(perpVect, to));

            return angle;
        }

        #endregion // Methods
    } // class RpyController
} // namespace Polarith.AI.Package
