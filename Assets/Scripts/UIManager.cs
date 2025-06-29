using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public GameObject Image_Menu, Image_Help;
    public Button Bt_Play;
    public Button Bt_Help;
    public Button Bt_Back;
    void Start()
    {
        Bt_Play.onClick.AddListener(Play);
        Bt_Help.onClick.AddListener(Help);
        Bt_Back.onClick.AddListener(Back);
    }

    void ShowImage(GameObject Tager)
    {
        Image_Menu.SetActive(false);
        Image_Help.SetActive(false);
        Tager.SetActive(true);
    }

    private void Play() => SceneManager.LoadScene("Game");
    private void Help() => ShowImage(Image_Help);
    private void Back() => ShowImage(Image_Menu);
}
