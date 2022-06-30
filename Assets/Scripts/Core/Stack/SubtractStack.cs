using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    using Management;
    public class SubtractStack : AbstractStack
    {
        public override bool Interact(Player player)
        {
            if (!base.Interact(player))
                return false;
            //TO DO: Interact with player is here
            AbstractStack stack = (AbstractStack)player.Stacks.Pop();
            PrefabManager.Inst.PushToPool(stack.gameObject, "AddStack");
            player.transform.localPosition -= new Vector3(0, Level.TileHeight, 0);
            return true;
        }
    }
}