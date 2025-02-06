using Unity.Netcode;
using UnityEngine;

public class Weapon : Item
{
    private void OnEnable()
    {
        Item itemcomp = gameObject.GetComponent<Item>();

        itemcomp.onUseItem += Attack;
    }

    public void Attack(UseItemStruct useData)
    {
        Debug.Log(useData.MouseLocation);
    }

    protected virtual void ApplyDamage() { }
}
