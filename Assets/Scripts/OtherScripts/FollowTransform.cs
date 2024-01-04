using UnityEngine;

namespace OtherScripts
{
    public class FollowTransform : MonoBehaviour
    {
        public Transform TargetTransform { get; set; }

        private void LateUpdate()
        {
            if (TargetTransform == null)
            {
                return;
            }

            transform.SetPositionAndRotation(TargetTransform.position, TargetTransform.rotation);
        }
    }
}