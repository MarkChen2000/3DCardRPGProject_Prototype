using UnityEngine;
using System.Collections;

public class CharacterMovementController : MonoBehaviour
{
    PlayerStatusController PlayerStatus_Con;
    PlayerAnimationController PlayerAnimation_Con;
    CharacterBasicAttackController PlayerBasicAttackCon;
    SoundEffectManager SoundEffectCon;

    [Tooltip("According to this camera direction, define which direction is forward.")]
    public Transform Trans_Camera;
    [HideInInspector]
    public bool Can_Control = true;
    [HideInInspector]
    public bool Is_Moving = false;

    private CharacterController player_controller;
    [Tooltip("The Actual move speed will multiplay by this value! To prevent too fast or too slow for current equipment and player setting")]
    public float MovementSpeedAdjustment = 0.1f;
    [Tooltip("When speed is more then this varible, character start moveing.")]
    public float MoveActSpeed = 0.1f;
    [Tooltip("Control the turning smoothness.")]
    public float TurningSmoothTime = 0.1f;
    [HideInInspector]
    public bool Is_Staring = false; // Staring state mean the character will not rotate with moveing direction.

    float Gravity_Speed = -9.8f;
    Vector3 Move_Dir = new Vector3();
    Vector3 Last_Dir = new Vector3();
    float Last_FacingAngle;
    float tunringsmooth_velocity;

    Transform RunningSmokeEffectSpot_Trans;
    public GameObject DodgeSmokeEffectPrefab;
    public float SmokeEffectLifeTIme = 1f;
    public float Dodge_CD = 1f;
    //public float Dash_Speed = 75f;
    public float DodgeSpeedAdjustment = 0.2f;
    public float DodgeDuration = 0.2f; // The whole dash move time.
    bool Is_Dodging = false;

    private void Awake()
    {
        if (Trans_Camera == null) Trans_Camera = GameObject.Find("MainCamera").transform;

        player_controller = GetComponent<CharacterController>();
        PlayerAnimation_Con = GetComponent<PlayerAnimationController>();
        PlayerBasicAttackCon = FindObjectOfType<CharacterBasicAttackController>();
        SoundEffectCon = FindObjectOfType<SoundEffectManager>();

        PlayerStatus_Con = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
        RunningSmokeEffectSpot_Trans = transform.GetChild(3).transform;
    }

    private void Start()
    {
        Last_FacingAngle = 0f;
        Last_Dir = transform.forward;
    }

    private void Update()
    {
        ClearMovementInput();
        if (Can_Control == false) return; // All the character control function has to be write down at below.

        Move_Dir = PlayerMovementInput();

        if (Input.GetKeyDown(KeyCode.Space)) Dodge();

        Is_Staring = Is_StaringCheck();
    }

    private void FixedUpdate()
    {
        Character_Moving();

        player_controller.Move(new Vector3(0f, Gravity_Speed*Time.fixedDeltaTime, 0f));

        if (Is_Dodging)
        {
            player_controller.Move(Last_Dir * DodgeSpeedAdjustment * PlayerStatus_Con.currentSP * Time.fixedDeltaTime);
        }
    }

    private Vector3 PlayerMovementInput()
    {
        float horizontalmove, verticalmove;
        horizontalmove = Input.GetAxisRaw("Horizontal");
        verticalmove = Input.GetAxisRaw("Vertical");

        if (horizontalmove == 0 && verticalmove == 0) Is_Moving = false;
        else Is_Moving = true;

        return new Vector3(horizontalmove,0f,verticalmove).normalized; 
        // this is the direction relative to the character default facing direction.
    }
    void ClearMovementInput()
    {
        Move_Dir = new Vector2(0f, 0f);
        Is_Moving = false;
    }

    void Character_Moving()
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

            Last_FacingAngle = targetangle;
            Last_Dir = adjusted_movedir;

            if (!Is_Staring) // when character isn't in staring state, it will rotate with the moveing direction.
            {
                float smoothed_targetangle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref tunringsmooth_velocity, TurningSmoothTime);
                transform.rotation = Quaternion.Euler(0f, smoothed_targetangle, 0f);
            }
        }
    }


    // This is simple dash moving method.
    /*private void StartDash() 
    {
        if (Time.time < dashtimer)
        {
            Debug.Log("Dash is still cooling!");
            return;
        }

        float targetangle = Mathf.Atan2(Move_Dir.x, Move_Dir.z) * Mathf.Rad2Deg + Trans_Camera.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, targetangle, 0f);
        player_controller.Move(Last_Dir * Dash_Speed * Time.deltaTime);
        // transform.Translate(Vector3.forward * Dash_Distance);
        // Translate function is defautly using local coordinate, so it can simply use forward to represent character facing direction.

        dashtimer = Time.time + Dash_CD;
    }*/

    float dodgetimer = 0f;

    // This is a more complicated method to do dodge roll move.
    void Dodge()
    {
        if (Is_Dodging)
        {
            Debug.Log("Now is Dodging!");
            return;
        }
        if (Time.time <= dodgetimer)
        {
            Debug.Log("Dodge is cooling!");
            return;
        }
        StartCoroutine(StartDodge());
    }

    IEnumerator StartDodge()
    {
        GameObject pf = Instantiate(DodgeSmokeEffectPrefab, RunningSmokeEffectSpot_Trans.position, Quaternion.identity);
        Destroy(pf, SmokeEffectLifeTIme);

        transform.rotation = Quaternion.Euler(0f, Last_FacingAngle, 0f);
        PlayerAnimation_Con.OnDodgeAnim();
        SoundEffectCon.SoundPlayPlayerRoll();

        player_controller.detectCollisions = false;
        Is_Dodging = true;
        Can_Control = false;
        ClearMovementInput();

        yield return new WaitForSeconds(DodgeDuration); 

        player_controller.detectCollisions = true;
        Is_Dodging = false;
        Can_Control = true;

        dodgetimer = Time.time + Dodge_CD;
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
