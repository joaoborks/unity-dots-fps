using Unity.Entities;

namespace MyFps
{
    [InternalBufferCapacity(16)]
    public struct TransformHierarchyBuffer : IBufferElementData, IEnableableComponent
    {
        public Entity Entity;
    }
}