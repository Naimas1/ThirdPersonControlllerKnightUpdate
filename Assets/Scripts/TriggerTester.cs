using UnityEngine;

public class TriggerTester : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            Debug.Log("Удар успішний");
        }
    }
}