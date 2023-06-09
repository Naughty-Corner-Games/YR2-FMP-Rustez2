using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
   

    [Header("Stats")]
    public float PlayerHealth = 100f;
    public float Hunger = 100f;
    public float HungerMulti = 0.1f;


    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private Transform orientation;
    public Animator playerAnim;
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] private float airMultiplier = 0.2f;
    [SerializeField] private float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] public float sprintSpeed = 6f;
    [SerializeField] private float acceleration = 10f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 7f;
    [SerializeField] private float airDrag = 1f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.2f;


    [Header("Tools")]
    public Axe axe;
    public Animator ToolAnim;
    public GameObject AXE;
    public GameObject Spear;

    public bool isGrounded { get; private set; }

    [SerializeField] private InventoryManager inventory;
    private PlayerLook look;
    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;
    private Rigidbody rb;
    private RaycastHit slopeHit;
    private EnemyAI enemy;
    public ItemClass AxeClass;
    public GameObject AxePickup;
    public ItemClass Apple;



    //[SerializeField] private Animator anim;

    public float originalHeight;
    public float reducedHeight;
    public CapsuleCollider col;

    [Header("Damage")]
    public Stats enemyStats;
    public Slider healthValue;
    public Slider hungerValue;


    

    /* public void TakeDamage(Stats stats)
     {
         PlayerHealth -= stats.Damage;
         Debug.Log("Takedamage 2");
         if(PlayerHealth <= 0) {
             GameOver();
         }
     }
     */
    public void GameOver()
    {
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(1);
        
    }


    public void HungerDown()
    {
        Hunger -= HungerMulti * Time.deltaTime;
    }

    #region Movement
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
                return true;
            else
                return false;
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        look = GetComponent<PlayerLook>();
    
       // anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        healthValue.value = PlayerHealth;
        hungerValue.value = Hunger;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();
        Crouch();
        PlayerStats();
        HungerDown();
        //TakeDamage();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
            Jump();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        //Player Uses Items.
        if (Input.GetMouseButtonDown(0))
        {

            if (!(inventory.selectedItem is null))
                inventory.selectedItem.Use(this);

            if(inventory.selectedItem.itemType == ItemType.Consumable)
            {
                inventory.Remove(Apple, 1);
                Hunger += 5;
                PlayerHealth += 5;
            }

            if (inventory.selectedItem.itemType == ItemType.Tool)
            {
                ToolAnim.SetTrigger("Swing");
            }
        }

        if (inventory.selectedItem is not null && inventory.selectedItem is ToolClass)
        {


            switch (inventory.selectedItem.GetTool().toolType)
            {



                case ToolType.Spear:
                    {
                        Spear.SetActive(true);
                        AXE.SetActive(false);
                        ToolAnim.runtimeAnimatorController = inventory.selectedItem.GetTool().AnimCont;
                        break;
                    }

                case ToolType.Axe:
                    {
                        Spear.SetActive(false);
                        AXE.SetActive(true);
                        ToolAnim.runtimeAnimatorController = inventory.selectedItem.GetTool().AnimCont;
                        break;

                    }






                   
            }

        }
        else
        {
            //nothing in hand
            Spear.SetActive(false);
            AXE.SetActive(false);
            ToolAnim.runtimeAnimatorController = null;
        }


    }
   
    public void PlayerStats()
    {
        if (PlayerHealth <= 0 || Hunger <= 0)
            GameOver();
            
    }

    private void MyInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetBool("walkBob", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.SetBool("walkBob", false);
        }
    }

    private void Jump()
    {
        if (isGrounded && Time.timeScale == 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ControlSpeed()
    {
        if(Input.GetKey(sprintKey) && isGrounded)
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
        MovePlayer(moveDirection);
    }

    private void MovePlayer(Vector3 direction)
    {
        if (isGrounded && !OnSlope())
            rb.AddForce(moveDirection * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else if (isGrounded && OnSlope())
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else if (!isGrounded)
            rb.AddForce(moveDirection * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
    }
    #endregion Movement


    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            col.height = reducedHeight;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {

            col.height = originalHeight;
        }

   
    }


    
    public void TakeDamage(float damage)
    {

        PlayerHealth -= damage;
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("AxePick"))
        {
            inventory.Add(AxeClass, 1,"Axe");

            AxePickup.SetActive(false);

        }

        if (collision.gameObject.CompareTag("Kill"))
            GameOver();
    }
}