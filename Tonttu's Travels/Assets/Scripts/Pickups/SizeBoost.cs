using System.Collections;
using UnityEngine;

public class SizeBoost : MonoBehaviour, IPickable
{
    public float duration;
    public float playerSizeBoostAmount;

    Vector3 previousSize;

    IEnumerator IncreaseSize(PlayerController player) {
        previousSize = player.transform.localScale;
        player.transform.localScale *= playerSizeBoostAmount;
        OnPickupDestroy();
        yield return new WaitForSeconds(duration);
        player.transform.localScale = previousSize;
    }

    public void OnPickupDestroy() {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(gameObject, duration + 1);
    }

    public void Pick(PlayerController player, HUDScript hud) {
        StartCoroutine(IncreaseSize(player));
    }
}
