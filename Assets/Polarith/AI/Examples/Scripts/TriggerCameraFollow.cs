using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// A script that changes parameters of <see cref="CameraFollow"/> for a focused view. 'OnTriggerEnter', the <see
    /// cref="CameraFollow.MoveSpeed"/>, <see cref="CameraFollow.CamAngle"/> and <see cref="CameraFollow.Target"/> are
    /// changed.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Trigger Camera Follow")]
    public sealed class TriggerCameraFollow : MonoBehaviour
    {
        #region Fields =================================================================================================

        [Tooltip("'CameraFollow' script that should be changed when entering the trigger collider.")]
        [SerializeField]
        private CameraFollow cameraFollow;

        [Tooltip("Movement speed of the 'CameraFollow' script when entering the trigger collider.")]
        [SerializeField]
        private float moveSpeed = 1f;

        [Tooltip("Camera Angle of the 'CameraFollow' script when entering the trigger collider.")]
        [SerializeField]
        private float cameraAngle = 25f;

        [Tooltip("Target object of the 'CameraFollow' script hat should be focused when entering the trigger" +
            "collider.")]
        [SerializeField]
        private GameObject target;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// <see cref="CameraFollow"/> script that should be changed when entering the trigger collider.
        /// </summary>
        public CameraFollow CameraFollow
        {
            get { return cameraFollow; }
            set { cameraFollow = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Movement speed of the <see cref="CameraFollow"/> script when entering the trigger collider.
        /// </summary>
        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Camera Angle of the <see cref="CameraFollow"/> script when entering the trigger collider.
        /// </summary>
        public float CameraAngle
        {
            get { return cameraAngle; }
            set { cameraAngle = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Target object of the <see cref="CameraFollow"/> script hat should be focused when entering the trigger
        /// collider.
        /// </summary>
        public GameObject Target
        {
            get { return target; }
            set { target = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void OnTriggerEnter(Collider collider)
        {
            if (CameraFollow != null)
            {
                CameraFollow.MoveSpeed = moveSpeed;
                CameraFollow.CameraAngle = cameraAngle;
                CameraFollow.Target = target.transform;
            }
        }

        #endregion // Methods
    } // class TriggerCameraFollow
} // namespace Polarith.AI.Package
