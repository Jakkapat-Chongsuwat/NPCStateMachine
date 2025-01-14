using UnityEngine;
using Jakkapat.ToppuFSM.Core;

namespace Jakkapat.ToppuFSM.Example
{
    public class TurnToPlayerState<TContext> : BaseState<TContext>
          where TContext : INpcContext
    {
        private float rotationSpeed = 5f;
        private float angleThreshold = 5f;

        public TurnToPlayerState(StateMachine<TContext> parentSM) : base(parentSM) { }

        public override void OnEnter()
        {
            base.OnEnter();

            needsExitTime = true;
            Context.HasFacedPlayer = false;

            Context.animationController?.SetSpeed(0f);
            Context.animationController?.SetMotionSpeed(0f);

            Debug.Log("NPC: Enter TurnToPlayerState");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Vector3 dir = Context.PlayerPosition - Context.NpcPosition;
            dir.y = 0f;
            dir.Normalize();

            if (dir.sqrMagnitude > 0.0001f)
            {
                Context.NpcTransform.rotation = Quaternion.Slerp(
                    Context.NpcTransform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * rotationSpeed
                );
            }

            float angle = Vector3.Angle(Context.NpcTransform.forward, dir);

            if (angle < angleThreshold)
            {
                Context.HasFacedPlayer = true;
                needsExitTime = false;
            }
        }

        public override void OnExit()
        {
            Debug.Log("NPC: Exit TurnToPlayerState");
            base.OnExit();
        }
    }
}
