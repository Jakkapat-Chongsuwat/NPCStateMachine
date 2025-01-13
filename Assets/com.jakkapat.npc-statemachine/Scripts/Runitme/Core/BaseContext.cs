using UnityEngine;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A generic base context class for AI,
    /// no longer a ScriptableObject, just a normal class.
    /// 
    /// You can store generic movement speeds or other shared data here,
    /// plus any default methods using that data.
    /// </summary>
    public class BaseContext
    {
        public float MoveSpeed { get; set; } = 2f;   // Generic speed for movement
        public float turnSpeed { get; set; } = 3f;   // Generic turn speed

        /// <summary>
        /// If you want to store an entity's transform,
        /// you can keep it here. For NPCContext, 
        /// you might override or just set it from outside.
        /// </summary>
        public Transform selfTransform { get; set; }


        /// <summary>
        /// Example method using turnSpeed from BaseContext.
        /// Rotates 'selfTransform' to face the 'targetPosition'.
        /// </summary>
        public virtual void RotateToward(Vector3 targetPosition)
        {
            if (!selfTransform) return;

            Vector3 dir = targetPosition - selfTransform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                selfTransform.rotation = Quaternion.Slerp(
                    selfTransform.rotation,
                    targetRot,
                    Time.deltaTime * turnSpeed
                );
            }
        }
    }
}
