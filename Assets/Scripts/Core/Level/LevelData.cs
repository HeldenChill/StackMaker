using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core.Data
{
    public class LevelData : ScriptableObject
    {
        [SerializeField]
        public Dictionary<Vector2Int, AbstractStack> PosToStack = new Dictionary<Vector2Int, AbstractStack>();
    }
}
