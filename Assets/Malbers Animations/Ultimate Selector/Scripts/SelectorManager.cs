using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using MalbersAnimations.Utilities;
using MalbersAnimations.Events;
using UnityEngine.Events;

namespace MalbersAnimations.Selector
{
    /// <summary> Master Script to Control and Hold the Character Selector Scripts</summary>
    [HelpURL("https://malbersanimations.gitbook.io/animal-controller/ultimate-selector/selector-manager")]
    public class SelectorManager : MonoBehaviour
    {
        #region Variables 
        public GameObject OriginalItemSelected;                     //Original Object of the Selected Item
        public MItem ItemSelected;                                  //The item Selected
        public bool DontDestroy = true;
        public bool DontDestroySelectedItem = true;

        public bool InstantiateItems = true;                        //Instantiate the original items on the scene
        public bool RemoveLast = false;

        private GameObject lastSpawnedItem;                         //the Last Spawned item
        private MItem lastItemAdded;                                //Store the last item added

        public Transform SpawnPoint;                                //Where do you want to spawn the Item.
        public MItemSet LoadItemSet;                                //Where do you want to spawn the Item.

        public GameObjectEvent OnSelected = new GameObjectEvent();
        public GameObjectEvent OnOpen = new GameObjectEvent();
        public GameObjectEvent OnClosed = new GameObjectEvent();
        public GameObjectEvent OnItemFocused = new GameObjectEvent();


        public UnityEngine.Events.UnityEvent OnDataChanged = new UnityEngine.Events.UnityEvent();

        private SelectorController controller;
        private SelectorEditor editor;
        private SelectorUI ui;

        /// <summary>Scriptable Object that contains the Save Data </summary>
        public SelectorData Data;

        /// <summary>Transform to animate the Open and Close Animations for the Selector</summary>
        public Transform Target;


        public TransformAnimation EnterAnimation;
        public TransformAnimation ExitAnimation;
        public TransformAnimation FocusItemAnim;
        public TransformAnimation SubmitItemAnim;

        DeltaTransform LastTarget = new DeltaTransform();

      //  public InputRow Input = new InputRow("Cancel", KeyCode.Escape, InputButton.Down);

        public bool enableSelector = true;


        #region Properties
        /// <summary>Store the Last Instantiate Item </summary>
        public GameObject Last_SpawnedItem
        {
            get { return lastSpawnedItem; }
            set { lastSpawnedItem = value; }
        }

        public SelectorController Controller
        {
            get
            {
                if (!controller) controller = GetComponentInChildren<SelectorController>();
                return controller;
            }
        }
        public SelectorEditor Editor
        {
            get
            {
                if (!editor) editor = GetComponentInChildren<SelectorEditor>();
                return editor;
            }
        }
        public SelectorUI UI
        {
            get
            {
                if (!ui) ui = GetComponentInChildren<SelectorUI>();
                return ui;
            }
        }

        /// <summary>List of items</summary>
        public List<MItem> Items
        {
            get
            {
                if (controller == null) return null;
                return controller.Items;
            }
        }

        /// <summary>
        /// Check if the Selector is Animating When Open or Close
        /// </summary>
        protected bool isAnimating = false;

        /// <summary>
        /// Hide/Show the Selector system
        /// </summary>
        public bool EnableSelector
        {
            get { return enableSelector; }
            set
            {
                if (enableSelector != value && !isAnimating)
                {
                    enableSelector = value;
                    StopAllCoroutines();

                    if (enableSelector)         //If the Selector is Open
                    {
                        StartCoroutine(PlayAnimation(EnterAnimation, true));
                        OnOpen.Invoke(Last_SpawnedItem);
                    }
                    else
                    {
                        StartCoroutine(PlayAnimation(ExitAnimation, false));
                        OnClosed.Invoke(Last_SpawnedItem);
                        if (Data) Data.UpdateData(this);            //Update Data when the selector closed
                    }
                }
            }
        }

        /// <summary>
        /// Return the last added item
        /// </summary>
        public MItem LastAddedItem
        {
            get { return lastItemAdded; }
            private set { lastItemAdded = value; }
        }

        #endregion
        #endregion

        //──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        void Awake()
        {
            if (Controller || Editor || UI) { }         //Store all the Selector Components on variables
        }
        private void Start()
        {
            _NewItemSet(LoadItemSet); //Load Item set at start

            if (DontDestroy) DontDestroyOnLoad(transform.root.gameObject);

            if (Data)
            {
                Data.LoadDataPlayerPref();              //Checks if you're using PlayerPref and load the Data from there
                Data.UpdateItems(this);                 //If you are using Data Update it   
            }

            LastTarget.StoreTransform(Target);          //Store the Transform of the Target to Animate.     

            if (!EnableSelector)
            {
                if (Controller) Controller.gameObject.SetActive(false);
                if (UI) UI.gameObject.SetActive(false);
            }

            if (Controller.CurrentItem)
                UpdateSelectedItem(Controller.CurrentItem.gameObject);          //Update the Focused item on the Manager

            if (ItemSelected)
            {
                UI.UpdateSelectedItemUI(ItemSelected.gameObject);       //Updates the UI.
            }
        }

        //──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        /// <summary>Toggle the Selector On and Off</summary>
        public virtual void ToggleSelector()
        {
            EnableSelector = !EnableSelector;
        }

        /// <summary>Open the Selector</summary>
        public virtual void OpenSelector()
        {
            EnableSelector = true;
        }

        /// <summary>Close the Selector</summary>
        public virtual void CloseSelector()
        {
            EnableSelector = false;
        }


        void OnEnable()
        {
            Data?.UpdateItems(this);
            OnItemFocused.AddListener(UpdateSelectedItem);
            if (UI) OnItemFocused.AddListener(UI.UpdateSelectedItemUI);
            Controller?.UpdateLockItems();
        }
        
        void OnDisable()
        {
             OnItemFocused.RemoveListener(UpdateSelectedItem);
            if (UI) OnItemFocused.RemoveListener(UI.UpdateSelectedItemUI);
        }

      

        //──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        private void UpdateSelectedItem(GameObject item)
        {
            if (item == null) return;
            ItemSelected = item.GetComponent<MItem>();

            if (ItemSelected)
            {
                OriginalItemSelected = ItemSelected.OriginalItem;
            }
        }

        /// <summary>Purchase item</summary>
        public void Purchase(MItem item)
        {
            if (Data)
            {
                if (Data.Save.Coins - item.Value >= 0)          //If we have money to buy?
                {
                    Data.Save.Coins -= item.Value;
                    item.Locked = false;                //Unlock the Item
                }
                Data.UpdateData(this);
            }
            else
            {
                item.Locked = false;
               // Controller.UpdateLockItems();
            }
        }

        /// <summary>Unlocks an Item </summary>
        public void UnlockItem(MItem item)
        {
            item.Locked = false;                //Unlock the Item
            if (Data) Data.UpdateData(this);
        }

        public virtual void UnlockSelected()
        {
            ItemSelected.Locked = false;
            if (Data) Data.UpdateData(this);
        }

        /// <summary>
        /// Spawns the Original Object of the Item
        /// </summary>
        public virtual void InstantiateItem()
        {
            if (ItemSelected == null) return;                           //If there's no Selected Item Skip

            if (RemoveLast && Last_SpawnedItem)                         //If Remove Last Spawned is enable remove the Last Instantiate items
            {
                Destroy(Last_SpawnedItem);
            }
            //Instantiate the Item Origninal GameObject on the SpawnPoint, if there's no spawnpoint use this transform
            Last_SpawnedItem =
                Instantiate(ItemSelected.OriginalItem ? ItemSelected.OriginalItem : ItemSelected.gameObject,
                SpawnPoint ? SpawnPoint.position : transform.position,
                SpawnPoint ? SpawnPoint.rotation : Quaternion.identity);

            Last_SpawnedItem.name = lastSpawnedItem.name.Replace("(Clone)", "");

            if (DontDestroySelectedItem) DontDestroyOnLoad(Last_SpawnedItem);       //if Dont Destroy is enabled set the selected item to the Don't Destroy
           

            Sync_Material_ActiveMeshes();                                                    //Sync all Material Changers on Item and Original GO
        }


        /// <summary>
        /// Parent the Selected Item Instance to the Spawn Point
        /// </summary>
        /// <param name="ResetScale"></param>
        public virtual void _ParentToSpawnPoint(bool ResetScale = false)
        {
            if (!SpawnPoint) return;

            lastSpawnedItem.transform.SetParent(SpawnPoint, true);
            if (ResetScale) lastSpawnedItem.transform.localScale = Vector3.one;
        }


        /// <summary>Reduce the Amount of the Focused item </summary>
        public virtual void _ReduceAmountSelected()
        {
            if (ItemSelected && ItemSelected.Amount > 0)
            {
                ItemSelected.Amount--;
            }

            if (Data) Data.UpdateData(this);      //Update the Data !!
        }

        /// <summary>Sync the values of the Item and Original Object if they have the Material & Mesh Changer </summary>
        protected virtual void Sync_Material_ActiveMeshes()
        {
            if (Last_SpawnedItem)
            {
                MaterialChanger OriginalMC = Last_SpawnedItem.GetComponent<MaterialChanger>();
                MaterialChanger ItemSelMC = ItemSelected.GetComponent<MaterialChanger>();

                if (OriginalMC && ItemSelMC)
                {
                    for (int i = 0; i < ItemSelMC.materialList.Count; i++)
                    {
                        OriginalMC.SetMaterial(i, ItemSelMC.CurrentMaterialIndex(i)); //Set the Materials from the Item to the Original
                    }
                }

                ActiveMeshes OriginalAM = Last_SpawnedItem.GetComponent<ActiveMeshes>();
                ActiveMeshes ItemSelAM = ItemSelected.GetComponent<ActiveMeshes>();

                if (OriginalAM && ItemSelAM)
                {
                    for (int i = 0; i < ItemSelAM.Count; i++)
                    {
                        OriginalAM.ChangeMesh(i, ItemSelAM.GetActiveMesh(i).Current); //Set the Meshes from the Item to the Original 
                    }
                }
            }
        }

        /// <summary> Save to the Data the Default Data </summary>
        public virtual void SaveDefaultData()
        {
            if (Data) Data.SetDefaultData(this);
        }

        /// <summary>Restore to the Data using the Default Data </summary>
        public virtual void RestoreToDefaultData()
        {
            if (Data) Data.RestoreData(this);                           //Restore the Data 

            if (Application.isPlaying)
            {
                Controller.UpdateLockItems();                           //Updates the Items if they were locked
                UI.UpdateSelectedItemUI(ItemSelected.gameObject);       //Updates the UI.
            }
        }


        /// <summary> Change the material on the selected Item, also updates the data</summary>
        /// <param name="next">true: Next, false: Previous</param>
        public virtual void _ChangeCurrentItemMaterial(bool Next)
        {
            if (Controller.IndexSelected == -1) return;    //Skip if the Selection is clear
            Controller.CurrentItem.SetAllMaterials(Next);
            if (Data) Data.UpdateData(this);

        }


        /// <summary> Change on Material Changer a Material item to the next using the Name</summary>
        /// <param name="Name">name of the Material Item</param>
        public virtual void _ChangeCurrentItemMaterial(string Name)
        {
            _ChangeCurrentItemMaterial(Name, true);
        }

        /// <summary>
        /// Change on Material Changer a Material item to the next or before using the Name
        /// </summary>
        /// <param name="Name">name of the Material Item</param>
        public virtual void _ChangeCurrentItemMaterial(string Name, bool Next)
        {
            if (Controller.IndexSelected == -1) return;    //Skip if the Selection is clear
            Controller.CurrentItem.SetMaterial(Name, Next);
            if (Data) Data.UpdateData(this);
        }

        /// <summary>
        /// Change on Material Changer a Material item to the next material using the Index
        /// </summary>
        /// <param name="Name">name of the Material Item</param>
        public virtual void _ChangeCurrentItemMaterial(int Index)
        {
            _ChangeCurrentItemMaterial(Index, true);
        }

        /// <summary>
        /// Change on Material Changer a Material item to the next or before Material using the Index
        /// </summary>
        /// <param name="Name">name of the Material Item</param>
        public virtual void _ChangeCurrentItemMaterial(int Index, bool Next)
        {
            if (Controller.IndexSelected == -1) return;    //Skip if the Selection is clear
            Controller.CurrentItem.SetMaterial(Index, Next);
            if (Data) Data.UpdateData(this);
        }

        /// <summary>
        /// If the  Current Item has an ActiveMeshes component, it change an Active Mesh on the list to the Next Mesh using the List “Index”.
        /// </summary>
        public virtual void _ChangeCurrentItemMesh(int index)
        {
            _ChangeCurrentItemMesh(index, true);
        }

        /// <summary>
        /// If the  Current Item has an ActiveMeshes component, it change an Active Mesh on the list to the Next(true) Before(false) Mesh using the List “Index”.
        /// </summary>
        public virtual void _ChangeCurrentItemMesh(int index, bool Next)
        {
            if (Controller.IndexSelected == -1) return;    //Skip if the Selection is clear
            Controller.CurrentItem.ChangeMesh(index, Next);
            if (Data) Data.UpdateData(this);
        }

        /// <summary>
        /// Change a  Mesh by Name on the Active Meshes List
        /// </summary>
        public virtual void _ChangeCurrentItemMesh(string name)
        {
            if (Controller.IndexSelected == -1) return;                                    //Skip if the Selection is clear
            Controller.CurrentItem.ChangeMesh(name);
            if (Data) Data.UpdateData(this);
        }

        /// <summary>  Change All Meshes on the Active Meshes List  </summary>
        public virtual void _ChangeCurrentItemMesh(bool next)
        {
            if (Controller.IndexSelected == -1) return;                                 //Skip if the Selection is clear
            Controller.CurrentItem.ChangeMesh(next);
            if (Data) Data.UpdateData(this);
        }

        /// <summary>Select the Item if is not locked and the amount is greater than zero, also instantiate it if Instantiate Item is enabled.</summary>
        public virtual void SelectItem()
        {
            if (!ItemSelected) return;                                          //If there's no Item Skip

            if (!ItemSelected.Locked && ItemSelected.Amount > 0)                //If the items is not locked and the we have one or More Item ....
            {
                if (InstantiateItems) InstantiateItem();
                OnSelected.Invoke(Last_SpawnedItem);
                Controller.CurrentItem.OnSelected.Invoke();                     //Invoke Events on the Selected Item
                controller._PlayAnimationTransform(SubmitItemAnim);
            }

            if (UI) UI.UpdateSelectedItemUI(ItemSelected.gameObject);       //Update the UI
        }


        /// <summary>Change to the next scene using the scene name </summary>
        public virtual void _ChangeToScene(string SceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }

        /// <summary>Change to the next scene using the scene Index</summary>
        public virtual void _ChangeToScene(int SceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneIndex);
        }


        /// <summary>  Adds an Item at Runtime </summary>
        /// <param name="item">the Item to Add</param>
        public virtual void _AddItem(MItem item)
        {
            if (!item) return;

            var gameObject = Instantiate(item.gameObject, Editor.transform, false);
            gameObject.name = gameObject.name.Replace("(Clone)", "");

            StartCoroutine(HideShowNewItem(gameObject));
            StartCoroutine(CheckItemsNextFrame());
            LastAddedItem = item;
        }

        IEnumerator HideShowNewItem(GameObject go)
        {
            go.SetActive(false);
            yield return null;
            go.SetActive(true);
        }
       

        /// <summary>  Removes an Item at Runtime using its Mitem script </summary>
        /// <param name="item">item to remove</param>
        public virtual void _RemoveItem(MItem item)
        {
            RemoveItemCommon(Editor.Items.Find(mitem => mitem == item));
        }

        /// <summary> Removes an Item at Runtime using the name  </summary>
        /// <param name="item">item to remove</param>
        public virtual void _RemoveItem(string itemName)
        {
            RemoveItemCommon(Editor.Items.Find(item => item.name == itemName));
        }

        /// <summary>
        /// Removes an Item at Runtime using the list index position
        /// </summary>
        /// <param name="item">item to remove</param>
        public virtual void _RemoveItem(int Index)
        {
            if (Index >= 0 && Index < Editor.Items.Count)
            {
                RemoveItemCommon(Editor.Items[Index]);
            }
        }

        public virtual void _NewItemSet(MItemSet set)
        {
            if (set)
            {
                _RemoveAllItems();

                foreach (var item in set.Set)
                {
                    var gameobj = Instantiate(item.gameObject, Editor.transform, false);
                    gameobj.name = gameobj.name.Replace("(Clone)", "");
                }

                StartCoroutine(CheckItemsNextFrame());
            }
        }


        /// <summary> Remove all items from the selector  </summary>
        public virtual void _RemoveAllItems()
        {
            Controller.FocusedItemIndex = 0;
            foreach (var item in Editor.Items)
            {
                Destroy(item.gameObject);
            }

            Editor.Items = new List<MItem>();

            Controller.ResetController();
        }


        private void RemoveItemCommon(MItem toRemove)
        {
            if (toRemove == null) return;

            int toRemoveIndex = Editor.Items.FindIndex(item => item == toRemove);           //Find the index of the item to remove..

            if (Controller.FocusedItemIndex > toRemoveIndex) Controller.FocusedItemIndex--;

            Destroy(toRemove.gameObject);

            StartCoroutine(CheckItemsNextFrame());                                            //Destroy first then the next frame update all
        }

        /// <summary> Plays an Enter or Exit Animation for the selector </summary>
        private IEnumerator PlayAnimation(TransformAnimation animTransform, bool Enter)
        {
            if (Enter)                                                      //Enable the controller and UI GAME OBJECTS
            {
                if (Controller)
                {
                    Controller.gameObject.SetActive(true);
                    if (Controller.FocusedItemIndex == -1) ItemSelected = null;
                    Controller.enabled = true;                       //Enable the Controller component after the animation.
                 //   StartCoroutine(CheckItemsNextFrame());           //Position all the Items on the default position
                }
                if (UI)
                {
                    UI.UpdateSelectedItemUI(ItemSelected);
                    UI.gameObject.SetActive(true);
                }
            }
            else
            {
            }


            if (animTransform && Target)
            {
                isAnimating = true;

                LastTarget.StoreTransform(Target);


                if (animTransform.delay > 0) yield return new WaitForSeconds(animTransform.delay);

                float elapsedTime = 0;

                while ((animTransform.time > 0) && (elapsedTime <= animTransform.time))

                {
                    float resultPos = animTransform.PosCurve.Evaluate(elapsedTime / animTransform.time);               //Evaluation of the Pos curve
                    float resultRot = animTransform.RotCurve.Evaluate(elapsedTime / animTransform.time);               //Evaluation of the Rot curve
                    float resultSca = animTransform.ScaleCurve.Evaluate(elapsedTime / animTransform.time);               //Evaluation of the Scale curve

                    if (animTransform.UsePosition)
                    {
                        Target.localPosition =
                        Vector3.LerpUnclamped(LastTarget.LocalPosition, LastTarget.LocalPosition + animTransform.Position, resultPos);
                    }

                    if (animTransform.UseRotation)
                    {
                        Target.localEulerAngles = Vector3.LerpUnclamped(LastTarget.LocalEulerAngles, animTransform.Rotation, resultRot);
                    }

                    if (animTransform.UseScale)
                        Target.localScale = Vector3.LerpUnclamped(LastTarget.LocalScale, Vector3.Scale(LastTarget.LocalScale, animTransform.Scale), resultSca);

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                isAnimating = false;
                LastTarget.RestoreLocalTransform(Target);

            }

            if (!Enter)
            {
                if (Controller)
                {
                    Controller.gameObject.SetActive(false);
                    Controller.enabled = false;                      //Disable the Controller component after the animation.
                }
                if (UI) UI.gameObject.SetActive(false);

            }
            else
            {
                if (UI)
                {
                    UI.gameObject.SetActive(true);
                    UI.UpdateSelectedItemUI(ItemSelected);
                }
            }

            yield return null;
        }

        private IEnumerator CheckItemsNextFrame()
        {
            yield return null;
            Editor.UpdateItemsList();                                           //Update the list!
            Controller.ResetController();

            foreach (var item in Editor.Items)
            {
                item.RestoreInitialTransform();
            }
            Controller.CheckFirstItemFocus();
        }

        private void OnApplicationQuit()
        {
            if (EnableSelector)
            {
                if (Data) Data.UpdateData(this);
            }
        }

        [ContextMenu("Create Inputs")]
        protected void CreateInputs()
        {
#if UNITY_EDITOR
            MInput input = GetComponent<MInput>();

            if (input == null)
                input = gameObject.AddComponent<MInput>();

            #region Open Close Input
            var OpenCloseInput = input.FindInput("OpenSelector");
            if (OpenCloseInput == null)
            {
                OpenCloseInput = new InputRow("OpenSelector", "OpenSelector", KeyCode.Escape, InputButton.Down, InputType.Key);
                input.inputs.Add(OpenCloseInput);
                UnityEditor.Events.UnityEventTools.AddPersistentListener (OpenCloseInput.OnInputDown, ToggleSelector);
            }
            #endregion

            #region Submit Input
            var Submit = input.FindInput("Submit");
            if (Submit == null)
            {
                Submit = new InputRow("Submit", "Submit", KeyCode.Return, InputButton.Down, InputType.Key);
                input.inputs.Add(Submit);
                UnityEditor.Events.UnityEventTools.AddPersistentListener(Submit.OnInputDown, Controller.Submit_Input);
            }
            #endregion

            #region ChangeLeft Input
            var ChangeLeft = input.FindInput("ChangeLeft");
            if (ChangeLeft == null)
            {
                ChangeLeft = new InputRow("ChangeLeft", "ChangeLeft", KeyCode.LeftArrow, InputButton.Down, InputType.Key);
                input.inputs.Add(ChangeLeft);
                UnityEditor.Events.UnityEventTools.AddPersistentListener(ChangeLeft.OnInputDown, Controller.ChangeLeft);
            }
            #endregion

            #region ChangeRight Input
            var ChangeRight = input.FindInput("ChangeRight");
            if (ChangeRight == null)
            {
                ChangeRight = new InputRow("ChangeRight", "ChangeRight", KeyCode.RightArrow, InputButton.Down, InputType.Key);
                input.inputs.Add(ChangeRight);
                UnityEditor.Events.UnityEventTools.AddPersistentListener(ChangeRight.OnInputDown, Controller.ChangeRight);
            }
            #endregion

            #region ChangeUp Input
            var ChangeUp = input.FindInput("ChangeUp");
            if (ChangeUp == null)
            {
                ChangeUp = new InputRow("ChangeUp", "ChangeUp", KeyCode.UpArrow, InputButton.Down, InputType.Key);
                input.inputs.Add(ChangeUp);
                UnityEditor.Events.UnityEventTools.AddPersistentListener(ChangeUp.OnInputDown, Controller.ChangeUp);
            }
            #endregion

            #region ChangeDown Input
            var ChangeDown = input.FindInput("ChangeDown");
            if (ChangeDown == null)
            {
                ChangeDown = new InputRow("ChangeDown", "ChangeDown", KeyCode.DownArrow, InputButton.Down, InputType.Key);
                input.inputs.Add(ChangeDown);
                UnityEditor.Events.UnityEventTools.AddPersistentListener(ChangeDown.OnInputDown, Controller.ChangeDown);
            }
            #endregion

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(input);
#endif
        }



        private void Reset()
        {
            CreateInputs();
        }

        [HideInInspector] public bool ShowEvents = true;
        [HideInInspector] public bool ShowAnims = true;
    }
}