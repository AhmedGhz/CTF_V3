using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

namespace MalbersAnimations.Selector
{
    public enum SelectorType { Radial, Linear, Grid, Custom }
    public enum RadialAxis { Up, Right, Forward}
    public enum ItemRenderer { Mesh, Sprite, Canvas }

    [ExecuteInEditMode]
    [HelpURL("https://docs.google.com/document/d/e/2PACX-1vTwj1r5z3KzslDAaI4bFPk6Un9GhQZHa6EIy39UGvpAlkcAuh3ttieQ3MSPwWauOR0tCMMpyzkFwtb0/pub#h.5ue5jztwo572")]
    public class SelectorEditor : MonoBehaviour
    {
        public Camera SelectorCamera;  //Get the Camera 
        public bool WorldCamera = false;
        public float CameraOffset = 2f;         
        public Vector3 CameraPosition;
        public Vector3 CameraRotation;

        public bool createColliders = true;

        [SerializeField] public List<MItem> Items;
        public SelectorType SelectorType;
        public ItemRenderer ItemRendererType = ItemRenderer.Mesh;
        public RadialAxis RadialAxis = RadialAxis.Up;

        public float distance = 3f;
        public float LinearX=1, LinearY=1, LinearZ=1;
        /// <summary>It will keep always looking forward on the radial Style </summary>
        public bool UseWorld = true;
        /// <summary> this will keep always looking forward on the radial Style </summary>
        public bool LookRotation;                       
        public Vector3 RotationOffSet;

        public int Grid = 6;
        public float GridWidth = 1;
        public float GridHeight = 1;

        public Vector3 LinearVector;

        private SelectorController controller;

        public SelectorController Controller
        {
            get
            {
                if (!controller) controller = GetComponentInChildren<SelectorController>();
                return controller;
            }
        }

        protected float angle = 0f;

        /// <summary>
        /// Angle distance between items
        /// </summary>
        public float Angle
        {
            get { return angle; }
        }

        void Awake()
        {
            UpdateItemsList();


        }

        void Update()
        {
            if (transform.childCount != Items.Count) //If is there a new Item Update it
            {
                UpdateItemsList();
            }
        }

        //-----------------------------------Set the Camera Position----------------------------------
        public void SetCamera()
        {
            if (SelectorCamera)
            {
                SelectorCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
                SelectorCamera.transform.eulerAngles += (CameraRotation);
                SelectorCamera.transform.localPosition  = CameraPosition + SelectorCamera.transform.forward*CameraOffset;
            }
        }

        public void StoreCustomLocation()
        {
            foreach (var item in Items)
            {
                item.CustomPosition = item.transform.localPosition;
                item.CustomRotation = item.transform.localRotation;
                item.CustomScale = item.transform.localScale;
            }
        }

        /// <summary>Store in an  Array all the childrens </summary>
        public void UpdateItemsList()
        {
            Items = new List<MItem>();                                          //Reset the list

            if (transform.childCount != 0)
                angle = 360f / transform.childCount;                            //Get The Angle for Radial Selectors

            int ID = 0;

            foreach (Transform child in transform)
            {
                AddItemScript(child);                                           //Add the Item Script and Colliders
                Items.Add(child.GetComponent<MItem>());                         //Update the list
                ItemsLocation(child, ID);                                       //Position the items
                ID++;
            }
        }


        /// <summary>
        /// Store the Initial Location of every Item on the 
        /// </summary>
        public virtual void StoreInitialLocation()
        {
            if (Items.Count>0)
            {
                foreach (var item in Items)
                {
                    item.StartPosition = item.transform.localPosition;
                    item.StartRotation = item.transform.localRotation;
                    item.StartScale = item.transform.localScale;
                }
            }
        }

        /// <summary>
        /// Positions all items depending of the selector type
        /// </summary>
        public void ItemsLocation()
        {
            int i = 0;

            foreach (Transform child in transform)
            {
                ItemsLocation( child, i);
                i++;
            }

       
        }

        /// <summary> Positions all items depending of the selector type </summary>
        public void ItemsLocation(Transform item, int ID)
        {
            Vector3 posItem = Vector3.zero;
            MItem m_item = item.GetComponent<MItem>();
            
            switch (SelectorType)
            {
                case SelectorType.Radial:
                    {
                        switch (RadialAxis)
                        {
                            case RadialAxis.Up:
                                posItem = new Vector3(Mathf.Cos(Angle * ID * Mathf.PI / 180) * distance, 0, Mathf.Sin(Angle * ID * Mathf.PI / 180) * distance);
                                break;
                            case RadialAxis.Right:
                                posItem = new Vector3(0, Mathf.Cos(Angle * ID * Mathf.PI / 180) * distance, Mathf.Sin(Angle * ID * Mathf.PI / 180) * distance);
                                break;
                            case RadialAxis.Forward:
                                posItem = new Vector3(Mathf.Cos(Angle * ID * Mathf.PI / 180) * distance, Mathf.Sin(Angle * ID * Mathf.PI / 180) * distance, 0);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case SelectorType.Linear:
                    posItem = LinearVector * (distance * ID / 2);
                    break;
                case SelectorType.Grid:
                    posItem = new Vector3(ID % Grid * GridWidth, ID / Grid * GridHeight);
                    break;
                case SelectorType.Custom:

                    if (m_item)
                    {
                        item.transform.localPosition = m_item.CustomPosition;
                        item.transform.localRotation = m_item.CustomRotation;
                        item.transform.localScale = m_item.CustomScale;

                        goto SetStartTransform;
                    }
                    break;
                default:
                    break;
            }

                item.transform.localPosition = posItem;

                item.localRotation = Quaternion.identity;

                if (LookRotation)
                {
                    Vector3 LookRotationAxis = Vector3.up;
                    if (RadialAxis == RadialAxis.Right) LookRotationAxis = Vector3.right;
                    if (RadialAxis == RadialAxis.Forward) LookRotationAxis = Vector3.forward;

                    item.transform.LookAt(transform, LookRotationAxis);
                }

                item.localRotation *= Quaternion.Euler(RotationOffSet);

           SetStartTransform:

            if (m_item)
            {
                m_item.StartPosition = item.localPosition;
                m_item.StartRotation = item.localRotation;
                m_item.StartScale =  item.localScale;
            }
        }


        /// <summary>
        /// Add ItemsManager to all Childs and colliders
        /// </summary>
        public virtual void AddItemScript()
        {
            foreach (Transform child in transform)
            {
                AddItemScript(child);
            }
        }

        /// <summary>
        /// Add ItemsManager to all Childs and colliders
        /// </summary>
        public virtual void AddItemScript(Transform child)
        {
            MItem mitem = child.GetComponent<MItem>();

            //Add ItemsManagerScript
            if (!mitem)
            {
                mitem = child.gameObject.AddComponent<MItem>();
            }

            child.gameObject.SetLayer(LayerMask.NameToLayer("UI"), true);           //Set the Layer to UI.

            Renderer renderer = child.GetComponentInChildren<Renderer>(); //Find the Renderer for this Item

            if (!child.GetComponent<Collider>())  //Check First if there's no collider
            {

                if (!renderer) return;

                if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)            //if the Item is a 3D Model
                {
                    if (!renderer.GetComponent<Collider>())
                    {
                        renderer.gameObject.AddComponent<BoxCollider>();
                    }
                }

                if (renderer is SpriteRenderer)
                {
                    if (!renderer.GetComponent<Collider2D>())
                    {
                        renderer.gameObject.AddComponent<BoxCollider2D>();
                    }
                }
            }
        }

    }
}