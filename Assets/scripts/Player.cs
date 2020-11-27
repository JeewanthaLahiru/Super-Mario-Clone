using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 velocity;
    public LayerMask wallMask;

    private bool walk, walk_left, walk_right, jump;
    // Start is called before the first frame update
    void Start()
    {
        
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
            if (walk_left)
            {
                pos.x -= velocity.x * Time.deltaTime;
                scale.x = -1;
            }
            if (walk_right)
            {
                pos.x += velocity.x * Time.deltaTime;
                scale.x = 1;
            }

            pos = checkWallRays(pos, scale.x);
        }
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
}
