using UnityEngine;

namespace MyGame.NPC
{
    public class NpcContext : INpcContext
    {
        public AnimationController animationController { get; set; }
        public bool IsPlayerApproaching { get; set; }
        public bool IsApproachFromBehind { get; set; }
        public Vector3 PlayerPosition { get; set; }
        public Vector3 NpcPosition { get; set; }
        public float GreetingTimer { get; set; }
    }
}
