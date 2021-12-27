using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBasicAttackController : MonoBehaviour
{
    public Camera MainCamera;
    private Transform Trans_Camera;

    public Transform AttackPointTrans;
    public GameObject DefaultHitEffectPrefab;

    private CharacterMovementController Character_MoveCon;
    private BattleValueCalculator BattleValueCal;
    private PlayerAnimationController Player_AnimationCon;
    private PlayerStatusController PlayerStatusCon;

    public bool Can_Attack = false;

    public float AttackRangeRadius = 5f;
    public float Character_AttackCD = 1f;
    private float AttackCDTimer = 0;

    [Tooltip("The time that make character turn in Staring state, which will make character stop turning toward the moveing direction.")]
    public float Character_AttackStaringTime = 2f;

    public float AttackTurningSmoothTime = 0.001f; // the time of character turning to target angle.
    float attacktunringsmooth_velocity;

    void Awake()
    {

        Character_MoveCon = GetComponent<CharacterMovementController>();
        //AttackPointTrans = transform.GetChild(1);
        BattleValueCal = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        Player_AnimationCon = GetComponent<PlayerAnimationController>();
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
    }

    private void Start()
    {
        if (MainCamera == null) MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        Trans_Camera = MainCamera.GetComponent<Transform>();
    }

    void Update()
    {
        if ( Input.GetMouseButtonDown(0) && Can_Attack ) // left mouse click
        {
            OnAttack();
        }
    }

    private void OnAttack() // Check the mouse ckilck
    {
        if ( Time.time < AttackCDTimer )
        {
            Debug.Log("Attack is cooling!");
            return;
        }

        Player_AnimationCon.OnAttack();

        Ray detectray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(detectray, out hit))
        {
            Vector3 MouseClickPos = hit.point;

            // Rotate character to the attack point.
            Vector3 face_dir = (MouseClickPos - transform.position).normalized;
            float targetangle = Mathf.Atan2(face_dir.x, face_dir.z) * Mathf.Rad2Deg + Trans_Camera.eulerAngles.y;
            float smoothed_targetangle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref attacktunringsmooth_velocity, AttackTurningSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothed_targetangle, 0f);

            Character_MoveCon.RefreshStaringDurationTimer(Character_AttackStaringTime);

            //Debug.Log("Attack! Target:" + MouseClickPos);
        }
        //else Debug.Log("Attack fail! didn't find a target!");

        AttackCDTimer = Time.time + Character_AttackCD;
    }

    public void AttackCheck() // Check did or didn't hit any enemy.
    {
        Collider[] detectedcolliders = Physics.OverlapSphere(AttackPointTrans.position, AttackRangeRadius);
        foreach (Collider item in detectedcolliders)
        {
            if ( item.gameObject.tag == "Monster")
            {
                Vector2 damageinfo = BattleValueCal.PlayerDamageCalculate(AttackType.Physics,0);
                item.gameObject.GetComponent<Monster_StatusController>().beAttacked(damageinfo);
                //Debug.Log("Attacked Enemy! Name:" + item.gameObject.name);

                if ( DefaultHitEffectPrefab != null )
                {
                    Transform trans = Instantiate(DefaultHitEffectPrefab,item.ClosestPoint(transform.position),Quaternion.identity).transform;
                    trans.LookAt(transform);

                    if ( damageinfo.x==1 ) // mean this hit is critical
                    {
                        trans.localScale = new Vector3(2f, 2f, 2f);
                    }

                }
            }
        }
    }

    public void PlayerBeAttack(int damage)
    {
        int finaldamage = BattleValueCal.PlayerTakeDamageCalculate(damage);
        Debug.Log("Player take " + finaldamage);
        PlayerStatusCon.TakeDamae( finaldamage );
    }


    private void OnDrawGizmos() // This draw the attack range radius.
    {
        Gizmos.color = new Color(255f, 0f, 0f);
        Gizmos.DrawWireSphere(AttackPointTrans.position, AttackRangeRadius);
    }
}
