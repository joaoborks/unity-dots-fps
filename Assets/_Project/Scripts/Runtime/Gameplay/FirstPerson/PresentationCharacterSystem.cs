using MyFps.Gameplay.FirstPerson;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

namespace MyFps
{
    public partial class PresentationCharacterSystem : SystemBase
    {
        public TransformAccessArray Transforms;
        public new NativeList<Entity> Entities;

        ComponentLookup<PresentationCharacterState> _stateLookup;
        List<GameObject> _gameObjects;

        protected override void OnCreate()
        {
            _gameObjects = new List<GameObject>();
            Transforms = new TransformAccessArray(8, 8);
            Entities = new NativeList<Entity>(8, Allocator.Persistent);
            _stateLookup = GetComponentLookup<PresentationCharacterState>();
        }

        protected override void OnDestroy()
        {
            foreach (var gameObject in _gameObjects)
                Object.Destroy(gameObject);

            Transforms.Dispose();
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
                    Transforms.Add(gameObject.transform);
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
                    Transforms.RemoveAtSwapBack(index);
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
    }

    [UpdateInGroup(typeof(TransformSystemGroup))]
    public partial class PresentationCharacterTransformSystem : SystemBase
    {
        PresentationCharacterSystem _presentationCharacterSystem;
        ComponentLookup<LocalTransform> _transformLookup;

        protected override void OnCreate()
        {
            _presentationCharacterSystem = World.GetExistingSystemManaged<PresentationCharacterSystem>();
            RequireForUpdate(GetEntityQuery(ComponentType.ReadOnly<PresentationCharacterPrefabReference>()));

            _transformLookup = GetComponentLookup<LocalTransform>(true);
        }

        protected override void OnUpdate()
        {
            _transformLookup.Update(this);

            var transformJob = new TransformUpdateJob
            {
                Entities = _presentationCharacterSystem.Entities,
                TransformFromEntity = _transformLookup,
            };
            Dependency = transformJob.Schedule(_presentationCharacterSystem.Transforms, Dependency);
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
                var ent = Entities[index];
                transform.localPosition = TransformFromEntity[ent].Position;
                transform.localRotation = TransformFromEntity[ent].Rotation;
            }
        }
    }
}