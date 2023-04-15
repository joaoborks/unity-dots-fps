using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace MyFps.Gameplay.Presentation
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class PresentationCharacterSystem : SystemBase
    {
        ComponentLookup<PresentationCharacterState> _stateLookup;

        List<PresentationCharacterBehavior> _presentationBehaviors;

        protected override void OnCreate()
        {
            EntityManager.CreateSingletonBuffer<PresentationCharacterBuffer>();

            _presentationBehaviors = new List<PresentationCharacterBehavior>();
            _stateLookup = GetComponentLookup<PresentationCharacterState>();
        }

        protected override void OnDestroy()
        {
            foreach (var behavior in _presentationBehaviors)
                Object.Destroy(behavior.gameObject);

            _presentationBehaviors.Clear();
        }

        protected override void OnUpdate()
        {
            var presentationBuffer = SystemAPI.GetSingletonBuffer<PresentationCharacterBuffer>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (prefabReference, entity) in SystemAPI.Query<RefRO<PresentationCharacterPrefabReference>>().WithNone<PresentationCharacterState>().WithEntityAccess())
            {
                var prefabComponent = EntityManager.GetComponentData<PresentationCharacterPrefab>(prefabReference.ValueRO.Reference);
                int index = -1;
                if (prefabComponent.Prefab != null)
                {
                    var behavior = Object.Instantiate(prefabComponent.Prefab).GetComponent<PresentationCharacterBehavior>();
                    var owner = behavior.gameObject.AddComponent<PresentationCharacterEntityOwner>();
                    owner.OwnerWorld = World;
                    owner.OwnerEntity = entity;

                    index = _presentationBehaviors.Count;
                    _presentationBehaviors.Add(behavior);
                    presentationBuffer.Add(new PresentationCharacterBuffer { Entity = entity });
                }
                ecb.AddComponent(entity, new PresentationCharacterState { GameObjectIndex = index });
            }

            _stateLookup.Update(this);
            foreach (var (presentationState, entity) in SystemAPI.Query<RefRO<PresentationCharacterState>>().WithNone<PresentationCharacterPrefabReference>().WithEntityAccess())
            {
                var index = presentationState.ValueRO.GameObjectIndex;
                if (index >= 0)
                {
                    presentationBuffer.RemoveAtSwapBack(index);
                    var last = _presentationBehaviors.Count - 1;
                    Object.Destroy(_presentationBehaviors[index]);
                    _presentationBehaviors[index] = _presentationBehaviors[last];
                    _stateLookup[presentationBuffer[index].Entity] = new PresentationCharacterState { GameObjectIndex = index };
                    _presentationBehaviors.RemoveAt(last);
                }
                ecb.RemoveComponent<PresentationCharacterState>(entity);
            }
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        public PresentationCharacterBehavior GetBehaviorForEntity(EntityManager entityManager, Entity entity)
        {
            if (!entityManager.HasComponent<PresentationCharacterState>(entity))
                return null;
            var index = entityManager.GetComponentData<PresentationCharacterState>(entity).GameObjectIndex;
            if (index < 0)
                return null;
            return _presentationBehaviors[index];
        }

        public bool TryGetBehaviorForEntity(EntityManager entityManager, Entity entity, out PresentationCharacterBehavior behavior)
        {
            behavior = GetBehaviorForEntity(entityManager, entity);
            return behavior;
        }
    }
}