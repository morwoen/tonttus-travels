using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  public GameObject pauseMenuPanel;
  public GameObject hud;
  private float initialTimeScale;
  private bool isPaused = false;

  public GameObject[] tutorials;

  private int openTutorial = -1;

  private void Start() {
    initialTimeScale = Time.timeScale;
  }

  private void Update() {
    bool pauseButtonPressed = Input.GetAxis("Menu") != 0;

    if (pauseButtonPressed && !isPaused) {
      PauseGame();
    } else if (pauseButtonPressed && isPaused) {
      ResumeGame();
    }
  }

  void ToggleMouse(bool locked) {
    if (locked) {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    } else {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }

  #region Mouse Methods
  public void ResumeGame() {
    Time.timeScale = initialTimeScale;

    if (openTutorial > -1) {
      tutorials[openTutorial].SetActive(true);
    }

    isPaused = false;
    pauseMenuPanel.SetActive(false);
    hud.SetActive(true);
    ToggleMouse(true);


    if (openTutorial != -1 && openTutorial == tutorials.Length - 1) {
      ToggleMouse(true);
    }
  }

  public void PauseGame() {
    Time.timeScale = 0;

    openTutorial = -1;
    for (var a = 0; a < tutorials.Length; a++) {
      if (tutorials[a].activeSelf) {
        openTutorial = a;
        tutorials[a].SetActive(false);
        break;
      }
    }

    isPaused = true;
    pauseMenuPanel.SetActive(true);
    hud.SetActive(false);
    ToggleMouse(false);
    Input.ResetInputAxes();
  }

  public void LoadCheckpoint() {
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    PlayerCheckpoint pCheckpoint = player.GetComponent<PlayerCheckpoint>();

    pCheckpoint.LoadCheckpoint();
    ResumeGame();
  }

  public void Quit() {
    SceneManager.LoadScene(0);
  }
  #endregion
}
