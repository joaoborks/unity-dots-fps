using Unity.CharacterController;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace MyFps.Gameplay.FirstPerson
{
    public struct CharacterUpdateContext
    {
        // Here, you may add additional global data for your character updates, such as ComponentLookups, Singletons, NativeCollections, etc...
        // The data you add here will be accessible in your character updates and all of your character "callbacks".

        public void OnSystemCreate(ref SystemState state)
        {
            // Get lookups
        }

        public void OnSystemUpdate(ref SystemState state)
        {
            // Update lookups
        }
    }

    public readonly partial struct CharacterAspect : IAspect, IKinematicCharacterProcessor<CharacterUpdateContext>
    {
        public readonly KinematicCharacterAspect KinematicCharacterAspect;
        public readonly RefRW<CharacterComponent> CharacterComponent;
        public readonly RefRW<CharacterControl> CharacterControl;

        public void PhysicsUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext)
        {
            ref CharacterComponent characterComponent = ref CharacterComponent.ValueRW;
            ref KinematicCharacterBody characterBody = ref KinematicCharacterAspect.CharacterBody.ValueRW;
            ref float3 characterPosition = ref KinematicCharacterAspect.LocalTransform.ValueRW.Position;

            // First phase of default character update
            KinematicCharacterAspect.Update_Initialize(in this, ref context, ref baseContext, ref characterBody, baseContext.Time.DeltaTime);
            KinematicCharacterAspect.Update_ParentMovement(in this, ref context, ref baseContext, ref characterBody, ref characterPosition, characterBody.WasGroundedBeforeCharacterUpdate);
            KinematicCharacterAspect.Update_Grounding(in this, ref context, ref baseContext, ref characterBody, ref characterPosition);

            // Update desired character velocity after grounding was detected, but before doing additional processing that depends on velocity
            HandleVelocityControl(ref context, ref baseContext);

            // Second phase of default character update
            KinematicCharacterAspect.Update_PreventGroundingFromFutureSlopeChange(in this, ref context, ref baseContext, ref characterBody, in characterComponent.StepAndSlopeHandling);
            KinematicCharacterAspect.Update_GroundPushing(in this, ref context, ref baseContext, characterComponent.Gravity);
            KinematicCharacterAspect.Update_MovementAndDecollisions(in this, ref context, ref baseContext, ref characterBody, ref characterPosition);
            KinematicCharacterAspect.Update_MovingPlatformDetection(ref baseContext, ref characterBody);
            KinematicCharacterAspect.Update_ParentMomentum(ref baseContext, ref characterBody);
            KinematicCharacterAspect.Update_ProcessStatefulCharacterHits();
        }

        private void HandleVelocityControl(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext)
        {
            float deltaTime = baseContext.Time.DeltaTime;
            ref KinematicCharacterBody characterBody = ref KinematicCharacterAspect.CharacterBody.ValueRW;
            ref CharacterComponent characterComponent = ref CharacterComponent.ValueRW;
            ref CharacterControl characterControl = ref CharacterControl.ValueRW;

            // Rotate move input and velocity to take into account parent rotation
            if (characterBody.ParentEntity != Entity.Null)
            {
                characterControl.MoveVector = math.rotate(characterBody.RotationFromParent, characterControl.MoveVector);
                characterBody.RelativeVelocity = math.rotate(characterBody.RotationFromParent, characterBody.RelativeVelocity);
            }

            if (characterBody.IsGrounded)
            {
                // Move on ground
                float3 targetVelocity = characterControl.MoveVector * characterComponent.GroundMaxSpeed;
                CharacterControlUtilities.StandardGroundMove_Interpolated(ref characterBody.RelativeVelocity, targetVelocity, characterComponent.GroundedMovementSharpness, deltaTime, characterBody.GroundingUp, characterBody.GroundHit.Normal);

                // Jump
                if (characterControl.Jump)
                {
                    CharacterControlUtilities.StandardJump(ref characterBody, characterBody.GroundingUp * characterComponent.JumpSpeed, true, characterBody.GroundingUp);
                }
            }
            else
            {
                // Move in air
                float3 airAcceleration = characterControl.MoveVector * characterComponent.AirAcceleration;
                if (math.lengthsq(airAcceleration) > 0f)
                {
                    float3 tmpVelocity = characterBody.RelativeVelocity;
                    CharacterControlUtilities.StandardAirMove(ref characterBody.RelativeVelocity, airAcceleration, characterComponent.AirMaxSpeed, characterBody.GroundingUp, deltaTime, false);

                    // Cancel air acceleration from input if we would hit a non-grounded surface (prevents air-climbing slopes at high air accelerations)
                    if (characterComponent.PreventAirAccelerationAgainstUngroundedHits && KinematicCharacterAspect.MovementWouldHitNonGroundedObstruction(in this, ref context, ref baseContext, characterBody.RelativeVelocity * deltaTime, out ColliderCastHit hit))
                    {
                        characterBody.RelativeVelocity = tmpVelocity;
                    }
                }

                // Gravity
                CharacterControlUtilities.AccelerateVelocity(ref characterBody.RelativeVelocity, characterComponent.Gravity, deltaTime);

                // Drag
                CharacterControlUtilities.ApplyDragToVelocity(ref characterBody.RelativeVelocity, deltaTime, characterComponent.AirDrag);
            }
        }

        public void VariableUpdate(ref CharacterUpdateContext context, ref KinematicCharacterUpdateContext baseContext)
        {
            ref KinematicCharacterBody characterBody = ref KinematicCharacterAspect.CharacterBody.ValueRW;
            ref CharacterComponent characterComponent = ref CharacterComponent.ValueRW;
            ref CharacterControl characterControl = ref CharacterControl.ValueRW;
            ref quaternion characterRotation = ref KinematicCharacterAspect.LocalTransform.ValueRW.Rotation;

            // Add rotation from parent body to the character rotation
            // (this is for allowing a rotating moving platform to rotate your character as well, and handle interpolation properly)
            KinematicCharacterUtilities.AddVariableRateRotationFromFixedRateRotation(ref characterRotation, characterBody.RotationFromParent, baseContext.Time.DeltaTime, characterBody.LastPhysicsUpdateDeltaTime);

            // Compute character & view rotations from rotation input
            CharacterUtilities.ComputeFinalRotationsFromRotationDelta(
                ref characterRotation,
                ref characterComponent.ViewPitchDegrees,
                characterControl.LookYawPitchDegrees,
                0,
                characterComponent.MinViewAngle,
                characterComponent.MaxViewAngle,
                out float canceledPitchDegrees,
                out characterComponent.ViewLocalRotation);
        }

        #region Character Processor Callbacks
        public void UpdateGroundingUp(
            ref CharacterUpdateContext context,
            ref KinematicCharacterUpdateContext baseContext)
        {
            ref KinematicCharacterBody characterBody = ref KinematicCharacterAspect.CharacterBody.ValueRW;

            KinematicCharacterAspect.Default_UpdateGroundingUp(ref characterBody);
        }

        public bool CanCollideWithHit(
            ref CharacterUpdateContext context,
            ref KinematicCharacterUpdateContext baseContext,
            in BasicHit hit)
        {
            return PhysicsUtilities.IsCollidable(hit.Material);
        }

        public bool IsGroundedOnHit(
            ref CharacterUpdateContext context,
            ref KinematicCharacterUpdateContext baseContext,
            in BasicHit hit,
            int groundingEvaluationType)
        {
            CharacterComponent characterComponent = CharacterComponent.ValueRO;

            return KinematicCharacterAspect.Default_IsGroundedOnHit(
                in this,
                ref context,
                ref baseContext,
                in hit,
                in characterComponent.StepAndSlopeHandling,
                groundingEvaluationType);
        }

        public void OnMovementHit(
                ref CharacterUpdateContext context,
                ref KinematicCharacterUpdateContext baseContext,
                ref KinematicCharacterHit hit,
                ref float3 remainingMovementDirection,
                ref float remainingMovementLength,
                float3 originalVelocityDirection,
                float hitDistance)
        {
            ref KinematicCharacterBody characterBody = ref KinematicCharacterAspect.CharacterBody.ValueRW;
            ref float3 characterPosition = ref KinematicCharacterAspect.LocalTransform.ValueRW.Position;
            CharacterComponent characterComponent = CharacterComponent.ValueRO;

            KinematicCharacterAspect.Default_OnMovementHit(
                in this,
                ref context,
                ref baseContext,
                ref characterBody,
                ref characterPosition,
                ref hit,
                ref remainingMovementDirection,
                ref remainingMovementLength,
                originalVelocityDirection,
                hitDistance,
                characterComponent.StepAndSlopeHandling.StepHandling,
                characterComponent.StepAndSlopeHandling.MaxStepHeight,
                characterComponent.StepAndSlopeHandling.CharacterWidthForStepGroundingCheck);
        }

        public void OverrideDynamicHitMasses(
            ref CharacterUpdateContext context,
            ref KinematicCharacterUpdateContext baseContext,
            ref PhysicsMass characterMass,
            ref PhysicsMass otherMass,
            BasicHit hit)
        {
            // Custom mass overrides
        }

        public void ProjectVelocityOnHits(
            ref CharacterUpdateContext context,
            ref KinematicCharacterUpdateContext baseContext,
            ref float3 velocity,
            ref bool characterIsGrounded,
            ref BasicHit characterGroundHit,
            in DynamicBuffer<KinematicVelocityProjectionHit> velocityProjectionHits,
            float3 originalVelocityDirection)
        {
            CharacterComponent characterComponent = CharacterComponent.ValueRO;

            KinematicCharacterAspect.Default_ProjectVelocityOnHits(
                ref velocity,
                ref characterIsGrounded,
                ref characterGroundHit,
                in velocityProjectionHits,
                originalVelocityDirection,
                characterComponent.StepAndSlopeHandling.ConstrainVelocityToGroundPlane);
        }
        #endregion
    }
}