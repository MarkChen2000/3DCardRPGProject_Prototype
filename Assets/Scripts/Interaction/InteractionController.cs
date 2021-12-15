using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Null, Conversation, Transaction, Mission
}

public class InteractionController : MonoBehaviour
{
    private InteractionUIController InteractionUICon;
    private ShopSystemAndUIController ShopSystemCon;
    private CharacterBasicAttackController Player_BasicAttackCon;
    private CharacterMovementController Player_MoveCon;

    public bool Is_Interacting = false;
    private bool Is_Dialoging = false;
    private bool Is_Transacting = false;

    private InteractionType CurrentInteractionType;

    private void Awake()
    {
        InteractionUICon = GetComponent<InteractionUIController>();
        ShopSystemCon = GetComponent<ShopSystemAndUIController>();
        Player_BasicAttackCon = GameObject.Find("Player").GetComponent<CharacterBasicAttackController>();
        Player_MoveCon = GameObject.Find("Player").GetComponent<CharacterMovementController>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if ( Is_Interacting ) // When the Interaction is going.
        {
            if (Is_Dialoging && Input.GetKeyDown(KeyCode.Space)) // only when in a convesation will trigger this "next text" botton.
            {
                if ( !InteractionUICon.DisplayNextConversationText() ) // return false mean the dialogu is end.
                {
                    Is_Dialoging = false;
                    if (CurrentInteractionType == InteractionType.Conversation )
                    {
                        PlayerEndInteraction();
                    }
                    else if (CurrentInteractionType == InteractionType.Transaction )
                    {
                        if ( CurrentConversationState ) // the conversation that saying before get into shopsystem is end.
                        {
                            // get into transaction state.
                            EnableShopSystem();
                        }
                        else // the conversation you that saying when leaving from shop is end.
                        {
                            // end the whole transction interaction. 
                            PlayerEndInteraction();
                        }
                    }
                }
            }

            if ( Is_Transacting && Input.GetKeyDown(KeyCode.E) ) 
                // you can only press this botton when you are in the shop system.
                // press this botton to leave shop system.
            {
                DisableShopSystem();
            }
        }
    }

    private void PlayerStartInteraction()
    {
        Is_Interacting = true;
        Player_BasicAttackCon.Can_Attack = false;
        Player_MoveCon.Can_Control = false;
    }

    public void PlayerEndInteraction()
    {
        Is_Interacting = false;
        Player_BasicAttackCon.Can_Attack = true;
        Player_MoveCon.Can_Control = true;
        InteractionUICon.HideAllInteractionUI();
    }

    public void StartConversationInteraction(ConversationTemplate template)
    {
        if (template.StringTextList.Count == 0)
        {
            Debug.Log("There is no Text in the Conversation!");
            return;
        }

        PlayerStartInteraction();
        CurrentInteractionType = InteractionType.Conversation;
        Is_Dialoging = true;
        InteractionUICon.StartDisplayConversaionText(template.TalkerName,template.StringTextList);
    }

    private bool CurrentConversationState;
    private TransactionTemplate current_transctiontem;

    public void StartTransactionInteraction(TransactionTemplate template)
    // transaction interaction order:
    // 1. run conversation that before into shop system. 
    // 2. run shop system. 
    // 3. leave shop system.
    // 4. run conversation that leaving shop system. 
    // 5. end whole interaction.
    {
        current_transctiontem = template;
        PlayerStartInteraction();
        CurrentInteractionType = InteractionType.Transaction;
        if (current_transctiontem.BeforeShopSystemTextList.Count != 0 )
        {
            Is_Dialoging = true;
            CurrentConversationState = true; // true mean you are in before interact conversation.
            InteractionUICon.StartDisplayConversaionText(current_transctiontem.ShopKeeperName, current_transctiontem.BeforeShopSystemTextList);
        }
        else
        {
            // get into shopsystem directly.
            EnableShopSystem();
        }
    }

    private void EnableShopSystem() // get into shop system.
    {
        Is_Transacting = true;
        InteractionUICon.ShowShopUI();
        ShopSystemCon.EnterShopSystem(current_transctiontem);
    }

    private void DisableShopSystem() // leave shop system.
    {
        Is_Transacting = false;
        InteractionUICon.HideAllInteractionUI();
        ShopSystemCon.LeaveShopSystem();

        if (current_transctiontem.LeavingShopSystemTextList.Count != 0) // and run the leaving conversation.
        {
            Is_Dialoging = true;
            CurrentConversationState = false; // false mean you are in leaving interact conversation.
            InteractionUICon.StartDisplayConversaionText(current_transctiontem.ShopKeeperName, current_transctiontem.LeavingShopSystemTextList);
        }
        else
        {
            // end whole interaction directly.
            PlayerEndInteraction();
        }
    }


}
