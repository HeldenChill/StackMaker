using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core {
    public class Room
    {
        private readonly float POSY_TALLGROUND = -2.55f;
        private readonly float POSY_STACK = 0.395f;
        private readonly float POSY_BRIDGE = -0.05f;
        private readonly int ADDROOM_SIZE = 1;
        private readonly Quaternion HORIZONTAL_BRIDGE = Quaternion.Euler(-90, 90, 0);
        private readonly Quaternion VERTICAL_BRIDGE = Quaternion.Euler(-90, 0, 0);


        private readonly Vector2Int[] DIRECTION = new Vector2Int[] {Vector2Int.right,Vector2Int.up,Vector2Int.left,Vector2Int.down};
        private Vector2Int startPos;
        private Vector2Int endPos;
        private Vector2Int max = new Vector2Int(int.MinValue, int.MinValue);
        private Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
        private TypeRoom typeRoom;
        private Level level;
        bool isHorizontal;
        public enum TypeRoom
        {
            Add = 0,
            Subtract = 1
        }

        private Dictionary<Vector2Int, AbstractStack> road;
        
        public void Initialize(Level level,TypeRoom typeRoom,Dictionary<Vector2Int, AbstractStack> road,bool isHorizontal = false)
        {
            this.level = level;
            this.road = road;
            this.typeRoom = typeRoom;
            this.isHorizontal = isHorizontal;
            //if(type is AddStack)
            //{
            //    typeRoom = TypeRoom.Add;
            //}
            //else if(type is SubtractStack)
            //{
            //    typeRoom = TypeRoom.Subtract;
            //}

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
                else if(value == 2)
                {
                    
                    if(tile.Value is CrossAddStack)
                    {
                        CrossAddStack crossAddStack = (CrossAddStack)tile.Value;
                        List<Vector2Int> aroundTiles = CheckAroundTile(tile.Key);
                        crossAddStack.SetStackDirection(aroundTiles[0], aroundTiles[1]);
                    }
                }
            }
        }

        private int CheckAroundMaxMin(Vector2Int pos) //Setup Max Min in this
        {
            if (pos.x > max.x)
            {
                max.Set(pos.x, max.y);
            }

            if (pos.y > max.y)
            {
                max.Set(max.x, pos.y);
            }
            if (pos.x < min.x)
            {
                min.Set(pos.x, min.y);
            }

            if (pos.y < min.y)
            {
                min.Set(min.x, pos.y);
            }

            return CheckAroundTile(pos).Count;
        }

        private List<Vector2Int> CheckAroundTile(Vector2Int pos)
        {
            List<Vector2Int> res = new List<Vector2Int>();
            for (int i = 0; i < DIRECTION.Length; i++)
            {
                if (road.ContainsKey(pos + DIRECTION[i]))
                {
                    res.Add(DIRECTION[i]);
                }
            }

            return res;
        }

        public void ConstuctRoom()
        {
            if(typeRoom == TypeRoom.Add)
            {
                for (int x = min.x - ADDROOM_SIZE; x <= max.x + ADDROOM_SIZE; x++)
                {
                    for(int y = min.y - ADDROOM_SIZE; y <= max.y + ADDROOM_SIZE; y++)
                    {
                        Vector2Int pos = new Vector2Int(x, y);

                        GameObject tallGroundBlank = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.TALLGROUNDBLANK);
                        tallGroundBlank.transform.parent = level.StaticEnvironment;
                        tallGroundBlank.transform.localPosition = new Vector3(pos.x, POSY_TALLGROUND, pos.y);
                        level.Data.PosToTallGround.Add(pos, tallGroundBlank);

                        //NOTE: Construct Wall Stack
                        if (level.Data.PosToStack.ContainsKey(pos))
                            continue;

                        GameObject wallStack = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.WALLSTACK);
                        wallStack.transform.parent = level.StaticEnvironment;
                        wallStack.transform.localPosition = new Vector3(pos.x, POSY_STACK, pos.y);
                        level.Data.PosToWall.Add(pos, wallStack);
                    }
                }
            }
            else if(typeRoom == TypeRoom.Subtract)
            {
                if (isHorizontal)
                {
                    foreach(var v in road)
                    {
                        Vector2Int pos = v.Key;
                        GameObject bridge = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.BRIDGE);
                        
                        for (int i = -ADDROOM_SIZE; i <= ADDROOM_SIZE; i++)
                        {
                            if (pos == startPos || pos == endPos)
                                continue;

                            Vector2Int posCheck = new Vector2Int(pos.x, pos.y + i);
                            if (level.Data.PosToTallGround.ContainsKey(posCheck))
                            {
                                PrefabManager.Inst.PushToPool(level.Data.PosToTallGround[posCheck], PrefabManager.Inst.TALLGROUNDBLANK);
                                level.Data.PosToTallGround.Remove(posCheck);
                            }

                            if (level.Data.PosToWall.ContainsKey(posCheck))
                            {                                
                                PrefabManager.Inst.PushToPool(level.Data.PosToWall[posCheck], PrefabManager.Inst.WALLSTACK);
                                level.Data.PosToWall.Remove(posCheck);
                            }
                        }
                        bridge.transform.parent = level.StaticEnvironment.transform;
                        bridge.transform.localRotation = HORIZONTAL_BRIDGE;
                        bridge.transform.localPosition = new Vector3(pos.x,POSY_BRIDGE, pos.y);
                    }
                    
                }
                else
                {
                    foreach (var v in road)
                    {
                        Vector2Int pos = v.Key;
                        GameObject bridge = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.BRIDGE);

                        for (int i = -ADDROOM_SIZE; i <= ADDROOM_SIZE; i++)
                        {
                            if (pos == startPos || pos == endPos)
                                continue;
                            Vector2Int posCheck = new Vector2Int(pos.x + 1, pos.y);
                            if (level.Data.PosToTallGround.ContainsKey(posCheck))
                            {
                                PrefabManager.Inst.PushToPool(level.Data.PosToTallGround[posCheck], PrefabManager.Inst.TALLGROUNDBLANK);
                                level.Data.PosToTallGround.Remove(posCheck);
                            }

                            if (level.Data.PosToWall.ContainsKey(posCheck))
                            {
                                PrefabManager.Inst.PushToPool(level.Data.PosToWall[posCheck], PrefabManager.Inst.WALLSTACK);
                                level.Data.PosToWall.Remove(posCheck);
                            }
                        }
                        bridge.transform.parent = level.StaticEnvironment.transform;
                        bridge.transform.localRotation = VERTICAL_BRIDGE;
                        bridge.transform.localPosition = new Vector3(pos.x, POSY_BRIDGE, pos.y);
                    }
                }
            }
            
        }
    }
}