using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    // Name of the NPC
    public Text nameTxt;

    // Dialogue of the NPC
    public Text dialogueTxt;

    // Boolean check to keep track of typewriter text
    public bool textIsTyping = false;

    // Speed of which the text writes (Should be changeable in Settings)
    public float textSpeed = 0.001f;

    // Object in world that corresponds to the visual of the TextBoxes
    public GameObject textSystem;

    // The skeleton of what the NPC will be saying (dialogue will be processed through here)
    private Queue<string> sentences;


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue d)
    {
        textSystem.SetActive(true); // visaully show the text box
        nameTxt.text = d.npcName; // assign name
        sentences.Clear(); // clean slate for conversations

        foreach(string sentence in d.sentences)
        {
            sentences.Enqueue(sentence); // fill the skeleton
        }

        DisplayNextSentence(d); // go to first message
    }

    public void DisplayNextSentence(Dialogue d)
    {

        if (sentences.Count == 0)
        {
            EndDialogue(d);
            return;
        }

        string sent = sentences.Dequeue();

        // dialogueTxt.text = sent;

        textIsTyping = true;
        StartCoroutine(TypeSentence(sent, d));
    }

    public void EndDialogue(Dialogue d)
    {
        Debug.Log("finished convo, resetting");

        sentences.Clear();

        d.speaking = false;
        textSystem.SetActive(false);

    }

    IEnumerator TypeSentence(string sent, Dialogue d)
    {
        dialogueTxt.text = "";

        Debug.Log("how many time we in here");

        foreach(char letter in sent.ToCharArray())
        {
            dialogueTxt.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        textIsTyping = false;
    }
}

