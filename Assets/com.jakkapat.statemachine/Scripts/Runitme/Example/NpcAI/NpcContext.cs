using UnityEngine;
using UnityEngine.AI;

namespace Jakkapat.ToppuFSM.Example
{
    public class NpcContext : INpcContext
    {
        public AnimationController animationController { get; set; }
        public NavMeshAgent navMeshAgent { get; set; }

        public bool IsPlayerApproaching { get; set; }
        public bool IsApproachFromBehind { get; set; }
        public float GreetingTimer { get; set; }
        public bool IsApproachDecisionComplete { get; set; }
        public bool ApproachFromBehind { get; set; }
        public bool SurpriseDone { get; set; }
        public float SurpriseTimer { get; set; }
        public bool HasFacedPlayer { get; set; }
        public bool IsGreetingDone { get; set; }
        public Vector3 NpcPosition { get; set; }
        public Vector3 PlayerPosition { get; set; }

        public Transform NpcTransform { get; set; }
    }
}
