using Cinemachine;
using Unity.Entities;

namespace MyFps.Gameplay.Cinemachine
{
    public class CinemachineBrainObjectReference : IComponentData
    {
        public CinemachineBrain Brain;
    }
}