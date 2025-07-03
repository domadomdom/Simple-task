using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using BrunoMikoski.AnimationSequencer;
public class OnBurgerEaten : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BurgerEaten;
    public int BurgerCount;
    [SerializeField] private AudioSource BurgerMunch;
    [SerializeField] private AnimationSequencerController BurgerUiAnimation;
    public ParticleSystem EatEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Burger"))
        {
            BurgerCount++;
            BurgerEaten.text = BurgerCount.ToString();
            BurgerMunch.Play();

            ParticleSystem eatEffectInstance = Instantiate(EatEffect, other.transform.position, Quaternion.identity);
            BurgerUiAnimation.Play();
            eatEffectInstance.Play();
            Destroy(other.gameObject);
        }
    }
}
