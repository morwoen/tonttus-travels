using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
  public Vector3 currentCheckpoint;
  private PlayerAudioController audioController;

  void Start()
  {
    audioController = GetComponentInChildren<PlayerAudioController>();
  }

  void SaveCheckpoint(Vector3 newCheckpoint)
  {
    currentCheckpoint = newCheckpoint;
  }

  public void LoadCheckpoint()
  {
    transform.position = currentCheckpoint;
    audioController.Death();
  }

  private void OnEnable()
  {
    SaveCheckpoint(transform.position);
  }

  private void OnTriggerEnter(Collider other)
  {
    string collisionTag = other.gameObject.tag;

    switch (collisionTag)
    {
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
}
