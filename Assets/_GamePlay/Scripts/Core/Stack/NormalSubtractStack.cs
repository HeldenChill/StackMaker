using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    public class NormalSubtractStack : SubtractStack
    {
        public override bool Interact(Player player)
        {
            if (!base.Interact(player))
                return false;
            PrefabManager.Inst.PushToPool(gameObject, PrefabManager.Inst.SUBTRACKSTACK);
            return true;
        }
    }
}