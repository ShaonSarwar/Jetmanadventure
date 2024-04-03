using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Crystal1,
    Crystal2,
    Crystal3,
    FuelCell, 
    Shield
}
public class CrystalPool : MonoBehaviour
{
   
    // Prefabs 
    [SerializeField] private Transform crystal1TransformPrefab;
    [SerializeField] private Transform crystal2TransformPrefab;
    [SerializeField] private Transform crystal3TransformPrefab;
    [SerializeField] private Transform fuelCellTransformPrefab;
    [SerializeField] private Transform sheildTransformPrefab;

    // Holders 
    [SerializeField] private Transform crystal1Holder;
    [SerializeField] private Transform crystal2Holder;
    [SerializeField] private Transform crystal3Holder;
    [SerializeField] private Transform fuelCellHolder;
    [SerializeField] private Transform shieldHolder;


    // Pool Amount
    [SerializeField] private int poolAmount;

    // Pool Objects  
    private Transform crystal1;
    private Transform crystal2;
    private Transform crystal3;
    private Transform fuelCell;
    private Transform shield;

    // Queues 
    [SerializeField] private Queue<Transform> crystal1Queue = new Queue<Transform>();
    [SerializeField] private Queue<Transform> crystal2Queue = new Queue<Transform>();
    [SerializeField] private Queue<Transform> crystal3Queue = new Queue<Transform>();
    [SerializeField] private Queue<Transform> fuelCellQueue = new Queue<Transform>();
    [SerializeField] private Queue<Transform> shieldQueue = new Queue<Transform>();

    private void Awake()
    {
        crystal1Queue.Clear();
        crystal2Queue.Clear();
        crystal3Queue.Clear();
        fuelCellQueue.Clear();
        shieldQueue.Clear(); 

        ConfigurePool(PoolType.Crystal1, crystal1TransformPrefab);
        ConfigurePool(PoolType.Crystal2, crystal2TransformPrefab);
        ConfigurePool(PoolType.Crystal3, crystal3TransformPrefab);
        ConfigurePool(PoolType.FuelCell, fuelCellTransformPrefab);
        ConfigurePool(PoolType.Shield, sheildTransformPrefab); 
    }

    private void ConfigurePool(PoolType poolType, Transform objectPrefab)
    {
        switch (poolType)
        {
            case PoolType.Crystal1:
                ConfigureObject(objectPrefab, poolAmount, crystal1Holder, crystal1Queue);
                crystal1 = objectPrefab; 
                break;
            case PoolType.Crystal2:
                ConfigureObject(objectPrefab, poolAmount, crystal2Holder, crystal2Queue);
                crystal3 = objectPrefab;
                break;
            case PoolType.Crystal3:
                ConfigureObject(objectPrefab, poolAmount, crystal3Holder, crystal3Queue);
                crystal3 = objectPrefab;
                break;
            case PoolType.FuelCell:
                ConfigureObject(objectPrefab, poolAmount, fuelCellHolder, fuelCellQueue);
                fuelCell = objectPrefab; 
                break;
            case PoolType.Shield:
                ConfigureObject(objectPrefab, poolAmount, shieldHolder, shieldQueue);
                shield = objectPrefab;
                break; 
            default:
                break;
        }
    }

    private void ConfigureObject(Transform objectPrefab, int poolAmount, Transform holder, Queue<Transform> objectQueue)
    {
        for (int i = 0; i < poolAmount; i++)
        {
            Transform objectToPool = Instantiate(objectPrefab);
            objectToPool.gameObject.SetActive(false);
            objectToPool.SetParent(holder);
            objectQueue.Enqueue(objectToPool); 
        }
    }

    public Transform GetPooledObject(PoolType poolType, Vector3 position, Quaternion quaternion)
    {
        switch (poolType)
        {
            case PoolType.Crystal1:
                return GetObject(crystal1Queue, position, quaternion); 
            case PoolType.Crystal2:
                return GetObject(crystal2Queue, position, quaternion);
            case PoolType.Crystal3:
                return GetObject(crystal3Queue, position, quaternion);
            case PoolType.FuelCell:
                return GetObject(fuelCellQueue, position, quaternion);
            case PoolType.Shield:
                return GetObject(shieldQueue, position, quaternion); 
            default:
                break;
        }
        return null; 
    }

    private Transform GetObject(Queue<Transform> queue, Vector3 position, Quaternion quaternion)
    {
        if (queue.Count > 0)
        {
            Transform pooledObject = queue.Dequeue();
            pooledObject.gameObject.SetActive(true);
            pooledObject.position = position;
            pooledObject.rotation = quaternion;
            return pooledObject; 
        }
        return null; 
    }

    public void ReturnObjectToPool(PoolType poolType, Transform returnObject)
    {
        if (returnObject.gameObject.activeInHierarchy)
        {
            returnObject.gameObject.SetActive(false); 
        }

        switch (poolType)
        {
            case PoolType.Crystal1:
                crystal1Queue.Enqueue(returnObject); 
                break;
            case PoolType.Crystal2:
                crystal2Queue.Enqueue(returnObject); 
                break;
            case PoolType.Crystal3:
                crystal3Queue.Enqueue(returnObject); 
                break;
            case PoolType.FuelCell:
                fuelCellQueue.Enqueue(returnObject); 
                break;
            case PoolType.Shield:
                shieldQueue.Enqueue(returnObject);
                break; 
            default:
                break;
        }
    }
}
