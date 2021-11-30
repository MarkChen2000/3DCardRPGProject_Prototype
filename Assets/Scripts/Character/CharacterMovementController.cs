using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    [Tooltip("According to this camera direction, define which direction is forward.")]
    public Transform Trans_Camera;

    public bool Can_Control = true;

    private CharacterController player_controller;
    public float PlayerMovementSpeed = 10f;
    [Tooltip("When speed is more then this varible, character start moveing.")]
    public float MoveActSpeed = 0.1f;
    [Tooltip("Control the turning smoothness.")]
    public float TurningSmoothTime = 0.1f;
    private Vector3 Move_Dir = new Vector3();
    private Vector3 Last_Dir = new Vector3();
    float tunringsmooth_velocity;

    public float Dash_CD = 1f;
    private float dashtimer = 0f;
    public float Dash_Speed = 100f;

    private void Awake()
    {
        if (Trans_Camera == null) Trans_Camera = GameObject.Find("MainCamera").transform;

        player_controller = GetComponent<CharacterController>();
    }

    private void Start()
    {    
    }

    private void Update()
    {

        if (Can_Control == false) return; // All the character control function has to be write down at below.
        Move_Dir = PlayerMovementInput();
        if (Input.GetKeyDown(KeyCode.Space)) Character_Dash();
    }

    private void FixedUpdate()
    {
        if (Move_Dir.magnitude >= MoveActSpeed)
        {
            float targetangle = Mathf.Atan2(Move_Dir.x, Move_Dir.z) * Mathf.Rad2Deg + Trans_Camera.eulerAngles.y;
            // By using Atan2 can get the radian number from defult character foward direction
            // (which is Z Axis(0,0,1)) to the moving direction. 
            // multiply by Rad2Deg can conversion the radian to angle.
            // And by plusing the rotation around Y Axis of camera, can conversion the facing direction by camera direction.
            
            float smoothed_targetangle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref tunringsmooth_velocity, TurningSmoothTime);
            // get the angle that is been smoothlize
            transform.rotation = Quaternion.Euler(0f, smoothed_targetangle, 0f);
            // Rotate the character to the targetangle, which is an eulerangle (angle around Y Axis).
            
            Vector3 adjusted_movedir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
            // conversion the adjusted angle into a vector,
            // which is relative to camera facing direction,
            // multiply by character default direction (forward(0,0,1))
            // meaning getting a new vector after orginal vector is rotated by the quaternion.

            Last_Dir = adjusted_movedir;
            player_controller.Move(adjusted_movedir * PlayerMovementSpeed * Time.fixedDeltaTime);
            // Because the Move function is using world coordinate, so it has to conversion.
        }
    }

    private Vector3 PlayerMovementInput()
    {
        float horizontalmove, verticalmove ;
        horizontalmove = Input.GetAxisRaw("Horizontal");
        verticalmove = Input.GetAxisRaw("Vertical");

        return new Vector3(horizontalmove,0f,verticalmove).normalized; 
        // this is the direction relative to the character default facing direction.
    }

    private void Character_Dash()
    {
        if (Time.time < dashtimer)
        {
            Debug.Log("Dash is cooling!");
            return;
        }
        player_controller.Move(Last_Dir * Dash_Speed * Time.deltaTime);

        // transform.Translate(Vector3.forward * Dash_Distance);
        // Translate function is defautly using local coordinate, so it can simply use forward to represent character facing direction.
        dashtimer = Time.time + Dash_CD;
    }
}
