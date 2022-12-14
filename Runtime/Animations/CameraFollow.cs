using UnityEngine;

namespace GameBase.Animations
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;
        public float Smooth = 10f;
        public float Factor = 10f;

        private void Update()
        {
            transform.position = Vector3.Lerp(
                transform.position,
                Vector3.Lerp(
                    Vector3.zero,
                    Target.position,
                    Factor),
                Time.deltaTime * Smooth);
        }
    }
}