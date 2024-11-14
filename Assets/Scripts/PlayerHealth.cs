using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private int currentHealth;
    [SerializeField]
    private int maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        HudController.instance.ShowDamageFlash();
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            print("Moriste");
        }
    }
}
