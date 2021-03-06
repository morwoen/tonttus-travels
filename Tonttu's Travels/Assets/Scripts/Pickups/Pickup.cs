using UnityEngine;

public class Pickup : MonoBehaviour, IPickable {
    public int amount;
    public float destroyAfterSeconds;

    public void OnPickupDestroy() {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void Pick(PlayerController player, HUDScript hud) {
        PlayerCollectables collectables = player.GetComponent<PlayerCollectables>();

        collectables.Add(amount);
        OnPickupDestroy();
    }
}
