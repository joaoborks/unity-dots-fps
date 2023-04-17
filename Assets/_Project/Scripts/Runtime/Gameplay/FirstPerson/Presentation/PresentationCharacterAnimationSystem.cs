using MyFps.Gameplay.FirstPerson;
using Unity.Entities;
using UnityEngine;

namespace MyFps.Gameplay.Presentation
{
    public partial class PresentationCharacterAnimationSystem : SystemBase
    {
        PresentationCharacterSystem _presentationSystem;
        bool _switch;
        int _hashActionPunchR;
        int _hashActionPunchL;

        protected override void OnCreate()
        {
            _presentationSystem = World.GetExistingSystemManaged<PresentationCharacterSystem>();

            _hashActionPunchR = Animator.StringToHash("action_punch_r");
            _hashActionPunchL = Animator.StringToHash("action_punch_l");

            RequireForUpdate(SystemAPI.QueryBuilder().WithAll<CharacterControl>().WithAll<PresentationCharacterState>().Build());
        }

        protected override void OnUpdate()
        {
            foreach (var (control, entity) in SystemAPI.Query<RefRO<CharacterControl>>().WithAll<PresentationCharacterState>().WithEntityAccess())
            {
                if (_presentationSystem.TryGetBehaviorForEntity(EntityManager, entity, out var behavior))
                {
                    if (control.ValueRO.Fire)
                    {
                        behavior.Animator.SetTrigger(_switch ? _hashActionPunchL : _hashActionPunchR);
                        _switch = !_switch;
                    }
                }
            }
        }
    }
}