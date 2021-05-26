using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
  public AudioSource dash;
  public AudioSource jump;
  public AudioSource death;
  public AudioSource sprint;
  public AudioSource powerup;
  public AudioSource climbing;
  public AudioSource footsteps;

  public void Footsteps(bool flag)
  {
    if (flag)
    {
      if(!footsteps.isPlaying)
      {
        footsteps.Play();
      }
    } else
    {
      if (footsteps.isPlaying)
      {
        footsteps.Stop();
      }
    }
  }

  public void Sprint(bool flag)
  {
    if (flag)
    {
      if(!sprint.isPlaying)
      {
        sprint.Play();
      }
    } else
    {
      if (sprint.isPlaying)
      {
        sprint.Stop();
      }
    }
  }

  public void Climb(bool flag)
  {
    if (flag)
    {
      if (!climbing.isPlaying)
      {
        climbing.Play();
      }
    }
    else
    {
      if (climbing.isPlaying)
      {
        climbing.Stop();
      }
    }
  }

  public void Dash()
  {
    dash.Play();
  }

  public void Jump()
  {
    jump.Play();
  }

  public void Death()
  {
    death.Play();
  }

  public void PowerUp()
  {
    powerup.Play();
  }
}
