using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    public float MoveDistance = 2.75f;
    public float duration = 0.2f;
    private bool isMoving = false;
    public int playerPosition = 1;

    Rigidbody r;
    Vector3 defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        r.freezeRotation = true;
        r.useGravity = false;
        defaultScale = transform.localScale;
    }


    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.D) && playerPosition < 2)
            {
                StartCoroutine(MoveOverTime(new Vector3(MoveDistance, 0, 0), duration));
                playerPosition++;
            }

            if (Input.GetKeyDown(KeyCode.A) && playerPosition > 0)
            {
                StartCoroutine(MoveOverTime(new Vector3(-MoveDistance, 0, 0), duration));
                playerPosition--;
            }
        }
    }


    void FixedUpdate()
    {
        // Set the player's velocity to zero in each FixedUpdate() call to remove gravity effect
        r.velocity = Vector3.zero;
    }


    private IEnumerator MoveOverTime(Vector3 targetOffset, float time)
    {
        isMoving = true;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + targetOffset;
        float startTime = Time.time;

        while (Time.time - startTime < time)
        {
            float elapsedTime = (Time.time - startTime) / time;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            yield return null;
        }

        // Ensure the final position is exactly equal to the target position
        transform.position = targetPosition;
        isMoving = false;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            //print("GameOver!");
            _GroundGenerator.instance.gameOver = true;
        }
    }
}