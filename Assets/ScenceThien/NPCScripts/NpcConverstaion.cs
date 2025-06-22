using UnityEngine;
using System.Collections;

public class NPCConversation : MonoBehaviour
{
    public IEnumerator StartConversation(NPCController self, NPCController other, float duration, System.Action onEnd)
    {
        self.BeginConversation();
        other.BeginConversation();

        // Xoay cả hai về nhau
        yield return self.StartCoroutine(self.RotateTowards(other.transform));
        other.StartCoroutine(other.RotateTowards(self.transform));

        // Hiển thị panel
        if (self.dialoguePanel && self.dialogueText)
        {
            self.dialoguePanel.SetActive(true);
            self.dialogueText.text = $"{self.gameObject.name}: Xin chào {other.gameObject.name}!";
        }

        yield return new WaitForSeconds(duration);

        if (self.dialoguePanel)
            self.dialoguePanel.SetActive(false);

        self.EndConversation();
        other.EndConversation();
        onEnd?.Invoke();
    }
}

