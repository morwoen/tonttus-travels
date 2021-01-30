using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    // TODO: map to unity input
    public KeyCode spawnKey = KeyCode.L;
    public Vector3 currentCheckpoint;

    void SaveCheckpoint(Vector3 newCheckpoint) {
        currentCheckpoint = newCheckpoint;
    }

    public void LoadCheckpoint() {
        transform.position = currentCheckpoint;
    }

    private void OnEnable() {
        SaveCheckpoint(transform.position);
    }

    private void OnTriggerEnter(Collider other) {
        string collisionTag = other.gameObject.tag;

        switch(collisionTag) {
            case "checkpoint":
                Vector3 spawnPoint = other.transform.GetChild(0).transform.position;
                SaveCheckpoint(spawnPoint);
                break;
            case "deadzone":
                LoadCheckpoint();
                break;
            default:
                return;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(spawnKey)) {
            LoadCheckpoint();
        }
    }
}
