using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShowTxtAfterLoad : MonoBehaviour
{
    [SerializeField] private Text loadedText;
    private SaveObject saveObject;

    private void Start()
    {
        string filePath = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(filePath))
        {
            LoadTimerText();
        }
        else
        {
            loadedText.text = "Join the points as fast as You can.";
        }
    }

    public void LoadTimerText()
    {
        string filePath = Application.persistentDataPath + "/saveData.json";
        if (saveObject == null)
        {
            string jsonString = File.ReadAllText(filePath);
            saveObject = JsonUtility.FromJson<SaveObject>(jsonString);
        }

        loadedText.text = "Your BEST time: " + saveObject.text + " s" + "\n" + "Try to do faster ;)";
    }

    [System.Serializable]
    public class SaveObject
    {
        public string text;
    }
}
