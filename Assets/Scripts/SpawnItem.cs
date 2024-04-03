using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem 
{
    public PoolType poolType; 

    public SpawnItem(PoolType poolType)
    {
        this.poolType = poolType; 
    }

    public PoolType GetPoolType() { return poolType; }
}
