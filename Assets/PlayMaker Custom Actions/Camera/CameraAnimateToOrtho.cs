// (c) Copyright HutongGames, LLC 2010-2020. All rights reserved.  
// License: Attribution 4.0 International(CC BY 4.0) 
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Smooth animation from perspective ( or ortho ) to Ortho. Use GameObject reference instead of camera reference")]
	public class CameraAnimateToOrtho : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The camera to animate. If null or not defined, uses the MainCamera")]
		[CheckForComponent(typeof(Camera))]
		public FsmOwnerDefault camera;
		
		[Tooltip("The orthographic size to reach")]
		public FsmFloat orthographicSize;
		
		[Tooltip("The near plane distance to reach. Leave to none for no effect")]
		public FsmFloat near;
		
		[Tooltip("The near plane distance to reach. Leave to none for no effect")]
		public FsmFloat far;
		
		[Tooltip("The duration of the transition animation")]
		public FsmFloat duration;
		
		[Tooltip("Event sent when transition is done")]
		public FsmEvent transitionDoneEvent;
		
		Camera _camera;
		Matrix4x4 _orthoMatrix;
		
		float _startTime;
		float _duration;
		
		public override void Reset()
		{
			camera = null;
			
			orthographicSize = 50f;
			near = new FsmFloat() {UseVariable=true};
			far = new FsmFloat() {UseVariable=true};
			
			duration = 1f;
			
			transitionDoneEvent = null;
		}

		public override void OnEnter()
		{
			
			_startTime = Time.time;
			_duration = duration.Value;
			
			AnimateToOrtho();
		}

		public override void OnUpdate()
		{
			
			if (_camera!=null)
			{
				
				if (Time.time - _startTime < _duration)
				{
				 	_camera.projectionMatrix = MatrixLerp(_camera.projectionMatrix,_orthoMatrix,(Time.time - _startTime) / _duration);
				}else{
					Fsm.Event(transitionDoneEvent);
					Finish();
				}
			}
		}

		void AnimateToOrtho()
		{
			GameObject _go = Fsm.GetOwnerDefaultTarget(camera);
			
			if (_go == null && _go.GetComponent<Camera>() != null)
			{
				_camera = _go.GetComponent<Camera>() ;
			}else{
				_camera = Camera.main;
			}
			
			float _near = near.IsNone?_camera.nearClipPlane:near.Value;
			float _far = far.IsNone?_camera.farClipPlane:far.Value;
			float _orthoSize = orthographicSize.Value;
			
			float aspect = (float) Screen.width / (float) Screen.height;

        	_orthoMatrix = Matrix4x4.Ortho(-_orthoSize * aspect, _orthoSize * aspect, -_orthoSize, _orthoSize, _near, _far);
			
			if (_duration==0f)
			{
				_camera.projectionMatrix = _orthoMatrix;
				Fsm.Event(transitionDoneEvent);
				Finish();
			}
		}
		
		Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
	    {
	
	        Matrix4x4 ret = new Matrix4x4();
	
	        for (int i = 0; i < 16; i++)
	
	            ret[i] = Mathf.Lerp(from[i], to[i], time);
	
	        return ret;
	
	    }
	}
}
