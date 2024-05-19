using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private VisualElement Container;

    #region TaskBook
    private VisualElement TaskBookUI;
    private VisualElement TaskBookContainer;
    private Button TaskBookButton;
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

    // private Button[] Slot;
    #endregion

    [Obsolete]
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Container = root.Q<VisualElement>("Container");

        #region TaskBookInit
        TaskBookUI = root.Q<VisualElement>("TaskBookUI");
        TaskBookContainer = TaskBookUI.Q<VisualElement>("TaskBookContainer");
        TaskBookButton = TaskBookUI.Q<Button>("TaskBookButton");
        Container.style.display = DisplayStyle.None;
        TaskBookButton.RegisterCallback<ClickEvent>(ToggleTaskBook);
        #endregion

        #region StoreInit
        StoreContainer = root.Q<VisualElement>("StoreContainer");

        StoreSubContainer = StoreContainer.Q<VisualElement>("StoreSubContainer");

        rightPane = StoreSubContainer.Q<VisualElement>("StoreRightPane");

        leftPane = StoreSubContainer.Q<VisualElement>("StoreLeftPane");

        ItemDetails = rightPane.Q<VisualElement>("ItemDetails");
        // rightPane = StoreContainer.Q<VisualElement>("StoreRightPane");
        // leftPane = StoreContainer.Q<VisualElement>("StoreLeftPane");
        // ItemDetails = rightPane.Q<VisualElement>("ItemDetails");

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
    }

    private void BuyItem(ClickEvent evt)
    {
        // Check if the selected item index is valid
        if (selectedItemIndex < 0 || selectedItemIndex >= storeItem.items.Count)
        {
            Debug.LogError("No item selected or invalid item index.");
        }

        ItemInfo selectedItem = storeItem.items[selectedItemIndex];

        // Find the GameManager instance
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found.");
        }

        // Check if the player has enough coins
        if (gameManager.Coins < selectedItem.price)
        {
            Debug.LogError("Not enough coins to buy the item.");
        }

        // Decrease the player's coins by the item's price
        gameManager.DecreaseCoins(selectedItem.price);

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

            itemButton.style.backgroundImage = itemInfo.itemImage.texture;
            // Add click event to the item button
            itemButton.RegisterCallback<ClickEvent>(OnSlotClick);

            // Add the item button to the grid
            Grid.Add(itemElement);
        }
    }

    void DisplayItemDetails(int index)
    {
        if (index < 0 || index >= storeItem.items.Count)
        {
            Debug.LogError("Invalid item index.");
        }

        ItemInfo selectedItem = storeItem.items[index];

        // Update the right pane with the selected item's details
        itemTitle.text = selectedItem.itemName;
        itemDescription.text = selectedItem.description;

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
            Debug.Log("Click to close");
        }
        else
        {
            TaskBookContainer.AddToClassList("TaskBookIn");
            Debug.Log("Click to open");
        }
    }

    void Update() { }
}
