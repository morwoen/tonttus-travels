using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // TODO: map to unity input
    public KeyCode pauseKey;
    public GameObject pauseMenuPanel;

    private float initialTimeScale;

    private void Start() {
        initialTimeScale = Time.timeScale;
    }

    private void Update() {
        if (Input.GetKeyDown(pauseKey)) {
            Time.timeScale = 0;

            pauseMenuPanel.SetActive(true);
            ToggleMouse(false);
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

        pauseMenuPanel.SetActive(false);
        ToggleMouse(true);
    }

    public void LoadCheckpoint() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerCheckpoint pCheckpoint = player.GetComponent<PlayerCheckpoint>();

        pCheckpoint.LoadCheckpoint();
        ResumeGame();
    }
    #endregion
}
