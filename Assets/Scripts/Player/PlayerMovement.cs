using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("PlayerMovement");
                    instance = instanceContainer.AddComponent<PlayerMovement>();
                }
            }
            return instance;
        }
    }
    private static PlayerMovement instance;
    Rigidbody rb;
    public float moveSpeed = 5f;
    public Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);
        if(JoyStickMovement.Instance.joyVec.x != 0 || JoyStickMovement.Instance.joyVec.y != 0)
        {
            rb.velocity = new Vector3(JoyStickMovement.Instance.joyVec.x * moveSpeed, rb.velocity.y, JoyStickMovement.Instance.joyVec.y * moveSpeed);
            rb.rotation = Quaternion.LookRotation(new Vector3(JoyStickMovement.Instance.joyVec.x, 0, JoyStickMovement.Instance.joyVec.y));
        }
    }
    // Update is called once per frame
}
