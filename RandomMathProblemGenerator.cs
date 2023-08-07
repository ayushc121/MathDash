using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomMathProblemGenerator : MonoBehaviour
{
    private int operand1;
    private int operand2;
    private string operatorSymbol;
    public int answer;
    public List<int> randomizedAnswers; // Store the randomized order of answers
    private bool hasGeneratedNewNumbers = false;

    private void Start()
    {
    }

    public void GenerateRandomMathProblem()
    {
        operand1 = UnityEngine.Random.Range(0, 10);
        operand2 = UnityEngine.Random.Range(1, 10); // Ensure operand2 is not zero
        int operatorIndex = UnityEngine.Random.Range(0, 4); // 0 for addition, 1 for subtraction, 2 for multiplication, 3 for division

        switch (operatorIndex)
        {
            case 0: // Addition
                operatorSymbol = "+";
                answer = operand1 + operand2;
                break;

            case 1: // Subtraction
                operatorSymbol = "-";
                // Ensure operand2 is smaller or equal to operand1 to avoid negative answers.
                if (operand2 > operand1)
                {
                    int temp = operand1;
                    operand1 = operand2;
                    operand2 = temp;
                }
                answer = operand1 - operand2;
                break;

            case 2: // Multiplication
                operatorSymbol = "x";
                answer = operand1 * operand2;
                break;

            case 3: // Division
                operatorSymbol = "/";
                // Ensure an exact division with no remainder
                int tempAnswer = operand1 * operand2;
                operand1 = tempAnswer;
                answer = operand1 / operand2;
                break;
        }

        randomizedAnswers = GenerateRandomizedAnswers();
        hasGeneratedNewNumbers = true;
    }


    private int GenerateIncorrectAnswer()
    {
        // Generate a random number between -5 to 5
        int randomOffset = UnityEngine.Random.Range(-5, 6);
        // Make sure the incorrect answer is different from the correct answer
        while (randomOffset == 0 || randomOffset == answer)
        {
            randomOffset = UnityEngine.Random.Range(-5, 6);
        }
        return answer + randomOffset;
    }

    private List<int> GenerateRandomizedAnswers()
    {
        List<int> answers = new List<int> { answer };
        for (int i = 0; i < 2; i++)
        {
            int incorrectAnswer = GenerateIncorrectAnswer();
            while (answers.Contains(incorrectAnswer))
            {
                incorrectAnswer = GenerateIncorrectAnswer();
            }
            answers.Add(incorrectAnswer);
        }

        // Shuffle the answers list randomly
        for (int i = 0; i < answers.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, answers.Count);
            int temp = answers[i];
            answers[i] = answers[randomIndex];
            answers[randomIndex] = temp;
        }

        return answers;
    }


    private string GetProblemText()
    {
        return $"{operand1} {operatorSymbol} {operand2} = ?";
    }

    private void OnGUI()
    {
        int labelWidth = 200;
        int labelHeight = 50;

        string problemText = GetProblemText();

        // Draw the problem text in the middle of the screen
        GUI.Label(new Rect(Screen.width / 2 - labelWidth / 2, Screen.height - labelHeight, labelWidth, labelHeight), problemText);

        // Draw each answer label in the middle of the screen in one row
        if (hasGeneratedNewNumbers)
        {
            for (int i = 0; i < randomizedAnswers.Count; i++)
            {
                GUI.Label(new Rect(Screen.width / 2 - labelWidth / 2 + (labelWidth + 10) * i / 2, Screen.height / 2 - labelHeight / 2 + 25, labelWidth, labelHeight), randomizedAnswers[i].ToString());
            }
        }
    }
}