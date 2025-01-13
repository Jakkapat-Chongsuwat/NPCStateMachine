using UnityEngine;
using Jakkapat.StateMachine.Core;

namespace Jakkapat.StateMachine.Example
{
    public class IdleState : BaseState<BaseContext, StateIDs>
    {
        public float idleTime = 2f;

        private float _timer;


        public override EnterState(BaseContext context, TStateEnum fromState)
        {
            _timer = 0f;
            context.StopMovement();
            return this.key;
        }

        public override StateKey OnUpdate(BaseContext context)
        {
            _timer += Time.deltaTime;
            if (!(context ;

            if (context.IsTargetInRange())
            {
                if (!context.HasApproached)
                {
                    // maybe do nothing, or approach logic, etc.
                }
                else
                {
                    context.RotateToFaceTarget();
                }
            }
            else
            {
                context.HasApproached = false;
            }

            if (_timer >= context.IdleDuration)
            {
                chanestae
            }
        }
    }
}
