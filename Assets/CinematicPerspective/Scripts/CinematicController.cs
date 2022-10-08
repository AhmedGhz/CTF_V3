using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CinematicPerspective
{
    public class CinematicController : MonoBehaviour
    {

        private CinematicTakes m_cinematicTakes
        {
            get
            {
                return GetComponentInChildren<CinematicTakes>();
            }
        }

        /// <summary>
        /// Get the active status of the component
        /// </summary>
        public bool active
        {
            get
            {
                return m_cinematicTakes.active;
            }
        }

        /// <summary>
        /// Array of all rigs in the CinematicTakes Component
        /// </summary>
        public CinematicTakesRig[] rigs
        {
            get
            {
               return m_cinematicTakes.rigs.Select(r => r.GetComponent<CinematicTakesRig>()).ToArray();
            }
        }

        /// <summary>
        /// Forces the selected rig to be the active rig
        /// </summary>
        /// <param name="rig">Rig to set active | If null, the component will select the rig normally</param>
        public void SetActiveRig(CinematicTakesRig rig)
        {
            m_cinematicTakes.ForceActiveRig(rig);
        }

        /// <summary>
        /// Activate the rendering and functionality of the CinematicTakes Component
        /// </summary>
        /// <param name="value">true to activate by default</param>
        /// <returns>false if can't be activated, for example, for not having a target or camera defined</returns>
        public bool setActive(bool value = true)
        {
            return m_cinematicTakes.active = value;
        }

        /// <summary>
        /// The distance, in game units, between the selected rig and the target
        /// </summary>
        /// <param name="rig">A CinematicTakes Rig</param>
        /// <returns>a floating point value, -1 if no target or rig has ben selected</returns>
        public float distanceToTarget()
        {
            return m_cinematicTakes.distanceToTarget;
        }

        /// <summary>
        /// The distance, in game units, between the rig and the target
        /// </summary>
        /// <param name="rig">A CinematicTakes Rig</param>
        /// <returns>a floating point value, -1 if no target has been defined</returns>
        public float distanceToTarget(CinematicTakesRig rig)
        {
            if (m_cinematicTakes.target == null)
                Debug.LogWarning("Trying to get distance to target when target is not selected");
            return Vector3.Distance(rig.transform.position, m_cinematicTakes.target.position);
        }

        /// <summary>
        /// Set the target
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Transform setTarget(Transform transform)
        {
            return  m_cinematicTakes.target = transform;            
        }

        /// <summary>
        /// Gets the selected rig (the active rig used to choose, place and mode the camera). 
        /// </summary>
        /// <returns>a CinematicTakes Rig</returns>
        public CinematicTakesRig getSelectedRig()
        {
            var ctr = m_cinematicTakes;
            if (ctr.selectedRig != null)
                return ctr.GetComponent<CinematicTakesRig>();

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CinematicTakesRig[] getAllRigs()
        {
            return m_cinematicTakes.GetComponentsInChildren<CinematicTakesRig>();
        }

    }
}
