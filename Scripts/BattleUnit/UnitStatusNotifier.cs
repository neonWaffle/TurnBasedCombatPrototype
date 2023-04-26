using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusNotifier : MonoBehaviour
{
    Queue<StatusIndicator> statusEffectIndicators = new Queue<StatusIndicator>();
    [SerializeField] StatusIndicator defaultIndicator;
    [SerializeField] Color defaultMessageColour;
    [SerializeField] float delay = 0.3f;
    [SerializeField] Transform spawnPoint;

    public void DisplayStatus(string text)
    {
        var indicator = Instantiate(defaultIndicator, spawnPoint.position, Quaternion.identity);
        indicator.Set(text, defaultMessageColour);
        indicator.gameObject.SetActive(false);
        statusEffectIndicators.Enqueue(indicator);

        if (statusEffectIndicators.Count == 1)
        {
            StartCoroutine(ShowIndicator());
        }
    }

    public void DisplayStatus(string text, Color messageColour)
    {
        var indicator = Instantiate(defaultIndicator, spawnPoint.position, Quaternion.identity);
        indicator.Set(text, messageColour);
        indicator.gameObject.SetActive(false);
        statusEffectIndicators.Enqueue(indicator);

        if (statusEffectIndicators.Count == 1)
        {
            StartCoroutine(ShowIndicator());
        }
    }

    IEnumerator ShowIndicator()
    {
        yield return new WaitForSeconds(delay);
        var indicator = statusEffectIndicators.Dequeue();
        indicator.gameObject.SetActive(true);
        indicator.Animate();
        if (statusEffectIndicators.Count > 0)
        {
            StartCoroutine(ShowIndicator());
        }
    }
}
