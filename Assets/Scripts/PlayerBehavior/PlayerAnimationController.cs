using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    CharacterBasicAttackController Player_AttackCon;
    CharacterMovementController Player_MovementCon;
    Animator Player_Animator;

    private void Awake()
    {
        Player_AttackCon = GetComponent<CharacterBasicAttackController>();
        Player_MovementCon = GetComponent<CharacterMovementController>();
        Player_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Is_MovingCheck();
    }

    public void Is_MovingCheck()
    {
        if (Player_MovementCon.Is_Moving) Player_Animator.SetBool("Is_Moving", true);
        else Player_Animator.SetBool("Is_Moving", false);
    }

    public void OnAttack() // Trigger the attack animation
    {
        Player_Animator.SetTrigger("OnAttack");
    }

    public void OnAttackCheck() // The animation will triiger this function in animation event, to check did or didn't hit enemy.
    {
        Player_AttackCon.AttackCheck();
    }


}
