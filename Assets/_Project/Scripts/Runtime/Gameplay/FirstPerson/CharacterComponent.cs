using System;
using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MyFps.Gameplay.FirstPerson
{
    [Serializable]
    public struct CharacterComponent : IComponentData
    {
        public float GroundMaxSpeed;
        public float GroundedMovementSharpness;
        public float AirAcceleration;
        public float AirMaxSpeed;
        public float AirDrag;
        public float JumpSpeed;
        public float3 Gravity;
        public bool PreventAirAccelerationAgainstUngroundedHits;
        public BasicStepAndSlopeHandlingParameters StepAndSlopeHandling;

        public float MinViewAngle;
        public float MaxViewAngle;
        public float CameraHeight;
        [HideInInspector]
        public Entity ViewEntity;
        [HideInInspector]
        public float ViewPitchDegrees;
        [HideInInspector]
        public quaternion ViewLocalRotation;

        public static CharacterComponent GetDefault()
        {
            return new CharacterComponent
            {
                GroundMaxSpeed = 10f,
                GroundedMovementSharpness = 15f,
                AirAcceleration = 50f,
                AirMaxSpeed = 10f,
                AirDrag = 0f,
                JumpSpeed = 10f,
                Gravity = math.up() * -30f,
                PreventAirAccelerationAgainstUngroundedHits = true,
                StepAndSlopeHandling = BasicStepAndSlopeHandlingParameters.GetDefault(),

                MinViewAngle = -90f,
                MaxViewAngle = 90f,
                CameraHeight = 1.5f
            };
        }
    }

    [Serializable]
    public struct CharacterControl : IComponentData
    {
        public float3 MoveVector;
        public float2 LookYawPitchDegrees;
        public bool Jump;
        public bool Fire;
    }

    [Serializable]
    public struct CharacterView : IComponentData
    {
        public Entity CharacterEntity;
    }
}