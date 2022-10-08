using Polarith.AI.Move;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Add objective values from the ground to prevent an agent from moving through the ground. The position on the
    /// ground is computed by the first hit of a raycast with a collider in the direction of the XZ-plane. This way, one
    /// can dynamically compute the ground's height. If there is no hit, the rigidbody's position is projected onto the
    /// XZ-plane. To generate objective values that are not directly under the rigidbody, one can increase the <see
    /// cref="PredictionTime"/> (in seconds). That way, an offset to the ground position is computed in the <see
    /// cref="Context.DecidedDirection"/> based on the rigidbody's velocity. Make sure to set the <see
    /// cref="RadiusSteeringBehaviour.InnerRadius"/> adequately based on the size of the agent's collider to prevent
    /// self-intersections during the raycast.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Seek Ground")]
    [HelpURL("http://docs.polarith.com/ai/component-aimp-seekground.html")]
    [RequireComponent(typeof(AIMRadiusSteeringBehaviour))]
    public sealed class SeekGround : MonoBehaviour
    {
        #region Fields =================================================================================================

        [Tooltip("Behaviour to compute the values. Make sure to drag the behaviour component directly into the " +
            "field. Recommended behaviours are 'AIMSeek' (for danger) and 'AIMFlee' (for interest).")]
        [SerializeField]
        private AIMRadiusSteeringBehaviour radiusSteering;

        [Tooltip("Reference rigidbody that is used to project the position on the ground.")]
        [SerializeField]
        private Rigidbody body;

        [Tooltip("Time in seconds to predict the projected position using the rigidbody's velocity.")]
        [SerializeField]
        private float predictionTime = 0f;

        [Tooltip("Enable gizmos to see the direction and projected hit point on the ground. Note that the ray starts "
            + "with an offset of 'RadiusSteeringBehaviour.InnerRadius'. The hit point on the ground is marked by a "
            + "sphere half the size of the 'RadiusSteeringBehaviour.InnerRadius'. Possible self-intersections result "
            + "in a sphere on the agent's collider.")]
        [SerializeField]
        private bool enableGizmos = false;

        //--------------------------------------------------------------------------------------------------------------

        private Vector3 point;
        private Vector3 origin;
        private Vector3 dir;
        private RaycastHit hit;
        private GameObject ground;
        private RadiusSteeringBehaviour rsb;
        private float diff = 0f;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Behaviour to compute the objective values. Make sure to drag the behaviour component directly into the
        /// editor field. Recommended behaviours are <see cref="AIMSeek"/> (for danger) and <see cref="AIMFlee"/> (for
        /// interest).
        /// </summary>
        public AIMRadiusSteeringBehaviour RadiusSteering
        {
            get { return radiusSteering; }
            set { radiusSteering = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Reference rigidbody that is used to project the position on the ground.
        /// </summary>
        public Rigidbody Body
        {
            get { return body; }
            set { body = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Time in seconds to predict the projected position using the rigidbody's velocity.
        /// </summary>
        public float PredictionTime
        {
            get { return predictionTime; }
            set { predictionTime = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Enable gizmos to see the direction and projected hit point on the ground. Note that the ray starts with an
        /// offset of <see cref="RadiusSteeringBehaviour.InnerRadius"/>. The hit point on the ground is marked by a
        /// sphere half the size of the <see cref="RadiusSteeringBehaviour.InnerRadius"/>. Possible self- intersections
        /// result in a sphere on the agent's collider.
        /// </summary>
        public bool EnableGizmos
        {
            get { return enableGizmos; }
            set { enableGizmos = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void Start()
        {
            try
            {
                body.IsSleeping();
            }
            catch (System.NullReferenceException ex)
            {
                Debug.Log("No rigidbody has been set.");
                Debug.LogException(ex, this);
            }

            point = body.transform.position;
            point.y = 0f;
            ground = new GameObject();
            ground.hideFlags = HideFlags.HideInHierarchy;
            ground.SetActive(false);
            radiusSteering.GameObjects.Add(ground);
            rsb = radiusSteering.RadiusSteeringBehaviour;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            dir = rsb.Context.DecidedDirection;
            dir.y = 0f;
            dir.Normalize();
            point = Body.transform.position + dir * body.velocity.magnitude * predictionTime;
            origin = body.transform.position + dir * rsb.InnerRadius;
            diff = (body.transform.position - point).magnitude;

            if (diff < rsb.InnerRadius)
                point.y -= diff;

            if (Physics.Raycast(point, Vector3.down, out hit, rsb.OuterRadius))
            {
                ground.SetActive(true);
                ground.transform.position = hit.point;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDrawGizmos()
        {
            if (enableGizmos)
            {
                if (ground != null &&
                    (ground.transform.position - body.transform.position).magnitude < rsb.OuterRadius)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(origin, ground.transform.position);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(ground.transform.position, rsb.InnerRadius * 0.5f);
                }
            }
        }

        #endregion // Methods
    } // class SeekGround
} // namespace Polarith.AI.Package
