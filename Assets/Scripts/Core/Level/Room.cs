using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core {
    public class Room: MonoBehaviour
    {
        private readonly float POSY_TALLGROUND = -2.55f;
        private readonly float POSY_STACK = 0.395f;
        private readonly Vector2Int[] DIRECTION = new Vector2Int[] {Vector2Int.right,Vector2Int.up,Vector2Int.left,Vector2Int.down};
        private Vector2Int startPos;
        private Vector2Int endPos;
        private Vector2Int max;
        private Vector2Int min;
        private TypeRoom typeRoom;
        private Level level;
        bool isHorizontal;
        public enum TypeRoom
        {
            Add = 0,
            Subtract = 1
        }

        private Dictionary<Vector2Int, AbstractStack> road = new Dictionary<Vector2Int, AbstractStack>();
        
        public void Initialize(Level level,AbstractStack type,Dictionary<Vector2Int, AbstractStack> road)
        {
            this.level = level;
            this.road = road;
            if(type is AddStack)
            {
                typeRoom = TypeRoom.Add;
            }
            else if(type is SubtractStack)
            {
                typeRoom = TypeRoom.Subtract;
            }

            foreach(var tile in road)
            {
                int value = CheckAroundMaxMin(tile.Key);
                if(value == 0)
                {
                    Debug.LogError("Road Error:" + tile.Key);
                }
                else if(value == 1)
                {
                    if (startPos == default)
                    {
                        startPos = tile.Key;
                    }
                    else
                    {
                        endPos = tile.Key;
                    }
                }
            }
        }

        private int CheckAroundMaxMin(Vector2Int pos) //Setup Max Min in this
        {
            int value = 0;
            if (max == default)
            {
                max = pos;
                min = pos;
            }
            else
            {
                if(pos.x > max.x)
                {
                    max.Set(pos.x, max.y); //NOTIFY: This may be error
                }

                if (pos.y > max.y)
                {
                    max.Set(max.x, pos.y); //NOTIFY: This may be error
                }
                if (pos.x < min.x)
                {
                    min.Set(pos.x, max.y); //NOTIFY: This may be error
                }

                if (pos.y < min.y)
                {
                    min.Set(min.x, pos.y); //NOTIFY: This may be error
                }

            }

           
            for (int i = 0; i < DIRECTION.Length; i++)
            {
                
                pos += DIRECTION[i];            
                if (road.ContainsKey(pos))
                {
                    value += 1;
                }
            }
            return value;
        }

        public void ConstuctRoom()
        {
            if(typeRoom == TypeRoom.Add)
            {
                for (int x = min.x - 1; x <= max.x; x++)
                {
                    for(int y = min.y - 1; y <= max.y; y++)
                    {
                        Vector2Int pos = new Vector2Int(x, y);

                        GameObject tallGroundBlank = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.TALLGROUNDBLANK);
                        tallGroundBlank.transform.parent = level.StaticEnvironment.transform;
                        tallGroundBlank.transform.localPosition = new Vector3(pos.x, POSY_TALLGROUND, pos.y);
                        level.Data.PosToTallGround.Add(pos, true);

                        //NOTE: Construct Wall Stack
                        if (level.Data.PosToStack.ContainsKey(pos))
                            continue;

                        GameObject wallStack = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.WALLSTACK);
                        wallStack.transform.parent = level.StaticEnvironment.transform;
                        wallStack.transform.localPosition = new Vector3(pos.x, POSY_STACK, pos.y);
                        level.Data.PosToWall.Add(pos, true);
                    }
                }
            }
            
        }
    }
}