using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class RigidbodyController : MonoBehaviour
{

    [SerializeField] private float maxJumpSpeed = 10f;
    [SerializeField] private float minJumpSpeed = 5f;
    [SerializeField] private float jumpClampValue = 15f;
    [SerializeField] private float jumpSpeedGainModifier = 30f;
    [SerializeField] private GameObject countdown;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private Text tapText; 
    private Rigidbody2D rb;
    private float jumpForce;
    private bool isTouched;
    private ParticleSystem jumpBoosterPS;
    private IPlayerProperties playerProperties;
    private float playerXcentreValue = 0f; 
    // Start is called before the first frame update
    void Start()
    {
        playerProperties = GetComponent<IPlayerProperties>(); 
        rb = GetComponent<Rigidbody2D>();
        jumpBoosterPS = transform.Find("JumpBooster").GetComponent<ParticleSystem>();
        StartCoroutine(StarDelay());      
    }

    // Update is called once per frame
    void Update()
    {
        if (playerProperties.IsGameOver) return; 
        GatherInputs();
    }

    private void FixedUpdate()
    {
        if (playerProperties.IsGameOver)
        {
            rb.isKinematic = true;
            return; 
        }
        PlayerMove();

        if (transform.position.x < -1.5f || transform.position.x > 1.5f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerXcentreValue, transform.position.y, transform.position.z), Time.fixedDeltaTime);
        }
    }

    private void GatherInputs()
    {   
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        jumpForce = 0f;
                        int tc = touch.tapCount;
                     
                        jumpForce = jumpClampValue * jumpSpeedGainModifier * Time.deltaTime;
                        if (tc == 2)
                        {
                            jumpForce = jumpForce * tc * 250f * Time.deltaTime; 
                            Debug.Log(tc);
                            tapText.text = tc.ToString();
                        }
                        else if (tc > 2)
                        {
                            jumpForce = jumpForce * tc * 500f * Time.deltaTime; 
                        }
                       
                        if (jumpForce > maxJumpSpeed) jumpForce = maxJumpSpeed;
                      
                        isTouched = true;
                        jumpBoosterPS.Play();
                        jumpSound.Play(); 
                        break;
                    case TouchPhase.Moved:
                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Ended:
                        break;
                    case TouchPhase.Canceled:
                        break;
                    default:
                        break;
                }
            }          
        }     
    }

    private void PlayerMove()
    {
        if (!isTouched) return;
        float jp = Mathf.Clamp(jumpForce, minJumpSpeed, maxJumpSpeed);
        rb.velocity = Vector2.up * jp; 
        isTouched = false;       
    }

    private IEnumerator StarDelay()
    {
        Time.timeScale = 0;
        float pauseTime = Time.realtimeSinceStartup + 2.5f;
        while (Time.realtimeSinceStartup < pauseTime)
        {
            yield return 0; 
        }
        countdown.SetActive(false); 
        Time.timeScale = 1.0f; 
    }
}
