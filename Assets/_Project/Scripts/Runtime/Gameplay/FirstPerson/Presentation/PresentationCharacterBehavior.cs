using Unity.Mathematics;
using UnityEngine;

namespace MyFps.Gameplay.Presentation
{
    public class PresentationCharacterBehavior : MonoBehaviour
    {
        public Transform View;
        public Animator Animator;

        public void SetTransform(float3 position, float cameraHeight, quaternion rotation)
        {
            float3 eulerAngles = math.Euler(rotation);
            float3 groundPosition = position - new float3(0, cameraHeight, 0);

            transform.SetPositionAndRotation(groundPosition, quaternion.Euler(0, eulerAngles.y, 0));
            View.SetLocalPositionAndRotation(new Vector3(0, cameraHeight), quaternion.Euler(eulerAngles.x, 0, 0));
        }
    }
}