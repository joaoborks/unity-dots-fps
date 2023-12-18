using MyFps.Input;
using Unity.Burst;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace MyFps.Gameplay.FirstPerson
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class PlayerInputsSystem : SystemBase
    {
        GameplayInput.NavigationActions _navigationActions;
        GameplayInput.CameraActions _cameraActions;
        GameplayInput.CombatActions _combatActions;

        protected override void OnCreate()
        {
            RequireForUpdate<FixedTickSystem.Singleton>();
            RequireForUpdate(SystemAPI.QueryBuilder().WithAll<Player, PlayerInputs>().Build());

            var gameplayInput = GameplayInput.Instance;
            _navigationActions = gameplayInput.Navigation;
            _cameraActions = gameplayInput.Camera;
            _combatActions = gameplayInput.Combat;

            _navigationActions.Enable();
            _cameraActions.Enable();
            _combatActions.Enable();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void OnUpdate()
        {
            uint fixedTick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;

            foreach (var (playerInputs, player) in SystemAPI.Query<RefRW<PlayerInputs>, Player>())
            {
                playerInputs.ValueRW.MoveInput = _navigationActions.Move.ReadValue<Vector2>();
                playerInputs.ValueRW.LookInput = _cameraActions.FreeLook.ReadValue<Vector2>();

                playerInputs.ValueRW.FirePressed = _combatActions.Fire.WasPerformedThisFrame();

                // For button presses that need to be queried during fixed update, use the "FixedInputEvent" helper struct.
                // This is part of a strategy for proper handling of button press events that are consumed during the fixed update group
                if (_navigationActions.Jump.WasPerformedThisFrame())
                {
                    playerInputs.ValueRW.JumpPressed.Set(fixedTick);
                }
            }
        }
    }

    /// <summary>
    /// Apply inputs that need to be read at a variable rate
    /// </summary>
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdateBefore(typeof(FixedStepSimulationSystemGroup))]
    [BurstCompile]
    public partial struct PlayerVariableStepControlSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<Player, PlayerInputs>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerInputs, player) in SystemAPI.Query<PlayerInputs, Player>().WithAll<Simulate>())
            {
                if (SystemAPI.HasComponent<CharacterControl>(player.ControlledCharacter))
                {
                    CharacterControl characterControl = SystemAPI.GetComponent<CharacterControl>(player.ControlledCharacter);

                    characterControl.LookYawPitchDegrees = playerInputs.LookInput * player.MouseSensitivity;
                    characterControl.Fire = playerInputs.FirePressed;

                    SystemAPI.SetComponent(player.ControlledCharacter, characterControl);
                }
            }
        }
    }

    /// <summary>
    /// Apply inputs that need to be read at a fixed rate.
    /// It is necessary to handle this as part of the fixed step group, in case your framerate is lower than the fixed step rate.
    /// </summary>
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
    [BurstCompile]
    public partial struct PlayerFixedStepControlSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<FixedTickSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<Player, PlayerInputs>().Build());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            uint fixedTick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;

            foreach (var (playerInputs, player) in SystemAPI.Query<PlayerInputs, Player>().WithAll<Simulate>())
            {
                if (SystemAPI.HasComponent<CharacterControl>(player.ControlledCharacter))
                {
                    CharacterControl characterControl = SystemAPI.GetComponent<CharacterControl>(player.ControlledCharacter);

                    quaternion characterRotation = SystemAPI.GetComponent<LocalTransform>(player.ControlledCharacter).Rotation;
                    
                    // Move
                    float3 characterForward = MathUtilities.GetForwardFromRotation(characterRotation);
                    float3 characterRight = MathUtilities.GetRightFromRotation(characterRotation);
                    characterControl.MoveVector = (playerInputs.MoveInput.y * characterForward) + (playerInputs.MoveInput.x * characterRight);
                    characterControl.MoveVector = MathUtilities.ClampToMaxLength(characterControl.MoveVector, 1f);

                    // Jump
                    characterControl.Jump = playerInputs.JumpPressed.IsSet(fixedTick);

                    SystemAPI.SetComponent(player.ControlledCharacter, characterControl);
                }
            }
        }
    }
}