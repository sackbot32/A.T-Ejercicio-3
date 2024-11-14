using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public static HudController instance;
    [SerializeField]
    private Image damageFlash;
    [SerializeField]
    private float damageFlashDuration;

    private Coroutine disapearCourritine;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void ShowDamageFlash()
    {
        if(disapearCourritine != null)
        {
            StopCoroutine(disapearCourritine);
        }

        damageFlash.color = Color.white;

        disapearCourritine = StartCoroutine(FlashDissapear());
    }

    private IEnumerator FlashDissapear()
    {
        float alpha = 1f;
        while(alpha > 0f)
        {
            alpha -= (1f/damageFlashDuration) * Time.deltaTime;
            damageFlash.color = new Color(1f,1f,1f,alpha);
            yield return null;
        }
    }
}
