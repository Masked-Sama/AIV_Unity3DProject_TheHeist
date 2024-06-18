using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    [SerializeField]
    private GameObject playerText;

    private Text uiText;
    private void Awake()
    {
        GlobalEventManager.AddListener(GlobalEventIndex.ShowStringInUI, ShowStringUI);
        GlobalEventManager.AddListener(GlobalEventIndex.HideStringInUI, HideStringUI);
        uiText = playerText.GetComponent<Text>();
    }

    private void ShowStringUI(GlobalEventArgs message)
    {
        GlobalEventArgsFactory.ShowStringInUIParser(message, out string stringToSet, out Color color, out uint fontSize);
        uiText.text = stringToSet;
        uiText.color = color;
        uiText.fontSize = (int)fontSize;
        playerText.SetActive(true);
    }
    private void HideStringUI(GlobalEventArgs message)
    {
        playerText.SetActive(false);
    }
}
