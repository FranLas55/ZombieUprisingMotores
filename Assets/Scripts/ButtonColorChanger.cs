using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    private Button button;
    public Color hoverColor = Color.green;
    private Color originalColor; 

    void Start()
    {
        button = GetComponent<Button>();
        originalColor = button.GetComponent<Image>().color; 
    }

    public void OnPointerEnter()
    {
        button.GetComponent<Image>().color = hoverColor;
    }

    public void OnPointerExit()
    {
        button.GetComponent<Image>().color = originalColor;
    }
}