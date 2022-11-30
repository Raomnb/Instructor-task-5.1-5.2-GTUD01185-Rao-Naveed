using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;


public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D playerRb;
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    bool animationActive = false;
    bool animationIdleActive = true;
    bool isOnGround = true;
    public bool isOnladder = false;
    private Collider2D boxCollider;
    Vector2 initialPosition;
    bool isDead = false;
    bool climbing = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // get gamemanager component
        initialPosition = transform.position; // store start position as initial position
        playerRb = GetComponent<Rigidbody2D>(); // get rigidbody component of player
        skeletonAnimation = GetComponent<SkeletonAnimation>(); // get animation of player
        spineAnimationState = skeletonAnimation.AnimationState; // get animation states of player
        skeleton = skeletonAnimation.Skeleton;
        spineAnimationState.SetAnimation(0, "idle_3", true); // set idle animation of player
        boxCollider = GetComponent<BoxCollider2D>(); // get box collider of player

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // get left right as horizontal input 
        float verticalInput = Input.GetAxis("Vertical"); // get up down as vertical input

        if (horizontalInput > 0 && !isDead && !gameManager.gameOver) // if right key is pressed player is not dead and game is not over then enter this if condition
        {
            if (!animationActive) 
            {
                spineAnimationState.SetAnimation(0, "run_shield", true);// if running animation is not active activate running animation
                animationActive = true;
                transform.rotation = new Quaternion(0, 0, 0, 1); // set rotation of player in right direction
                animationIdleActive = false;
            }

            transform.Translate(Vector3.right * 5 * Time.deltaTime); // move player forward

        }
        else if (horizontalInput < 0 && !isDead && !gameManager.gameOver) // if left key is pressed player is not dead and game is not over then enter this if condition
        {
            if (!animationActive)
            {
                spineAnimationState.SetAnimation(0, "run_shield", true);// if running animation is not active activate running animation
                animationActive = true;
                transform.rotation = new Quaternion(0, 180, 0, 1); //set rotation of player in left direction
                animationIdleActive = false;
            }

            transform.Translate(Vector3.right * 5 * Time.deltaTime); // move player forwards
        }
        else
        {
            if (!animationIdleActive && !isDead)
            {
                spineAnimationState.SetAnimation(0, "idle_3", true);// if animation is not idle set idle animation
                animationActive = false;
                animationIdleActive = true;
            }

        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround &&!climbing && !isDead && !gameManager.gameOver) //if space is pressed and player is on ground not climbing and is not dead and game is not over then jump
        {
            playerRb.AddForce(Vector3.up * 10, ForceMode2D.Impulse); // add force to player in upwards direction to jump
            isOnGround = false;
        }
        if(verticalInput>0 && isOnladder && !isDead && !gameManager.gameOver) // if up key is pressed and player is at ladder and is not dead and game is not over climb stairs
        {
            playerRb.gravityScale = 0; // set gravity off so player can climb stairs
            boxCollider.isTrigger = true;  // set boxCollider to trigger so it can pass floor of upper layer
            transform.Translate(Vector3.up * 5 * Time.deltaTime); // move player upwards on stairs
            climbing = true; // set climbing to true 
        }
        else
        {
            playerRb.gravityScale = 2; // set gravity to 2 after ladder is climbed
            boxCollider.isTrigger = false; // make box collider a collider from trigger so player can walk on floor
            climbing = false; // set climbing state false
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true; // if player is on ground set is ground to true
        }
        if(collision.gameObject.CompareTag("Barrel"))
        {
            Destroy(collision.gameObject); // if barrel colides with player destroy barrel
            StartCoroutine(PlayerDeath()); // start player death co routine
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ladder"))
        {
            isOnladder = true; // if trigger is with ladder set isonladder true so player can climb ladder
        }
        if (collision.gameObject.CompareTag("Barrel") && !isOnGround)
        {
            gameManager.score += 100; // if collision is with barrels outer collider set to trigger and player is in air add score of passing barrel
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isOnladder = false; // on exiting trigger set on ladder to false
        }
       

    }
    IEnumerator PlayerDeath()
    {
        isDead = true; // set is dead true so player can not move
        spineAnimationState.SetAnimation(0, "dead", false); // run death animation
        yield return new WaitForSeconds(1); // wait 1 second for death animation to complete
        transform.position = initialPosition; // reset player position to initial position
        spineAnimationState.SetAnimation(0, "idle_3", true); // set player animation to idle
        isDead = false;
        animationIdleActive = true;
        animationActive = false;
        gameManager.lives--; // decrease one life
    }
}

    
