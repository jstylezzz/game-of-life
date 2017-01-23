using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private Text m_statusText;

    public void UpdateLoadingStatus(int current, int max)
    {
        m_statusText.text = "Game state: loading (" + current + " of " + max + ")";
    }

    public void OnGamePause()
    {
        m_statusText.text = "Game state: editor mode";
    }

    public void OnGamePlay()
    {
        m_statusText.text = "Game state: simulating";
    }
	
}
