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
            AbstractStack stack = player.ReturnStack();
            PrefabManager.Inst.PushToPool(stack.gameObject,PrefabManager.Inst.ADDSTACK);

            //NOTE: Create obj represent interaction of player and stack
            GameObject addStatusObj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.ISUSED_SUBTRACTSTACK);
            LevelManager.Inst.CurrentLevel.Data.IsUsedSubtractStacks.Add(addStatusObj);
            AddObjectStatus(addStatusObj);

            PrefabManager.Inst.PushToPool(gameObject, PrefabManager.Inst.SUBTRACKSTACK);

            player.transform.localPosition -= new Vector3(0, Level.TileHeight, 0);
            return true;
        }
    }
}