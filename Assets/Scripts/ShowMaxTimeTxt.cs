using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShowMaxTimeTxt : MonoBehaviour
{
    private SaveObject saveObject;
    [SerializeField] private Text bestTimeTxt;

    private void Start()
    {
        string filePath = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(filePath))
        {
            ShowBestTime();
        }
    }

    public void ShowBestTime()
    {
        string filePath = Application.persistentDataPath + "/saveData.json";
        if (saveObject == null)
        {
            string jsonString = File.ReadAllText(filePath);
            saveObject = JsonUtility.FromJson<SaveObject>(jsonString);
        }

        bestTimeTxt.text = "Your best time: " + saveObject.text + " s";
    }

    [System.Serializable]
    public class SaveObject
    {
        public string text;
    }
}
