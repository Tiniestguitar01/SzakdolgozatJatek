using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotateSpeed = 60f;
    public float maxSpeed = 20f;
    public float accelerationRate = 0.3f;
    public float stoppingRate = 1.5f;

    [SerializeField]
    float acceleration = 0f;
    bool isMoving = false;
    bool isTurning = false;
    [SerializeField]
    int direction = 1;
    bool isGrounded = false;
    bool isStopping = false;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            direction = 1;
            isMoving = true;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            direction = -1;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isStopping = true;
        }
        else
        {
            isStopping = false;
        }


        if (isGrounded && rb.velocity.magnitude > 1f)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Rotate(Vector3.down);
                isTurning = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Rotate(Vector3.up);
                isTurning = true;
            }
            else
            {
                isTurning = false;
            }
        }
        else
        {
            isTurning = false;
        }

        if (isMoving && isGrounded)
        {
            if(isStopping == false)
            {
                acceleration += Time.deltaTime * accelerationRate * direction;
                acceleration = Mathf.Clamp(acceleration, -1f, 1f);
            }
            else
            {
                acceleration -= Mathf.Sign(acceleration) * Time.deltaTime * stoppingRate;

                if (Mathf.Abs(acceleration) < 0.1f && Mathf.Abs(acceleration) > -0.1f)
                {
                    acceleration = 0;
                }

            }

            Move(transform.forward);
        }
        else
        {
            acceleration = Mathf.Sign(acceleration) * rb.velocity.magnitude / maxSpeed;

            if (Mathf.Abs(acceleration) < 0.1f && Mathf.Abs(acceleration) > -0.1f)
            {
                acceleration = 0;
            }
        }

    }

    private void FixedUpdate()
    {
        isGrounded = GetGrounded();
        /*if(!GetGrounded())
        {
            StartCoroutine("RespawnText");
        }
        else
        {
            StopCoroutine("RespawnText");
            UIManager.Instance.respawnText.text = "";
        }*/
    }

    public void Move(Vector3 direction)
    {
        if(rb.velocity.magnitude < maxSpeed)
        {
            rb.velocity = direction * moveSpeed * acceleration;
        }

    }

    public void Rotate(Vector3 direction)
    {
        if(acceleration > 0.5f)
        {
            transform.Rotate(direction, rotateSpeed * Time.deltaTime / acceleration);
        }
        else
        {
            transform.Rotate(direction, rotateSpeed * Time.deltaTime);
        }
    }

    public bool GetIsTurning()
    {
        return isTurning;
    }

    public bool GetIsStopping()
    {
        return isStopping;
    }

    public bool GetGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.3f))
        {
            return true;
        }
        return false;
    }

    /*IEnumerator RespawnText()
    {
        yield return new WaitForSeconds(10f);
        UIManager.Instance.respawnText.text = "Na látod erre is gondoltam ám fiam.\n 'R' gombot nyomd meg szépen (mint R3NATO)";
    }*/
}
