using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckEditorLayout : MonoBehaviour, IDropHandler
{
    public const float WidthCheck = 1100f;

    public Vector2 DeckButtonsLandscapePosition {
        get { return new Vector2(-450f, 0f); }
    }

    public Vector2 DeckButtonsPortraitPosition {
        get { return new Vector2(-50f, -(GetComponent<RectTransform>().rect.height + 110f)); }
    }

    public DeckEditor deckEditor;
    public RectTransform deckButtons;

    void Start()
    {
        if (GetComponent<RectTransform>().rect.width < WidthCheck)
            deckButtons.anchoredPosition = DeckButtonsPortraitPosition;
    }

    void OnRectTransformDimensionsChange()
    {
        if (!this.gameObject.activeInHierarchy)
            return;
        
        if (GetComponent<RectTransform>().rect.width < WidthCheck)
            deckButtons.anchoredPosition = DeckButtonsPortraitPosition;
        else
            deckButtons.anchoredPosition = DeckButtonsLandscapePosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardModel cardModel = eventData.pointerDrag.GetComponent<CardModel>();
        if (cardModel != null) {
            CardModel draggedCardModel;
            if (cardModel.DraggedClones.TryGetValue(eventData.pointerId, out draggedCardModel))
                cardModel = draggedCardModel;
            deckEditor.AddCard(cardModel.RepresentedCard);
        }
    }
}
