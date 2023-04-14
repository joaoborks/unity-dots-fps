using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.Jobs;

namespace MyFps
{
    [UpdateInGroup(typeof(TransformSystemGroup))]
    public partial class TransformHierarchySyncSystem : SystemBase
    {
        ComponentLookup<LocalTransform> _transformLookup;
        TransformHierarchyLinkSystem _linkSystem;

        protected override void OnCreate()
        {
            _linkSystem = World.GetExistingSystemManaged<TransformHierarchyLinkSystem>();

            var query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<TransformHierarchyBuffer>()
                .WithAll<TransformHierarchySync>()
                .Build(this);

            RequireForUpdate(query);

            _transformLookup = GetComponentLookup<LocalTransform>(true);
        }

        protected override void OnUpdate()
        {
            _transformLookup.Update(this);

            var transformJob = new TransformUpdateJob
            {
                Entities = _linkSystem.Entities,
                TransformFromEntity = _transformLookup
            };
            Dependency = transformJob.Schedule(_linkSystem.Transforms, Dependency);
        }

        [BurstCompile]
        struct TransformUpdateJob : IJobParallelForTransform
        {
            [ReadOnly]
            public NativeList<Entity> Entities;
            [ReadOnly]
            public ComponentLookup<LocalTransform> TransformFromEntity;

            public void Execute(int index, TransformAccess transform)
            {
                var entityTransform = TransformFromEntity[Entities[index]];
                transform.SetLocalPositionAndRotation(entityTransform.Position, entityTransform.Rotation);
            }
        }
    }
}