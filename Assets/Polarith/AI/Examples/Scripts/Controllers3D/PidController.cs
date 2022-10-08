using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// A proportional, integral, and derivative ( <a href="https://en.wikipedia.org/wiki/PID_controller">PID</a>)
    /// controller that takes an error as input and calculates appropriate control values to compensate this error. Each
    /// part of the controller has its own gain. A gain unequal to 0 will activate this part of the controller.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [System.Serializable]
    public sealed class PidController
    {
        #region Fields =================================================================================================

        [SerializeField]
        private float gainP = 2;

        [SerializeField]
        private float gainI = 0.6f;

        [SerializeField]
        private float gainD = 0.2f;

        private float errorSum = 0;
        private float errorOld = 0;
        private float errorLimit = 180.0f;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Influences the proportional part of the PID controller.
        /// </summary>
        public float GainP
        {
            get { return gainP; }
            set { gainP = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Influences the integral part of the PID controller.
        /// </summary>
        public float GainI
        {
            get { return gainI; }
            set { gainI = value; }
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Influences the derivative part of the PID controller.
        /// </summary>
        public float GainD
        {
            get { return gainD; }
            set { gainD = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        /// <summary>
        /// Calculates and returns the control output.
        /// </summary>
        /// <param name="error">Difference between current value and desired value.</param>
        /// <returns>Control output.</returns>
        public float GetOutput(float error)
        {
            errorSum += error;
            if (errorSum > errorLimit)
                errorSum = errorLimit;
            else if (errorSum < -errorLimit)
                errorSum = -errorLimit;

            float output = gainP * error
                + gainI * Time.fixedDeltaTime * errorSum
                + gainD * (error - errorOld) / Time.fixedDeltaTime;
            errorOld = error;

            return output;
        }

        #endregion // Methods
    } // class PidController
} // namespace Polarith.AI.Package
