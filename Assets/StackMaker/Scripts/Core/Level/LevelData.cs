using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core.Data
{

    //IDEA:
    /*
     * 1: Get Road from scene(By Overlap Box)
     * 2: Break Road -> 2 TYPE(Add and Subtract,continous) of Road
     * 3: Build Room depend on each Road
     * 4: Connect all room together
     */
    public class LevelData : ScriptableObject
    {
        private readonly Vector2Int[] DIRECTION = new Vector2Int[] { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
        [SerializeField]
        private Dictionary<Vector2Int, AbstractStack> posToStack = new Dictionary<Vector2Int, AbstractStack>();
        //Test
        public Dictionary<Vector2Int, AbstractStack> PosToStack => posToStack;
        public Dictionary<Vector2Int, GameObject> PosToTallGround = new Dictionary<Vector2Int, GameObject>();
        public Dictionary<Vector2Int, GameObject> PosToWall = new Dictionary<Vector2Int, GameObject>();
        public List<Room> addRooms = new List<Room>();
        public List<Room> subtractRooms = new List<Room>();


        private List<Vector2Int> addStack = new List<Vector2Int>();
        private List<Vector2Int> subtrackStack = new List<Vector2Int>();
        private List<Dictionary<Vector2Int, AbstractStack>> addRoomRoads = new List<Dictionary<Vector2Int, AbstractStack>>();
        private List<Dictionary<Vector2Int, AbstractStack>> subtractRoomRoads = new List<Dictionary<Vector2Int, AbstractStack>>();
        private List<bool> isHorizontal = new List<bool>();
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
              
        public void CreateRoom(Level level)
        {
            InitRoomRoad();
            for(int i = 0; i < addRoomRoads.Count; i++)
            {
                Room room = new Room();
                room.Initialize(level, Room.TypeRoom.Add, addRoomRoads[i]);
                room.ConstuctRoom();
                addRooms.Add(room);
            }

            for(int i = 0; i < subtractRoomRoads.Count; i++)
            {
                Room room = new Room();
                room.Initialize(level, Room.TypeRoom.Subtract, subtractRoomRoads[i],isHorizontal[i]);               
                room.ConstuctRoom();
                subtractRooms.Add(room);
            }
        }


        bool isFirstSpreadOut = true;
        private void SpreadOut(Vector2Int pos,Room.TypeRoom type, Dictionary<Vector2Int, AbstractStack> road)
        {

            if(type == Room.TypeRoom.Add)
            {
                road.Add(pos, posToStack[pos]);
                addStack.Remove(pos);               
                for (int i = 0; i < DIRECTION.Length; i++)
                {
                    if (addStack.Contains(pos + DIRECTION[i]))
                    {
                        SpreadOut(pos + DIRECTION[i], type, road);
                    }
                }
                return;
            }
            else if(type == Room.TypeRoom.Subtract)
            {

                road.Add(pos, posToStack[pos]);
                subtrackStack.Remove(pos);
                for (int i = 0; i < DIRECTION.Length; i++)
                {
                    if (subtrackStack.Contains(pos + DIRECTION[i]))
                    {
                        if (isFirstSpreadOut)
                        {
                            if(DIRECTION[i].x == 0)
                            {
                                isHorizontal.Add(false);
                            }
                            else
                            {
                                isHorizontal.Add(true);
                            }
                            isFirstSpreadOut = false;
                        }
                        SpreadOut(pos + DIRECTION[i], type, road);
                    }
                }
                return;
            }
        }

        private void InitRoomRoad()
        {
            //addStack.Sort(new Vector2IntComparer());
            //subtrackStack.Sort(new Vector2IntComparer());
            Dictionary<Vector2Int, AbstractStack> addRoad;
            Dictionary<Vector2Int, AbstractStack> subtractRoad;

            while (addStack.Count > 0)
            {
                addRoad = new Dictionary<Vector2Int, AbstractStack>();
                SpreadOut(addStack[0], Room.TypeRoom.Add, addRoad);
                addRoomRoads.Add(addRoad);
            }

            while (subtrackStack.Count > 0)
            {
                isFirstSpreadOut = true;
                subtractRoad = new Dictionary<Vector2Int, AbstractStack>();
                SpreadOut(subtrackStack[0], Room.TypeRoom.Subtract, subtractRoad);
                subtractRoomRoads.Add(subtractRoad);
            }
        }
    }
}
