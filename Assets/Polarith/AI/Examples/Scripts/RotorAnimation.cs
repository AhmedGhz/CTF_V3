using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This script manages the animation of the copter´s rotor blades. The greater the 'Thrust' of the copter, the
    /// faster the animation of the rotor blades.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Rotor Animation")]
    public sealed class RotorAnimation : MonoBehaviour
    {
        #region Fields =================================================================================================

        private Animator animator;
        private CopterPhysics physics;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            animator = GetComponent<Animator>();
            physics = GetComponent<CopterPhysics>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            if (animator != null && physics != null)
            {
                animator.SetFloat("Thrust", physics.Thrust);
                if (physics.Thrust >= 0.0f)
                    animator.speed = physics.Thrust;
            }
        }

        #endregion // Methods
    } // class RotorAnimation
} // namespace Polarith.AI.Package
