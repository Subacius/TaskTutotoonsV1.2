using System.Collections.Generic;
using UnityEngine;

public class Resizer3 : MonoBehaviour
{
    public string tagToMove = "GemTxt";
    public float threshold = 10f;
    public float speed = 2f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private List<GameObject> objectsToMove = new List<GameObject>();
    private int screenWidth;
    private int screenHeight;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tagToMove);
        objectsToMove.AddRange(foundObjects);

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void Update()
    {
        if (Screen.width == screenWidth && Screen.height == screenHeight)
        {
            return;
        }

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        foreach (GameObject obj in objectsToMove)
        {
            if (obj == null || obj.CompareTag(tagToMove) == false)
            {
                continue;
            }

            RectTransform objRectTransform = obj.GetComponent<RectTransform>();
            if (objRectTransform == null)
            {
                continue;
            }

            Vector3[] corners = new Vector3[4];
            objRectTransform.GetWorldCorners(corners);
            Vector3 bottomRightCorner = corners[2];
            Vector3 screenPos = Camera.main.WorldToScreenPoint(bottomRightCorner);
            float distanceToBottomEdge = screenPos.y;

            if (distanceToBottomEdge < threshold)
            {
                obj.transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }
    }
}
