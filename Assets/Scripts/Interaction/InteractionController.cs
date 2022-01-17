using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Null, Conversation, Transaction, Mission, ChangeGameState
}

public class InteractionController : MonoBehaviour
{
    InteractionUIController InteractionUICon;
    ShopSystemAndUIController ShopSystemCon;
    CharacterBasicAttackController Player_BasicAttackCon;
    CharacterMovementController Player_MoveCon;
    GameStateChangeUIController GameStateChangeUICon;
    EntireInventoryController InvCon;

    public bool Is_Interacting = false;
    bool Is_Dialoging = false;
    bool Is_Transacting = false;
    bool Is_ChangingGameState = false;

    bool CurrentConversationState; // it mean player is in the dialogue of before or after interaction.

    InteractionType CurrentInteractionType;

    private void Awake()
    {
        InteractionUICon = GetComponent<InteractionUIController>();
        ShopSystemCon = GetComponent<ShopSystemAndUIController>();
        GameStateChangeUICon = GetComponent<GameStateChangeUIController>();
        Player_BasicAttackCon = GameObject.Find("Player").GetComponent<CharacterBasicAttackController>();
        Player_MoveCon = GameObject.Find("Player").GetComponent<CharacterMovementController>();
        InvCon = FindObjectOfType<EntireInventoryController>();
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

                    else if( CurrentInteractionType == InteractionType.ChangeGameState )
                    {
                        if (CurrentConversationState) // the conversation that saying before get into change game state is end.
                        {
                            // get into transaction state.
                            EnableChangeStateSystem();
                        }
                        else // the conversation you that saying when leaving from change game state is end.
                        {
                            // end the whole interaction. 
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

            if ( Is_ChangingGameState && Input.GetKeyDown(KeyCode.E))
            {
                DisableChangeStateSystem();
            }

        }
    }

    private void PlayerStartInteraction()
    {
        Is_Interacting = true;
        Player_MoveCon.Can_Control = false;
        InvCon.Can_TurnOnInv = false;
    }

    public void PlayerEndInteraction()
    {
        Is_Interacting = false;
        Is_ChangingGameState = false;
        Is_Transacting = false;
        Player_MoveCon.Can_Control = true;
        InvCon.Can_TurnOnInv = true;
        InteractionUICon.HideAllInteractionUI();
    }

    public void StartConversationInteraction(ConversationTemplate template)
    {
        if (template.StringTextList.Count == 0)
        {
            //Debug.Log("There is no Text in the Conversation!");
            return;
        }

        PlayerStartInteraction();
        CurrentInteractionType = InteractionType.Conversation;
        Is_Dialoging = true;
        InteractionUICon.StartDisplayConversaionText(template.TalkerName,template.StringTextList);
    }

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
        CurrentConversationState = true; // true mean you are in before interact conversation.
        if (current_transctiontem.BeforeShopSystemTextList.Count != 0 )
        {
            Is_Dialoging = true;
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

    private ChangeGameStateTemplate current_changestate_tem;

    public void StartChangeGameStateInteraction(ChangeGameStateTemplate changestate_tem)
        // change game state order:
        // 1. run conversation.
        // 2. turn on change state menu, and wait for player choose.
        // 3. enter the chosen state or leave ame state change menu.
        // 4. if player leave, run the conversation.
    {
        current_changestate_tem = changestate_tem;
        CurrentInteractionType = InteractionType.ChangeGameState;
        PlayerStartInteraction();
        CurrentConversationState = true; // true mean you are in before interact conversation.
        if (current_changestate_tem.BeforeChangeStateSystemTextList.Count != 0)
        {
            Is_Dialoging = true;
            InteractionUICon.StartDisplayConversaionText(current_changestate_tem.VillagerName, current_changestate_tem.BeforeChangeStateSystemTextList);
        }
        else
        {
            // get into interaction directly.
            EnableChangeStateSystem();
        }
    }

    private void EnableChangeStateSystem()
    {
        Is_ChangingGameState = true;
        InteractionUICon.ShowChangeStateUI();
        GameStateChangeUICon.EnterChangeStateSystem(current_changestate_tem);
    }

    private void DisableChangeStateSystem()
    {
        Is_ChangingGameState = false;
        InteractionUICon.HideAllInteractionUI();

        if (current_changestate_tem.LeavingChangeStateSystemTextList.Count != 0)
        {
            Is_Dialoging = true;
            CurrentConversationState = false; // true mean you are in leaving interact conversation.
            InteractionUICon.StartDisplayConversaionText(current_changestate_tem.VillagerName, current_changestate_tem.LeavingChangeStateSystemTextList);
        }
        else
        {
            PlayerEndInteraction();
        }
    }

}
