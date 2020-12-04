﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float gravity;
    public Vector2 velocicy;
    public LayerMask groundMask;
    public bool walk_left = true;

    public bool grounded = false;

    public enum EnemyState
    {
        walking,
        falling,
        dead
    }
    public EnemyState enemyState = EnemyState.falling;
    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        Fall();
    }
    void OnBecameVisible()
    {
        enabled = true;
    }
    void Update()
    {
        EnemyPositionUpdate();
    }


    void EnemyPositionUpdate()
    {
        if (enemyState != EnemyState.dead)
        {
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;

            if (enemyState == EnemyState.falling)
            {
                pos.y += velocicy.y * Time.deltaTime;
                velocicy.y -= gravity * Time.deltaTime;
            }
            if (enemyState == EnemyState.walking)
            {
                if (walk_left)
                {
                    pos.x -= velocicy.x * Time.deltaTime;
                    scale.x = -1;
                }
                else
                {
                    pos.x += velocicy.x * Time.deltaTime;
                    scale.x = 1;
                }
            }

            if (velocicy.y <= 0)
                pos = CheckGround(pos);

            transform.localPosition = pos;
            transform.localScale = scale;
        }
    }

    Vector3 CheckGround(Vector3 pos)
    {
        Vector2 origin_left = new Vector2(pos.x - 0.5f + 0.2f, pos.y - 0.5f);
        Vector2 origin_middle = new Vector2(pos.x , pos.y - 0.5f);
        Vector2 origin_right = new Vector2(pos.x + 0.5f - 0.2f, pos.y - 0.5f);

        RaycastHit2D ground_left = Physics2D.Raycast(origin_left, Vector2.down, velocicy.y * Time.deltaTime, groundMask);
        RaycastHit2D ground_middle = Physics2D.Raycast(origin_middle, Vector2.down, velocicy.y * Time.deltaTime, groundMask);
        RaycastHit2D ground_right = Physics2D.Raycast(origin_right, Vector2.down, velocicy.y * Time.deltaTime, groundMask);

        if (ground_left.collider != null || ground_middle.collider != null || ground_right.collider != null)
        {
            RaycastHit2D hitRay = ground_left;
            if (ground_left)
            {
                hitRay = ground_left;
            }
            else if (ground_middle)
            {
                hitRay = ground_middle;
            }
            else if (ground_right)
            {
                hitRay = ground_right;
            }
            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + 0.5f;
            enemyState = EnemyState.walking;
            grounded = true;
            velocicy.y = 0;

        }
        else
        {
            if (enemyState != EnemyState.falling)
            {
                Fall();
            }
        }
        return pos;
    }

    

    // Update is called once per frame
    

    void Fall()
    {
        velocicy.y = 0;
        enemyState = EnemyState.falling;
        grounded = false;
    }
}
