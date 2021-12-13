using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBasicAttackController : MonoBehaviour
{
    public Camera MainCamera;
    private Transform Trans_Camera;
    public Transform AttackPointTrans;
    private CharacterMovementController Character_MoveCon;
    private BattleValueCalculator BattleValueCal;

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
            BasicAttack();
        }
    }

    private void BasicAttack()
    {
        if ( Time.time < AttackCDTimer )
        {
            Debug.Log("Attack is cooling!");
            return;
        }

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

            AttackCheck();

            //Debug.Log("Attack! Target:" + MouseClickPos);
        }
        else Debug.Log("Attack fail! didn't find a target!");

        AttackCDTimer = Time.time + Character_AttackCD;
    }

    private void AttackCheck()
    {
        Collider[] detectedcolliders = Physics.OverlapSphere(AttackPointTrans.position, AttackRangeRadius);
        foreach (Collider item in detectedcolliders)
        {
            if ( item.gameObject.tag == "Monster")
            {
                int damage = 0 ;
                damage = BattleValueCal.PlayerDamageCalculate(AttackType.Physics,0);
                item.gameObject.GetComponent<Monster_StatusController>().beAttacked(damage);
                Debug.Log("Attacked Enemy! Name:" + item.gameObject.name);
            }
        }
    }

    private void OnDrawGizmos() // This draw the attack range radius.
    {
        Gizmos.color = new Color(255f, 0f, 0f);
        Gizmos.DrawWireSphere(AttackPointTrans.position, AttackRangeRadius);
    }
}
