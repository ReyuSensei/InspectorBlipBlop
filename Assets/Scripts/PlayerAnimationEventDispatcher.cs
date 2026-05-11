using UnityEngine;

public class PlayerAnimationEventDispatcher : MonoBehaviour
{
    public void EventShowMeleeWeapon()
    {
        transform.parent.SendMessage("EventShowMeleeWeapon");
    }

    public void EventEnableMeleeCollider()
    {
        transform.parent.SendMessage("EventEnableMeleeCollider");
    }

    public void EventDisableMeleeCollider()
    {
        transform.parent.SendMessage("EventDisableMeleeCollider");
    }

    public void EventHideMeleeWeapon()
    {
        transform.parent.SendMessage("EventHideMeleeWeapon");
    }

    public void EventShowRangeWeapon()
    {
        transform.parent.SendMessage("EventShowRangeWeapon");
    }

    public void EventFireRangeWeapon()
    {
        transform.parent.SendMessage("EventFireRangeWeapon");
    }

    public void EventHideRangeWeapon()
    {
        transform.parent.SendMessage("EventHideRangeWeapon");
    }

    public void EventStep()
    {
        transform.parent.SendMessage("EventStep");
    }
}
