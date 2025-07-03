using UnityEngine;

public class Burger : MonoBehaviour
{
    void OnDestroy()
    {
        BrugerSpawner spawner = FindObjectOfType<BrugerSpawner>();
        if (spawner != null)
        {
            spawner.OnBurgerDestroyed();
        }
    }
}