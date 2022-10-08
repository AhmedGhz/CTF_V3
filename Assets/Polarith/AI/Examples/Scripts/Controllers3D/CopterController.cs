using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This component is an example class to show you how one could implement a controller that handles rotorcraft
    /// vehicles in 3D. Its purpose is to provide meaningful rotation (roll, pitch, yaw) and thrust values derived from
    /// <see cref="AIMContext.DecidedDirection"/>. It also provides different flight modes that mainly differ in the
    /// orientation that the rotorcraft has during flight. While <see cref="FlightMode.Direct"/> keeps the heading of
    /// the copter at a stable value, <see cref="FlightMode.Forward"/> will make the copter always turn towards its
    /// current <see cref="AIMContext.DecidedDirection"/>. In <see cref="FlightMode.ObservationPoint"/> you can specify
    /// any point in your scene by the variable <see cref="ObservationPoint"/> so that the copter tries to maintain its
    /// heading towards this point.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Character/Copter Controller")]
    [HelpURL("http://docs.polarith.com/ai/component-aimp-coptercontroller.html")]
    [DisallowMultipleComponent]
    public sealed class CopterController : RpyController
    {
        #region Fields =================================================================================================

        [Tooltip("Target point to which the copter should look while flying. Only used in " +
            "'FlightMode.ObservationPoint'.")]
        [SerializeField]
        private Transform observationPoint;

        [Tooltip("'Direct' has no turn around the Y-Axis. 'Forward' turns the copter to its "
            + "current movement direction. 'Observation Point' turns the copter to the given observation point while "
            + "moving to the current target.")]
        [SerializeField]
        private FlightMode flightMode = FlightMode.Direct;

        [Tooltip("Typically roll ranges from -1 to 1. Use this factor to increase the desired maximum value.")]
        [SerializeField]
        private float rollFactor = 20;

        [Tooltip("Typically pitch ranges from -1 to 1. Use this factor to increase the desired maximum value.")]
        [SerializeField]
        private float pitchFactor = 20;

        [Tooltip("Typically yaw ranges from -1 to 1. Use this factor to increase the desired maximum value.")]
        [SerializeField]
        private float yawFactor = 5;

        [Tooltip("Typically thrust ranges from -1 to 1. Use this factor to increase the desired maximum value.")]
        [SerializeField]
        private float thrustFactor = 30;

        [SerializeField]
        private PidController thrustController;

        [Tooltip("Controls movement speed through pitch and roll. Thus, the agent moves faster but becomes physically "
            + "more instable.")]
        [SerializeField]
        private float speedFactor = 5;

        [SerializeField]
        [Tooltip("If set equal to or greater than 0, the evaluated AI decision value scales the translation force")]
        [Move.TargetObjective(true)]
        private int objectiveAsSpeed = -1;

        private static int decisionsSize = 100;
        private Vector3[] decisions = new Vector3[decisionsSize];
        private int decisionCounter = 0;
        private Vector3 avgDecision;
        private float startingYaw;
        private CopterPhysics physics;
        private float currentRoll;
        private float currentPitch;
        private float angleToUp;

        #endregion // Fields

        #region Enums ===================================================================================================

        /// <summary>
        /// The different supported flight modes.
        /// </summary>
        public enum FlightMode
        {
            /// <summary>
            /// The rotorcraft flies to the target using roll, pitch and thrust.
            /// </summary>
            Direct,

            /// <summary>
            /// The rotorcraft turns toward the target while flying to the target.
            /// </summary>
            Forward,

            /// <summary>
            /// The rotorcraft flies to the target while forward is always facing to the given observation point.
            /// </summary>
            ObservationPoint
        }

        #endregion // Enums

        #region Properties =============================================================================================

        /// <summary>
        /// Target point to which the copter should look while flying. Only used in <see
        /// cref="FlightMode.ObservationPoint"/>.
        /// </summary>
        public Transform ObservationPoint
        {
            get { return observationPoint; }
            set { observationPoint = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The <see cref="FlightMode"/> defines how the copter is flying towards the target. In <see
        /// cref="FlightMode.Direct"/> the copter does not turn around its Y-axis. In <see cref="FlightMode.Forward"/>
        /// the copter tries to turn towards its current movement direction. In FlightMode.Observationpoint the copter
        /// turns towards the given observation point while flying to its current target.
        /// </summary>
        public FlightMode FlightModus
        {
            get { return flightMode; }
            set { flightMode = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Typically <i>roll</i> ranges from -1 to 1. Use this factor to increase the desired maximum value. It also
        /// works as a limiter for the maximum roll angle of the copter.
        /// </summary>
        public float RollFactor
        {
            get { return rollFactor; }
            set { rollFactor = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Typically <i>pitch</i> ranges from -1 to 1. Use this factor to increase the desired maximum value. It also
        /// works as a limiter for the maximum pitch angle of the copter.
        /// </summary>
        public float PitchFactor
        {
            get { return pitchFactor; }
            set { pitchFactor = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Typically <i>yaw</i> ranges from -1 to 1. Use this factor to increase the desired maximum value. This also
        /// defines the maximum yaw turn rate of the copter.
        /// </summary>
        public float YawFactor
        {
            get { return yawFactor; }
            set { yawFactor = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Typically <i>thrust</i> ranges from -1 to 1. Use this factor to increase the desired maximum value.
        /// </summary>
        public float ThrustFactor
        {
            get { return thrustFactor; }
            set { thrustFactor = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// PID-controller for thrust. This will be enabled when <see cref="RpyController.UsePidController"/> is true.
        /// </summary>
        public PidController ThrustController
        {
            get { return thrustController; }
            set { thrustController = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Controls movement speed through pitch and roll. Thus, the agent moves faster, but becomes physically more
        /// instable.
        /// </summary>
        public float SpeedFactor
        {
            get { return speedFactor; }
            set { speedFactor = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

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
        /// Calculates the new pitch value based on the averaged <see cref="AIMContext.LocalDecidedDirection"/>.
        /// </summary>
        protected override void CalculatePitch()
        {
            Vector3 target = avgDecision;

            Vector3 projectedRight = Vector3.ProjectOnPlane(transform.right, Vector3.up);
            Vector3 projectedUp = Vector3.ProjectOnPlane(transform.up, projectedRight);
            currentPitch = SignedAngle(Vector3.up, projectedUp, projectedRight);

            float setpoint = target.normalized.z * SpeedFactor;
            setpoint *= objectiveAsSpeed >= 0 ? Context.DecidedValues[objectiveAsSpeed] : 1;
            pitch = setpoint - currentPitch;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculates the new roll value based on the averaged <see cref="AIMContext.LocalDecidedDirection"/>.
        /// </summary>
        protected override void CalculateRoll()
        {
            Vector3 target = avgDecision;

            Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            Vector3 projectedUp = Vector3.ProjectOnPlane(transform.up, projectedForward);
            currentRoll = SignedAngle(Vector3.up, projectedUp, projectedForward);

            float setpoint = -(target.normalized.x) * SpeedFactor;
            setpoint *= objectiveAsSpeed >= 0 ? Context.DecidedValues[objectiveAsSpeed] : 1;
            roll = setpoint - currentRoll;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculates the new thrust value based on the averaged <see cref="AIMContext.DecidedDirection"/>.
        /// </summary>
        protected override void CalculateForce()
        {
            Vector3 velocity = Body.velocity;
            Vector3 targetGlobal = Context.DecidedDirection;
            float deltaAngle = Vector3.Angle(velocity, targetGlobal) * Mathf.Deg2Rad;
            deltaAngle = Mathf.Clamp(deltaAngle, 0, 1);

            // calculate up/downward force
            float thrust = (ThrustFactor * Mathf.Cos(deltaAngle) * targetGlobal.y - velocity.y);
            angleToUp = Vector3.Angle(transform.up, Vector3.up) * Mathf.Deg2Rad;
            thrust = thrust / Mathf.Cos(angleToUp);

            if (UsePidController)
                thrust = ThrustController.GetOutput(thrust);

            thrust *= objectiveAsSpeed >= 0 ? Context.DecidedValues[objectiveAsSpeed] : 1;

            // compensate gravity
            float nominalThrottle = -Physics.gravity.y * Body.mass;
            thrust = Mathf.Clamp(thrust, -nominalThrottle / 2, nominalThrottle / 2);
            thrust = thrust + nominalThrottle / Mathf.Cos(angleToUp);

            // no downward acceleration
            if (thrust < 0)
                thrust = 0.0f;

            force = thrust * transform.up;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculates the new yaw value based on the <see cref="flightMode"/> and if needed the given <see
        /// cref="ObservationPoint"/>.
        /// </summary>
        protected override void CalculateYaw()
        {
            Vector3 viewpoint = transform.forward;
            switch (flightMode)
            {
                case FlightMode.Direct:
                    float currentYaw = transform.eulerAngles.y;
                    if (currentYaw > 180.0f)
                        currentYaw = currentYaw - 360.0f;
                    float yawDiff = startingYaw - currentYaw;
                    if (yawDiff > 180)
                        yawDiff -= 360;
                    yaw = yawDiff * YawFactor;
                    return;

                case FlightMode.Forward:
                    viewpoint = Context.DecidedDirection;
                    break;

                case FlightMode.ObservationPoint:
                    if (ObservationPoint == null)
                    {
                        Debug.LogWarning("Warning! No observation point given. This flight mode won´t work.");
                        yaw = 0;
                        return;
                    }
                    viewpoint = ObservationPoint.position - transform.position;
                    break;
            }

            // find yaw difference towards the viewpoint
            Vector3 projection = Vector3.ProjectOnPlane(viewpoint, Vector3.up);
            float turnDirection = Mathf.Sign(Vector3.Cross(transform.forward, projection).y);
            yaw = turnDirection * YawFactor *
                Vector3.Angle(projection, Vector3.ProjectOnPlane(transform.forward, Vector3.up));

            yaw *= objectiveAsSpeed >= 0 ? Context.DecidedValues[objectiveAsSpeed] : 1;
            yaw = Mathf.Abs(yaw) < 1.0f ? 0.0f : yaw;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Limits the control values in a way that roll and pitch should not exceed the limits that are specified by
        /// <see cref="RollFactor"/> and <see cref="PitchFactor"/>. Also clamps the yaw value to stay within
        /// - <see cref="YawFactor"/> and <see cref="YawFactor"/>.
        /// </summary>
        protected override void LimitControls()
        {
            rollLimit = RollFactor;
            pitchLimit = PitchFactor;
            yawLimit = YawFactor;

            if (Mathf.Abs(currentRoll + roll) > rollLimit)
                roll = Mathf.Sign(roll) * rollLimit - currentRoll;

            if (Mathf.Abs(currentPitch + pitch) > pitchLimit)
                pitch = Mathf.Sign(pitch) * pitchLimit - currentPitch;

            // hover for downward movement
            if (Context.DecidedDirection.normalized.y <= -0.95f)
            {
                pitch = -currentPitch;
                roll = -currentRoll;
            }

            yaw = Mathf.Clamp(yaw, -yawLimit, yawLimit);
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Pre-process data by averaging decisions before the main processing.
        /// </summary>
        protected override void PreprocessData()
        {
            avgDecision -= decisions[decisionCounter] / decisionsSize;
            avgDecision += Context.LocalDecidedDirection / decisionsSize;
            decisions[decisionCounter] = Context.LocalDecidedDirection;

            decisionCounter = ++decisionCounter % decisionsSize;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            Time.timeScale = 2.5f;
            startingYaw = transform.eulerAngles.y;
            avgDecision = Vector3.zero;

            if (Body == null)
            {
                Body = GetComponent<Rigidbody>();
                if (Body == null)
                {
                    Debug.LogError("No Rigidbody attached");
                }
            }
            Body.maxAngularVelocity = 3.0f;
        }

        #endregion // Methods
    } // class CopterController
} // namespace Polarith.AI.Package
