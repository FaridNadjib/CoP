using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// This class will hold all text from npcs and will manage it.
/// </summary>
public class DialogManager : MonoBehaviour
{
    [Tooltip("The npc this trigger belongs too.")]
    [SerializeField] private NpcManager npc;

    [SerializeField] private Interactable item;

    [Header("Dialog:")]
    [Tooltip("The text a npc can talk before any combat.")]
    [SerializeField] private Dialog preCombatDialog;

    [Tooltip("The text a npc can talk after combat and if he has no combat at all.")]
    [SerializeField] private Dialog dialog;

    [SerializeField] private int lastLineIndex;
    private int dialogIndex;

    private TextMeshProUGUI dialogTextField;
    [SerializeField] private float normalSpeed = 0.22f;
    private float speed;
    private bool finishedLine;
    private bool endDialog;

    /// <summary>
    /// Start dialog on enabled.
    /// </summary>
    private void OnEnable()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.ShowDialogScreen(true);

        dialogTextField = UIManager.Instance.GetDialogTextField();
        speed = normalSpeed;
        dialogTextField.text = "";

        finishedLine = false;
        if (npc != null && npc.CanFight)
            StartCoroutine("TextTyping", preCombatDialog.dialogs[dialogIndex].text);
        else
            StartCoroutine("TextTyping", dialog.dialogs[dialogIndex].text);
    }

    private void OnDisable()
    {
        UIManager.Instance.ShowDialogScreen(false);
    }

    private void Update()
    {
        // Let the player scroll through the dialoglines.
        if (Input.GetKeyDown(KeyCode.Space) && endDialog)
        {
            if (npc != null && npc.CanFight)
            {
                endDialog = false;
                dialogIndex = 0;
                npc.FinishedPreCombat();
            }
            else if (npc != null)
            {
                endDialog = false;
                dialogIndex = lastLineIndex;
                if (npc != null)
                    npc.FinishedPostCombat();
            }
            else if (item != null)
            {
                endDialog = false;
                dialogIndex = lastLineIndex;
                item.FinishedInteraction();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && finishedLine && !endDialog)
        {
            if (npc != null && npc.CanFight)
            {
                StartCoroutine("TextTyping", preCombatDialog.dialogs[dialogIndex].text);
            }
            else
            {
                StartCoroutine("TextTyping", dialog.dialogs[dialogIndex].text);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !finishedLine)
        {
            speed = 0.01f;
        }
    }

    /// <summary>
    /// Start the text flow.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private IEnumerator TextTyping(string message)
    {
        dialogTextField.text = "";
        speed = normalSpeed;
        finishedLine = false;

        if (npc != null && npc.CanFight)
        {
            npc.ShowEmotion(preCombatDialog.dialogs[dialogIndex].emotion);
        }
        else if (npc != null)
        {
            npc.ShowEmotion(dialog.dialogs[dialogIndex].emotion);
        }
        else if (item != null)
        {
            item.ShowEmotion(dialog.dialogs[dialogIndex].emotion);
        }

        foreach (var singleChar in message.ToCharArray())
        {
            yield return new WaitForSecondsRealtime(speed);
            dialogTextField.text += singleChar;
        }
        dialogIndex++;
        finishedLine = true;

        if (npc != null && npc.CanFight && dialogIndex == preCombatDialog.dialogs.Length)
        {
            endDialog = true;
        }
        else if (dialogIndex == dialog.dialogs.Length)
        {
            dialogIndex = lastLineIndex;
            endDialog = true;
        }
    }
}