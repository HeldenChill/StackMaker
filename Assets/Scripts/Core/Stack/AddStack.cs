using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    using Management;
    public class AddStack : AbstractStack
    {
        public override void Interact(Player player)
        {
            player.Stacks.Push(this);
            gameObject.transform.parent = player.transform.parent;
            gameObject.transform.localPosition = player.Benchmark.localPosition - new Vector3(0, Level.TileHeight * player.Stacks.Count, 0);
            player.transform.localPosition += new Vector3(0, Level.TileHeight, 0);
        }
    }
}