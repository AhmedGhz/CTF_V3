using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Sets the properties of a MeshGenerator component")]
    public class MeshGeneratorSetProperties : SplineUserSetProperties
    {
        private MeshGenerator _meshGen = null;

        public FsmFloat size;
        public FsmColor color;
        public FsmVector3 offset;
        [Range(-180f, 180f)]
        public FsmFloat rotation;
        public FsmBool doubleSided;
        public FsmBool flipFaces;

        public bool setSize = false;
        public bool setColor = false;
        public bool setOffset = false;
        public bool setRotation = false;
        public bool setDoubleSided = false;
        public bool setFlipFaces = false;

        public override void Reset()
        {
            base.Reset();
            size = null;
            color = null;
            offset = null;
            rotation = null;
            doubleSided = null;
            flipFaces = null;
        }

        protected override bool OnOwnerUpdate()
        {
            if (base.OnOwnerUpdate())
            {
                if (user is MeshGenerator)
                {
                    _meshGen = (MeshGenerator)user;
                    return true;
                }
                else
                {
                    _meshGen = null;
                }
            }
            return false;
        }

        protected override void RunAction()
        {
            base.RunAction();
            if (_meshGen == null)
            {
                Debug.LogError("The supplied object is not a Mesh generator for action " + DisplayName + " in " + Fsm.Name);
                return;
            }
            if (setSize) _meshGen.size = size.Value; 
            if(setColor) _meshGen.color = color.Value;
            if(setOffset) _meshGen.offset = offset.Value;
            if(setRotation) _meshGen.rotation = rotation.Value;
            if(setDoubleSided) _meshGen.doubleSided = doubleSided.Value;
            if(setFlipFaces) _meshGen.flipFaces = flipFaces.Value;
        }
    }
}