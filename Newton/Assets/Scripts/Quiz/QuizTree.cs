using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizDatabase", menuName = "Quiz")]
public class QuizTree : ScriptableObject
{
    public List<QuizDetails> QuizDetails = new();

    public void ShuffleQuizItems()
    {
        // Fisher-Yates shuffle algorithm for the list of quiz details
        for (int i = QuizDetails.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            QuizDetails temp = QuizDetails[i];
            QuizDetails[i] = QuizDetails[j];
            QuizDetails[j] = temp;
        }

        // Shuffle answers within each quiz item
        foreach (var quiz in QuizDetails)
        {
            quiz.ShuffleAnswers();
        }
    }
}

[System.Serializable]
public class QuizDetails
{
    public GradeLevel level;

    public string Question;

    public List<string> Answers;

    public int CorrectAnswer;

    public bool IsCorrectAnswer(int selectedAnswer)
    {
        return selectedAnswer == CorrectAnswer;
    }

    public void ShuffleAnswers()
    {
        // Save the correct answer text before shuffling
        string correctAnswerText = Answers[CorrectAnswer];

        // Fisher-Yates shuffle algorithm
        for (int i = Answers.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = Answers[i];
            Answers[i] = Answers[j];
            Answers[j] = temp;
        }

        // Update CorrectAnswer to the new index of the correct answer text
        CorrectAnswer = Answers.IndexOf(correctAnswerText);
    }
}

public enum GradeLevel
{
    GradeSeven,
    GradeEight,
    GradeNine,
    GradeTen,
}
