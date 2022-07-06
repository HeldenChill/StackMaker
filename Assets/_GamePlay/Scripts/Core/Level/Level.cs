using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    using Data;
    using System;
    public class Level : MonoBehaviour
    {
        public event Action OnWinGame;
        public enum LevelType
        {
            ConstructFromTile = 0,
            ConstructFromText = 1
        }
        [SerializeField]
        private const float tileWide = 1f;
        [SerializeField]
        private const float tileHeight = 0.25f;
        [SerializeField]
        private readonly Vector3 STACK_SCALE = new Vector3(1, 1, 1.5f);
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
        [SerializeField]
        TextAsset mapDataText;
        private Vector2Int playerPosition;

        public LevelType Type;
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

        public void SetWinPos(WinPosition winPos)
        {
            this.winPos = winPos;
        }

        private void Awake()
        {
            data = ScriptableObject.CreateInstance("LevelData") as LevelData;
        }

        private void Start()
        {
            //ConstructWorld();
        }

        private void OnEnable()
        {
            player.OnPlayerReachDes += WinGame;
        }

        public static Vector2Int GetPosition(Vector3 pos)
        {
            pos = pos / tileWide;
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
        }
        public void Initialize(TextAsset levelData)
        {
            player.ResetPlayer();
            mapDataText = levelData;
            ConstructWorld();        
        }
        private void ConstructWorld() //NOTE: Depend on LevelData 
        {
            if (Type == LevelType.ConstructFromTile)
            {
                GetStackDataFromScene();
            }
            else if (Type == LevelType.ConstructFromText)
            {
                mapData = ConvertStringToMapData();
                LoadStackData();
            }
            Data.CreateRoom(this);
        }       

        private void GetStackDataFromScene()
        {

            Collider[] stack = Physics.OverlapBox(transform.localPosition, MapShape, Quaternion.identity, stackMask);
            for (int i = 0; i < stack.Length; i++)
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
        List<List<int>> mapData;
        private void LoadStackData()
        {
            for (int y = 0; y < mapData.Count; y++)
            {
                for (int x = 0; x < mapData[0].Count; x++)
                {
                    // 0:None
                    // 1: Add Stack
                    // 2: Subtract Stack
                    // 3: Cross Add Stack
                    // 4: Des Subtract Stack
                    // 5: Start Position
                    GameObject obj = null;
                    if (mapData[y][x] == 0)
                    {
                        continue;
                    }
                    else if (mapData[y][x] == 1)
                    {
                        obj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.ADDSTACK);
                        obj.transform.localScale = STACK_SCALE;
                    }
                    else if (mapData[y][x] == 2)
                    {
                        obj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.SUBTRACKSTACK);
                    }
                    else if (mapData[y][x] == 3)
                    {
                        obj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.CROSS_ADDSTACK);
                    }
                    else if (mapData[y][x] == 4)
                    {
                        obj = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.DES_SUBTRACTSTACK); 

                    }
                    else if (mapData[y][x] == 5)
                    {
                        playerPosition = new Vector2Int(x, -y);
                    }

                    if (obj != null)
                    {
                        AbstractStack objScript = obj.gameObject.GetComponent<AbstractStack>();
                        objScript.State = AbstractStack.Status.Active;
                        data.AddPosStackData(new Vector2Int(x, -y), objScript);

                        
                        obj.transform.parent = DynamicEnvironment;
                        obj.transform.localPosition = new Vector3(x, 0, -y);
                        if(obj.name == "Here")
                        {
                            Debug.Log("Use Here");
                        }
                        if(x == 18 && y == 9)
                        {
                            obj.name = "Here";
                        }
                    }
                    else
                    {
                        player.transform.localPosition = new Vector3(playerPosition.x, 0, playerPosition.y);
                    }
                    
                }
            }
            ConvertStringToMapData();
        }

        private List<List<int>> ConvertStringToMapData()
        {
            string data = mapDataText.text;
            List<List<int>> res = new List<List<int>>();
            List<int> row = new List<int>();
            foreach (var c in data)
            {
                if (c == '#')
                {
                    res.Add(row);
                    row = new List<int>();
                    continue;
                }
                else if (c == '0')
                {
                    row.Add(0);
                }
                else if (c == '+')
                {
                    row.Add(1);
                }
                else if (c == '-')
                {
                    row.Add(2);
                }
                else if (c == 'x')
                {
                    row.Add(3);
                }
                else if (c == '=')
                {
                    row.Add(4);
                }
                else if (c == 'S')
                {
                    row.Add(5);
                }
            }
            return res;
        }

        private void WinGame()
        {
            //winPos.WinGame();
            OnWinGame?.Invoke();
            //TODO: SHOW GUI
            //TODO: RESET LEVEL
            //TODO: CHANGE CAMERA
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, MapShape * 2);
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
            player.OnPlayerReachDes -= WinGame;
        }

    }
}