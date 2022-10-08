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
using System.Collections.Generic;

public class TurretSystem_ObjectPooler
{
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool willGrow = true;
	
	public List<GameObject> pooledObjects;

	public void Start() 
	{
		pooledObjects = new List<GameObject>();
		for(int i = 0; i < pooledAmount; i++)
		{
			GameObject obj = GameObject.Instantiate(pooledObject) as GameObject;
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}
	}
	
	public GameObject GetPooledObject()
	{
		for(int i = 0; i < pooledObjects.Count; i++)
		{
			if(!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}
		
		if(willGrow)
		{
			GameObject obj = GameObject.Instantiate(pooledObject) as GameObject;
			pooledObjects.Add(obj);
			return obj;
		}
		
		return null;
	}
}

//public class TurretSystem_ObjectPooler : MonoBehaviour 
//{
//	public TurretSystem_ObjectPooler current;
//	public GameObject pooledObject;
//	public int pooledAmount = 20;
//	public bool willGrow = true;
//
//	List<GameObject> pooledObjects;
//
//	void Awake()
//	{
//		current = this;
//	}
//
//	void Start () 
//	{
//		pooledObjects = new List<GameObject>();
//		for(int i = 0; i < pooledAmount; i++)
//		{
//			GameObject obj = (GameObject)Instantiate(pooledObject);
//			obj.SetActive(false);
//			pooledObjects.Add(obj);
//			obj.transform.parent = this.transform;
//		}
//	}
//
//	public GameObject GetPooledObject()
//	{
//		for(int i = 0; i < pooledObjects.Count; i++)
//		{
//			if(!pooledObjects[i].activeInHierarchy)
//			{
//				return pooledObjects[i];
//			}
//		}
//
//		if(willGrow)
//		{
//			GameObject obj = (GameObject)Instantiate(pooledObject);
//			pooledObjects.Add(obj);
//			return obj;
//		}
//
//		return null;
//	}
//}
