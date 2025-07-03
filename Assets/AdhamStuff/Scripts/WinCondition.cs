using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject WinPanel;
    [SerializeField] private OnBurgerEaten burgerEaten;



    private void Update()
    {
        if(burgerEaten.BurgerCount >= 10)
        {
            WinPanel.SetActive(true);
        }
    }
}
