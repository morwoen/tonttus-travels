using UnityEngine;

public class PlayerCollectables : MonoBehaviour
{
    [SerializeField]
    int collectedAmount = 0;

    public void Add(int amount) {
        collectedAmount += amount;
    }
}
