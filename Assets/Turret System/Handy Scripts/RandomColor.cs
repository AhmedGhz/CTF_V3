/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour 
{
	//pick different colors to randomize. you dont have to set the alpha value too. its maxed out in the script
	public Color[] colors;

	void Start () 
	{
		Color newColor = colors[Random.Range(0,colors.Length)]; //pick a random color
		newColor.a = 1; //set alpha to max
		if(GetComponent<ParticleSystem>()) //assign it to the particle system
			GetComponent<ParticleSystem>().startColor = newColor;
		if(GetComponent<Renderer>()) //assign it to the material
			GetComponent<Renderer>().material.SetColor("_Color", newColor);
	}

}
