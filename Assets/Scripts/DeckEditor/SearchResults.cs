﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SearchResults : MonoBehaviour
{
    public const string EmptyFilterText = "*";

    public GameObject searchMenuPrefab;
    public DeckEditor deckEditor;
    public RectTransform layoutArea;
    public InputField nameInputField;
    public Text filtersText;
    public Text countText;

    public int CardsPerPage {
        get { return Mathf.FloorToInt(layoutArea.rect.width / (deckEditor.cardModelPrefab.GetComponent<RectTransform>().rect.width + layoutArea.gameObject.GetOrAddComponent<HorizontalLayoutGroup>().spacing)); }
    }

    public int TotalPageCount {
        get { return CardsPerPage == 0 ? 0 : (AllResults.Count / CardsPerPage) + ((AllResults.Count % CardsPerPage) == 0 ? -1 : 0); }
    }

    public int CurrentPageIndex { get; set; }

    private SearchMenu _searchMenu;
    private List<Card> _allResults;

    void OnEnable()
    {
        SearchMenu.SearchCallback = ShowResults;
        CardGameManager.Instance.OnSelectActions.Add(SearchMenu.ClearSearch);
    }

    public string SetNameInputField(string name)
    {
        nameInputField.text = name;
        return nameInputField.text;
    }

    public void SetNameFilter(string name)
    {
        SearchMenu.NameFilter = name;
    }

    public void SetFiltersText(string filters)
    {
        if (string.IsNullOrEmpty(filters))
            filters = EmptyFilterText;
        filtersText.text = filters;
    }

    public void Search()
    {
        SearchMenu.Search();
    }

    public void MoveLeft()
    {
        CurrentPageIndex--;
        if (CurrentPageIndex < 0)
            CurrentPageIndex = TotalPageCount;
        UpdateSearchResultsPanel();
    }

    public void MoveRight()
    {
        CurrentPageIndex++;
        if (CurrentPageIndex > TotalPageCount)
            CurrentPageIndex = 0;
        UpdateSearchResultsPanel();
    }

    public void UpdateSearchResultsPanel()
    {
        layoutArea.DestroyAllChildren();

        for (int i = 0; i < CardsPerPage && CurrentPageIndex >= 0 && CurrentPageIndex * CardsPerPage + i < AllResults.Count; i++) {
            string cardId = AllResults [CurrentPageIndex * CardsPerPage + i].Id;
            Card cardToShow = CardGameManager.Current.Cards.Where(card => card.Id == cardId).FirstOrDefault();
            CardModel cardModelToShow = Instantiate(deckEditor.cardModelPrefab, layoutArea).GetOrAddComponent<CardModel>();
            cardModelToShow.RepresentedCard = cardToShow;
            cardModelToShow.ClonesOnDrag = true;
            cardModelToShow.DoubleClickEvent = new OnDoubleClickDelegate(deckEditor.AddCardModel);
        }

        countText.text = (CurrentPageIndex + 1) + "/" + (TotalPageCount + 1);
    }

    public void ShowSearchMenu()
    {
        SearchMenu.Show(SetNameInputField, SetFiltersText, ShowResults);
    }

    public void ShowResults(List<Card> results)
    {
        AllResults = results;
    }

    void OnDisable()
    {
        CardGameManager.Instance.OnSelectActions.Remove(SearchMenu.ClearSearch);
    }

    public SearchMenu SearchMenu {
        get {
            if (_searchMenu == null)
                _searchMenu = Instantiate(searchMenuPrefab, this.gameObject.FindInParents<Canvas>().transform).GetOrAddComponent<SearchMenu>();
            return _searchMenu;
        }
    }

    public List<Card> AllResults {
        get {
            if (_allResults == null)
                _allResults = new List<Card>();
            return _allResults;
        }
        set {
            _allResults = value;
            CurrentPageIndex = 0;
            UpdateSearchResultsPanel();
        }
    }
}