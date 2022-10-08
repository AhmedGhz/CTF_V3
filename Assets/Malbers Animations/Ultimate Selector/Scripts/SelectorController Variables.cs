using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using MalbersAnimations.Utilities;
using MalbersAnimations.Events;
using System;

namespace MalbersAnimations.Selector
{
    //──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    //VARIABLES AND PROPERTIES
    //──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    public partial class SelectorController
    {
        static Keyframe[] K = { new Keyframe(0, 0), new Keyframe(1, 1) };
        static Keyframe[] K2 = { new Keyframe(0,1,0,0), new Keyframe(1, 0,0,0) };

        #region Public Variables

        /// <summary>Animate(Move|Rotate) the Selector setting the Focused Item on the center of the Selector Controller</summary>
        public bool AnimateSelection = true;

        public bool SoloSelection = false;                          //Select the items by click in on the item
        public bool Hover = false;                                  //if true, selection by hovering on the item is activated
        public float SelectionTime = 0.3f;
        public float RestoreTime = 0.15f;                            //Restore Time of an Item to its original position.
        public AnimationCurve SelectionCurve = new AnimationCurve(K);

        public int focusedItemIndex = 0;                            //This is for set the Selection everytime is called to this new Index
        /// <summary> Drag Multiplier when Swipping </summary>
        public float DragSpeed = 20;                                //Drag/Swipe Speed        
        public bool dragHorizontal = true;

        public float Threshold = 1f;                                //Range to identify the hold and swap/drag as a click

        /// <summary> Click over an item to focus and centered it if is not the current focused item   </summary>
        public bool ClickToFocus = true;
        public Material LockMaterial;                               //Lock Material 

        public bool frame_Camera;                                   //Frame the camera to the bound box of the Item
        public float frame_Multiplier = 1;                          //Multiplier for make

        public bool RotateIdle, MoveIdle, ScaleIdle;                //Set Animations Idles

        #region Inertia
        /// <summary> The Selector uses Inertia</summary>
        public bool inertia = false;
        public float inertiaTime = 1;
        public AnimationCurve inertiaCurve = new AnimationCurve(K2);
        public float minInertiaSpeed = 1f;
        protected IEnumerator CInertia;
        #endregion


        public float ItemRotationSpeed = 5;                         //TurnTable Speed        
        public Vector3 TurnTableVector = Vector3.up;                //TurnTable Vector 
        public bool ChangeOnEmptySpace = true;                      //If there's a Click/Touch on an empty space change to the next/previous item
        public TransformAnimation MoveIdleAnim, ScaleIdleAnim;      //Idle Animations
        #endregion

        public bool UseSelectionZone = false;
        [Range(0, 1)]
        public float ZMinX = 0;
        [Range(0, 1)]
        public float ZMaxX = 1;
        [Range(0, 1)]
        public float ZMinY = 0;
        [Range(0, 1)]
        public float ZMaxY = 1;

        /// <summary> The Selector is a Radial one </summary>
        public bool RadialSelector => S_Editor.SelectorType == SelectorType.Radial;

        /// <summary> The Selector is a Linear one </summary>
        public bool LinearSelector => S_Editor.SelectorType == SelectorType.Linear;

        /// <summary> The Selector is a Linear one </summary>
        public bool GridSelector => S_Editor.SelectorType == SelectorType.Grid;



        #region Internal Variables
        /// <summary>  Initial transform of the whole  Selector  </summary>
        protected DeltaTransform InitialTransform;

        /// <summary>The Angle between Items (For Circle Selector) </summary>
        protected float angle => S_Editor.Angle;
      

        /// <summary>The Distance between items    (For Linear Selector) </summary>
        protected float distance =>S_Editor.distance / 2; 
       
        protected bool isActive = true;

        /// <summary>Initial Camera Position where the CircleEditor edited it</summary>
        protected Vector3 InitialPosCam;

        public bool debug = false;
        
        
        /// <summary> Current Selected Mitem</summary>
        private MItem current;                                      

        /// <summary> Gets the total of the Rows for the grid  </summary>
        protected int Rows =>  Items.Count / S_Editor.Grid + 1;

        /// <summary>Gets the total grid size</summary>
        protected int GridTotal => Rows * S_Editor.Grid;

        protected Vector3 Linear => S_Editor.LinearVector;

        protected bool isChangingItem = false;                      //Check if is changin items
        protected bool isAnimating = false;                         //Check if is Animating                   
        protected bool isSwapping = false;                          //Check if is Swapping
        /// <summary>is the Selector moving by inertia </summary>
        bool isInInertia;
       // bool InertiaInterrupted;
        protected bool isRestoring = false;                         //Check if is Restoring

        protected DeltaTransform LastTCurrentItem;                  //Last current item position

        private int indexSelected = -1;                             //The selected Index
        private int previousIndex = -1;                             //The Previous selected Index

        internal float IdleTimeMove = 0, IdleTimeScale = 0;         //The global Idle Time for playing idle transform animations.
        internal float DragDistance = 0;                            //How much distance was traveled when is dragin/swapping
        internal float InertiaLastDrag = 0;
        /// <summary>Speed acumulated while draging</summary>
        protected float DeltaSpeed;
        /// <summary> Drag on the Last Frame </summary>
        protected float lastDrag; 

        internal int UILayer;

        public List<MItem> Items => s_Editor.Items;
       

        #region MouseVariables
        protected DeltaTransform DeltaSelectorT;
        protected Vector3 MouseStartPosition;                       //the First mouse/touch position when the click/touch is down.
        protected Vector2 DeltaMousePos;
        /// <summary> Last Animated Item</summary>
        protected MItem LastAnimatedItem;

        /// <summary>Last animation of the last Animated Item</summary>
        protected TransformAnimation LastAnimation;                   

        /// <summary> Use for Skip the Animation and Restoring when the Controller is enabled </summary>
        private bool isEnabling = true;                             

        private PointerEventData CurrentEventData;                  //Store the current event data
        /// <summary>Hover Last Hitted Item </summary>
        private MItem HitItem;
        #endregion
        #endregion

        #region Coroutines
        IEnumerator AnimateSelectionC;
        IEnumerator PlayTransformAnimationC;
        #endregion

        #region Events
        public GameObjectEvent OnClickOnItem = new GameObjectEvent();
        public BoolEvent OnIsChangingItem = new BoolEvent();
        #endregion

        #region Properties
        /// <summary> Returns the Current Selected Item   </summary> 
        public MItem CurrentItem => current;
         

        /// <summary>Return the LocalRotation of the Radial Axis of the Selector
        protected Vector3 RadialVector
        {
            get
            {
                switch (S_Editor.RadialAxis)
                {
                    case RadialAxis.Up:
                        return transform.InverseTransformDirection(transform.up);
                    case RadialAxis.Right:
                        return transform.InverseTransformDirection(-transform.right);
                    case RadialAxis.Forward:
                        return transform.InverseTransformDirection(-transform.forward);
                }
                return transform.up;
            }
        }

        /// <summary> Return the Index of the Selected Item... also Invoke OnItemSelected</summary>
        public  int IndexSelected
        {
            get => indexSelected; 
            private set
            {
                if (Items == null || Items.Count == 0) return;
                if (!gameObject.activeInHierarchy || !enabled) return;
                 

                previousIndex = indexSelected;                  //Set the previous item to the las tiem selected

                indexSelected = value;
                
                if (debug) Debug.Log($"Focused Item Index <B>[{indexSelected}]</B>");

                Animate_to_Next_Item();                         //Activate all the Animations to change Item

                if (indexSelected == -1)                        //Clear Selection
                {
                    current = null;
                    S_Manager.OnItemFocused.Invoke(null);                //Let everybody knows that NO item was selected;
                }
                else
                {
                    indexSelected =
                        indexSelected < 0 ? Items.Count - 1 : indexSelected % Items.Count; //Just set the IndexSelect to the Limit Range

                    current = Items[indexSelected];
                    FocusedItemIndex = indexSelected;

                    if (S_Manager.Data) S_Manager.Data.Save.FocusedItem = FocusedItemIndex;         //UPDATE THE DATA WITH THE FOCUSED ITEM

                    LastTCurrentItem.LocalPosition = current.StartPosition;
                    LastTCurrentItem.LocalScale = current.StartScale;
                    LastTCurrentItem.LocalRotation = current.StartRotation;

                    IdleTimeMove = IdleTimeScale = 0;                                   //Reset the Idle Time
                    if (CurrentItem)
                    {
                        S_Manager.OnItemFocused.Invoke(CurrentItem.gameObject);         //Let everybody knows that  the item selected have been changed   
                        _PlayAnimationTransform(S_Manager.FocusItemAnim);
                        CurrentItem.OnFocused.Invoke();                                 //Invoke from the Item Event (On Focused) 
                    }
                    FrameCamera();
                }
            }
        }

        /// <summary> The Index of the last previous selected item</summary>
        public int PreviousIndex => previousIndex;
         

        /// <summary>The LocalRotation of the World Equivalent</summary>
        protected Quaternion UseWorldRotation => Quaternion.Inverse(transform.localRotation) * InitialTransform.LocalRotation;
       

        /// <summary> Reference for the MItem of the previous Item </summary>
        public MItem PreviousItem
        {
            get
            {
                if (previousIndex == -1 || previousIndex >= Items.Count)
                    return null;

                return Items[previousIndex];
            }
        }

        /// <summary>True if is on transition from one item to another. Also Invoke OnChanging Item</summary>
        protected bool IsChangingItem
        {
            get => isChangingItem; 
            set
            {
                isChangingItem = value;
                OnIsChangingItem.Invoke(isChangingItem);
            }
        }

        /// <summary>True if is Swapping/Dragging from one item to another. Also Invoke OnChanging Item</summary>
        protected bool IsSwapping
        {
            get => isSwapping;  
            set
            {
                isSwapping = value;
                OnIsChangingItem.Invoke(isSwapping);
            }
        }

        /// <summary>Main Input for the Selector True when the Input is Pressed</summary> ********************************??????????????
        protected bool MainInputPress => Input.GetMouseButton(0);
        /// <summary> Main Input for the Selector True when the Input is Down</summary>
        protected bool MainInputDown => Input.GetMouseButtonDown(0);
      

        /// <summary>Main Input for the Selector True when the Input is Released </summary>
        protected bool MainInputUp => Input.GetMouseButtonUp(0);

        /// <summary>Set the Selector Controller Active or inactive</summary>
        public bool IsActive { get => isActive; set => isActive = value; }

        /// <summary> Return true if is not Animating, Restoring or Swapping </summary>
        public bool ZeroMovement => !isRestoring && !IsSwapping && !isAnimating;
       
        /// <summary> Reference for the Selector Editor </summary>
        protected SelectorEditor S_Editor
        {
            get
            {
                if (s_Editor == null)
                {
                    s_Editor = GetComponent<SelectorEditor>();
                }
                return s_Editor;
            }
        }

        /// <summary>Reference for the Selector Manager</summary>
        protected SelectorManager S_Manager
        {
            get
            {
                if (s_manager == null)
                {
                    s_manager = this.FindComponent<SelectorManager>();
                }
                return s_manager;
            }
        }

        #endregion

        #region Reference Variables
        private SelectorEditor s_Editor;
        private SelectorManager s_manager;

        /// <summary>
        /// The Camera for the Selector
        /// </summary>
        protected Camera S_Camera
        { get {return S_Editor.SelectorCamera; } }

        public int FocusedItemIndex
        {
            get { return focusedItemIndex; }

            set { focusedItemIndex = value; }
        }
        #endregion

        #region Editor Variables
        ////EDITOR VARIABLES
        [HideInInspector] public bool EditorShowEvents = true;
        [HideInInspector] public bool EditorIdleAnims = true;
        [HideInInspector] public bool EditorInput = true;
        [HideInInspector] public bool EditorAdvanced = true;
        #endregion
    }
}