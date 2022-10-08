using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI3DEnums
{
    public enum UI3DMaskRenderMode
    {
        JustDepth, CullingMask
    };

    public enum UI3DMaskSourceMode
    {
        Image, RawImage, MaskTexture
    };

    public enum UI3DMaskAlphaMode
    {
        AlphaTest, Dithering, NoAlpha, Translucency
    }
}