using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    private PlayerStatusController PlayerStatus_Con;

    [Tooltip("According to this camera direction, define which direction is forward.")]
    public Transform Trans_Camera;
    [HideInInspector]
    public bool Can_Control = true;

    private CharacterController player_controller;
    [Tooltip("The Actual move speed will multiplay by this value! To prevent too fast or too slow for current equipment and player setting")]
    public float MovementSpeedAdjustment = 0.1f;
    [Tooltip("When speed is more then this varible, character start moveing.")]
    public float MoveActSpeed = 0.1f;
    [Tooltip("Control the turning smoothness.")]
    public float TurningSmoothTime = 0.1f;
    [HideInInspector]
    public bool Is_Staring = false; // Staring state mean the character will not rotate with moveing direction.

    private float Gravity_Speed = -9.8f;
    private Vector3 Move_Dir = new Vector3();
    private Vector3 Last_Dir = new Vector3();
    float tunringsmooth_velocity;

    public float Dash_CD = 1f;
    public float Dash_Speed = 75f;

    private void Awake()
    {
        if (Trans_Camera == null) Trans_Camera = GameObject.Find("MainCamera").transform;

        player_controller = GetComponent<CharacterController>();

        PlayerStatus_Con = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
    }

    private void Start()
    {
        Last_Dir = transform.forward;
    }

    private void Update()
    {
        if (Can_Control == false) return; // All the character control function has to be write down at below.
        Move_Dir = PlayerMovementInput();
        if (Input.GetKeyDown(KeyCode.Space)) Character_Dash();

        Is_Staring = Is_StaringCheck();
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
            
            Vector3 adjusted_movedir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
            // conversion the adjusted angle into a vector,
            // which is relative to camera facing direction,
            // multiply by character default direction (forward(0,0,1))
            // meaning getting a new vector after orginal vector is rotated by the quaternion.

            player_controller.Move(adjusted_movedir * MovementSpeedAdjustment * PlayerStatus_Con.currentSP * Time.fixedDeltaTime);
            // Because the Move function is using world coordinate, so it has to be conversion.
            Last_Dir = adjusted_movedir;

            if ( !Is_Staring ) // when character isn't in staring state, it will rotate with the moveing direction.
            {
                float smoothed_targetangle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref tunringsmooth_velocity, TurningSmoothTime);
                transform.rotation = Quaternion.Euler(0f, smoothed_targetangle, 0f);
            }
        }
        player_controller.Move(new Vector3(0f, Gravity_Speed*Time.fixedDeltaTime, 0f));
    }

    private Vector3 PlayerMovementInput()
    {
        float horizontalmove, verticalmove ;
        horizontalmove = Input.GetAxisRaw("Horizontal");
        verticalmove = Input.GetAxisRaw("Vertical");

        return new Vector3(horizontalmove,0f,verticalmove).normalized; 
        // this is the direction relative to the character default facing direction.
    }

    private float dashtimer = 0f;
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

    private float staringdurationtimer = 0f;
    private bool Is_StaringCheck()
    {
        staringdurationtimer = Mathf.Clamp(staringdurationtimer - Time.deltaTime,0f,10f);
        if (staringdurationtimer > 0) return true;
        else return false;
    }

    public void RefreshStaringDurationTimer(float wait_sec)
    {
        staringdurationtimer = wait_sec;
    }
}
