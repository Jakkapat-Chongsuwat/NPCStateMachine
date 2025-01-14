using Jakkapat.ToppuFSM.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Jakkapat.ToppuFSM.Example
{
    public interface INpcContext : IContext
    {
        AnimationController animationController { get; set; }
        NavMeshAgent navMeshAgent { get; set; }

        bool IsPlayerApproaching { get; set; }
        bool IsApproachFromBehind { get; set; }
        float GreetingTimer { get; set; }
        bool IsApproachDecisionComplete { get; set; }
        bool ApproachFromBehind { get; set; }
        bool SurpriseDone { get; set; }
        float SurpriseTimer { get; set; }
        bool HasFacedPlayer { get; set; }
        bool IsGreetingDone { get; set; }

        Vector3 NpcPosition { get; set; }
        Vector3 PlayerPosition { get; set; }

        // NEW: So we can actually rotate the NPC's transform
        Transform NpcTransform { get; set; }
    }
}
