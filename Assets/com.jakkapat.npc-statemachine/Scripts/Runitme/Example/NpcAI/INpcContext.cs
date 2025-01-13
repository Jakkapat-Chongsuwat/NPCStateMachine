using UnityEngine;

namespace MyGame.NPC
{
    public interface INpcContext
    {
        bool IsPlayerApproaching { get; set; }
        bool IsApproachFromBehind { get; set; }
        Vector3 PlayerPosition { get; }
        Vector3 NpcPosition { get; set; }
        float GreetingTimer { get; set; }
    }
}