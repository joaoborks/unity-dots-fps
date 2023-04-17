using MyFps.Input;
using System;
using Unity.Entities;
using Unity.Mathematics;

namespace MyFps.Gameplay.FirstPerson
{
    [Serializable]
    public struct Player : IComponentData
    {
        public Entity ControlledCharacter;
        public float MouseSensitivity;
    }

    [Serializable]
    public struct PlayerInputs : IComponentData
    {
        public float2 MoveInput;
        public float2 LookInput;
        public FixedInputEvent JumpPressed;
        public bool FirePressed;
    }
}