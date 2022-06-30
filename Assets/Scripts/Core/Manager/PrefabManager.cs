using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilitys;
public class PrefabManager : MonoBehaviour
{
    //NOTE:Specific for game,remove to reuse
    public readonly string ISUSEDSUBTRACTSTACK = "IsUsedSubtractStack";
    public readonly string ISUSEDADDSTACK = "IsUsedAddStack";

    public static PrefabManager Inst;
    [SerializeField]
    private GameObject isUsedAddStack;
    [SerializeField]
    private GameObject isUsedSubtractStack;

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

    private void Start()
    {
        //NOTE:Specific for game,remove to reuse
        CreatePool(isUsedSubtractStack,ISUSEDSUBTRACTSTACK,Quaternion.Euler(-90,0,0));
        CreatePool(isUsedAddStack,ISUSEDADDSTACK, Quaternion.Euler(-90, 0, 0));
    }
    public void CreatePool(GameObject obj,string namePool,Quaternion quaternion = default)
    {
        if (!poolData.ContainsKey(namePool))
        {
            GameObject newPool = Instantiate(pool, Vector3.zero, Quaternion.identity);
            Pool poolScript = newPool.GetComponent<Pool>();
            newPool.name = namePool;
            poolScript.Initialize(obj,quaternion);
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

    public GameObject PopFromPool(string namePool,GameObject obj = null)
    {
        if (!poolData.ContainsKey(namePool))
        {
            if(obj == null)
            {
                Debug.LogError("No pool was found!!!");
                return null;
            }
        }

        return poolData[namePool].Pop();
    }
    
}
