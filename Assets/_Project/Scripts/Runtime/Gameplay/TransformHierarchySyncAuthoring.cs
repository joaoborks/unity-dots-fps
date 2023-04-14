using Unity.Entities;
using UnityEngine;

namespace MyFps
{
    public class TransformHierarchySyncAuthoring : MonoBehaviour
    {
        [SerializeField]
        GameObject[] _hierarchy;

        public class TransformHierarchySyncAuthoringBaker : Baker<TransformHierarchySyncAuthoring>
        {
            public override void Bake(TransformHierarchySyncAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic | TransformUsageFlags.WorldSpace);

                if (authoring._hierarchy == null)
                    return;

                var count = authoring._hierarchy.Length;

                AddBuffer<TransformHierarchyBuffer>(entity);
                for (int i = 0; i < count; i++)
                {
                    AppendToBuffer(entity, new TransformHierarchyBuffer
                    {
                        Entity = GetEntity(authoring._hierarchy[i], TransformUsageFlags.Dynamic)
                    });
                }

                AddComponent<TransformHierarchySync>(entity);
                SetComponentEnabled<TransformHierarchySync>(entity, false);
            }
        }
    }
}