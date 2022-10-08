using Polarith.AI.Move;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// In the Start method the given <see cref="Path"/> is set to all instances of <see cref="AIMFollowWaypoints"/>
    /// which are attached to this object and its children.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Path Connector Setter")]
    public sealed class PathConnectorSetter : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// Is assigned to all <see cref="AIMFollowWaypoints"/> instances of this object and its children on Start.
        /// </summary>
        [Tooltip("Is assigned to all 'AIMFollowWaypoints' instances of this object and its children on " +
            "Start.")]
        public AIMPathConnector Path;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            AIMFollowWaypoints[] behaviours = gameObject.GetComponentsInChildren<AIMFollowWaypoints>();
            foreach (AIMFollowWaypoints b in behaviours)
                b.PathConnector = Path;
        }

        #endregion // Methods
    } // class PathConnectorSetter
} // namespace Polarith.AI.Package
