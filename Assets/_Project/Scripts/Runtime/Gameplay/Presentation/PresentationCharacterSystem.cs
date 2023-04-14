using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace MyFps.Gameplay.Presentation
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class PresentationCharacterSystem : SystemBase
    {
        public new NativeList<Entity> Entities;

        ComponentLookup<PresentationCharacterState> _stateLookup;
        List<GameObject> _gameObjects;

        protected override void OnCreate()
        {
            _gameObjects = new List<GameObject>();
            Entities = new NativeList<Entity>(16, Allocator.Persistent);
            _stateLookup = GetComponentLookup<PresentationCharacterState>();
        }

        protected override void OnDestroy()
        {
            foreach (var gameObject in _gameObjects)
                Object.Destroy(gameObject);

            Entities.Dispose();
            _gameObjects.Clear();
        }

        protected override void OnUpdate()
        {
            var buffer = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (prefabReference, entity) in SystemAPI.Query<RefRO<PresentationCharacterPrefabReference>>().WithNone<PresentationCharacterState>().WithEntityAccess())
            {
                var prefabComponent = EntityManager.GetComponentData<PresentationCharacterPrefab>(prefabReference.ValueRO.Reference);
                int index = -1;
                if (prefabComponent.Prefab != null)
                {
                    var gameObject = Object.Instantiate(prefabComponent.Prefab);
                    var owner = gameObject.AddComponent<PresentationCharacterEntityOwner>();
                    owner.OwnerWorld = World;
                    owner.OwnerEntity = entity;

                    index = _gameObjects.Count;
                    _gameObjects.Add(gameObject);
                    Entities.Add(entity);
                }
                buffer.AddComponent(entity, new PresentationCharacterState { GameObjectIndex = index });
            }

            _stateLookup.Update(this);
            foreach (var (presentationState, entity) in SystemAPI.Query<RefRO<PresentationCharacterState>>().WithNone<PresentationCharacterPrefabReference>().WithEntityAccess())
            {
                var index = presentationState.ValueRO.GameObjectIndex;
                if (index >= 0)
                {
                    Entities.RemoveAtSwapBack(index);
                    var last = _gameObjects.Count - 1;
                    Object.Destroy(_gameObjects[index]);
                    _gameObjects[index] = _gameObjects[last];
                    _stateLookup[Entities[index]] = new PresentationCharacterState { GameObjectIndex = index };
                    _gameObjects.RemoveAt(last);
                }
                buffer.RemoveComponent<PresentationCharacterState>(entity);
            }
            buffer.Playback(EntityManager);
            buffer.Dispose();
        }

        public GameObject GetGameObjectForEntity(EntityManager entityManager, Entity entity)
        {
            if (!entityManager.HasComponent<PresentationCharacterState>(entity))
                return null;
            var index = entityManager.GetComponentData<PresentationCharacterState>(entity).GameObjectIndex;
            if (index < 0)
                return null;
            return _gameObjects[index];
        }
    }
}