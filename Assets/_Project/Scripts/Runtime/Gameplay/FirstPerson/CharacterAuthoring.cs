using Unity.CharacterController;
using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

namespace MyFps.Gameplay.FirstPerson
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PhysicsShapeAuthoring))]
    public class CharacterAuthoring : MonoBehaviour
    {
        public GameObject ViewEntity;
        public AuthoringKinematicCharacterProperties CharacterProperties = AuthoringKinematicCharacterProperties.GetDefault();
        public CharacterComponent Character = CharacterComponent.GetDefault();

        public class Baker : Baker<CharacterAuthoring>
        {
            public override void Bake(CharacterAuthoring authoring)
            {
                KinematicCharacterUtilities.BakeCharacter(this, authoring, authoring.CharacterProperties);

                authoring.Character.ViewEntity = GetEntity(authoring.ViewEntity, TransformUsageFlags.Dynamic);

                Entity entity = GetEntity(TransformUsageFlags.Dynamic | TransformUsageFlags.WorldSpace);

                AddComponent(entity, authoring.Character);
                AddComponent(entity, new CharacterControl());
            }
        }
    }
}