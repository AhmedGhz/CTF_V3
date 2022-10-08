using UnityEngine;
using MalbersAnimations.Utilities;
using System.Collections.Generic;

namespace MalbersAnimations.Selector
{
    [System.Serializable]
    public class SelectorSave
    {
        #region Variables

        /// <summary>Current Total of coins on the selector</summary>
        public int Coins;

        /// <summary>Default ammount of coins to return to when Restore Data is called</summary>
        public int RestoreCoins;

        /// <summary>Saves the last focused item on the Selector</summary>
        public int FocusedItem;
        /// <summary>List of all current Locked Items</summary>
        public List<bool> Locked;

        /// <summary>List of all Initial Locked Items to restore when Restore Data is called</summary>
        public List<bool> RestoreLocked;

        /// <summary>The current elements amount  of each item on the selector </summary>
        public List<int> ItemsAmount;

        /// <summary>The Default elements amount of each item on the selector to restore when Restore Data is called</summary>
        public List<int> RestoreItemsAmount;

        /// <summary> Are the Selector Items using the Material-Changer component?</summary>
        public bool UseMaterialChanger;

        /// <summary>Current Material Item Index the items are using</summary>
        public List<string> MaterialIndex;

        /// <summary>Default Material Item Index the items were using to restore when Restore Data is called</summary>
        public List<string> RestoreMaterialIndex;

        /// <summary>Are the Selector Items using the  Active-Mesh component?</summary>
        public bool UseActiveMesh;

        /// <summary>Current Active Mesh Item Index the items are using</summary>
        public List<string> ActiveMeshIndex;

        /// <summary> Default Active Mesh Item Index the items were using to restore when Restore Data is called</summary>
        public List<string> RestoreActiveMeshIndex;
        #endregion
    }

    [CreateAssetMenu(menuName = "Malbers Animations/Ultimate Selector/Selector Data")]
    public class SelectorData : ScriptableObject
    {
        public bool usePlayerPref = true;
        public string PlayerPrefKey = "SaveSelectorData";

        [Header("Data to Save")]
        public SelectorSave Save;

        public int Coins
        {
            set
            {
                if (Save != null)
                {
                    Save.Coins = value;
                    SaveDataPlayerPrefs();
                }
            }
            get {return Save.Coins;}
        }

        /// <summary>Modify the Coins values</summary>       
        public virtual void ModifyCoins(int value)
        { Coins += value; }
      
        /// <summary>Saves the Initial Data of a Selector Manager </summary>       
        public virtual void SetDefaultData(SelectorManager manager)
        {
            Save.RestoreCoins = Save.Coins;                                       //Save the RestoreCoins to the Current Coins

            Save.RestoreLocked = new List<bool>();
            Save.Locked = new List<bool>();

            Save.RestoreItemsAmount = new List<int>();
            Save.ItemsAmount = new List<int>();

            Save.UseMaterialChanger = Save.UseActiveMesh =  false;
            Save.FocusedItem = manager.Controller.FocusedItemIndex;

            Save.ActiveMeshIndex = null;
            Save.RestoreActiveMeshIndex = null;
            Save.MaterialIndex = null;
            Save.RestoreMaterialIndex = null;

            if (manager.Editor.Items != null && manager.Editor.Items.Count > 1)         //Check for Material Changer and Active Mesh
            {
                if (manager.Editor.Items[0].MatChanger != null)
                {
                    Save.UseMaterialChanger = true;
                    Save.RestoreMaterialIndex = new List<string>();
                    Save.MaterialIndex = new List<string>();
                }

                if (manager.Editor.Items[0].ActiveMesh != null)
                {
                    Save.UseActiveMesh = true;
                    Save.RestoreActiveMeshIndex = new List<string>();
                    Save.ActiveMeshIndex = new List<string>();
                }
            }

            for (int i = 0; i < manager.Editor.Items.Count; i++)                
            {
                Save.RestoreLocked.Add(manager.Editor.Items[i].Locked);
                Save.RestoreItemsAmount.Add(manager.Editor.Items[i].Amount);

                if (Save.UseMaterialChanger && manager.Editor.Items[i].MatChanger)
                    Save.RestoreMaterialIndex.Add(manager.Editor.Items[i].MatChanger.AllIndex);

                if (Save.UseActiveMesh && manager.Editor.Items[i].ActiveMesh)
                    Save.RestoreActiveMeshIndex.Add(manager.Editor.Items[i].ActiveMesh.AllIndex);
            }

            Update_Current_Data_from_Restore();

            if (usePlayerPref)
            {
                SaveDataPlayerPrefs();
                Debug.Log("Default Data saved on PlayerPref using the key: '" + PlayerPrefKey + "'\n" + MTools.Serialize<SelectorSave>(Save));
            }

            manager.OnDataChanged.Invoke();


#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Restore the items values from the Restore Data</summary>
        public virtual void RestoreData(SelectorManager manager)
        {
            LoadDataPlayerPref();

            Update_Current_Data_from_Restore();

            for (int i = 0; i < manager.Editor.Items.Count; i++)            //Save the Locked Elements
            {
                manager.Editor.Items[i].Locked = Save.RestoreLocked[i];
                manager.Editor.Items[i].Amount = Save.RestoreItemsAmount[i];


                if (Save.UseMaterialChanger && manager.Editor.Items[i].MatChanger)
                    manager.Editor.Items[i].MatChanger.AllIndex = Save.RestoreMaterialIndex[i];

                if (Save.UseActiveMesh && manager.Editor.Items[i].ActiveMesh)
                    manager.Editor.Items[i].ActiveMesh.AllIndex = Save.RestoreActiveMeshIndex[i];
            }


            SaveDataPlayerPrefs();

            UpdateItems(manager);                                       //Update the Items from the Data
            manager.OnDataChanged.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        private void Update_Current_Data_from_Restore()
        {
            Save.Coins = Save.RestoreCoins;

            Save.Locked = new List<bool>(Save.RestoreLocked);
            Save.ItemsAmount = new List<int>(Save.RestoreItemsAmount);

            if (Save.UseMaterialChanger) Save.MaterialIndex = new List<string>(Save.RestoreMaterialIndex);
            if (Save.UseActiveMesh) Save.ActiveMeshIndex = new List<string>(Save.RestoreActiveMeshIndex);
        }


        /// <summary>Update the Data values with the Items values</summary>
        public virtual void UpdateData(SelectorManager manager)
        {
            if (manager.Editor.Items.Count != Save.ItemsAmount.Count)
            {
                Debug.LogWarning("Please, on the Selector Manager Press 'Save initial Data'\nYou have add or remove items and the current Items ammount and the Items amount in the Data File are not the same");
                return;
            }

            for (int i = 0; i < manager.Editor.Items.Count; i++)            //Save the Locked Elements
            {
                Save.Locked[i] = manager.Editor.Items[i].Locked;
                Save.ItemsAmount[i] = manager.Editor.Items[i].Amount;

                if (Save.UseMaterialChanger && manager.Editor.Items[i].MatChanger)
                    Save.MaterialIndex[i] = manager.Editor.Items[i].MatChanger.AllIndex;

                if (Save.UseActiveMesh && manager.Editor.Items[i].ActiveMesh)
                    Save.ActiveMeshIndex[i] = manager.Editor.Items[i].ActiveMesh.AllIndex;
            }

            Save.FocusedItem = manager.Controller.FocusedItemIndex;

            SaveDataPlayerPrefs();

            manager.OnDataChanged.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public virtual void LoadDataPlayerPref()
        {
            if (usePlayerPref && PlayerPrefs.HasKey(PlayerPrefKey))
            {
                Save = MTools.Deserialize<SelectorSave>(PlayerPrefs.GetString(PlayerPrefKey));            //Get the Data from the Player Preff.
            }
        }

        /// <summary>Save the Data on the Player Pref Settings </summary>
        public virtual void SaveDataPlayerPrefs()
        {
            if (usePlayerPref)
            {
                if (PlayerPrefs.HasKey(PlayerPrefKey)) PlayerPrefs.DeleteKey(PlayerPrefKey);

                PlayerPrefs.SetString(PlayerPrefKey, MTools.Serialize<SelectorSave>(Save));
            }
        }

        /// <summary>
        /// Update the Items values with the Data Values.
        /// </summary>
        public virtual void UpdateItems(SelectorManager manager)
        {
            if (!manager.Editor) return;

            if (manager.Editor.Items.Count != Save.Locked.Count)
            {
                Debug.LogWarning("Please, on the Selector Manager Press 'Save initial Data'\nYou have add or remove items and the current Items ammount and the Items amount in the Data File are not the same");
                return;
            }

            for (int i = 0; i < manager.Editor.Items.Count; i++)            //Save the Locked Elements
            {
                manager.Editor.Items[i].Locked = Save.Locked[i];
                manager.Editor.Items[i].Amount = Save.ItemsAmount[i];

                if (Save.UseMaterialChanger && manager.Editor.Items[i].MatChanger)
                    manager.Editor.Items[i].MatChanger.AllIndex = Save.MaterialIndex[i];

                if (Save.UseActiveMesh && manager.Editor.Items[i].ActiveMesh)
                    manager.Editor.Items[i].ActiveMesh.AllIndex = Save.ActiveMeshIndex[i];
            }

            manager.Controller.FocusedItemIndex = Save.FocusedItem;

            manager.OnDataChanged.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
