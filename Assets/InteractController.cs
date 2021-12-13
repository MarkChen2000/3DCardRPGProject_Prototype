using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType
{
    Null, Conversation, Transaction, Mission
}

public class InteractController : MonoBehaviour
{
    private InteractUIController InteractUICon;
    private CharacterBasicAttackController Player_BasicAttackCon;
    private CharacterMovementController Player_MoveCon;

    public bool Is_Interacting = false;
    private bool Is_Dialogue = false;

    private void Awake()
    {
        InteractUICon = GetComponent<InteractUIController>();
        Player_BasicAttackCon = GameObject.Find("Player").GetComponent<CharacterBasicAttackController>();
        Player_MoveCon = GameObject.Find("Player").GetComponent<CharacterMovementController>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if ( Is_Interacting ) // When the conversaion is going.
        {
            if (Is_Dialogue && Input.GetKeyDown(KeyCode.Space)) // only in convesation will trigger this "next text" botton.
            {
                if ( !InteractUICon.DisplayNextConversationText() ) // return false mean the dialogu is end.
                {
                    Is_Dialogue = false;
                    PlayerEndInteract();
                }
            }
        }
    }

    private void PlayerStartInteract()
    {
        Is_Interacting = true;
        Player_BasicAttackCon.Can_Attack = false;
        Player_MoveCon.Can_Control = false;
    }

    public void PlayerEndInteract()
    {
        Is_Interacting = false;
        Player_BasicAttackCon.Can_Attack = true;
        Player_MoveCon.Can_Control = true;
        InteractUICon.HideAllInteractUI();
    }

    public void StartConversationInteract(ConversationTemplate template)
    {
        if (template.StringTextList.Count == 0)
        {
            Debug.Log("There is no Text in the Conversation!");
            return;
        }

        Is_Dialogue = true;
        PlayerStartInteract();
        InteractUICon.StartDisplayConversaionText(template.TalkerName,template.StringTextList);
    }

}
