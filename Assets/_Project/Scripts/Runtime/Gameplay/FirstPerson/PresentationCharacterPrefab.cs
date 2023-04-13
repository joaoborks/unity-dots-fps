using Unity.Entities;
using UnityEngine;

namespace MyFps.Gameplay.FirstPerson
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
}