using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    GameManager gameManager;
    QuestGiver questGiver;

    public GameObject ArcadeCamera;
    private VisualElement Container;

    #region TaskBook
    private VisualElement TaskBookUI;
    private VisualElement TaskBookContainer;
    private Button TaskBookButton;

    private VisualElement TaskBookImage1;
    private VisualElement TaskBookImage2;
    private VisualElement tabsbuttoncontainer;
    private VisualElement tabsContainer;

    private VisualElement tab1;
    private VisualElement tab2;
    private VisualElement tab3;
    private Button tab1Button;
    private Button tab2Button;
    private Button tab3Button;

    private VisualTreeAsset QuestItemTemplate;

    #endregion

    #region StoreUI
    public StoreItem storeItem;
    private VisualElement StoreContainer;
    private VisualElement StoreSubContainer;
    private VisualElement rightPane;
    private VisualElement leftPane;
    private VisualElement ItemDetails;
    private Label itemTitle;
    private VisualElement imageContainer;
    private Label itemDescription;
    private Button buyButton;
    private VisualElement Grid;
    public VisualTreeAsset storeItemButtonTemplate;

    private Button CloseButton;

    private int selectedItemIndex = 0;

    private Label CoinCount;

    private VisualElement CharacterDetails;

    // private Button[] Slot;
    #endregion


    #region Arcade
    VisualElement Arcade;
    Button PlayGame;
    #endregion
    [Obsolete]
    void Start()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        questGiver = GameObject.FindAnyObjectByType<QuestGiver>();

        var root = GetComponent<UIDocument>().rootVisualElement;

        #region TaskBookInit



        TaskBookUI = root.Q<VisualElement>("TaskBookUI");
        TaskBookContainer = TaskBookUI.Q<VisualElement>("TaskBookContainer");
        TaskBookButton = TaskBookUI.Q<Button>("TaskBookButton");
        TaskBookImage1 = TaskBookButton.Q<VisualElement>("TaskButtonImage1");
        TaskBookImage2 = TaskBookButton.Q<VisualElement>("TaskButtonImage2");

        TaskBookUI.style.display = DisplayStyle.None;
        TaskBookContainer.style.display = DisplayStyle.None;

        TaskBookImage2.style.display = DisplayStyle.None;

        // Container.style.display = DisplayStyle.None;
        TaskBookButton.RegisterCallback<ClickEvent>(ToggleTaskBook);

        tabsbuttoncontainer = TaskBookContainer.Q<VisualElement>(
            "TaskBookContainerTabButtonContainer"
        );
        tab1Button = tabsbuttoncontainer.Q<Button>("Tab1Button");
        tab2Button = tabsbuttoncontainer.Q<Button>("Tab2Button");
        tab3Button = tabsbuttoncontainer.Q<Button>("Tab3Button");

        tab1Button.style.display = DisplayStyle.None;
        tab2Button.style.display = DisplayStyle.None;
        tab3Button.style.display = DisplayStyle.None;

        tabsContainer = TaskBookContainer.Q<VisualElement>("TasbContentContainer");

        tab1 = tabsContainer.Q<VisualElement>("Quest");
        tab2 = tabsContainer.Q<VisualElement>("Bag");
        tab3 = tabsContainer.Q<VisualElement>("RecordBook");

        tab1Button.RegisterCallback<ClickEvent>(evt => ShowTabContent(tab1));
        tab2Button.RegisterCallback<ClickEvent>(evt => ShowTabContent(tab2));
        tab3Button.RegisterCallback<ClickEvent>(evt => ShowTabContent(tab3));

        QuestItemTemplate = Resources.Load<VisualTreeAsset>("QuestItem");

        #endregion

        #region StoreInit
        StoreContainer = root.Q<VisualElement>("StoreContainer");

        StoreSubContainer = StoreContainer.Q<VisualElement>("StoreSubContainer");

        CharacterDetails = StoreContainer.Q<VisualElement>("CharacterDetails");
        // CoinCount = CharacterDetails.Q<Label>("CoinLabel");

        // CoinCount.text = gameManager.Coins.ToString();

        rightPane = StoreSubContainer.Q<VisualElement>("StoreRightPane");

        leftPane = StoreSubContainer.Q<VisualElement>("StoreLeftPane");

        ItemDetails = rightPane.Q<VisualElement>("ItemDetails");

        CloseButton = StoreContainer.Q<Button>("CloseButton");

        Grid = leftPane.Q<VisualElement>("Grid");
        storeItemButtonTemplate = Resources.Load<VisualTreeAsset>("ItemSlotStore");

        itemTitle = ItemDetails.Q<Label>("ItemTitle");
        imageContainer = ItemDetails.Q<VisualElement>("ImageHolder");
        itemDescription = ItemDetails.Q<Label>("ItemDescription");
        buyButton = ItemDetails.Q<Button>("buyButton");

        StoreContainer.style.display = DisplayStyle.None;
        buyButton.RegisterCallback<ClickEvent>(BuyItem);
        CloseButton.RegisterCallback<ClickEvent>(CloseStore);
        PopulateSlots();
        #endregion

        #region  ArcadeInit
        Arcade = root.Q<VisualElement>("MiniGame");
        PlayGame = Arcade.Q<Button>("PlayGame");

        PlayGame.RegisterCallback<ClickEvent>(PlayMiniGame);

        Arcade.style.display = DisplayStyle.None;
        ArcadeCamera.SetActive(false);
        #endregion
    }

    private void PlayMiniGame(ClickEvent evt)
    {
        SceneManager.LoadScene("MiniGames");
    }

    public void ShowTheTaskBookUI()
    {
        TaskBookUI.style.display = DisplayStyle.Flex;
        TaskBookContainer.style.display = DisplayStyle.Flex;
    }

    public void EnableCameraArcade()
    {
        ArcadeCamera.SetActive(true);
        StartCoroutine(DelayedDisplayArcade());
    }

    private IEnumerator DelayedDisplayArcade()
    {
        yield return new WaitForSeconds(1.0f); // Replace 1.0f with your desired delay in seconds
        Arcade.style.display = DisplayStyle.Flex;
    }

    private void ShowTabContent(VisualElement CurrentTab)
    {
        // Hide all tabs
        tab1.style.display = DisplayStyle.None;
        tab2.style.display = DisplayStyle.None;
        tab3.style.display = DisplayStyle.None;

        // Show the selected tab
        CurrentTab.style.display = DisplayStyle.Flex;
    }

    private void BuyItem(ClickEvent evt)
    {
        // Check if the selected item index is valid
        if (selectedItemIndex < 0 || selectedItemIndex >= storeItem.items.Count)
        {
            Debug.LogError("No item selected or invalid item index.");
        }

        ItemInfo selectedItem = storeItem.items[selectedItemIndex];

        // Check if the player has enough coins
        if (gameManager.Coins < selectedItem.price)
        {
            Debug.LogError("Not enough coins to buy the item.");
        }

        // Decrease the player's coins by the item's price
        gameManager.DecreaseCoins(selectedItem.price);

        CoinCount.text = gameManager.Coins.ToString();

        // Additional logic for handling the purchased item can be added here
        // For example, adding the item to the player's inventory
    }

    private void CloseStore(ClickEvent evt)
    {
        StoreContainer.style.display = DisplayStyle.None;
        StoreContainer.AddToClassList("StoreContainer-hide");
    }

    private void OnSlotClick(ClickEvent evt)
    {
        Button clickedButton = evt.currentTarget as Button;
        if (clickedButton != null)
        {
            int index = (int)clickedButton.userData;
            Debug.Log("Button clicked: " + clickedButton.name + " at index: " + index);
            selectedItemIndex = index;
            DisplayItemDetails(index);
        }
    }

    public void OpenStore()
    {
        StoreContainer.style.display = DisplayStyle.Flex;
        StoreContainer.RemoveFromClassList("StoreContainer-hide");
    }

    void PopulateSlots()
    {
        for (int i = 0; i < storeItem.items.Count; i++)
        {
            ItemInfo itemInfo = storeItem.items[i];

            // Instantiate the template
            TemplateContainer itemElement = storeItemButtonTemplate.CloneTree();
            Button itemButton = itemElement.Q<Button>("ItemButton");
            itemButton.userData = i; // Store the index of the item

            VisualElement itemButtonImageHolder = itemButton.Q<VisualElement>("ButtonImageHolder");

            itemButtonImageHolder.style.backgroundImage = itemInfo.itemImage.texture;
            // Add click event to the item button
            itemButton.RegisterCallback<ClickEvent>(OnSlotClick);

            // Add the item button to the grid
            Grid.Add(itemElement);
        }
    }

    void PopulateQuestSlots() { }

    void DisplayItemDetails(int index)
    {
        if (index < 0 || index >= storeItem.items.Count)
        {
            Debug.LogError("Invalid item index.");
        }

        ItemInfo selectedItem = storeItem.items[index];

        // Update the right pane with the selected item's details
        itemTitle.text = selectedItem.itemName;

        // Update the image
        imageContainer.Clear();
        var imageElement = new Image
        {
            image = selectedItem.itemImage.texture,
            scaleMode = ScaleMode.StretchToFill // Adjust as needed
        };
        imageContainer.Add(imageElement);
    }

    private void ToggleTaskBook(ClickEvent evt)
    {
        if (TaskBookContainer.ClassListContains("TaskBookIn"))
        {
            TaskBookContainer.RemoveFromClassList("TaskBookIn");
            tab1Button.style.display = DisplayStyle.None;
            tab2Button.style.display = DisplayStyle.None;
            tab3Button.style.display = DisplayStyle.None;
            TaskBookImage1.style.display = DisplayStyle.Flex;
            TaskBookImage2.style.display = DisplayStyle.None;

            Debug.Log("Click to close");
        }
        else
        {
            TaskBookContainer.AddToClassList("TaskBookIn");
            tab1Button.style.display = DisplayStyle.Flex;
            tab2Button.style.display = DisplayStyle.Flex;
            tab3Button.style.display = DisplayStyle.Flex;
            TaskBookImage1.style.display = DisplayStyle.None;
            TaskBookImage2.style.display = DisplayStyle.Flex;
            Debug.Log("Click to open");
        }
    }

    void Update()
    {
        if (questGiver.isQuestWindowOpen)
        {
            TaskBookUI.style.display = DisplayStyle.None;
        }
        else
        {
            TaskBookUI.style.display = DisplayStyle.Flex;
        }
    }
}
