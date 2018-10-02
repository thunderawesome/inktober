using UnityEngine;

namespace Battlerock
{    
    public class Spin : MonoBehaviour
    {
        #region Public Variables

        public enum Axis
        {
            X,
            Y,
            Z
        }

        /// <summary>
        /// Speed at which the object this script
        /// is attached to will spin.
        /// </summary>
        public float rotationSpeed = 10.0f;

        /// <summary>
        /// The axis to spin around.
        /// </summary>
        public Axis axis = Axis.Y;

        /// <summary>
        /// Check if spinning should be localRotation or not.
        /// </summary>
        public bool isLocalAxis = false;

        #endregion

        #region Unity Methods

        void LateUpdate()
        {
            transform.Rotate(GetAxis() * (rotationSpeed * Time.deltaTime));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determine which Axis is being used
        /// and if it it should be local.
        /// </summary>
        /// <returns></returns>
        private Vector3 GetAxis()
        {
            Vector3 vector = Vector3.zero;

            switch (axis)
            {
                case Axis.X:
                    if (isLocalAxis == true)
                    {
                        vector = transform.right;
                    }
                    else
                    {
                        vector = Vector3.right;
                    }

                    break;
                case Axis.Y:
                    if (isLocalAxis == true)
                    {
                        vector = transform.up;
                    }
                    else
                    {
                        vector = Vector3.up;
                    }

                    break;
                case Axis.Z:
                    if (isLocalAxis == true)
                    {
                        vector = transform.forward;
                    }
                    else
                    {
                        vector = Vector3.forward;
                    }

                    break;
                default:
                    break;
            }

            return vector;
        }

        #endregion
    }
}