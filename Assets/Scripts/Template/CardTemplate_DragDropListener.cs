using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TemplateType
{
    InventoryTem,
    DeckTem,
    EquidmentTem
}


public class CardTemplate_DragDropListener: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public TemplateType _TemplateType; 

    private InventoryController Inventory_Con;
    private Card_DataLoaderAndDisplay Card_DataLoader;

    private CardDeckController Deck_Con;
    private DeckCard_DataLoaderAndDisplay DeckCard_DataLoader;

    private EquipmentSlotController EquipSlot_Con;
    private EquipmentCard_DataLoaderAndDisplay EquipCard_DataLoader;

    private Canvas Canvas_Invetory;
    private RectTransform RTrans_Template;
    public Vector3 BiggerScale = new Vector3(1.1f, 1.1f, 1.1f);

    private Vector3 OriginPos ;

    void Awake()
    {
        switch ( _TemplateType ) // According to which type of the template, choose different function.
        {
            case TemplateType.InventoryTem:
                Inventory_Con = transform.root.GetComponent<InventoryController>();
                Card_DataLoader = GetComponent<Card_DataLoaderAndDisplay>();
                break;
            case TemplateType.DeckTem:
                Deck_Con = transform.root.GetComponent<CardDeckController>();
                DeckCard_DataLoader = GetComponent<DeckCard_DataLoaderAndDisplay>();
                break;
            case TemplateType.EquidmentTem:
                EquipSlot_Con = transform.root.GetComponent<EquipmentSlotController>();
                EquipCard_DataLoader = GetComponent<EquipmentCard_DataLoaderAndDisplay>();
                break;
        }
        Canvas_Invetory = transform.root.GetChild(0).GetComponent<Canvas>();
        RTrans_Template = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Temporary stop the drag and drop function.
        //OriginPos = RTrans_Template.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData) // when dragging, call by every frame.
    {
        //RTrans_Template.anchoredPosition += eventData.delta / Canvas_Invetory.scaleFactor ;
        // Anchorposition is the anchor position of the Object. // eventData.delta mean the mouse movement data. 
        // the Object transform will be affect by the canvas scale, so mouse movement delta should division by the scale.
        //Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //RTrans_Template.anchoredPosition = OriginPos;

        /*switch (_TemplateType)
        {
            case TemplateType.InventoryTem:
                if (true) // if drag on a dropzone that is OK to transfer card.
                {
                    Inventory_Con.TryTransferCard(Card_DataLoader);
                }
                else
                {
                }

                break;
            case TemplateType.DeckTem:
                if (true) 
                {
                    Deck_Con.TryTransferCardtoInv(DeckCard_DataLoader);
                }
                else
                {
                }
                
                break;
            case TemplateType.EquidmentTem:
                if (true) 
                {
                    EquipSlot_Con.TryTransferCardtoInv(EquipCard_DataLoader);
                }
                else
                {
                }
                break;
        }*/
    }

    public void OnPointerClick(PointerEventData eventData) // Whenever call by mouse Up on the Object.
    {
        switch (_TemplateType)
        {
            case TemplateType.InventoryTem:
                Inventory_Con.TryTransferCard(Card_DataLoader);

                break;
            case TemplateType.DeckTem:
                Deck_Con.TryTransferCardtoInv(DeckCard_DataLoader);

                break;
            case TemplateType.EquidmentTem:
                EquipSlot_Con.TryTransferCardtoInv(EquipCard_DataLoader);

                break;
        }
    }

    public void OnPointerDown(PointerEventData eventData) // Whenever call by mouse Down on the Object.
    {
        //Debug.Log("OPDown");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RTrans_Template.localScale = BiggerScale;
        //Debug.Log("OPEeter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RTrans_Template.localScale = new Vector3(1f, 1f, 1f);
        //Debug.Log("OPExit");
    }

}
