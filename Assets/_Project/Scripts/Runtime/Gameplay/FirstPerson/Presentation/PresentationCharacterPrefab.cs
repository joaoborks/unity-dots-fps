using Unity.Entities;
using UnityEngine;

namespace MyFps.Gameplay.Presentation
{
    public class PresentationCharacterPrefab : IComponentData
    {
        public GameObject Prefab;
    }

    public struct PresentationCharacterPrefabReference : IComponentData
    {
        public Entity Reference;
    }

    public struct PresentationCharacterState : ICleanupComponentData
    {
        public int GameObjectIndex;
    }

    [InternalBufferCapacity(4)]
    public struct PresentationCharacterBuffer : IBufferElementData
    {
        public Entity Entity;
    }
}