using MyFps.Gameplay.FirstPerson;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Jobs;

namespace MyFps
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class TransformHierarchyLinkSystem : SystemBase
    {
        public TransformAccessArray Transforms;
        public new NativeList<Entity> Entities;

        const int _capacity = 16;

        PresentationCharacterSystem _presentationCharacterSystem;

        protected override void OnCreate()
        {
            var query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PresentationCharacterPrefabReference>()
                .WithAll<TransformHierarchyBuffer>()
                .WithNone<TransformHierarchySync>()
                .Build(this);

            RequireForUpdate(query);

            Transforms = new TransformAccessArray(_capacity, 16);
            Entities = new NativeList<Entity>(_capacity, Allocator.Persistent);

            _presentationCharacterSystem = World.GetExistingSystemManaged<PresentationCharacterSystem>();
        }

        protected override void OnDestroy()
        {
            Transforms.Dispose();
            Entities.Dispose();
        }

        protected override void OnUpdate()
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (_, buffer, entity) in SystemAPI.Query<RefRO<PresentationCharacterState>, DynamicBuffer<TransformHierarchyBuffer>>().WithEntityAccess())
            {
                if (!SystemAPI.IsComponentEnabled<TransformHierarchySync>(entity))
                {
                    var gameObject = _presentationCharacterSystem.GetGameObjectForEntity(EntityManager, entity);
                    if (gameObject && gameObject.TryGetComponent<TransformHierarchyTargetBehavior>(out var target))
                    {
                        var count = buffer.Length;
                        for (int i = 0; i < count; i++)
                        {
                            Transforms.Add(target.TransformHierarchy[i]);
                            Entities.Add(buffer[i].Entity);
                        }
                        entityCommandBuffer.SetComponentEnabled<TransformHierarchySync>(entity, true);
                    }
                }
            }

            entityCommandBuffer.Playback(EntityManager);
            entityCommandBuffer.Dispose();
        }
    }
}