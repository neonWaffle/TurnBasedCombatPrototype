using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityParticle : MonoBehaviour
{
    BattleUnit user;
    BattleUnit target;
    AbilitySO abilitySO;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] AudioClip destroyAudio;
    [SerializeField] GameObject destroyVFX;
    [SerializeField] VFXSlotType targetSlot = VFXSlotType.MEDIUM;

    public void Set(AbilitySO abilitySO, BattleUnit user, BattleUnit target)
    {
        this.abilitySO = abilitySO;
        this.target = target;
        this.user = user;

        if (abilitySO.IsRanged)
        {
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        var startPos = transform.position;
        var targetPos = target.Data.GetVFXSlot(targetSlot).position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        //Only ranged abilities use AbilityParticle to apply their effects to the target
        abilitySO.Apply(user, target);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (destroyAudio != null)
        {
            AudioManager.Instance.PlaySFX(destroyAudio);
        }

        if (destroyVFX != null)
        {
            Instantiate(destroyVFX, transform.position, destroyVFX.transform.rotation);
        }
    }
}
