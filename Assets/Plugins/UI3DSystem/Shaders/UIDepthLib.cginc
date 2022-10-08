#ifndef UI_DEPTH_LIB
#define UI_DEPTH_LIB

#if defined(ALPHAMODE_ALPHATEST) || defined(ALPHAMODE_DITHERING)
	uniform fixed _AlphaTestTreshold;//[0-1]
	uniform fixed _DitheringStep;//(0-1]

	inline void discardMaskByAlpha(fixed alpha, float4 screenPos)
	{
	#ifdef ALPHAMODE_ALPHATEST
		clip(alpha - _AlphaTestTreshold);
	#elif ALPHAMODE_DITHERING
		if (alpha > 0.99)
			alpha = 1;
		else
			alpha -= fmod(alpha, _DitheringStep);
		float4x4 thresholdMatrix =
		{ 
			1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
			13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
			4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
			16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
		};
		float4x4 rowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
		float2 pos = screenPos.xy / screenPos.w;
		pos *= _ScreenParams.xy;
		clip(alpha - thresholdMatrix[fmod(pos.x, 4)] * rowAccess[fmod(pos.y, 4)]);
	#endif
	}

#else//noalpha and translucency
	inline void discardMaskByAlpha(fixed alpha, float4 screenPos){}
#endif //UI depth mask
	
#if defined(IS_UI_3D_RENDERER)  //for 3D renderers
	
	#if defined(USE_CLIPPING_MASK) || defined(USE_CLIPPING_MASK_ON)
		fixed _ClippingMaskVal;
	#endif

	sampler2D _UIDepthTex;
	float4 _UI3DDepthTexPosParams;//world xy, width, height
	float2 _UIDepthTex_TexelSize;
	float _UI3DCanvasZMin;
	float _UI3DCanvasZMax;
	
	#if SOFT_COLLISION_MODE
		float _UISoftModeFadeSmooth;
	#endif

	inline float2 svPositionUIToScreenPos(float4 svPosition)
	{
		float2 screenPos = svPosition.xy / svPosition.w;
		screenPos = (screenPos + 1.0) * 0.5;
	#if UNITY_UV_STARTS_AT_TOP
		if (_UIDepthTex_TexelSize.y < 0)
			screenPos.y = 1 - screenPos.y;
	#endif
		return screenPos;
	}
	
	//note: screenPos is unity screenPos.xy / screenPos.w
	inline float2 calcUIDepthTexUv(float3 worldPos, float2 screenPos)
	{
		#if defined(CAST_UI_CULLING_TO_SCREEN_SPACE) || defined(CAST_UI_CULLING_TO_SCREEN_SPACE_ON)
			return screenPos;
		#else
			return float2((worldPos.x - _UI3DDepthTexPosParams.x) / _UI3DDepthTexPosParams.z,
					  (worldPos.y - _UI3DDepthTexPosParams.y) / _UI3DDepthTexPosParams.w);
		#endif
	}

	inline fixed makeUI3DClipping(float2 uiDepthTexUv, float worldZPos)
	{
		#if !defined(DISABLE_UI_CULLING)
		    fixed4 depthMask = tex2D(_UIDepthTex, uiDepthTexUv);
		    fixed translucencyAdd = 0;
		#if defined(USE_CLIPPING_MASK) || defined(USE_CLIPPING_MASK_ON)
			#if defined(CLIPPINGMODE_OUTSIDE) || defined(CLIPPINGMODE_OUTSIDE_ON)
				    clip(abs(_ClippingMaskVal - depthMask.a) - 0.0039/*aprox 1/255*/);
			#else //inside
				    clip(0.0039/*aprox 1/255*/ - abs(_ClippingMaskVal - depthMask.a));
			    #endif
			    #if defined(TRANSLUCENCY_MODE)
				    translucencyAdd = (1 - depthMask.b) * step(abs(_ClippingMaskVal - depthMask.a), 0.0039/*aprox 1/255*/);
			#endif
		#endif


		    #if defined(TRANSLUCENCY_MODE)
		        #if SOFT_COLLISION_MODE
			        float depthMaskZ = depthMask.r * (_UI3DCanvasZMax - _UI3DCanvasZMin) + _UI3DCanvasZMin;
				    fixed softAlphaMul = saturate((depthMaskZ - worldZPos) * _UISoftModeFadeSmooth);
				    depthMask.g *= step(worldZPos, depthMaskZ);
		        #else
				    fixed zDepthVal = saturate((worldZPos - _UI3DCanvasZMin) / (_UI3DCanvasZMax - _UI3DCanvasZMin));
				    depthMask.g *= step(depthMask.r, zDepthVal);
		        #endif
            #else 
		#if SOFT_COLLISION_MODE
			float depthMaskZ = depthMask.r * (_UI3DCanvasZMax - _UI3DCanvasZMin) + _UI3DCanvasZMin;
			clip(depthMaskZ - worldZPos);
			return saturate((depthMaskZ - worldZPos) * _UISoftModeFadeSmooth);
		#else
			fixed zDepthVal = saturate((worldZPos - _UI3DCanvasZMin) / (_UI3DCanvasZMax - _UI3DCanvasZMin));
			clip(depthMask.r - zDepthVal);
		#endif

		#endif

		    #if defined(TRANSLUCENCY_MODE)
                return saturate(1.0 - (translucencyAdd + depthMask.g));
            #else
		        return 1.0;
            #endif
        #else
		return 1.0;
        #endif
	}
	
	//note: screenPos is unity screenPos.xy / screenPos.w
	inline fixed makeUI3DClippingOnWorldPos(float3 worldPos, float2 screenPos)
	{
		float2 uiDepthTexUv = calcUIDepthTexUv(worldPos, screenPos);
		return makeUI3DClipping(uiDepthTexUv, worldPos.z);
	}
	
#endif//IS_3D_RENDERER

#endif//UI_DEPTH_LIB
