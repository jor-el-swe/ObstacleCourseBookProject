using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
