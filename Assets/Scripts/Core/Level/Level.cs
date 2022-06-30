using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core {
    using Data;
    public class Level : MonoBehaviour
    {
        [SerializeField]
        private static float tileWide = 1f;
        [SerializeField]
        private static float tileHeight = 0.5f;
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
                    data.PosToStack.Add(pos, stackScript);
                    stack[i].transform.localPosition = new Vector3(pos.x, 0, pos.y);                   
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