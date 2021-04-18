using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue dialogues;
    public string dialogueStrName;
    void Start()
    {
        dialogues = this.GetComponent<Dialogue>();
        if (!GameManager.GM.passLevel1)
            dialogues.ChangeAsset("level1"+dialogueStrName);
        else if (!GameManager.GM.passLevel2)
            dialogues.ChangeAsset("level2" + dialogueStrName);
        else
            dialogues.ChangeAsset("level3" + dialogueStrName);
    }
}
