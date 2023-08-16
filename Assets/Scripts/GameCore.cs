using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoBehaviour
{
    [SerializeField] private Gem GemTemplate;
    [SerializeField] private LineRenderer Rope;
    public GameObject Win;
    private int currentNumberToClick;

    public void UpdateCurrentNumberToClick(int newCurrentNumberToClick)
    {
        currentNumberToClick = newCurrentNumberToClick;
    }

    [SerializeField] private Camera cam;
    public LevelData levelData { get; private set; }
    public Level level;
    public int levelNumber = 0;
    Vector3[] ropePositions;
    Queue<int> ropePointsQueue = new Queue<int>();
    Coroutine ropeDeplyCoroutine = null;
    private GameObject timer;
    [SerializeField] private LevelManager levelManager;
    int resw, resh;
    public static bool isPaused;
    [SerializeField] private Text pausedTxt;
    private List<LineRenderer> ropeSegments = new List<LineRenderer>();
    [SerializeField] GameObject panelOnPause;
    public Action onLevelChangedCheckAudio;

    private void Start()
    {
        Application.targetFrameRate = 120;
        panelOnPause.SetActive(false);
        cam = Camera.main;
        timer = GameObject.FindGameObjectWithTag("Timer");
        TextAsset levelDataTextAsset = Resources.Load<TextAsset>("level_data");
        if (levelDataTextAsset != null)
        {
            levelData = JsonUtility.FromJson<LevelData>(levelDataTextAsset.text);
        }
        else
        {
            Debug.LogError("level_data.json not found in Resources folder!");
        }

        foreach (Level level in levelData.levels)
        {
            level.positions = new Vector2[level.level_data.Length / 2];
            for (int i = 0; i < level.level_data.Length; i += 2)
            {
                level.positions[i / 2] = new Vector2(level.level_data[i] * .001f, 1 - level.level_data[i + 1] * .001f);
            }
        }

        levelNumber = 0;
        level = levelData.levels[levelNumber];
        GenerateLevel();
        currentNumberToClick = 1;
        InitRope();
        Win.SetActive(false);
        pausedTxt.enabled = false;
    }

    public void GenerateLevel()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        List<Gem> instantiatedGems = new List<Gem>();

        for (int i = 0; i < level.positions.Length; i++)
        {
            Gem gem = Instantiate(GemTemplate, transform);
            instantiatedGems.Add(gem);
            gem.audioSource = gem.GetComponent<AudioSource>();
            gem.Position = level.positions[i];
            gem.SetNumber(i + 1);
        }
    }

    private void CellClicked(Gem gem)
    {
        if (gem.IsClicked || gem.number != currentNumberToClick)
        {
            return;
        }

        gem.OnGemClicked();

        if (currentNumberToClick > 1)
        {
            ropePointsQueue.Enqueue(currentNumberToClick);
        }
        currentNumberToClick++;

        if (currentNumberToClick > level.positions.Length)
        {
            Invoke("WaitforButtonOpen", 2);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
        if (!isPaused)
        {
            pausedTxt.enabled = false;
            panelOnPause.SetActive(false);
            if (resw != Screen.width || resh != Screen.height)
            {
                int num = currentNumberToClick;
                for (int i = 0; i < ropePositions.Length; i++)
                {
                    Vector3 pos = level.positions[i % level.positions.Length];
                    Vector3 worldPos = cam.ViewportToWorldPoint(pos);
                    worldPos.z = 1;
                    ropePositions[i] = worldPos;
                }
                Rope.SetPositions(ropePositions);
                resw = Screen.width;
                resh = Screen.height;
            }

            if (ropePointsQueue.Count > 0 && ropeDeplyCoroutine == null)
            {
                ropeDeplyCoroutine = StartCoroutine(DeployRope(ropePointsQueue.Dequeue()));
            }

            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), new Vector2(0, 0));
                if (hit.collider == null)
                {
                    return;
                }
                else
                {
                    Gem gem = hit.collider.GetComponent<Gem>();
                    if (gem == null)
                    {
                        return;
                    }
                    else
                    {
                        CellClicked(gem);
                    }
                }
            }
        }
    }

    public void InitRope()
    {
        ropePositions = new Vector3[level.positions.Length + 1];
        for (int i = 0; i < ropePositions.Length; i++)
        {
            Vector3 pos = level.positions[i % level.positions.Length];
            Vector3 worldPos = cam.ViewportToWorldPoint(pos);
            worldPos.z = 1;
            ropePositions[i] = worldPos;
        }

        Rope.positionCount = 0;
        Rope.SetPositions(ropePositions);
    }

    IEnumerator DeployRope(int part)
    {
        if (part < 2) yield break;
        part -= 1;
        Vector3 savedPosition = ropePositions[part];
        ropePositions[part] = ropePositions[part - 1];
        Rope.positionCount = part + 1;
        Rope.SetPositions(ropePositions);

        float time = 0.5f;
        while (time < 1)
        {
            time += Time.deltaTime;
            ropePositions[part] = Vector3.Lerp(ropePositions[part - 1], savedPosition, time);

            Rope.SetPositions(ropePositions);

            yield return null;
        }

        ropePositions[Rope.positionCount - 1] = savedPosition;

        if (currentNumberToClick == level.positions.Length + 1)
        {
            ropePointsQueue.Enqueue(currentNumberToClick);
            currentNumberToClick = -1;
        }
        ropeDeplyCoroutine = null;
    }

    public void Nextlevel()
    {
        if (!isPaused)
        {
            levelManager.NextLevel();
            onLevelChangedCheckAudio?.Invoke();
        }
    }

    private void WaitforButtonOpen()
    {
        Win.SetActive(true);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pausedTxt.enabled = true;
        panelOnPause.SetActive(true);
    }
}

[System.Serializable]
public class LevelData
{
    public Level[] levels;
}

[System.Serializable]
public class Level
{
    public int[] level_data;
    public Vector2[] positions;
}
