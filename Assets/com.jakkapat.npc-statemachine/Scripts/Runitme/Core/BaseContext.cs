using UnityEngine;

namespace Jakkapat.StateMachine.Core
{
    /// <summary>
    /// A generic base context for AI, stored as a ScriptableObject
    /// so it can be easily referenced in the Inspector if needed.
    /// </summary>
    [CreateAssetMenu(menuName = "Jakkapat/StateMachine/BaseContext", fileName = "BaseContext")]
    public class BaseContext : ScriptableObject
    {
        [Header("Generic Speed for movement")]
        public float moveSpeed = 2f;

        [Header("Generic Turn Speed")]
        public float turnSpeed = 3f;

        [System.NonSerialized]
        public Transform selfTransform;

        /// <summary>
        /// Example method using turnSpeed from BaseContext.
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