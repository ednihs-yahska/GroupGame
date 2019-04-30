using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class ThirdController: MonoBehaviour
{
    public float speed = 3.0F;              //the moving speed of the character
    public float run_factor = 1.6f;         //The speed up factor when running
    public float slide_factor = 1.6f;       //The speed up factor when slide while running
    public float rotateSpeed = 3.0F;        //the rotate speed of the character
    public float jumpSpeed = 5.0f;          //the jump force of the character
    public float gravity = 9.8f;            //the force of gravity on the character
    public float GroundOffset = .2f;        //the offset for the IsGrounded check. Useful for recognizing slopes and imperfect ground.

    private Vector3 moveDirection = Vector3.zero;   //the direction the character should move.
    private Vector3 jumpDirection = Vector3.zero;
    private CharacterController controller;         //The character controller object

    private Animator anim;                  //The current animator object
    private float Horizontal, Vertical;     //The input from player
    private bool Run, Jump, Slide;          //The flags to control the animation


    //The check to see if the character is currently on the ground.
    private bool isGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, 10);                //A short ray shot directly downward from the center of the character.

        if (System.Math.Abs(hit.distance) < System.Single.Epsilon)                      //if the distance is zero, the ray probably did not hit anything.
        {
            return false;
        }

        //If the distance from the ray is less than half the height of the character (plus the offset), the character us grounded.
        if (hit.distance <= (this.transform.lossyScale.y / 2 + GroundOffset))
        {
            Debug.Log("On the ground!");
            return true;
        }
        Debug.Log("Not on the ground!");
        return false;
    }


    //This built-in function will be called after the script first time loaded into the scene
    void Start()
    {
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();

        anim = gameObject.GetComponent<Animator>();     //Get the animator object

        Run = Jump = Slide = false;                     //Initialize the flag
    }

    void Update()
    {
        //Get the value for each axis from input
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");


        /********** 2D Plane Movement **********/

        moveDirection = new Vector3(Horizontal, 0.0f, Vertical);            //Create the player's movement from keyboard in local space
        moveDirection = transform.TransformDirection(moveDirection);        //Transform the moveMent from local space to world space
        moveDirection *= speed;      //Based on base speed

        //If player is walking and is not running
        if (Vertical != 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Run = !Run;
            }
        }

        //If the player is running and hit ctrl, then slide
        if (Run)
        {
            moveDirection *= run_factor;        //If the player is running, then speed up

            //If the player choose to slide
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Slide = true;
                moveDirection *= slide_factor;  //If the player is sliding, then speed up
            }
            else
            {
                Slide = false;
            }

            if (Vertical == 0)
            {
                Run = false;
            }
        }



        /********** Verticle Movement **********/

        //If the player hit the space, then enter the jump animation
        if (Input.GetButton("Jump") && isGrounded())
        {
            Jump = true;                        //Set the jump flag
            jumpDirection.y = jumpSpeed;        //Give a jump speed to player
        }
        else
        {
            Jump = false;
        }

        //Add gravity and move the character
        controller.Move(moveDirection * Time.deltaTime);    //move the character based on the gravitational force.
        jumpDirection.y -= gravity * Time.deltaTime;
        controller.Move(jumpDirection * Time.deltaTime);


        /********** Animation Part **********/

        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
        anim.SetBool("Jump", Jump);
        anim.SetBool("Run", Run);
        anim.SetBool("Slide", Slide);
    }
}
