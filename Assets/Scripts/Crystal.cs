using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private PoolType poolType; 
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private Transform fuelCollectVFX; 
    private float speedModifier; 
    private PlatformController platformController;


    public void CrystalSetup(PlatformController platformController)
    {
        this.platformController = platformController;
    }

   
    // Update is called once per frame
    void Update()
    {
        speedModifier = platformController.GetCrystalSpeedModifier(); 
        transform.position += speedModifier * Time.deltaTime * new Vector3(-moveSpeed, 0, 0);  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            switch (poolType)
            {
                case PoolType.Crystal1:
                    break;
                case PoolType.Crystal2:
                    break;
                case PoolType.Crystal3:
                    break;
                case PoolType.FuelCell:
                    platformController.RemoveItems(this.transform);
                    Debug.Log("Fuel Collected");
                    break;
                case PoolType.Shield:
                    platformController.RemoveItems(this.transform);
                    break;
                default:
                    break;
            }
        }
    }
}
