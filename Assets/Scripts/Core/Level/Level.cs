using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core {
    using Data;
    public class Level : MonoBehaviour
    {
        private readonly float POSY_TALLGROUND = -2.55f;
        private readonly float POSY_STACK = 0.395f;
        [SerializeField]
        private static float tileWide = 1f;
        [SerializeField]
        private static float tileHeight = 0.25f;
        public static float TileHeight => tileHeight;
        private LevelData data;
        [SerializeField]
        private GameObject dynamicEnvironment;
        [SerializeField]
        private GameObject staticEnvironment;
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
            GetStackData();
            Data.GetRoom();
            ConstructWorld();
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
                AbstractStack stackScript = stack[i].gameObject.GetComponent<AbstractStack>();
                Vector2Int pos = GetPosition(stack[i].transform.localPosition);
                if(stackScript != null)
                {
                    stackScript.State = AbstractStack.Status.Active;
                    data.AddPosStackData(pos, stackScript);
                    stack[i].transform.localPosition = new Vector3(pos.x, 0, pos.y);                   
                }
                
            }
        }

        private void ConstructWorld() //NOTE: Depend on LevelData 
        {
            //TO DO: Static Envi
            foreach(var tileStack in data.PosToStack)
            {
                if (tileStack.Value is AddStack) 
                {
                    for(int x = -2; x <= 2; x++)
                    {
                        for(int y = -2; y <= 2; y++)
                        {
                            //NOTE: Construct Tall Ground
                            Vector2Int pos = new Vector2Int(x, y) + tileStack.Key;
                            if (data.PosToTallGround.ContainsKey(pos))
                                continue;

                            GameObject tallGroundBlank = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.TALLGROUNDBLANK);
                            tallGroundBlank.transform.parent = staticEnvironment.transform;
                            tallGroundBlank.transform.localPosition = new Vector3(pos.x, POSY_TALLGROUND, pos.y);
                            data.PosToTallGround.Add(pos, true);

                            //NOTE: Construct Wall Stack
                            if (data.CheckPosStackData(pos))
                                continue;

                            GameObject wallStack = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.WALLSTACK);
                            wallStack.transform.parent = staticEnvironment.transform;
                            wallStack.transform.localPosition = new Vector3(pos.x, POSY_STACK, pos.y);
                            data.PosToWall.Add(pos, true);
                        }
                    }
                }
            }
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

    }
}