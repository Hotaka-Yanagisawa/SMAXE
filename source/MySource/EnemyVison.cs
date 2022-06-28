using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// “G‚ÌŽ‹–ìƒNƒ‰ƒX
/// </summary>
public class EnemyVison : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;

    private void OnTriggerStay2D(Collider2D collision)
    {
        string destLayerName = LayerMask.LayerToName(collision.gameObject.layer);
        if (destLayerName == "Player")
        {
            _event?.Invoke();
        }
    }
}
