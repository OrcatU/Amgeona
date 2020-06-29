using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    public int[] howManyHit = { 4, 0, 0, 0 };
    public int[] howCanMove = { 1, 2, 0, 0 };
    public RaycastHit2D[][] hit = new RaycastHit2D[3][];
    public bool isDebug;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            hit[i] = new RaycastHit2D[howManyHit[i]];
        }
        transform.position = new Vector2(0, 0);
        isDebug = false;
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(100, 0));
            float leftX = transform.position.x - (gameObject.GetComponent<BoxCollider2D>().size.x * transform.localScale.x / 2);
            float rightX = transform.position.x + (gameObject.GetComponent<BoxCollider2D>().size.x * transform.localScale.x / 2);
            float downY = transform.position.y - (gameObject.GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 2);
            float upY = transform.position.y + (gameObject.GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 2);
            for (int i = 0; i < howManyHit[0]; i += 1)
            {
                float thatX = leftX;
                float minusX = rightX - leftX;
                thatX += minusX * (i / (float)howManyHit[0]);
                float thatY = downY - 0.1F;
                if (isDebug) { Instantiate(GameObject.Find("Dot"), new Vector2(thatX, thatY), new Quaternion()); }
                hit[0][i] = Physics2D.Raycast(new Vector2(thatX, thatY), transform.up * -1, 0.000001F);
                if (isDebug)
                {
                    try
                    {
                        Debug.Log(hit[0][i].collider.name + ": " + i.ToString());
                    }
                    catch (NullReferenceException)
                    {
                        Debug.Log("Null" + ": " + i.ToString());
                    }
                }
                try
                {
                    if (hit[0][i].collider.CompareTag("Floor"))
                    {

                    }
                }
                catch (NullReferenceException) { }

            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-100, 0));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            bool canJump = Check(0);
            if (canJump)
            {
                rb.AddForce(new Vector2(0, 2000), ForceMode2D.Force);
            }
        }
        if (rb.velocity.x > 15)
        {
            rb.velocity = new Vector2(15, rb.velocity.y);
        }
        if (rb.velocity.x < -15)
        {
            rb.velocity = new Vector2(-15, rb.velocity.y);
        }
        if (rb.velocity.x > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        if (rb.velocity.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
    }

    bool Check(int intDirection)
    {
        // 기본 변수 설정
        int canMove = 0;
        float leftX = transform.position.x - (gameObject.GetComponent<BoxCollider2D>().size.x * transform.localScale.x / 2);
        float rightX = transform.position.x + (gameObject.GetComponent<BoxCollider2D>().size.x * transform.localScale.x / 2);
        float downY = transform.position.y - (gameObject.GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 2);
        float upY = transform.position.y + (gameObject.GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 2);
        Vector2 direction = new Vector2();
        if (intDirection == 0)
        {
            direction = transform.up * -1;
        }
        for (int i = 0; i < howManyHit[intDirection]; i += 1)
        { 
            float thatX = leftX;
            float minusX = rightX - leftX;
            thatX += minusX * (i / (float)howManyHit[intDirection]);
            float thatY = downY;
            float minusY = upY - downY;
            thatY += minusY * (i / (float)howManyHit[intDirection]);
            float[, ] where = new float[4, 2] { {thatX, downY-0.1F},{leftX, thatY},{ rightX, thatY},{thatX, upY } };
            hit[intDirection][i] = Physics2D.Raycast(new Vector2(where[intDirection, 0], where[intDirection, 1]), direction, 0.000001F);
            try
            {
                if (hit[intDirection][i].collider.CompareTag("Floor"))
                {
                    canMove += 1;
                }
            }
            catch (NullReferenceException) { }

        }
        Debug.Log(canMove);
        return canMove >= howCanMove[intDirection];
    }
}
