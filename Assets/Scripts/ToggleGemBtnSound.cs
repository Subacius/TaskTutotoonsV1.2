using UnityEngine;
using UnityEngine.UI;

public class ToggleGemBtnSound : MonoBehaviour
{
    public Toggle soundToggle;
    private bool isSoundOn = true;
    [SerializeField] private GameCore gameCore;

    private void Start()
    {
        soundToggle.onValueChanged.AddListener(OnToggleValueChanged);
        OnToggleValueChanged(isSoundOn);
        gameCore.onLevelChangedCheckAudio += OnLevelChangedCheckAudio;
    }

    private void OnLevelChangedCheckAudio()
    {
        if (!isSoundOn)
        {
            GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");

            foreach (GameObject gem in gems)
            {
                Gem gemScript = gem.GetComponent<Gem>();
                if (gemScript != null)
                {
                    gemScript.audioSource.mute = true;
                }
            }
        }
    }

    private void OnToggleValueChanged(bool isMuted)
    {
        isSoundOn = isMuted;

        GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");

        foreach (GameObject gem in gems)
        {
            Gem gemScript = gem.GetComponent<Gem>();
            if (gemScript != null)
            {
                gemScript.audioSource.mute = !isSoundOn;
            }
        }
    }
}
