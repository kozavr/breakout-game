using UnityEngine;

public class ScenePersistance : MonoBehaviour
{
    public static ScenePersistance Instance;

    public string playerName;

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

}
