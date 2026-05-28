using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float playerHeight = 2f;

    [SerializeField] private Transform orientation;
    
    [Header("Movement")] 
    public float moveSpeed = 6f;
    private float movementMultiplier = 10f;
    [SerializeField] private float airMultiplier = 0.2f;

    [Header("Sprinting")] 
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float acceleration = 10f;
    
    [Header("Jumping")] 
    public float jumpForce = 5f;

    [Header("Keybinds")] 
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    private float horizontalMovement;
    private float verticalMovement;

    [Header("Ground Detection")] 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    private float groundDistance = 0.4f;

    [Header("Drag")]
    private float groundDrag = 6f;
    private float airDrag = 2f;
    
    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;

    private Rigidbody rb;

    private RaycastHit slopeHit;
    
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
                return true;
            return false;
        }

        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
    }
    
    private void ControlDrag()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if(isGrounded && !OnSlope())
            rb.AddForce(moveDirection.normalized * (moveSpeed * movementMultiplier), ForceMode.Acceleration);
        else if(isGrounded && OnSlope())
            rb.AddForce(slopeMoveDirection.normalized * (moveSpeed * movementMultiplier), ForceMode.Acceleration);
        else
            rb.AddForce(moveDirection.normalized * (moveSpeed * movementMultiplier * airMultiplier), ForceMode.Acceleration);
    }
    
    private bool isDead = false;
    private Coroutine dangerCoroutine;
    private GameObject dangerCanvasObj;
    private AudioClip warningSound;
    private AudioSource warningAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.gameObject.name.Contains("Border"))
        {
            isDead = true;
            
            // Play warning sound
            if (warningSound == null)
                warningSound = Resources.Load<AudioClip>("warning");
            if (warningSound != null)
            {
                warningAudioSource = gameObject.AddComponent<AudioSource>();
                warningAudioSource.clip = warningSound;
                warningAudioSource.loop = true;
                warningAudioSource.Play();
            }
            
            dangerCoroutine = StartCoroutine(ShowDangerAndRestart());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player came back inside the play area — cancel the restart
        if (isDead && other.gameObject.name.Contains("Border"))
        {
            isDead = false;

            if (dangerCoroutine != null)
            {
                StopCoroutine(dangerCoroutine);
                dangerCoroutine = null;
            }

            if (dangerCanvasObj != null)
            {
                Destroy(dangerCanvasObj);
                dangerCanvasObj = null;
            }

            // Stop warning sound
            if (warningAudioSource != null)
            {
                warningAudioSource.Stop();
                Destroy(warningAudioSource);
                warningAudioSource = null;
            }
        }
    }

    private IEnumerator ShowDangerAndRestart()
    {
        // Create overlay canvas
        dangerCanvasObj = new GameObject("DangerCanvas");
        Canvas canvas = dangerCanvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        dangerCanvasObj.AddComponent<CanvasScaler>();

        // Red blinking overlay (fullscreen)
        GameObject redObj = new GameObject("RedFlash");
        redObj.transform.SetParent(dangerCanvasObj.transform, false);
        Image redImg = redObj.AddComponent<Image>();
        redImg.color = new Color(1f, 0f, 0f, 0f); // start transparent
        RectTransform redRect = redObj.GetComponent<RectTransform>();
        redRect.anchorMin = Vector2.zero;
        redRect.anchorMax = Vector2.one;
        redRect.offsetMin = Vector2.zero;
        redRect.offsetMax = Vector2.zero;

        // Danger zone image (smaller, centered)
        Texture2D dangerTexture = Resources.Load<Texture2D>("danger_zone");
        if (dangerTexture != null)
        {
            GameObject imgObj = new GameObject("DangerImage");
            imgObj.transform.SetParent(dangerCanvasObj.transform, false);
            Image img = imgObj.AddComponent<Image>();
            img.sprite = Sprite.Create(dangerTexture, new Rect(0, 0, dangerTexture.width, dangerTexture.height), new Vector2(0.5f, 0.5f));
            img.preserveAspect = true;

            RectTransform rect = imgObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(500f, 300f);
            rect.anchoredPosition = Vector2.zero;
        }

        // Blink red for 3 seconds then restart
        float elapsed = 0f;
        float blinkSpeed = 6f;
        while (elapsed < 3f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Abs(Mathf.Sin(elapsed * blinkSpeed)) * 0.45f;
            redImg.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        // Restart game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
