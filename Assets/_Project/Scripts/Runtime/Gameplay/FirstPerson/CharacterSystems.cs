using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.CharacterController;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace MyFps.Gameplay.FirstPerson
{
    [UpdateInGroup(typeof(KinematicCharacterPhysicsUpdateGroup))]
    [BurstCompile]
    public partial struct CharacterPhysicsUpdateSystem : ISystem
    {
        private EntityQuery _characterQuery;
        private CharacterUpdateContext _context;
        private KinematicCharacterUpdateContext _baseContext;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _characterQuery = KinematicCharacterUtilities.GetBaseCharacterQueryBuilder()
                .WithAll<
                    CharacterComponent,
                    CharacterControl>()
                .Build(ref state);

            _context = new CharacterUpdateContext();
            _context.OnSystemCreate(ref state);
            _baseContext = new KinematicCharacterUpdateContext();
            _baseContext.OnSystemCreate(ref state);

            state.RequireForUpdate(_characterQuery);
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _context.OnSystemUpdate(ref state);
            _baseContext.OnSystemUpdate(ref state, SystemAPI.Time, SystemAPI.GetSingleton<PhysicsWorldSingleton>());

            CharacterPhysicsUpdateJob job = new CharacterPhysicsUpdateJob
            {
                Context = _context,
                BaseContext = _baseContext,
            };
            job.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(Simulate))]
        public partial struct CharacterPhysicsUpdateJob : IJobEntity, IJobEntityChunkBeginEnd
        {
            public CharacterUpdateContext Context;
            public KinematicCharacterUpdateContext BaseContext;

            void Execute(CharacterAspect characterAspect)
            {
                characterAspect.PhysicsUpdate(ref Context, ref BaseContext);
            }

            public bool OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                BaseContext.EnsureCreationOfTmpCollections();
                return true;
            }

            public void OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted) {}
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    [BurstCompile]
    public partial struct CharacterVariableUpdateSystem : ISystem
    {
        private EntityQuery _characterQuery;
        private CharacterUpdateContext _context;
        private KinematicCharacterUpdateContext _baseContext;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _characterQuery = KinematicCharacterUtilities.GetBaseCharacterQueryBuilder()
                .WithAll<
                    CharacterComponent,
                    CharacterControl>()
                .Build(ref state);

            _context = new CharacterUpdateContext();
            _context.OnSystemCreate(ref state);
            _baseContext = new KinematicCharacterUpdateContext();
            _baseContext.OnSystemCreate(ref state);

            state.RequireForUpdate(_characterQuery);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _context.OnSystemUpdate(ref state);
            _baseContext.OnSystemUpdate(ref state, SystemAPI.Time, SystemAPI.GetSingleton<PhysicsWorldSingleton>());

            CharacterVariableUpdateJob variableUpdateJob = new CharacterVariableUpdateJob
            {
                Context = _context,
                BaseContext = _baseContext,
            };
            variableUpdateJob.ScheduleParallel();

            CharacterViewJob viewJob = new CharacterViewJob
            {
                CharacterLookup = SystemAPI.GetComponentLookup<CharacterComponent>(true),
            };
            viewJob.ScheduleParallel();
        }

        [BurstCompile]
        [WithAll(typeof(Simulate))]
        public partial struct CharacterVariableUpdateJob : IJobEntity, IJobEntityChunkBeginEnd
        {
            public CharacterUpdateContext Context;
            public KinematicCharacterUpdateContext BaseContext;

            void Execute(CharacterAspect characterAspect)
            {
                characterAspect.VariableUpdate(ref Context, ref BaseContext);
            }

            public bool OnChunkBegin(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
            {
                BaseContext.EnsureCreationOfTmpCollections();
                return true;
            }

            public void OnChunkEnd(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask, bool chunkWasExecuted) {}
        }

        [BurstCompile]
        [WithAll(typeof(Simulate))]
        public partial struct CharacterViewJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<CharacterComponent> CharacterLookup;

            void Execute(ref LocalTransform localTransform, in CharacterView characterView)
            {
                if (CharacterLookup.TryGetComponent(characterView.CharacterEntity, out CharacterComponent character))
                {
                    localTransform.Rotation = character.ViewLocalRotation;
                    localTransform.Position = new float3(0, character.CameraHeight, 0);
                }
            }
        }
    }
}