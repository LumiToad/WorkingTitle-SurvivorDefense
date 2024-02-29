using Sirenix.OdinInspector;
using UnityEngine;

public class XPItem : AbstractItem
{
    [Title("XpItem")]
    [SerializeField]
    private int value;
    [SerializeField]
    private float movementSpeed;

    private TrailRenderer trail;

    public bool IsOnScreen => gameObject.IsOnScreen(60.0f);

    private Player p;

    private void Start()
    {
        p = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (p != null)
        {
            transform.Translate((p.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime);
        }
    }

    protected override void PickedUp(Player player)
    {
        player.EarnXP(value);
        trail.enabled = false;
        Pool.Return<XPItem>(this.gameObject);
    }

    protected override void InternalOnReturnedToPool()
    {
        if(trail == null)
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }
    }

    protected override void InternalOnTakenfromPool()
    {
        if (trail == null)
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }
        trail.enabled = true;
    }
}
