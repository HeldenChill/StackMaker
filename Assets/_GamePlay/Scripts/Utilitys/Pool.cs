using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys
{
    public class Pool : MonoBehaviour
    {
        public GameObject mainPool;
        [HideInInspector]
        private GameObject obj;

        List<GameObject> unavailable;
        List<GameObject> available;
        Quaternion initQuaternion;
        int numObj = 10;
        public void Initialize(GameObject obj,Quaternion initQuaternion = default,int numObj = 10)
        {
            this.numObj = numObj;
            this.obj = obj;
            unavailable = new List<GameObject>();
            available = new List<GameObject>();
            this.initQuaternion = initQuaternion;
            AddObject();
        }


        public void AddObject()
        {
            for (int i = 0; i < numObj; i++)
            {
                GameObject obj = Instantiate(this.obj, Vector3.zero, this.initQuaternion, mainPool.transform);
                obj.SetActive(false);
                available.Add(obj);
            }
        }

        public void Push(GameObject obj)
        {
            available.Add(obj);
            obj.SetActive(false);
            obj.transform.parent = mainPool.transform;
            obj.transform.position = Vector3.zero;
        }

        public GameObject Pop()
        {
            if(available.Count <= 0)
            {
                AddObject();
            }
            GameObject returnObj = available[0];
            unavailable.Add(returnObj);
            available.RemoveAt(0);
            returnObj.SetActive(true);
            return returnObj;
        }

    }
}

