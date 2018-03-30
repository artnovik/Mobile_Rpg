using System.Collections;
using System.Collections.Generic;
using TDC;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviourSingleton<EnemyUI>
{
    [SerializeField]
    private Image enemyHealthBarCurrent;
    [SerializeField]
    private Image enemyHealthBarEmpty;

    private Color32 colorEngaged = new Color32(255, 255, 255, 255);
    private Color32 colorCalm = new Color32(255, 255, 255, 100);

    [HideInInspector]
    public Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void HealthBarDamage(int currentHealth)
    {
        enemyHealthBarCurrent.fillAmount = (float)currentHealth / 100;
    }

    public void SetHealthBarStatus(bool brightVisibility)
    {
        if (enemyHealthBarCurrent != null && enemyHealthBarEmpty != null)
        {
            if (brightVisibility)
            {
                enemyHealthBarCurrent.color = colorEngaged;
                enemyHealthBarEmpty.color = colorEngaged;
            }
            else
            {
                enemyHealthBarCurrent.color = colorCalm;
                enemyHealthBarEmpty.color = colorCalm;
            }
        }
    }

    public void DestroyEnemyUI(float duration)
    {
        //StartCoroutine(DestroyEnemyUIRoutine(duration));
        Destroy(enemyHealthBarCurrent.gameObject);
        Destroy(enemyHealthBarEmpty.gameObject);
    }

    private IEnumerator DestroyEnemyUIRoutine(float duration)
    {
        enemyHealthBarEmpty.CrossFadeAlpha(0f, duration, true);
        yield return new WaitForSeconds(duration);
        Destroy(enemyHealthBarCurrent.gameObject);
        Destroy(enemyHealthBarEmpty.gameObject);
    }
}
