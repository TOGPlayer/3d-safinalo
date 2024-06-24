using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum FrontRear
    {
        Front,
        Rear
    }
    public AudioSource audioSource;
    public AudioClip acseleration;
    public AudioClip idle;


    Rigidbody rb;
    Vector3 massCenter;
    float moveInput;
    float steerInput;
    public float speed;
    public float brakeSpeed;
    public float steerAngle;
    public float steerSpeed;
    [Serializable]
    public struct Wheel
    {
        public GameObject wheel;
        public WheelCollider wheelCollider;
        public FrontRear frontRear;
        
    }
    public List<Wheel> wheels;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = massCenter;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        WheelAnimation();
    }
    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
        
    }
    void GetInputs()
    {
        steerInput = Input.GetAxis("Horizontal");
        moveInput = Input.GetAxis("Vertical");


    }
    void Move()
    {
        foreach (var wheel in wheels)
        {
            if(wheel.frontRear == FrontRear.Rear)
            {
                if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    audioSource.pitch = audioSource.pitch + 0.25f;
                    if(audioSource.pitch >= 2.5)
                    {
                        audioSource.pitch = 2.5f;
                    }
                    
                }
                if(wheel.wheelCollider.motorTorque == 0)
                {
                
                 audioSource.pitch = 1f;
                    
                }
               
                wheel.wheelCollider.motorTorque = moveInput * speed;
            }
            
        }
        
    }
    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if(wheel.frontRear == FrontRear.Front)
            {
                var _steerAngle = steerInput * steerSpeed * steerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 1);


            }
            
        }
    }
    void Brake()
    {
         foreach (var wheel in wheels)
        {
            if(wheel.frontRear == FrontRear.Rear)
            {
                if(Input.GetKey(KeyCode.Space))
                {
                    wheel.wheelCollider.brakeTorque = brakeSpeed;
                    audioSource.pitch = audioSource.pitch - 0.25f;
                    if(audioSource.pitch <= 1)
                    {
                        audioSource.pitch = 1f;
                    }
                    
                }
                else
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }
            }
            
            
        }
    }
    void WheelAnimation()
    {
         foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot );
            wheel.wheel.transform.position = pos;
            wheel.wheel.transform.rotation = rot;


        }
    }
}
