using UnityEngine;
using TMPro;

public class ChangeTextHeightAndColor : MonoBehaviour
{
    [SerializeField] private float scaleFactor = 1.1f;
    [SerializeField] private Color hoverColor = Color.blue;
    private Transform text;
    private Gem gem;
    
    private void Start()
    {
        gem = GetComponent<Gem>();
        text = transform.Find("TextMeshPro");
    }

    private void OnMouseEnter()
    {   
        if (gem.IsClicked)
        {
            return;
        }
        else
        {
            text.GetComponent<TextMeshPro>().fontSize *= scaleFactor;
            text.GetComponent<TextMeshPro>().color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (gem.IsClicked)
        {
            return;
        }
        else
        {
            text.GetComponent<TextMeshPro>().fontSize /= scaleFactor;
            text.GetComponent<TextMeshPro>().color = Color.magenta;
        }
    }
}
