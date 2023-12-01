using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] lines;
    public float textSpeed = 1f;
    private int index;
    private bool isWriting = false;
    public PlayerController playerController;

    void OnEnable()
    {
        dialogueText.text = string.Empty;
        isWriting = false;
        index = 0;
        playerController.enabled = false;
    }

    void OnDisable()
    {
        playerController.enabled = true;
    }

    void Update()
    {
        if (isWriting)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
                isWriting = false;
            }
        }
        else
        {
            if (index < lines.Length)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    NextLine();
                }
            }
        }
    }

    public void writeArray(string[] newLines)
    {
        lines = newLines;
        index = 0;
        dialogueText.text = string.Empty;
        isWriting = true;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        if (index < lines.Length)
        {
            foreach (char c in lines[index].ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
            isWriting = false;
        }
        else
        {
            index = 0;
            isWriting = false;
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            isWriting = true;
            StartCoroutine(WriteLine());
        }
        else
        {
            index = 0;
            lines = null;
            dialogueText.text = string.Empty;
            playerController.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
