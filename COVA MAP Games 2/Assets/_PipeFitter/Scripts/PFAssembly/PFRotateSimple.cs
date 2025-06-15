using UnityEngine;

namespace PipeFitter.Assembly
{
    public class PFRotateSimple : MonoBehaviour
    {
        protected bool RotateActivated;
        public Vector3 LocalRotationSpeed;
        public bool UnscaledTime;
        public Transform ObjectToRotate;
        public void ActivateRotation()
        {
            RotateActivated = true;
        }
        public void DeactivateRotation()
        {
            RotateActivated = false;
        }
        void Update()
        {
            if (RotateActivated)
            {
                LocalRotation();
            }
        }

        // ...

        void LocalRotation()
        {
            // Calling Rotate can be expensive.
            // Doing the if-checks is worth it.

            float deltaTime = !UnscaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
            ObjectToRotate.Rotate(LocalRotationSpeed * deltaTime, Space.Self);
        }
    }
}
