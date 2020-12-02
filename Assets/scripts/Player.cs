using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpVelocity;
    public Vector2 velocity;
    public float gravity;
    public LayerMask wallMask;
    public LayerMask floorMask;

    private bool walk, walk_left, walk_right, jump;
    // Start is called before the first frame update

    public enum PlayerState
    {
        jumping,
        idle,
        walking
    }
    public PlayerState playerState = PlayerState.idle;
    private bool grounded = false;

    void Start()
    {
        //Fall();
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerInput();
        updatePlayerPosition();

    }

    //Update player with the input
    void updatePlayerPosition()
    {
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;

        if (walk)
        {
            if (walk_right)
            {
                pos.x += velocity.x * Time.deltaTime;
                scale.x = 1;
            }
            if (walk_left)
            {
                pos.x -= velocity.x * Time.deltaTime;
                scale.x = -1;
            }

            pos = checkWallRays(pos, scale.x);
        }

        /*if (jump && playerState != PlayerState.jumping)
        {
            playerState = PlayerState.jumping;
            velocity = new Vector2(velocity.x, jumpVelocity);
        }

        if (playerState == PlayerState.jumping)
        {
            pos.y += velocity.y * Time.deltaTime;
            velocity.y -= gravity * Time.deltaTime;
        }

        if (velocity.y <= 0)
        {
            pos = checkFloorRays(pos);
        }*/

        transform.localPosition = pos;
        transform.localScale = scale;

        
    }
    
    //check player input method 
    void checkPlayerInput()
    {
        bool input_left = Input.GetKey(KeyCode.LeftArrow);
        bool input_right = Input.GetKey(KeyCode.RightArrow);
        bool input_space = Input.GetKeyDown(KeyCode.Space);

        walk = input_left || input_right;
        walk_left = input_left && !input_right;
        walk_right = !input_left && input_right;

        jump = input_space;
    }

    Vector3 checkWallRays (Vector3 pos, float direction)
    {
        Vector2 playerTop = new Vector2(pos.x + direction * .4f, pos.y + 1f - 0.2f);
        Vector2 playerMid = new Vector2(pos.x + direction * .4f, pos.y );
        Vector2 playerBottom = new Vector2(pos.x + direction * .4f, pos.y - 1f);

        RaycastHit2D wallTop = Physics2D.Raycast(playerTop, new Vector2(direction,0),velocity.x*Time.deltaTime,wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast(playerMid, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast(playerBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
        {
            pos.x -= direction * Time.deltaTime * velocity.x;
        }

        return pos;

    }

    Vector3 checkFloorRays(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y - 1f);
        Vector2 originMiddle = new Vector2(pos.x , pos.y - 1f);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y - 1f);

        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
        {
            RaycastHit2D hitRay = floorRight;
            if (floorLeft)
            {
                hitRay = floorLeft;
            }
            else if (floorMiddle)
            {
                hitRay = floorMiddle;
            }
            else if (floorRight)
            {
                hitRay = floorRight;
            }

            playerState = PlayerState.idle;
            grounded = true;
            velocity.y = 0;

            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + 1;
        }
        return pos;
    }

    void Fall()
    {
        velocity.y = 0;
        playerState = PlayerState.jumping;
        grounded = false;
    }
}
