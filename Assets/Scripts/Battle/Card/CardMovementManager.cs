using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovementManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    private Vector3 initPosition;
    private Vector2 lastMousePosition;
    // Start is called before the first frame update
    void Start()
    {
        initPosition = this.GetComponent<RectTransform>().position;
        this.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
        this.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        GameObject.Find("BattleManager").GetComponent<BattleManager>().currentCard(this.gameObject.GetComponent<BattleCard_LoaderAndDisplay>()._CardData);
        this.gameObject.GetComponent<BattleCard_LoaderAndDisplay>()._CardData.CardName.ToString();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
        //GameObject.Find("BattleManager").GetComponent<BattleManager>().currentCard(null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTransform rect = this.GetComponent<RectTransform>();

        Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
        rect.position = newPosition;
        lastMousePosition = currentMousePosition;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            this.GetComponent<RectTransform>().position = new Vector3(initPosition.x, initPosition.y, initPosition.z);
            GameObject.Find("BattleManager").GetComponent<BattleManager>().currentCard(null);
            this.OnEndDrag(eventData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //this.GetComponent<RectTransform>().position = new Vector3(initPosition.x, initPosition.y, initPosition.z);
        GameObject.Find("BattleManager").GetComponent<BattleManager>().executeCard(this.gameObject.GetComponent<BattleCard_LoaderAndDisplay>()._CardData);
        GameObject.Find("BattleManager").GetComponent<BattleManager>().currentCard(null);
        Destroy(this.gameObject);
        GameObject.Find("BattleManager").GetComponent<BattleManager>().DrawCard();
    }
}
