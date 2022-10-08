using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Sets the properties of a MeshGenerator component")]
    public class MeshGeneratorGetProperties : SplineUserSetProperties
    {
        private MeshGenerator _meshGen = null;

        public FsmFloat size;
        public FsmColor color;
        public FsmVector3 offset;
        [Range(-180f, 180f)]
        public FsmFloat rotation;
        public FsmBool doubleSided;
        public FsmBool flipFaces;

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
            if(_meshGen == null)
            {
                Debug.LogError("The supplied object is not a Mesh generator for action " + DisplayName + " in " + Fsm.Name);
                return;
            }
            size.Value = _meshGen.size;
            color.Value = _meshGen.color;
            offset.Value = _meshGen.offset;
            rotation.Value = _meshGen.rotation;
            doubleSided.Value = _meshGen.doubleSided;
            flipFaces.Value = _meshGen.flipFaces;
        }
    }
}