using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core.Data
{
    public class LevelData : ScriptableObject
    {
        private readonly Vector2Int[] DIRECTION = new Vector2Int[] { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
        [SerializeField]
        private Dictionary<Vector2Int, AbstractStack> posToStack = new Dictionary<Vector2Int, AbstractStack>();
        public Dictionary<Vector2Int, bool> PosToTallGround = new Dictionary<Vector2Int, bool>();
        public Dictionary<Vector2Int, bool> PosToWall = new Dictionary<Vector2Int, bool>();
        private List<Vector2Int> addStack = new List<Vector2Int>();
        private List<Vector2Int> subtrackStack = new List<Vector2Int>();

        //Test
        public Dictionary<Vector2Int, AbstractStack> PosToStack => posToStack;


        private List<Dictionary<Vector2Int, AbstractStack>> res = new List<Dictionary<Vector2Int, AbstractStack>>();
        public void AddPosStackData(Vector2Int key,AbstractStack value)
        {
            posToStack.Add(key,value);
            if(value is AddStack)
            {
                addStack.Add(key);
            }
            else if(value is SubtractStack)
            {
                subtrackStack.Add(key);
            }
        }

        public KeyValuePair<Vector2Int,AbstractStack> GetPosStackData(Vector2Int key)
        {
            return new KeyValuePair<Vector2Int, AbstractStack>(key,posToStack[key]);
        }

        public bool CheckPosStackData(Vector2Int key)
        {
            return posToStack.ContainsKey(key);
        }
        public List<Dictionary<Vector2Int, AbstractStack>> GetRoom()
        {
            addStack.Sort();
            subtrackStack.Sort();
            Debug.Log(addStack.ToString());
            //foreach(var tile in posToStack)
            //{

            //}
            return null;
        }
    }
}
