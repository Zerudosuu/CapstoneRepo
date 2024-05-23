using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class QuestGiver : MonoBehaviour
{
    #region  UITOOLKIT

    // MainContainer
    private VisualElement QuestWindowContainer;

    // QuestWindowContainer
    private VisualElement QuestWindow;

    // QuestTitle
    private Label QuestTitle;

    //QuestDescription
    private ScrollView QuestDescriptionContainer;
    private Label QuestDescription;

    // QuestObjectContainer
    private VisualElement ItemtoFindContainer;
    private ScrollView ItemsToCollectContainer;

    private VisualElement ObjectContainer;

    //QuestButtons
    private VisualElement QuestButtonContainer;
    private Button AcceptQuestButton;
    private Button DeclineQuestButton;

    private VisualTreeAsset ObjectToCollect;

    #endregion


    public PlayerQuest player;

    public bool isQuestWindowOpen = false;

    public QuestTree questTree;
    public int QuestCurrentIndex = 0;
    public bool IsActive => questTree.story[QuestCurrentIndex].isActive;

    public int requireQuestAmount = 0;

    void Start()
    {
        var root = GameObject.FindAnyObjectByType<UIDocument>().rootVisualElement;

        QuestWindowContainer = root.Q<VisualElement>("QuestWindowContainer");
        QuestWindow = QuestWindowContainer.Q<VisualElement>("QuestWindow");
        QuestTitle = QuestWindow.Q<Label>("QuestTitle");
        QuestDescriptionContainer = QuestWindow.Q<ScrollView>("QuestDescriptionContainer");
        QuestDescription = QuestDescriptionContainer.Q<Label>("QuestDescription");
        ItemtoFindContainer = QuestWindow.Q<VisualElement>("ItemtoFindContainer");
        ItemsToCollectContainer = ItemtoFindContainer.Q<ScrollView>("ItemsToCollectContainer");
        ObjectContainer = ItemsToCollectContainer.Q<VisualElement>("unity-content-container");

        QuestButtonContainer = QuestWindow.Q<VisualElement>("QuestButtonContainer");
        AcceptQuestButton = QuestButtonContainer.Q<Button>("ButtonAccept");
        DeclineQuestButton = QuestButtonContainer.Q<Button>("ButtonDecline");

        ObjectToCollect = Resources.Load<VisualTreeAsset>("QuestItem");

        AcceptQuestButton.RegisterCallback<ClickEvent>(AcceptQuest);
        DeclineQuestButton.RegisterCallback<ClickEvent>(DeclineQuest);

        QuestWindowContainer.style.display = DisplayStyle.None;

        foreach (var quest in questTree.story)
        {
            if (quest.isActive)
            {
                quest.ResetQuest();

                for (int i = 0; i < quest.QuestRequiredItem.Count; i++)
                {
                    quest.QuestRequiredItem[i].questGoal.ResetGoal();
                }
            }
        }

        // QuestWindow.style.display = DisplayStyle.None;
    }

    public void OpenQuestWindow()
    {
        QuestWindowContainer.style.display = DisplayStyle.Flex;
        QuestTitle.text = questTree.story[QuestCurrentIndex].title;
        QuestDescription.text = questTree.story[QuestCurrentIndex].description;

        isQuestWindowOpen = true;
        PopulateQuestObjects();
    }

    public void PopulateQuestObjects()
    {
        // Ensure container is ready and clear any existing items
        ItemsToCollectContainer?.Clear();

        // Loop through each quest object
        for (int i = 0; i < questTree.story[QuestCurrentIndex].QuestRequiredItem.Count; i++)
        {
            string questObject = questTree.story[QuestCurrentIndex].QuestRequiredItem[i].ItemName;

            // Clone the template to create a new element
            TemplateContainer itemElement = ObjectToCollect.CloneTree();
            ;

            // Find and update the Label element
            Label objectName = itemElement.Query<Label>("QuestItemLabel");
            objectName.text = questObject;

            // Add the new element to the container
            ObjectContainer.Add(itemElement);
        }
    }

    public void AcceptQuest(ClickEvent evt)
    {
        questTree.story[QuestCurrentIndex].isActive = true;

        //give to player
        player.AssignQuest(questTree.story[QuestCurrentIndex]);

        foreach (var quest in questTree.story[QuestCurrentIndex].QuestRequiredItem)
        {
            requireQuestAmount = quest.ItemQuantity;

            quest.questGoal.reqAmount = requireQuestAmount;
        }

        print(requireQuestAmount);
        player.currentQuest.QuestRequiredItem[QuestCurrentIndex].questGoal.reqAmount =
            requireQuestAmount;

        QuestWindowContainer.style.display = DisplayStyle.None;

        isQuestWindowOpen = false;
    }

    private void DeclineQuest(ClickEvent evt)
    {
        QuestWindowContainer.style.display = DisplayStyle.None;
        isQuestWindowOpen = false;
    }
}
