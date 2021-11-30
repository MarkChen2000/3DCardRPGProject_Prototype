using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;


    private float inputH; // horizontal input
    private float inputV; // vertical input
    private bool run;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        run = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
        {
            anim.Play("WAIT01"); // test: press key "1" to play WAIT01
        }

        if (Input.GetKeyDown("2")){
            anim.Play("DAMAGED00",-1,0f); // play DAMAGE00 animation on the beginning
        }

        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
        anim.SetBool("run", run);


        float moveX = inputH * 100f * Time.deltaTime;
        float moveZ = inputV * 250f * Time.deltaTime;

        rb.velocity = new Vector3(moveX, 0f, moveZ);
        
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }


    }

    

}
