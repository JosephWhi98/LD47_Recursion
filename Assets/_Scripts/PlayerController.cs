using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] Rigidbody m_rigidbody;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform cameraT;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip windClip;


    public bool isRunning;

    public GameObject gun;

    private void Awake()
    {
        if (!instance)
            instance = this; 
    }

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void PickUpGun()
    {
        gun.SetActive(true);
    }

    private void Update()
    {

        if (!ProgressManager.instance.lockPlayerMovement)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                m_rigidbody.AddForce(transform.up * 5f, ForceMode.Impulse);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 0.3f;
                isRunning = true;
            }
            else
            {
                moveSpeed = 0.2f;
                isRunning = false;
            }
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (!ProgressManager.instance.lockPlayerMovement)
        {
            Move(h, v);
        }
    }


    private void Move(float h, float v)
    {
        Vector3 fwd = cameraT.transform.forward;
        fwd.y = 0;

        Vector3 right = cameraT.transform.right;
        right.y = 0;

        if(m_rigidbody)
            m_rigidbody.MovePosition(transform.position + (fwd * v * moveSpeed) + (right * h * moveSpeed));
    }
}
