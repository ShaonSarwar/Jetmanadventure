using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformController : MonoBehaviour, IGameProperties
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private CrystalPool crystalPool;
    [SerializeField] private float spawnTimer = 2f;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    [SerializeField] private Transform playerCollision; 

    private List<Transform> spawnedItemList;
    private Dictionary<Transform, SpawnItem> spawnDic; 
    private float timer;
    private int timeDivider = 10; 
    private float crystalSpeedModifier = 1f;
    private bool canSpeedGain;
    private int itemDivider = 5;
    private IPlayerProperties playerProperties; 

    public float TimeSpend { get; private set; }

    public int Point1ItemCounter { get; private set; }

    public int Point2ItemCounter { get; private set; }

    public event EventHandler OnGameScoreChanged; 

    // Start is called before the first frame update
    void Start()
    {
        playerProperties = playerCollision.GetComponent<IPlayerProperties>(); 
        spawnedItemList = new List<Transform>();
        timer = 0.1f;
        spawnDic = new Dictionary<Transform, SpawnItem>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerProperties.IsGameOver) return; 
        TimeSpend = Time.time; 
        if (spawnedItemList.Count > 0)
        {
            for (int i = 0; i < spawnedItemList.Count; i++)
            {
                Transform spawnTransform = spawnedItemList[i];
                if (spawnTransform.position.x < endPoint.position.x)
                {
                    RemoveItems(spawnTransform);
                }
            }
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            float scaleOffset = UnityEngine.Random.Range(2.8f, 4.5f);
            float secondScaleOffset = 7.3f - scaleOffset;
            
            
            SpawnItemsOnPoint1(spawnPoint1.position, spawnPoint1.rotation, scaleOffset);
            SpawnItemsOnPoint2(spawnPoint2.position, spawnPoint2.rotation, UnityEngine.Random.Range(2.8f, secondScaleOffset)); 
            timer = spawnTimer; 
        }

        if (((int)TimeSpend % timeDivider) != 0)
        {
            canSpeedGain = true;
        }
        if (((int)TimeSpend % timeDivider) == 0 && canSpeedGain)
        {
            crystalSpeedModifier += 0.1f;
            canSpeedGain = false;
            if (spawnTimer >= 2f)
            {
                spawnTimer -= 0.1f;
            }           
        }
    }

    private void SpawnItemsOnPoint1(Vector3 position, Quaternion quaternion, float size)
    {
        float horizontalPositionOffsetPoint1 = UnityEngine.Random.Range(1f, 20f);
        position += new Vector3(-horizontalPositionOffsetPoint1, 0, 0); 
        bool canSizeChange; 
        PoolType poolType; 
        if (Point1ItemCounter % itemDivider == 0)
        {
            poolType = (PoolType)UnityEngine.Random.Range(3, Enum.GetValues(typeof(PoolType)).Length);
            canSizeChange = false;
        }
        else
        {
            poolType = (PoolType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PoolType)).Length - 2);
            canSizeChange = true;
        }
        quaternion.eulerAngles = new Vector3(0, 0, 180f);
        Transform spwanObj = crystalPool.GetPooledObject(poolType, position, quaternion);
        if (canSizeChange)
        {
            spwanObj.localScale = new Vector3(size, size, size);
        }
        spwanObj.GetComponent<Crystal>().CrystalSetup(this);
        spawnedItemList.Add(spwanObj);
        SpawnItem spawnItem = new SpawnItem(poolType);
        spawnDic[spwanObj] = spawnItem;
        Point1ItemCounter += 1;
       
    }

    private void SpawnItemsOnPoint2(Vector3 position, Quaternion quaternion, float size)
    {
        float horizontalPositionOffsetPoint2 = UnityEngine.Random.Range(1f, 10f);
        position += new Vector3(-horizontalPositionOffsetPoint2, 0, 0);
        bool canSizeChange; 
        PoolType poolType;
        if (Point2ItemCounter % itemDivider == 0)
        {
            poolType = (PoolType)UnityEngine.Random.Range(3, Enum.GetValues(typeof(PoolType)).Length);
            canSizeChange = false; 
        }
        else
        {
            poolType = (PoolType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PoolType)).Length - 2);
            canSizeChange = true; 
        }

        Transform spwanObj = crystalPool.GetPooledObject(poolType, position, quaternion);
        if (canSizeChange)
        {
            spwanObj.localScale = new Vector3(size, size, size);
        }
        spwanObj.GetComponent<Crystal>().CrystalSetup(this);
        spawnedItemList.Add(spwanObj);
        SpawnItem spawnItem = new SpawnItem(poolType);
        spawnDic[spwanObj] = spawnItem;
        Point2ItemCounter += 1;
    }

    public void RemoveItems(Transform spawnTransform)
    {
        spawnedItemList.Remove(spawnTransform);
        if (spawnDic.ContainsKey(spawnTransform))
        {
            SpawnItem spawnItem = spawnDic[spawnTransform];
            PoolType poolType = spawnItem.GetPoolType();
            crystalPool.ReturnObjectToPool(poolType, spawnTransform);
            spawnDic.Remove(spawnTransform);
            OnGameScoreChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Key not found");
        }
    }

    public float GetCrystalSpeedModifier() { return crystalSpeedModifier; }
}
