using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    public GameObject minionPrefab; 

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        SpawnNewMinion();
    }

    private void SpawnNewMinion()
    {
        // Spawn một instance mới của "minion" tại vị trí của GameObject hiện tại
            Debug.Log("SpawnMinion");

        Instantiate(minionPrefab, transform.position, Quaternion.identity);
    }
}