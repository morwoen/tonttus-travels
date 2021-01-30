using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private float initialTimeScale;
    private bool isPaused = false;

    private void Start() {
        initialTimeScale = Time.timeScale;
    }

    void Update() {
        bool pauseButtonPressed = Input.GetAxis("Menu") != 0;

        if (pauseButtonPressed && !isPaused) {
            PauseGame();
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

        isPaused = false;
        pauseMenuPanel.SetActive(false);
        ToggleMouse(true);
    }

    public void PauseGame() {
        Time.timeScale = 0;

        isPaused = true;
        pauseMenuPanel.SetActive(true);
        ToggleMouse(false);
        Input.ResetInputAxes();
    }

    public void LoadCheckpoint() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerCheckpoint pCheckpoint = player.GetComponent<PlayerCheckpoint>();

        pCheckpoint.LoadCheckpoint();
        ResumeGame();
    }
    #endregion
}
