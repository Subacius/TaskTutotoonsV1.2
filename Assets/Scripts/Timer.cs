using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float startTime;
    public bool isTimerRunning = false;
    private float currentTime = 0f;
    private float bestTime = Mathf.Infinity;

    private void Start()
    {
        timerText = GetComponent<Text>();
        startTime = Time.time;
        isTimerRunning = true;
        LoadBestTime();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime = Time.time - startTime;
            timerText.text = currentTime.ToString("F2");
        }
    }

    public void SaveTimerText()
    {
        if (currentTime < bestTime)
        {
            bestTime = currentTime;

            string textToSave = bestTime.ToString("F2");
            SaveObject saveObject = new SaveObject();
            saveObject.text = textToSave;

            string jsonString = JsonUtility.ToJson(saveObject);
            string filePath = Application.persistentDataPath + "/saveData.json";
            File.WriteAllText(filePath, jsonString);
        }
    }

    public float GetBestTime()
    {
        return bestTime;
    }

    private void LoadBestTime()
    {
        string filePath = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(jsonString);
            bestTime = float.Parse(saveObject.text);
        }
    }

    [System.Serializable]
    public class SaveObject
    {
        public string text;
    }
}
