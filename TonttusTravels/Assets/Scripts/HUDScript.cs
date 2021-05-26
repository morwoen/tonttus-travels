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

  public Image key;
  public Image quill;
  public Image coin;
  public Image ring;
  public Image lighter;

  public Sprite keySprite;
  public Sprite quillSprite;
  public Sprite coinSprite;
  public Sprite ringSprite;
  public Sprite lighterSprite;

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

  public void ToggleKey() {
    var current = key.sprite;
    key.sprite = keySprite;
    keySprite = current;
  }
  public void ToggleCoin() {
    var current = coin.sprite;
    coin.sprite = coinSprite;
    coinSprite = current;
  }
  public void ToggleRing() {
    var current = ring.sprite;
    ring.sprite = ringSprite;
    ringSprite = current;
  }
  public void ToggleLighter() {
    var current = lighter.sprite;
    lighter.sprite = lighterSprite;
    lighterSprite = current;
  }
  public void ToggleQuill() {
    var current = quill.sprite;
    quill.sprite = quillSprite;
    quillSprite = current;
  }
}
