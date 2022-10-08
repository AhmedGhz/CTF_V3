using System.Collections.Generic;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// A script that starts or stops particle systems. 'OnTriggerEnter', the particle systems are activated.
    /// 'OnTriggerExit', the particle systems are deactivated.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Trigger Particles")]
    public sealed class TriggerParticles : MonoBehaviour
    {
        #region Fields =================================================================================================

        [Tooltip("List of particle systems that should emit on enter.")]
        [SerializeField]
        private List<ParticleSystem> particlesToActivate;

        [Tooltip("List of particle systems that should stop emit on exit.")]
        [SerializeField]
        private List<ParticleSystem> particlesToDeactivate;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// List of particle systems that should emit on enter.
        /// </summary>
        public List<ParticleSystem> ParticlesToActivate
        {
            get { return particlesToActivate; }
            set { particlesToActivate = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// List of particle systems that should stop emit on exit.
        /// </summary>
        public List<ParticleSystem> ParticlesToDeactivate
        {
            get { return particlesToDeactivate; }
            set { particlesToDeactivate = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void OnTriggerEnter(Collider collider)
        {
            // place and activate particle system
            Vector3 pos = collider.transform.position;
            foreach (ParticleSystem ps in particlesToActivate)
            {
                ps.transform.position = pos;
                ps.Play();
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerExit(Collider collider)
        {
            foreach (ParticleSystem ps in particlesToDeactivate)
                ps.Stop();
        }

        #endregion // Methods
    } // class TriggerParticles
} // namespace Polarith.AI.Package
