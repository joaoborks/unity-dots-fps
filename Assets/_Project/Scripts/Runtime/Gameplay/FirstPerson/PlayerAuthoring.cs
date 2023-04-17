using UnityEngine;
using Unity.Entities;

namespace MyFps.Gameplay.FirstPerson
{
    [DisallowMultipleComponent]
    public class PlayerAuthoring : MonoBehaviour
    {
        public GameObject ControlledCharacter;
        public float MouseSensitivity = 1f;

        public class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Player
                {
                    ControlledCharacter = GetEntity(authoring.ControlledCharacter, TransformUsageFlags.Dynamic),
                    MouseSensitivity = authoring.MouseSensitivity,
                });
                AddComponent(entity, new PlayerInputs());
            }
        }
    }
}