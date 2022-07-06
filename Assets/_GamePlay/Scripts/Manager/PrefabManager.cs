using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utilitys;
public class PrefabManager : MonoBehaviour
{
    //NOTE:Specific for game,remove to reuse
    public readonly string ISUSED_SUBTRACTSTACK = "IsUsedSubtractStack";
    public readonly string ISUSED_ADDSTACK = "IsUsedAddStack";
    public readonly string TALLGROUNDBLANK = "TallGroundBlank";
    public readonly string ADDSTACK = "AddStack";
    public readonly string SUBTRACKSTACK = "SubtractStack";
    public readonly string CROSS_ADDSTACK = "CrossAddStack";
    public readonly string DES_SUBTRACTSTACK = "DesSubtractStack";
    public readonly string WALLSTACK = "WallStack";
    public readonly string BRIDGE = "Bridge";
    public readonly string END_POSITION = "EndPosition";

    private readonly int INITNUMBER_POOL_OBJECT = 50;

    public static PrefabManager Inst;
    [SerializeField]
    private GameObject isUsedAddStack;
    [SerializeField]
    private GameObject isUsedSubtractStack;
    [SerializeField]
    private GameObject tallGroundBlank;
    [SerializeField]
    private GameObject addStack;
    [SerializeField]
    private GameObject subtractStack;
    [SerializeField]
    private GameObject crossAddStack;
    [SerializeField]
    private GameObject desSubtractStack;
    [SerializeField]
    private GameObject wallStack;
    [SerializeField]
    private GameObject bridge;
    [SerializeField]
    private GameObject endPosition;
    //-----
    private List<string> namePools = new List<string>();
    public List<string> NamePools => namePools;
    private void Awake()
    {
        if(Inst == null)
        {
            Inst = this;
            //NOTE:Specific for game,remove to reuse
            CreatePool(isUsedSubtractStack, ISUSED_SUBTRACTSTACK, Quaternion.Euler(-90, 0, 0));
            CreatePool(isUsedAddStack, ISUSED_ADDSTACK, Quaternion.Euler(-90, 0, 0));
            CreatePool(addStack, ADDSTACK, Quaternion.Euler(-90, 0, 0));
            CreatePool(subtractStack, SUBTRACKSTACK, Quaternion.Euler(-90, 0, 0));
            CreatePool(crossAddStack, CROSS_ADDSTACK, Quaternion.Euler(-90, 0, 0));
            CreatePool(desSubtractStack, DES_SUBTRACTSTACK, Quaternion.Euler(-90, 0, 0), 1);

            CreatePool(endPosition, END_POSITION, Quaternion.identity, 1);
            CreatePool(tallGroundBlank, TALLGROUNDBLANK, Quaternion.Euler(-90, 0, 0), INITNUMBER_POOL_OBJECT);
            CreatePool(wallStack, WALLSTACK, Quaternion.Euler(-90, 0, 0), INITNUMBER_POOL_OBJECT);
            CreatePool(bridge, BRIDGE, Quaternion.identity, INITNUMBER_POOL_OBJECT);
            return;
        }
        
        Destroy(gameObject);
    }
    public GameObject pool;
    Dictionary<string, Pool> poolData = new Dictionary<string, Pool>();

    private void Start()
    {
        
        
    }
    public void CreatePool(GameObject obj,string namePool,Quaternion quaternion = default,int numObj = 10)
    {
        if (!poolData.ContainsKey(namePool))
        {
            GameObject newPool = Instantiate(pool, Vector3.zero, Quaternion.identity);
            Pool poolScript = newPool.GetComponent<Pool>();
            newPool.name = namePool;
            poolScript.Initialize(obj,quaternion,numObj);
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
                Debug.LogError("No pool name " + namePool + " was found!!!" );
                return null;
            }
        }

        return poolData[namePool].Pop();
    }
    
}
