using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jakkapat.StateMachine.Core
{
    public enum StateIDs
    {
        Idle,
        Roaming,
        PlayerApproach,    // the nestable parent
        ApproachInitial,
        ApproachSurprise,
        ApproachGreeting,
        ApproachIdle
        // Add more states here as needed
    }
}
