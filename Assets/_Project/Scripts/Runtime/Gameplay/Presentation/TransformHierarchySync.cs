using Unity.Entities;

namespace MyFps.Gameplay.Presentation
{
    public struct TransformHierarchySync : IComponentData, IEnableableComponent { }

    [InternalBufferCapacity(16)]
    public struct TransformHierarchyBuffer : IBufferElementData, IEnableableComponent
    {
        public Entity Entity;
    }
}