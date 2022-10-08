using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This component is a simple class to simulate some physical aspects of a rotorcraft, like quadcopters. Its
    /// purpose is to grab control values from the <see cref="Package.CopterController"/> and to create an authentic
    /// movement. The most important values are related to the rotation: <see cref="RpyController.Roll"/>, <see
    /// cref="RpyController.Pitch"/> and <see cref="RpyController.Yaw"/>. Additionally to the rotation, thrust
    /// determines in which direction the copter will move. While the rotation is responsible for the movement in the
    /// XZ-plane, thrust affects the change in Y-direction.
    /// <para/>
    /// A positive <see cref="RpyController.Roll"/> value will turn the copter by a positive angle around the Z-axis
    /// and, thus, will create a movement in the negative X-direction and vice versa. A positive <see
    /// cref="RpyController.Pitch"/> angle will also rotate the copter by a positive value, this time around the X-axis.
    /// But this rotation will result in a movement along the positive Z-direction. A positive <see
    /// cref="RpyController.Yaw"/> value will turn the copter with a positive angle around the Y-axis.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Character/Copter Physics")]
    [HelpURL("http://docs.polarith.com/ai/component-aimp-coptercontroller.html")]
    [RequireComponent(typeof(CopterController))]
    public sealed class CopterPhysics : MonoBehaviour
    {
        #region Fields =================================================================================================

        [SerializeField]
        [Tooltip("The 'Copter Controller' component that will be used to steer the agent.")]
        private CopterController copterController;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Returns the total thrust that is applied to the copter. This includes nominal hover throttle that is used to
        /// keep the copter hovering and the thrust that is used to make the copter ascend or descend ( <see
        /// cref="RpyController.Force"/>.y).
        /// </summary>
        public float Thrust
        {
            get { return CopterController.Force.magnitude; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The <see cref="Package.CopterController"/> component that will be used to steer the agent.
        /// </summary>
        public CopterController CopterController
        {
            get { return copterController; }
            set { copterController = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void Start()
        {
            CopterController = GetComponent<CopterController>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            Vector3 torque = new Vector3(CopterController.Pitch,
                                         CopterController.Yaw,
                                         CopterController.Roll) * Time.deltaTime;
            CopterController.Body.AddRelativeTorque(torque);

            CopterController.Body.AddForce(CopterController.Force);
        }

        #endregion // Methods
    } // class CopterPhysics
} // namespace Polarith.AI.Package
