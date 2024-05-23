using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuizManager : MonoBehaviour
{
    public QuizTree quizTree;
    private int currentQuestionIndex = 0;

    private VisualElement QuizContainer;

    private VisualElement MainScreen;
    private Button buttonStart;

    private VisualElement QuestionContainer;

    private Label Question;

    private VisualElement QuizButtonContainer;

    private VisualTreeAsset buttonChoice;

    void Start()
    {
        var root = GameObject.FindAnyObjectByType<UIDocument>().rootVisualElement;

        QuizContainer = root.Q<VisualElement>("QuizContainer");
        MainScreen = root.Q<VisualElement>("MainScreen");

        buttonStart = MainScreen.Q<Button>("StartButton");
        QuestionContainer = QuizContainer.Q<VisualElement>("QuestionContainer");
        QuizButtonContainer = QuizContainer.Q<VisualElement>("QuizButtonContainer");

        Question = QuestionContainer.Q<Label>("Question");

        buttonChoice = Resources.Load<VisualTreeAsset>("OptionTemplate");

        quizTree.ShuffleQuizItems();

        currentQuestionIndex = 0;

        QuizContainer.style.display = DisplayStyle.None;

        buttonStart.RegisterCallback<ClickEvent>(StarGame);
    }

    private void StarGame(ClickEvent evt)
    {
        MainScreen.style.display = DisplayStyle.None;
        QuizContainer.style.display = DisplayStyle.Flex;

        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex < 0 || currentQuestionIndex >= quizTree.QuizDetails.Count)
        {
            Debug.LogError("Question index out of range.");
            return;
        }

        // Clear previous buttons
        QuizButtonContainer.Clear();

        QuizDetails currentQuestion = quizTree.QuizDetails[currentQuestionIndex];
        Question.text = currentQuestion.Question;

        for (int i = 0; i < currentQuestion.Answers.Count; i++)
        {
            string buttonNameChoice = currentQuestion.Answers[i];

            // Clone the template to create a new element
            TemplateContainer itemElement = buttonChoice.CloneTree();
            // Find and update the Button element
            Button choice = itemElement.Q<Button>("OptionButton");
            choice.text = buttonNameChoice;

            // Capture the current index for the click event
            int answerIndex = i;
            choice.clicked += () => OnAnswerSelected(answerIndex);

            // Add the new element to the container
            QuizButtonContainer.Add(itemElement);
        }
    }

    void OnAnswerSelected(int selectedAnswerIndex)
    {
        QuizDetails currentQuestion = quizTree.QuizDetails[currentQuestionIndex];
        bool isCorrect = selectedAnswerIndex == currentQuestion.CorrectAnswer;

        if (isCorrect)
        {
            Debug.Log("Correct answer!");
        }
        else
        {
            Debug.Log("Wrong answer.");
        }

        // Load next question or show results, etc.
        currentQuestionIndex++;
        if (currentQuestionIndex <= quizTree.QuizDetails.Count - 1)
        {
            DisplayQuestion();
        }
        else
        {
            QuestionContainer.Clear();
            QuizButtonContainer.Clear();
            Debug.Log("Quiz completed!");
            // Handle end of quiz
        }
    }
}
