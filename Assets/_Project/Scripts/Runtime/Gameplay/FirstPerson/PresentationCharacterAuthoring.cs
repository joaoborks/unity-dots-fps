using Unity.Entities;
using UnityEngine;

namespace MyFps.Gameplay.FirstPerson
{
    public class PresentationCharacterAuthoring : MonoBehaviour
    {
        [SerializeField]
        GameObject _presentationPrefab;

        public class PresentationCharacterAuthoringBaker : Baker<PresentationCharacterAuthoring>
        {
            public override void Bake(PresentationCharacterAuthoring authoring)
            {
                var managedEntity = CreateAdditionalEntity(TransformUsageFlags.None, entityName: nameof(PresentationCharacterPrefab));

                AddComponentObject(managedEntity, new PresentationCharacterPrefab
                {
                    Prefab = authoring._presentationPrefab
                });

                var entity = GetEntity(TransformUsageFlags.Dynamic | TransformUsageFlags.WorldSpace);
                AddComponent(entity, new PresentationCharacterPrefabReference
                {
                    Reference = managedEntity
                });
            }
        }
    }
}