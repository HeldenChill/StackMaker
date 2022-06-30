using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    using Management;
    public class AddStack : AbstractStack
    {
        public override bool Interact(Player player)
        {
            if (!base.Interact(player))
                return false;
            player.Stacks.Push(this);
            gameObject.transform.parent = player.transform;
            player.transform.localPosition += new Vector3(0, Level.TileHeight, 0);
            gameObject.transform.localPosition = player.Benchmark.localPosition - new Vector3(0, Level.TileHeight * (player.Stacks.Count), 0);
            return true;
        }
    }
}