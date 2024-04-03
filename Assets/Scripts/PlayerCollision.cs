using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI; 

public class PlayerCollision : MonoBehaviour, IPlayerProperties
{
    [SerializeField] private PlatformController platformController; 
    [SerializeField] private Transform shieldPS;
    [SerializeField] private float initialfuelAmount = 50f;
    [SerializeField] private float maxFuelValue = 100f;
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private Image fuelPanel; 
    [SerializeField] private ParticleSystem fuelCollect;
    [SerializeField] private AudioSource collectSound;
    [SerializeField] private AudioSource crystalHitSound; 

    public int FuelCellCount { get; set; }

    public int ShieldCount { get; set; }

    public bool IsShielded { get; set; }

    public bool IsGameOver { get; set; }

    private float shieldTime = 10f;
    private float currentShieldTime;
    private float fuelLeft; 

    private ParticleSystem crystalBlast;
    public event EventHandler OnPlayerScoreChanged;
    public event EventHandler OnPlayerDie;
    public event EventHandler OnFuelFinished; 

    private void Awake()
    {
        crystalBlast = transform.Find("BlastWithCrystal").GetComponent<ParticleSystem>(); 
        currentShieldTime = shieldTime;
        fuelLeft = initialfuelAmount; 
        shieldPS.gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameOver) return; 

        if (IsShielded)
        {
            shieldPS.gameObject.SetActive(true);
            shieldPS.GetComponent<ParticleSystem>().Play();
            currentShieldTime -= Time.deltaTime;
            if (currentShieldTime <= 0)
            {
                shieldPS.GetComponent<ParticleSystem>().Stop();
                shieldPS.gameObject.SetActive(false); 
                IsShielded = false;
                currentShieldTime = shieldTime; 
            }
        }

        fuelLeft -= Time.deltaTime;
        fuelText.text = Mathf.FloorToInt(fuelLeft).ToString();
        if (fuelLeft < 20f)
        {
            fuelPanel.color = Color.red; 
        }
        else
        {
            fuelPanel.color = Color.white; 
        }
        if (fuelLeft <= 0.0f)
        {
            IsGameOver = true;
            OnFuelFinished?.Invoke(this, EventArgs.Empty); 
            OnPlayerDie?.Invoke(this, EventArgs.Empty); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            FuelCellCount += 1;
            OnPlayerScoreChanged?.Invoke(this, EventArgs.Empty);
            fuelCollect.transform.position = Vector3.Lerp(collision.transform.position, transform.position, 0.5f);  
            fuelCollect.Play();
            collectSound.Play(); 
            fuelLeft += 20f;
            if (fuelLeft > maxFuelValue) fuelLeft = 100f; 
        }
        else if (collision.gameObject.layer == 9)
        {
            ShieldCount += 1;
            if (!IsShielded) 
            {
                IsShielded = true;
                collectSound.Play(); 
                
            }
            OnPlayerScoreChanged?.Invoke(this, EventArgs.Empty);
        }
        else if (collision.gameObject.layer == 7)
        {
            if (!IsShielded)
            {
                OnPlayerDie?.Invoke(this, EventArgs.Empty);
                crystalBlast.Play();
                IsGameOver = true;
                crystalHitSound.Play(); 
            }          
        }
        else if (collision.gameObject.layer == 10)
        {
            OnPlayerDie?.Invoke(this, EventArgs.Empty);
            crystalBlast.Play();
            IsGameOver = true;
        }
    }
}
