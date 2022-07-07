using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    using Data;
    using System;
    public class Level : MonoBehaviour
    {
        public event Action OnWinGame;
        [SerializeField]
        private const float tileWide = 1f;
        [SerializeField]
        private const float tileHeight = 0.25f;
        public static float TileWide => tileWide;
        public static float TileHeight => tileHeight;
        [SerializeField]
        private readonly Vector3 STACK_SCALE = new Vector3(1, 1, 1.5f);
        
        private LevelData data;
        [SerializeField]
        private GameObject dynamicEnvironment;
        [SerializeField]
        private GameObject staticEnvironment;
        [SerializeField]
        private Player player;
        private WinPosition winPos;
        TextAsset mapDataText;
        private Vector2Int playerPosition;
        
        public LayerMask stackMask;

        
        
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
            mapData = ConvertStringToMapData();
            LoadStackData();
            Data.CreateRoom(this);
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


        

        private void OnDisable()
        {
            player.OnPlayerReachDes -= WinGame;
        }

    }
}