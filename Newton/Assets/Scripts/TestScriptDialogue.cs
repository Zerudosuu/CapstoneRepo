using System.Collections;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using UnityEngine;

public class TestScriptDialogue : MonoBehaviour
{
    public DialogueTreeController dialogue;

    void Start()
    {
        dialogue.StartDialogue();
    }

    public void TRYTHISCONSOLELOG()
    {
        print("HOOOOOOOEAY");
    }
}
