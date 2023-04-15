using Cinemachine;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace MyFps.Gameplay.Cinemachine
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup), OrderFirst = true)]
    public partial class CinemachineManualUpdateSystem : SystemBase
    {
        protected override void OnCreate()
        {
            var managedEntity = EntityManager.CreateEntity();
            EntityManager.AddComponentObject(managedEntity, new CinemachineBrainObjectReference());

            var query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<CinemachineBrainObjectReference>()
                .Build(this);

            RequireForUpdate(query);
        }

        protected override void OnUpdate()
        {
            foreach (var reference in SystemAPI.Query<CinemachineBrainObjectReference>())
            {
                if (!reference.Brain)
                    reference.Brain = Object.FindAnyObjectByType<CinemachineBrain>();
                else
                {
                    reference.Brain.ManualUpdate();
                    CinemachineCore.CurrentTimeOverride = (float)World.Time.ElapsedTime;
                    CinemachineCore.UniformDeltaTimeOverride = World.Time.DeltaTime;
                }
            }
        }
    }
}