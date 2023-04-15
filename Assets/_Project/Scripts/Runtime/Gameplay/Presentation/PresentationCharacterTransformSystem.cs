using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace MyFps.Gameplay.Presentation
{
    [UpdateInGroup(typeof(TransformSystemGroup))]
    public partial class PresentationCharacterTransformSystem : SystemBase
    {
        PresentationCharacterSystem _presentationSystem;

        protected override void OnCreate()
        {
            _presentationSystem = World.GetExistingSystemManaged<PresentationCharacterSystem>();

            var query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PresentationCharacterState>()
                .WithAll<LocalToWorld>()
                .Build(this);

            RequireForUpdate(query);
        }

        protected override void OnUpdate()
        {
            foreach (var (_, transform, entity) in SystemAPI.Query<RefRO<PresentationCharacterState>, RefRO<LocalToWorld>>().WithEntityAccess())
            {
                if (_presentationSystem.TryGetBehaviorForEntity(EntityManager, entity, out var behavior))
                    behavior.transform.SetPositionAndRotation(transform.ValueRO.Position, transform.ValueRO.Rotation);
            }
        }
    }

    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class PresentationCharacterViewSystem : SystemBase
    {
        PresentationCharacterSystem _presentationSystem;

        protected override void OnCreate()
        {
            _presentationSystem = World.GetExistingSystemManaged<PresentationCharacterSystem>();

            var query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<FirstPersonCharacterView>()
                .WithAll<LocalToWorld>()
                .Build(this);

            RequireForUpdate(query);
        }

        protected override void OnUpdate()
        {
            foreach (var (view, transform) in SystemAPI.Query<RefRO<FirstPersonCharacterView>, RefRO<LocalToWorld>>())
            {
                if (_presentationSystem.TryGetBehaviorForEntity(EntityManager, view.ValueRO.CharacterEntity, out var behavior))
                    behavior.View.SetPositionAndRotation(transform.ValueRO.Position, transform.ValueRO.Rotation);
            }
        }
    }
}