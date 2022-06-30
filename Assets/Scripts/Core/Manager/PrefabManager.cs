using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilitys;
public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Inst;
    private List<string> namePools = new List<string>();
    public List<string> NamePools => namePools;
    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
            return;
        }
        Destroy(gameObject);
    }
    public GameObject pool;
    Dictionary<string, Pool> poolData = new Dictionary<string, Pool>();

    public void CreatePool(GameObject obj,string namePool)
    {
        if (!poolData.ContainsKey(namePool))
        {
            GameObject newPool = Instantiate(pool, Vector3.zero, Quaternion.identity);
            Pool poolScript = newPool.GetComponent<Pool>();
            newPool.name = namePool;
            poolScript.Initialize(obj);
            poolData.Add(namePool, poolScript);
            namePools.Add(namePool);
        }   
    }

    public void PushToPool(GameObject obj,string namePool)
    {
        if (!poolData.ContainsKey(namePool))
        {
            CreatePool(obj, namePool);
        }

        poolData[namePool].Push(obj);
    }

    public GameObject PopFromPool(string namePool)
    {
        if (!poolData.ContainsKey(namePool))
        {
            Debug.LogError("No pool was found!!!");
            return null;
        }
        return poolData[namePool].Pop();
    }
    
}
