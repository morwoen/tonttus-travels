using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{

  public GameObject JumpBoostContainer;
  public Image JumpBoostImage;
  public Image SprintImage;
  public Image DashImage;

  float duration = 0f;
  float totalDuration = 0f;
  bool isBoostRunning = false;

  void Start() {
    JumpBoostImage.fillAmount = 0;
    JumpBoostContainer.SetActive(false);
    SprintImage.fillAmount = 0;
    DashImage.fillAmount = 0;
  }

  public void SetSprint(float value, float total) {
    SprintImage.fillAmount = 1 - (value / total);
  }

  public void SetDash(float value, float total) {
    DashImage.fillAmount = value / total;
  }

  public void SetJumpBoost(float duration) {
    this.duration = duration;
    this.totalDuration = duration;
    this.isBoostRunning = true;
    JumpBoostContainer.SetActive(true);
  }

  void Update() {
    if (!isBoostRunning) {
      return;
    }

    duration -= Time.deltaTime;

    if (duration <= 0) {
      isBoostRunning = false;
      JumpBoostContainer.SetActive(false);
      return;
    }

    JumpBoostImage.fillAmount = 1 - (duration / totalDuration);
  }
}
