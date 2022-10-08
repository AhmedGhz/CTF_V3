using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This is a helper script to utilize the ability of the <see cref="SpaceshipController"/> to change the horizontal
    /// alignment by the <see cref="RpyController.UpVector"/>. This script only works together with a trigger collider.
    /// As long as an object with a SpaceshipController component is inside this trigger, the UpVector of the
    /// SpaceshipController is set to the position difference. This way, the spaceship tries to stay horizontal towards
    /// the trigger object. When the spaceship exits the trigger, the UpVector is reset to <see cref="Vector3.up"/>.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Spaceship Up")]
    public sealed class SpaceshipUp : MonoBehaviour
    {
        #region Methods ================================================================================================

        private void OnTriggerStay(Collider other)
        {
            SpaceshipController controller = other.gameObject.GetComponentInParent<SpaceshipController>();
            if (controller != null)
            {
                Vector3 upVector = other.transform.position - transform.position;
                controller.UpVector = upVector;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerExit(Collider other)
        {
            SpaceshipController controller = other.gameObject.GetComponentInParent<SpaceshipController>();
            if (controller != null)
                controller.UpVector = Vector3.up;
        }

        #endregion // Methods
    } // class SpaceshipUp
} // namespace Polarith.AI.Package
