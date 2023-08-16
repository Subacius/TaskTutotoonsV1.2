using UnityEngine;
using TMPro;
using System.Collections;

public class Gem : MonoBehaviour
{
    [SerializeField] SpriteRenderer Visual;
    [SerializeField] private Sprite BlueGem;
    public int number { get; private set; }
    public Vector2 Position;
    public bool IsClicked { get; private set; }
    private Camera cam;
    [SerializeField] public TextMeshPro NumberText;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
        UpdateGemPosition();
    }

    private void Update()
    {
        UpdateGemPosition();
    }

    private void UpdateGemPosition()
    {
        Vector3 worldPos = cam.ViewportToWorldPoint(Position);
        worldPos.z = 0;
        transform.position = worldPos;
    }

    public void SetNumber(int num)
    {
        number = num;
        NumberText.text = num.ToString();
    }

    public void OnGemClicked()
    {
        if (IsClicked)
        {
            return;
        }
        audioSource.Play();
        IsClicked = true;
        Visual.sprite = BlueGem;
        NumberText.color = Color.blue;
        StartCoroutine(FadeGemNumber());
    }

    private IEnumerator FadeGemNumber()
    {
        float time = 0.5f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            if (time < 0) time = 0;
            NumberText.color = new Color32(255, 255, 255, (byte)(255 * 2 * time));
            NumberText.color = new Color32(0, 0, 0, (byte)(255 * 2 * time));
            yield return null;
        }
    }

    public void SetClicked(bool value)
    {
        IsClicked = value;
    }

}
