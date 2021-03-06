using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    using Management;
    public abstract class AddStack : AbstractStack
    {
        public override bool Interact(Player player)
        {
            if (!base.Interact(player))
                return false;
            //NOTE: Create obj represent interaction of player and stack
            player.AddScore(1);

            GameObject addStatusObj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.ISUSED_ADDSTACK);
            LevelManager.Inst.CurrentLevel.Data.IsUsedAddStacks.Add(addStatusObj);

            AddObjectStatus(addStatusObj);
            return true;
        }
    }
}