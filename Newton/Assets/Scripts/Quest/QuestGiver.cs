using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class QuestGiver : MonoBehaviour
{
    #region  UITOOLKIT

    private VisualElement Container;
    private VisualElement ObjectToFind;
    private VisualElement TestContainer;

    public Label titleText;
    public Label descriptionText;

    private Button AcceptButton;

    #endregion

    public Quest quest;

    public bool IsActive => quest.isActive;
    public PlayerQuest player;

    public bool QuestWindowIsOpen = false;

    #region GUISection
    public GameObject questWindow;

    public GameObject questItemContainer;

    public GameObject itemNamePrefab; // Prefab for the item name

    // public TextMeshProUGUI titleText;
    // public TextMeshProUGUI descriptionText;
    #endregion



    void Start()
    {
        var root = GameObject.FindAnyObjectByType<UIDocument>().rootVisualElement;

        Container = root.Q<VisualElement>("Container");
        ObjectToFind = root.Q<VisualElement>("ObjectToFind");
        titleText = root.Q<Label>("TitleText");
        descriptionText = root.Q<Label>("DescriptionText");
        TestContainer = root.Q<VisualElement>("TestContainer");
        Container.style.display = DisplayStyle.None;

        AcceptButton = root.Q<Button>("AcceptButton");
        AcceptButton.RegisterCallback<ClickEvent>(AcceptQuest);
    }

    public void OpenQuestWindow()
    {
        TestContainer.ToggleInClassList("RightIn");
        Container.style.display = DisplayStyle.Flex;
        titleText.text = quest.title;
        descriptionText.text = "Description: " + quest.description;

        // Clear existing items in the container
        ObjectToFind.Clear();

        // Instantiate Label for each quest item and set text
        foreach (string questItemText in quest.quest)
        {
            Label newLabel = new Label();
            newLabel.text = questItemText;
            ObjectToFind.Add(newLabel);
        }
    }

    public void AcceptQuest(ClickEvent evt)
    {
        TestContainer.ToggleInClassList("RightIn");
        QuestWindowIsOpen = false;

        quest.isActive = true;

        //give to player
        player.quest = quest;
        quest.questGoal.reqAmount = quest.quest.Count;

        Container.style.display = DisplayStyle.None;
    }
}
