using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    public class DesSubtractStack : SubtractStack
    {
        public override bool Interact(Player player)
        {
            if (!base.Interact(player))
                return false;
            return true;
        }
    }
}