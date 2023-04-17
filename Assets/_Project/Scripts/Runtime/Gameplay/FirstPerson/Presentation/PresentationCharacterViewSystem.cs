using MyFps.Gameplay.FirstPerson;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace MyFps.Gameplay.Presentation
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class PresentationCharacterViewSystem : SystemBase
    {
        PresentationCharacterSystem _presentationSystem;

        protected override void OnCreate()
        {
            _presentationSystem = World.GetExistingSystemManaged<PresentationCharacterSystem>();

            var query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<CharacterView>()
                .WithAll<LocalToWorld>()
                .Build(this);

            RequireForUpdate(query);
        }

        protected override void OnUpdate()
        {
            foreach (var (view, transform) in SystemAPI.Query<RefRO<CharacterView>, RefRO<LocalToWorld>>())
            {
                if (_presentationSystem.TryGetBehaviorForEntity(EntityManager, view.ValueRO.CharacterEntity, out var behavior))
                    behavior.View.SetPositionAndRotation(transform.ValueRO.Position, transform.ValueRO.Rotation);
            }
        }
    }
}