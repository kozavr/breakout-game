using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    // references
    public Brick BrickPrefab;
    public Rigidbody Ball;
    public Text bestScoreText;
    public Text ScoreText;
    public GameObject GameOverText;

    // variables
    public int LineCount = 6;
    private int m_Points;

    private bool m_Started = false;
    private bool m_GameOver = false;

    string bestPlayerEver;
    int bestScoreEver;

    void Start()
    {
        LoadBestScoreEver();
        ShowBestScore();
        CloneBricks();
    }

    void CloneBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void ShowBestScore()
    {
        bestScoreText.text = $"Best score : {bestPlayerEver}: {bestScoreEver}";
    }

    public void GameOver()
    {
        if (m_Points > bestScoreEver)
        {
            SaveBestScoreEver();
            bestScoreText.text = $"Best score : {ScenePersistance.Instance.playerName}: {m_Points}";
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [Serializable]
    private class BestScoreEver
    {
        public string bestPlayerEver;
        public int bestScoreEver;
    }

    public void SaveBestScoreEver()
    {
        BestScoreEver newData = new BestScoreEver();

        newData.bestPlayerEver = ScenePersistance.Instance.playerName;
        Debug.Log("newData.bestPlayerEver" + newData.bestPlayerEver);

        newData.bestScoreEver = m_Points;
        Debug.Log("newData.bestScoreEver" + newData.bestScoreEver);

        string json = JsonUtility.ToJson(newData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScoreEver()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BestScoreEver newData = JsonUtility.FromJson<BestScoreEver>(json);
            bestPlayerEver = newData.bestPlayerEver;
            bestScoreEver = newData.bestScoreEver;
        }

    }
}
