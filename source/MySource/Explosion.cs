using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//‹N”šŒ^‚Ì”š”­‚Ìˆ—‚ğs‚¤
public class Explosion : MonoBehaviour
{
    [SerializeField] private float deleteTime = 0;
    [SerializeField] private float _impulseForce;

    void Start()
    {
        SoundMngr.Instance.PlaySE(SoundMngr.E_SE.ENEMY_EXPLOD);
    }

    void FixedUpdate()
    {
        deleteTime -= Time.fixedDeltaTime;
        if(deleteTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Vector2 dir = new Vector2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y).normalized;
           
            collision.GetComponent<Enemy>().ChangeToKnockback(new Vector2(_impulseForce * dir.x, _impulseForce * dir.y));
        }

    }
}
