﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipSlot : Slot
{
    public override void Select()
    {
        if (slotItem != null)
        {
            if (Inventory.Instance.equipMode)
            {
                
            }
            else
            {
                //InventoryUI.Instance.currentEquipWeaponSlot = this;
                //InventoryUI.Instance.MakeSlotActive(this);
            }
        }
    }
}