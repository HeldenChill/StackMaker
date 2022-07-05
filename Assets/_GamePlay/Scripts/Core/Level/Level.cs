using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core {
    using Data;
    public class Level : MonoBehaviour
    {
        [SerializeField]
        private const float tileWide = 1f;
        [SerializeField]
        private const float tileHeight = 0.25f;
        public static float TileHeight => tileHeight;
        private LevelData data;
        [SerializeField]
        private GameObject dynamicEnvironment;
        [SerializeField]
        private GameObject staticEnvironment;
        [SerializeField]
        private Player player;
        [SerializeField]
        private WinPosition winPos;
        private Vector2Int playerPosition;
        public float mapWide = 10f;
        public LayerMask stackMask;

        private float mapHeight = 2f;
        private Vector3 MapShape
        {
            get
            {
                Vector3 shape = Vector3.one * mapWide;
                return new Vector3(shape.x, mapHeight, shape.z);
            }
        }
        public LevelData Data
        {
            get => data;
        }
        public Transform DynamicEnvironment
        {
            get => dynamicEnvironment.transform;
        }
        public Transform StaticEnvironment
        {
            get => staticEnvironment.transform;
        }

        private void Awake()
        {
            data = ScriptableObject.CreateInstance("LevelData") as LevelData;
        }

        private void Start()
        {
            //GetStackData();
            //TEST:LOAD DATA
            LoadData(mapData);
            Data.CreateRoom(this);     
        }

        private void OnEnable()
        {
            player.OnPlayerReachDes += OnWinGame;
        }
        public static Vector2Int GetPosition(Vector3 pos)
        {
            pos = pos / tileWide;
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
        }
        private void Initialized()
        {

        }

        private void GetStackData()
        {

            Collider[] stack = Physics.OverlapBox(transform.localPosition, MapShape, Quaternion.identity, stackMask);
            for(int i = 0; i < stack.Length; i++)
            {
                stack[i].transform.parent = DynamicEnvironment;
                AbstractStack stackScript = stack[i].gameObject.GetComponent<AbstractStack>();
                Vector2Int pos = GetPosition(stack[i].transform.localPosition);
                if (stackScript != null)
                {
                    stackScript.State = AbstractStack.Status.Active;
                    data.AddPosStackData(pos, stackScript);
                    stack[i].transform.localPosition = new Vector3(pos.x, 0, pos.y);
                }
            }
        }

        //For Game Design
        private void LoadData(int[,] mapData)
        {
            for (int y = 0; y < mapData.GetLength(0); y++)
            {
                for (int x = 0; x < mapData.GetLength(1); x++)
                {
                    // -1:None
                    // 0: Add Stack
                    // 1: Subtract Stack
                    // 2: Cross Add Stack
                    // 3: Des Subtract Stack
                    // 4: Start Position
                    GameObject obj = null;
                    if(mapData[y,x] == 0)
                    {
                        continue;
                    }
                    else if(mapData[y,x] == 1)
                    {
                        obj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.ADDSTACK);
                    }
                    else if (mapData[y,x] == 2)
                    {
                        obj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.SUBTRACKSTACK);
                    }
                    else if (mapData[y,x] == 3)
                    {
                        obj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.CROSS_ADDSTACK); //TEST: Not have pool -yet
                    }
                    else if (mapData[y,x] == 4)
                    {
                        obj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.DES_SUBTRACTSTACK); //TEST: Not have pool -yet
                    }
                    else if (mapData[y,x] == 5)
                    {
                        playerPosition = new Vector2Int(x, -y);                       
                    }
                    if(obj != null)
                    {
                        AbstractStack objScript = obj.gameObject.GetComponent<AbstractStack>();
                        objScript.State = AbstractStack.Status.Active;
                        data.AddPosStackData(new Vector2Int(x, -y), objScript);

                        obj.transform.parent = DynamicEnvironment;
                        obj.transform.localPosition = new Vector3(x, 0, -y);
                    }
                    else
                    {
                        player.transform.localPosition = new Vector3(playerPosition.x, 0, playerPosition.y);
                    }
                    
                }
            }
        }

        private void ConstructWorld() //NOTE: Depend on LevelData 
        {
           
        }
        private void OnWinGame()
        {
            winPos.OnGameWin();
            //TODO: RESET LEVEL
            //TODO: CHANGE CAMERA
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position,MapShape * 2);
        }

        #region Editor Function
        public void NormalizeStackPosition()
        {
            Collider[] stack = Physics.OverlapBox(transform.position, Vector3.one * mapWide, Quaternion.identity, stackMask);
            for (int i = 0; i < stack.Length; i++)
            {
                Vector2Int pos = GetPosition(stack[i].transform.localPosition);
                stack[i].transform.localPosition = new Vector3(pos.x, 0, pos.y);
            }
        }
        #endregion

        private void OnDisable()
        {
            player.OnPlayerReachDes -= OnWinGame;
        }

        //TEST: For Game Design
        int[,] mapData = new int[,]
        {
            {5, 1, 1, 1, 0, 0 ,0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            {0, 0, 0, 1, 0, 0 ,0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            {0, 0, 1, 1, 0, 0 ,0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            {0, 0, 1, 0, 0, 0 ,0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            {0, 0, 1, 0, 0, 0 ,0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            {0, 0, 2, 0, 0, 0 ,0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            {0, 0, 2, 0, 0, 0 ,0, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0 },
            {0, 0, 2, 0, 0, 0 ,0, 0, 1, 0, 0, 0, 0, 0, 0, 2, 0 },
            {0, 0, 2, 0, 0, 0 ,0, 1, 1, 0, 0, 0, 0, 0, 0, 2, 0 },
            {0, 0, 1, 1, 1, 1 ,1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
            {0, 0, 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
            {0, 0, 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
            {0, 0, 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
            {0, 0, 0, 0, 0, 0 ,0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
            {0, 0, 0, 0, 0, 0 ,0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0 },
            {0, 0, 0, 0, 0, 0 ,0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            {0, 0, 1, 0, 0, 0 ,0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            {0, 0, 1, 2, 2, 2 ,2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0 },
        };
    }
}