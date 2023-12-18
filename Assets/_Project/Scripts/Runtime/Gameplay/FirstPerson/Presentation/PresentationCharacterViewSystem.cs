using MyFps.Gameplay.FirstPerson;
using Unity.Entities;
using Unity.Transforms;

namespace MyFps.Gameplay.Presentation
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class PresentationCharacterViewSystem : SystemBase
    {
        PresentationCharacterSystem _presentationSystem;
        ComponentLookup<CharacterComponent> _characterLookup;

        protected override void OnCreate()
        {
            _presentationSystem = World.GetExistingSystemManaged<PresentationCharacterSystem>();
            _characterLookup = SystemAPI.GetComponentLookup<CharacterComponent>(true);

            RequireForUpdate(SystemAPI.QueryBuilder().WithAll<CharacterView>().WithAll<LocalToWorld>().Build());
        }

        protected override void OnUpdate()
        {
            _characterLookup.Update(this);
            foreach (var (view, transform) in SystemAPI.Query<RefRO<CharacterView>, RefRO<LocalToWorld>>())
            {
                var characterEntity = view.ValueRO.CharacterEntity;
                if (_presentationSystem.TryGetBehaviorForEntity(EntityManager, characterEntity, out var behavior))
                {
                    if (_characterLookup.TryGetComponent(characterEntity, out var characterComponent))
                    {
                        behavior.SetTransform(transform.ValueRO.Position, characterComponent.CameraHeight, transform.ValueRO.Rotation);
                    }
                }
            }
        }
    }
}