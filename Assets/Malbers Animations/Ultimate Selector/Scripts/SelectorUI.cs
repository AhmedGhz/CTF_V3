using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace MalbersAnimations.Selector
{
    public class SelectorUI : MonoBehaviour
    {
        private SelectorManager manager;               //Reference for the Selector Manager

        public Text TextSelectedName;
        public Text TextItemData;
        public Text TextTotalCoins;
        public Text TextItemValue;
        public Text TextItemAmount;
        public GameObject HighLight;

        [Space]

        [Header("Text to show:")]
        public string ItemOwned = "Already Owned";
        public string ItemLocked = "Locked";
        public string SelectLocked = "Unlock First";
        public string NoMoney = "Not enough coins";
        public string NoAmount = "You dont have any more";

        MItem ItemSelected;

        public SelectorManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = this.FindComponent<SelectorManager>();
                }
                return manager;
            }
        }


        /// <summary>Update the UI</summary>
        public virtual void UpdateSelectedItemUI(GameObject item)
        {
            UpdateSelectedItemUI(item ? item.GetComponent<MItem>() : null);
        }

        /// <summary> Update the UI </summary>
        public virtual void UpdateSelectedItemUI(MItem item)
        {
            string _name = string.Empty;
            string _data = string.Empty;
            string _Value = string.Empty;
            string _Amount = string.Empty;

            ItemSelected = item;

            if (ItemSelected)                                   //if there's an item selected
            {
                _name = item.name;                              //get the Item's Name 
                _data = ItemSelected.ItemData;                  //get the Item's Description
                _Value = ItemSelected.Value.ToString();         //get the Item's Value
                _Amount = ItemSelected.Amount.ToString();         //get the Item's Value

                if (ItemSelected.Amount <= 0)
                {
                    _data = NoAmount;
                }
            }

            if (TextTotalCoins) TextTotalCoins.text = string.Empty;            //Update the Name

            if (Manager && TextTotalCoins && Manager.Data)
            {
                TextTotalCoins.text = Manager.Data.Save.Coins.ToString();      //Updates the Total Coins
            }

            if (TextSelectedName) TextSelectedName.text = _name;            //Update the Name
            if (TextItemData) TextItemData.text = _data;                    //Update the description  
            if (TextItemValue) TextItemValue.text = _Value;                 //Update the Value 
            if (TextItemAmount) TextItemAmount.text = _Amount;

            if (ItemSelected && ItemSelected.Locked && TextItemData)        //if the item is locked set on the Descroption Locked
            {
                TextItemData.text = ItemLocked;
            }
        }

        /// <summary>
        /// Hide the Changer GameObject while is changing
        /// </summary>
        /// <param name="value"></param>
        public virtual void HideHighLight(bool value)
        {
            if (HighLight) HighLight.SetActive(!value);
        }


        /// <summary>Hides All item Data</summary>
        public virtual void HideAllItemData(bool value)
        {
            if (TextSelectedName) TextSelectedName.gameObject.SetActive(!value);
            if (TextItemValue) TextItemValue.gameObject.SetActive(!value);
            if (TextItemAmount) TextItemAmount.gameObject.SetActive(!value);
            if (TextItemData) TextItemData.gameObject.SetActive(!value);
        }

        /// <summary>Move the HighLight </summary>
        public virtual void MoveHighlight(GameObject Item)
        {
            if (HighLight && Item)
            {
                HighLight.transform.position = Item.transform.position;
            }
        }

        /// <summary>Update the UI and calls SelectorManager.Purchase() </summary>
        public virtual void Purchase()
        {
            if (!ItemSelected.Locked && TextItemData)                   //If the Item is already Unlocked  and it's has a TextData to show
            {
                TextItemData.text = ItemOwned;                          //Show on the Text Data that you already own that Item.
                return;
            }

            if (Manager.Data)                                         //If the Selector Manager has a Scriptable DATA 
            {
                if (Manager.Data.Save.Coins >= ItemSelected.Value)         //If you have more coins than the Item value (then you can make a purchase)
                {
                    Manager.Purchase(ItemSelected);                   //Call the Main Purchase Method from the Selector Manager
                    UpdateSelectedItemUI(ItemSelected.gameObject);      //Update the UI
                }
                else                                                    //if you dont have enough money 
                {
                    TextItemData.text = NoMoney;                        //Update the ui that you don't have $
                }
            }
        }

        /// <summary> Update the UI and calls SelectorManager.SelectItem() </summary>
        public virtual void SelectItem()
        {
            if (!ItemSelected) return;

            if (ItemSelected.Amount <= 0)
            {
              if(TextItemData)  TextItemData.text = NoAmount;
                return;
            }

            if (!ItemSelected.Locked)                                                       //If the Items is NOT Locked
            {
                Manager.Controller.OnClickOnItem.Invoke(ItemSelected.gameObject);         //Invoke the Controller Event
                Manager.SelectItem();                                             
            }
            else                                                                            //Else show on the Data is locked
            {
                if (TextItemData) TextItemData.text = SelectLocked;
            }
        }

        /// <summary>Update the UI and calls SelectorManager.RestoreToDefaultData(); </summary>
        public virtual void ResetPurchase()
        { Manager.RestoreToDefaultData(); }


        public virtual void HideName(bool value)
        {
            if (!TextSelectedName) return;
            TextSelectedName.gameObject.SetActive(!value);
        }
    }
}