using UnityEngine;

public class OpenGacha : MonoBehaviour
{
    public GameObject panelGacha;
    public GameObject closeBtn;

    private void Start()
    {
        panelGacha.SetActive(false);
        closeBtn.SetActive(false);

    }

    public void OpenGachaPanel()
    {
        panelGacha.SetActive(true);
        closeBtn.SetActive(true);
    }
    public void CloseGachaPanel()
    {
        panelGacha.SetActive(false);
        closeBtn.SetActive(false);
    }

    public void OpenCase()
    {

    }
}
