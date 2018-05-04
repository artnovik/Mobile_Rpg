﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    #region Singleton

    public static ContainerUI Instance;

    private void Awake()
    {
        Instance = this;

        slots = slotsContainer.GetComponentsInChildren<ContainerSlot>();
    }

    #endregion

    [Header("Container_UI")] [SerializeField]
    private Text nameText;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button takeButton;
    [SerializeField] private Button takeAllButton;

    [Header("InfoWindow")] [SerializeField]
    private Text itemNameInfo;

    [SerializeField] private Image itemIconInfo;

    [SerializeField] private Text itemDescriptionInfo;

    [SerializeField] private RectTransform slotsContainer;
    public ContainerSlot[] slots;

    public Item currentClickedItem;

    public Container currentContainer;

    public void InitializeContainerUI(Container container)
    {
        nameText.text = container.containerType.ToString();
        currentContainer = container;
        MakeAllSlotsInactive();
        UpdateContainerSlots(container);
        SelectFirstSlot();
    }

    public void UpdateContainerSlots(Container container)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < container.containerItems.Count)
            {
                slots[i].FillSlot(currentContainer.containerItems[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void MakeSlotActive(ContainerSlot clickedSlot)
    {
        MakeAllSlotsInactive();
        clickedSlot.slotButton.GetComponent<Image>().color = Colors.playerActiveUI;
        clickedSlot.countText.color = Color.white;

        ActivateItemInfo(true);
    }

    private void SelectFirstSlot()
    {
        slots[0].Select();
    }

    public void SelectNextSlot(bool isFirstSlotActive)
    {
        if (isFirstSlotActive)
        {
            foreach (ContainerSlot slot in slots)
            {
                if (slot.slotButton.GetComponent<Image>().color == Colors.playerActiveUI)
                {
                    return;
                }
            }

            SelectFirstSlot();
            return;
        }
        else
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].slotItem == currentClickedItem)
                {
                    slots[i].ClearSlot();
                    MakeAllSlotsInactive();

                    if (i > 0)
                    {
                        slots[i - 1].Select();
                    }
                }
            }
        }
    }

    public void MakeAllSlotsInactive()
    {
        foreach (ContainerSlot slot in slots)
        {
            slot.slotButton.GetComponent<Image>().color = Colors.playerDefaultUI;
            slot.countText.color = Color.black;
        }

        ActivateItemInfo(false);
    }

    private void ActivateItemInfo(bool value)
    {
        takeButton.gameObject.SetActive(value);

        if (value)
        {
            FillInfoWindow(currentClickedItem.inventorySprite, currentClickedItem.name, currentClickedItem.description);
        }
        else
        {
            ClearInfoWindow();
        }
    }

    private void FillInfoWindow(Sprite itemSprite, string itemName, string itemDescription)
    {
        itemIconInfo.sprite = itemSprite;
        itemNameInfo.text = itemName;
        itemDescriptionInfo.text = itemDescription;
    }

    private void ClearInfoWindow()
    {
        itemIconInfo.sprite = null;
        itemNameInfo.text = null;
        itemDescriptionInfo.text = null;
    }

    private void ClearActiveSlot()
    {
        foreach (ContainerSlot slot in slots)
        {
            if (currentClickedItem == slot.slotItem)
            {
                slot.ClearSlot();
            }
        }

        MakeAllSlotsInactive();
    }

    public void TakeItemClick()
    {
        currentContainer.MoveItemToInventory(currentClickedItem);
    }

    public void TakeAllItemsClick()
    {
        currentContainer.MoveAllItemsToInventory();
    }

    public void CloseContainerClick()
    {
        UIGamePlay.Instance.ContainerClose();
        if (currentContainer.IsChest())
        {
            currentContainer.GetComponent<ChestAnimation>().CloseChestAnimation();
        }
    }
}