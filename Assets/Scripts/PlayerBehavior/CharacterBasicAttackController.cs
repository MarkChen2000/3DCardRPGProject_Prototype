using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBasicAttackController : MonoBehaviour
{
    public Camera MainCamera;
    Transform Trans_Camera;

    public Transform AttackPointTrans;
    public GameObject DefaultHitEffectPrefab;
    public Image GetHitEffect;

    BattleHitPauseManager HitPauseManager;
    CharacterMovementController Character_MoveCon;
    BattleValueCalculator BattleValueCal;
    PlayerAnimationController Player_AnimationCon;
    PlayerStatusController PlayerStatusCon;

    public float AttackRangeRadius = 5f;
    public float AttackCD = 1f;
    //public float AttackDuration = 1f;
    public float HitPauseStopDuration = 0.1f;
    public float HitPauseRestoreTime = 0.1f;
    public float GetHittedHitPauseStopDuration = 0.3f;
    public float GetHittedHitPauseRestoreTime = 0.5f;
    public float InvincibleTime = 1f;

    [Tooltip("The time that make character turn in Staring state, which will make character stop turning toward the moveing direction.")]
    public float AttackStaringTime = 2f;
    public float AttackTurningSmoothTime = 0.001f; // the time of character turning to target angle.
    float attacktunringsmooth_velocity;

    void Awake()
    {
        HitPauseManager = GameObject.Find("BattleManager").GetComponent<BattleHitPauseManager>();
        Character_MoveCon = GetComponent<CharacterMovementController>();
        //AttackPointTrans = transform.GetChild(1);
        BattleValueCal = GameObject.Find("BattleManager").GetComponent<BattleValueCalculator>();
        Player_AnimationCon = GetComponent<PlayerAnimationController>();
        PlayerStatusCon = GameObject.Find("PlayerManager").GetComponent<PlayerStatusController>();
    }

    void Start()
    {
        if (MainCamera == null) MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        Trans_Camera = MainCamera.GetComponent<Transform>();
    }

    void Update()
    {
        if ( Input.GetMouseButtonDown(1) && Character_MoveCon.Can_Control ) 
        {
            OnAttack();
        }
    }

    float AttackCDTimer = 0f;
    void OnAttack() // Check the mouse ckilck
    {
        if ( Time.time < AttackCDTimer )
        {
            Debug.Log("Attack is cooling!");
            return;
        }

        Player_AnimationCon.OnAttack();

        //StartCoroutine(AttackingControlFreeze(Character_AttackDuration));

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

            Character_MoveCon.RefreshStaringDurationTimer(AttackStaringTime);

            //Debug.Log("Attack! Target:" + MouseClickPos);
        }
        //else Debug.Log("Attack fail! didn't find a target!");

        AttackCDTimer = Time.time + AttackCD;
    }

    /*IEnumerator AttackingControlFreeze(float duration) // maybe attack freeze in bullet dodge game is not a good idea.
    {
        Character_MoveCon.Can_Control = false;
        yield return new WaitForSeconds(duration);
        Character_MoveCon.Can_Control = true;
    }*/

    public void AttackCheck() // Check did or didn't hit any enemy. This will called by animation event.
    {
        Collider[] detectedcolliders = Physics.OverlapSphere(AttackPointTrans.position, AttackRangeRadius);
        foreach (Collider item in detectedcolliders)
        {
            if ( item.gameObject.tag == "Monster")
            {
                Vector2 damageinfo = BattleValueCal.PlayerDamageCalculate(AttackType.Physics,0);
                item.gameObject.GetComponent<Monster_StatusAndUIController>().beAttacked(damageinfo);
                //Debug.Log("Attacked Enemy! Name:" + item.gameObject.name);

                HitPauseManager.HitPauseStopTime(HitPauseStopDuration, HitPauseRestoreTime);

                if ( DefaultHitEffectPrefab != null )
                {
                    StartCoroutine(WaitSpawnPrefab(item,damageinfo));
                }
            }
        }
    }

    IEnumerator WaitSpawnPrefab(Collider item, Vector2 damageinfo) // Wait until hit pause effect end, then run the hit effect prefab.
    {
        Vector3 closestpoint = item.ClosestPoint(transform.position); // Sometime when hit effec end, monster collider is already destryoed.
        while ( Time.timeScale != 1f)
        {
            yield return null;
        }
        Transform trans = Instantiate(DefaultHitEffectPrefab, closestpoint, Quaternion.identity).transform;
        trans.LookAt(transform);

        if ( damageinfo.x==1 ) // mean this hit is critical
        {
            trans.localScale = new Vector3(2f, 2f, 2f);
        }
        Destroy(trans.gameObject, 3f);
    }

    float InvincibleTimer = 0f;
    public void PlayerBeAttack(int damage)
    {
        if (Time.time < InvincibleTimer)
        {
            // Debug.Log("Player is invincible now!");
            return;
        }

        HitPauseManager.HitPauseStopTime(GetHittedHitPauseStopDuration, GetHittedHitPauseRestoreTime);

        Color color = GetHitEffect.color;
        color.a = 0.8f;
        GetHitEffect.color = color;
        StartCoroutine(ReduceGetHitEffectAlpha(color));

        int finaldamage = BattleValueCal.PlayerTakeDamageCalculate(damage);
        //Debug.Log("Player take " + finaldamage);
        PlayerStatusCon.TakeDamage( finaldamage );

        InvincibleTimer = Time.time + InvincibleTime;
    }

    IEnumerator ReduceGetHitEffectAlpha(Color color)
    {
        float al = color.a;
        while ( GetHitEffect.color.a > 0 )
        {
            color.a -= al / InvincibleTime * Time.unscaledDeltaTime;
            GetHitEffect.color = color;
            //Debug.Log(al+" "+color.a);
            yield return null;
        }
    }

    void OnDrawGizmos() // This draw the attack range radius.
    {
        Gizmos.color = new Color(255f, 0f, 0f);
        Gizmos.DrawWireSphere(AttackPointTrans.position, AttackRangeRadius);
    }
}
