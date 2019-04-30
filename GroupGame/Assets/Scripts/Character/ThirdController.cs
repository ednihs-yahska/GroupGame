using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdController: MonoBehaviour
{

    private Animator anim;                  //The current animator object
    private Rigidbody rb;                   //The current rigidbody object
    private float Horizontal, Vertical;     //The input from player
    private bool Run, Jump, Slide;          //The flags to control the animation
    private float speed;                    //The real speed of the player inside the game, the combination of move_speed and run_factor

    public float move_speed;                //The moving speed of player
    public float run_factor;                //The multipler to the speed when player run
    public float jump_force;                //The force will applyed to player when jump

	// Use this for initialization
	void Start ()
    {
        anim = gameObject.GetComponent<Animator>();     //Get the animator object
        rb = gameObject.GetComponent<Rigidbody>();

        Run = Jump = Slide = false;     //Initialize the flag     
	}
	
	// Update is called once per frame
	void Update ()
    {
        /********** Horizontal Part **********/

        //Get the value for each axis from input
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        //If player is walking and is not running
        if(Vertical != 0f)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                Run = !Run;
            }
        }

        //If the player is running and hit ctrl, then slide
        if(Run)
        {
            speed *= run_factor;        //Increase the speed

            if(Input.GetKey(KeyCode.LeftControl))
            {
                Slide = true;
            }
            else
            {
                Slide = false;
            }

            if(Vertical == 0)
            {
                Run = false;
            }
        }

        /********** Verticle Part **********/

        //If the player hit the space, then enter the jump animation
        if (Input.GetKey(KeyCode.Space))
        {
            Jump = true;
        }
        else
        {
            Jump = false;
        }

        /********** Animation Part **********/

        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
        anim.SetBool("Jump", Jump);
        anim.SetBool("Run", Run);
        anim.SetBool("Slide", Slide);
	}
}
