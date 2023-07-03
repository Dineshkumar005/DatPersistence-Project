using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public int HighScore;
    public string PlayerName;
    public string currentPlayerName;
    public InputField NameField;
    public static MainMenuScript Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        if (NameField.text.Length>0)
        {
            currentPlayerName = NameField.text;
        }
        else
        {
            currentPlayerName = "Player";
        }
        LoadHighScore();
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }

    [System.Serializable]
    class HighScoreData
    {
        public string PlayerName;
        public int Points;
    }

    public void SaveHighScore(int score)
    {
        HighScoreData data = new HighScoreData();
        data.Points = score;

        data.PlayerName = currentPlayerName;

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/UserData";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(Application.persistentDataPath + "/UserData/highscoredata.json", json);

        LoadHighScore();
    }

    void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/UserData/highscoredata.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);

            HighScore = data.Points;
            PlayerName = data.PlayerName;
        }
    }
}
