using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
  public GameObject button1;
  public GameObject button2;

  private int selectedID;
  private Button selectedOption;

  public void Quit() {
    Application.Quit();
  }

  public void Play() {
    SceneManager.LoadScene(1);
  }

  private void SelectOption(GameObject option)
  {
    //button1.transform.GetChild(0).gameObject.SetActive(false);
    //button2.transform.GetChild(0).gameObject.SetActive(false);

    //option.transform.GetChild(0).gameObject.SetActive(true);
    selectedOption = option.GetComponent<Button>();
  }

  void Start()
  {
    SelectOption(button1);
    selectedID = 0;
  }

  void Update()
  {
    if (Input.GetAxis("Vertical") != 0)
    {
      Input.ResetInputAxes();
      switch (selectedID)
      {
        case 0:
          SelectOption(button2);
          selectedID = 1;
          break;
        case 1:
          SelectOption(button1);
          selectedID = 0;
          break;
      }
    }

    if (Input.GetAxis("Jump") != 0)
    {
      selectedOption.onClick.Invoke();
    }
  }
}
