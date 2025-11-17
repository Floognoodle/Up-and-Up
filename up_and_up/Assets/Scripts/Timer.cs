using UnityEngine;
using TMPro;

public class PlayTimeTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Sprite upAndUpSpace;
    public SpawnManagerSky spawnManager;
    public string[] backgroundNames = new string[] { "Background Sky", "Background Sky2", "Background Sky3" };

    private float elapsedTime = 0f;
    private bool minuteReached = false;
    private bool GameOver = false;

    void Awake()
    {
        if (timerText == null)
        {
            GameObject go = GameObject.Find("Time");
            if (go != null)
            {
                timerText = go.GetComponent<TextMeshProUGUI>();
            }
        }

        if (spawnManager == null)
        {
            spawnManager = FindObjectOfType<SpawnManagerSky>();
        }
    }

    void Update()
    {
        // Stop updating when gnome dies
        if (GameOver) return;

        if (timerText == null) return;

        elapsedTime = elapsedTime + Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // Minutes-seconds format
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Trigger at 1 minute
        if (!minuteReached && elapsedTime >= 60f)
        {
            minuteReached = true;
            OnOneMinuteReached();
        }
    }

    // Switches hazard & background after 1 minute to space theme
    private void OnOneMinuteReached()
    {
        if (upAndUpSpace != null)
        {
            for (int i = 0; i < backgroundNames.Length; i++)
            {
                string name = backgroundNames[i];
                GameObject bg = GameObject.Find(name);
                if (bg != null)
                {
                    SpriteRenderer sr = bg.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite = upAndUpSpace;
                    }
                }
            }
        }

        if (spawnManager != null)
        {
            spawnManager.SetMinutePassed(true);
        }
    }

    // End game
    public void ShowLose()
    {
        if (timerText == null) return;
        if (GameOver) return;

        timerText.text = timerText.text + " You Lose";
        GameOver = true;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        minuteReached = false;
        GameOver = false;

        if (timerText != null)
        {
            timerText.text = "00:00";
        }

        if (spawnManager != null)
        {
            spawnManager.SetMinutePassed(false);
        }
    }
}