using UnityEngine;
using System.Collections.Generic;

public class TestScene2 : MonoBehaviour {
	public GameObject cam1;
	public GameObject cam2;
//	public GameObject cam3;
	
	public ForceField forceField;
	public GameObject prefab;
	private List<GameObject> obj = new List<GameObject>();
	private float r;
	
	private bool createBox2;
	private bool createBox;
	private float timeBox;
	
	void Start () {
//		CreateObjectA();
		cam1.SetActive(true);
		cam2.SetActive(false);
//		cam3.SetActive(false);
	}
	
	void Update(){
		if (Input.GetKey(KeyCode.W)){
			createBox=true;
		}
		
		if(createBox==true){
			timeBox+=Time.deltaTime*100;
			if(timeBox>1){
				GameObject inst = (GameObject)Instantiate(prefab, new Vector3(Random.Range(-60,60),150,Random.Range(-60,60)), Quaternion.Euler(0,0,0));	
				timeBox=0;
			}
		}
		
		if(Input.GetKey(KeyCode.Alpha1)){			
			forceField.DestroyForceField();
			forceField.sphere = true;
			forceField.CreateForceField();
		}
		if(Input.GetKey(KeyCode.Alpha2)){			
			forceField.DestroyForceField();
			forceField.sphere = false;
			forceField.CreateForceField();
		}
		
		if(Input.GetKey(KeyCode.S)){
			cam1.SetActive(false);
			cam2.SetActive(true);
//			cam3.SetActive(false);
		}
		if(Input.GetKey(KeyCode.D)){
			cam1.SetActive(true);
			cam2.SetActive(false);
//			cam3.SetActive(false);
		}
//		if(Input.GetKey(KeyCode.F)){
//			cam1.SetActive(false);
//			cam2.SetActive(false);
//			cam3.SetActive(true);
//		}
		r+=Time.deltaTime*10;
		cam2.transform.eulerAngles = new Vector3(0,r,0);
		
//		if (Input.GetKey(KeyCode.Q)){
//			for (int i = 0; i < obj.Count; i++){
//				Destroy(obj[i]);
//				obj.RemoveAt(i);
//				i -= 1;
//			}			
//		}	
		
//		if (Input.GetKey(KeyCode.W)){
//			for (int i = 0; i < obj.Count; i++){
//				Destroy(obj[i]);
//				obj.RemoveAt(i);
//				i -= 1;
//			}
//			CreateObjectA();			
//		}	
//		if (Input.GetKey(KeyCode.E)){
//			for (int i = 0; i < obj.Count; i++){
//				Destroy(obj[i]);
//				obj.RemoveAt(i);
//				i -= 1;
//			}		
//			CreateObjectB();			
//		}	
	}
	
	void CreateObjectA(){
		for (int i = 0; i < 100; i++ ){ 
			obj.Add((GameObject)Instantiate(prefab, new Vector3(Random.Range(-60,60),Random.Range(0,60),Random.Range(-60,60)), Quaternion.Euler(0,0,0)));
		}
	}
	
	void CreateObjectB(){
		CreateSquare(28, 0, 28, 10);
		CreateSquare(28.5f, 1.5f, 28.5f, 9);
		CreateSquare(29f, 2.5f, 29f, 8);
		CreateSquare(29.5f, 3.5f, 29.5f, 7);
		CreateSquare(30f, 4.5f, 30f, 6);
		CreateSquare(30.5f, 5.5f, 30.5f, 5);
		CreateSquare(31f, 6.5f, 31f, 4);
		CreateSquare(31.5f, 7.5f, 31.5f, 3);
		CreateSquare(32f, 8.5f, 32f, 2);
		CreateSquare(32.5f, 9.5f, 32.5f, 1);
		
		CreateSquare(-38, 0, 28, 10);
		CreateSquare(-37.5f, 1.5f, 28.5f, 9);
		CreateSquare(-37f, 2.5f, 29f, 8);
		CreateSquare(-36.5f, 3.5f, 29.5f, 7);
		CreateSquare(-36f, 4.5f, 30f, 6);
		CreateSquare(-35.5f, 5.5f, 30.5f, 5);
		CreateSquare(-35f, 6.5f, 31f, 4);
		CreateSquare(-34.5f, 7.5f, 31.5f, 3);
		CreateSquare(-34f, 8.5f, 32f, 2);
		CreateSquare(-33.5f, 9.5f, 32.5f, 1);
		
		CreateSquare(28, 0, -38, 10);
		CreateSquare(28.5f, 1.5f, -37.5f, 9);
		CreateSquare(29f, 2.5f, -37f, 8);
		CreateSquare(29.5f, 3.5f, -36.5f, 7);
		CreateSquare(30f, 4.5f, -36f, 6);
		CreateSquare(30.5f, 5.5f, -35.5f, 5);
		CreateSquare(31f, 6.5f, -35f, 4);
		CreateSquare(31.5f, 7.5f, -34.5f, 3);
		CreateSquare(32f, 8.5f, -34f, 2);
		CreateSquare(32.5f, 9.5f, -33.5f, 1);
		
		CreateSquare(-38, 0, -38, 10);
		CreateSquare(-37.5f, 1.5f, -37.5f, 9);
		CreateSquare(-37f, 2.5f, -37f, 8);
		CreateSquare(-36.5f, 3.5f, -36.5f, 7);
		CreateSquare(-36f, 4.5f, -36f, 6);
		CreateSquare(-35.5f, 5.5f, -35.5f, 5);
		CreateSquare(-35f, 6.5f, -35f, 4);
		CreateSquare(-34.5f, 7.5f, -34.5f, 3);
		CreateSquare(-34f, 8.5f, -34f, 2);
		CreateSquare(-33.5f, 9.5f, -33.5f, 1);
		
		CreateLineX(-10,0,33,20);
		CreateLineX(-10,1.5f,33,20);
		CreateLineX(-10,2.5f,33,20);
		CreateLineX(-10,3.5f,33,20);
		CreateLineX(-10,4.5f,33,20);
		CreateLineX(-10,5.5f,33,20);
		CreateLineX(-10,6.5f,33,20);
		CreateLineX(-10,7.5f,33,20);
		CreateLineX(-10,8.5f,33,20);
		CreateLineX(-10,9.5f,33,20);
		
		CreateLineX(-10,0,-33,20);
		CreateLineX(-10,1.5f,-33,20);
		CreateLineX(-10,2.5f,-33,20);
		CreateLineX(-10,3.5f,-33,20);
		CreateLineX(-10,4.5f,-33,20);
		CreateLineX(-10,5.5f,-33,20);
		CreateLineX(-10,6.5f,-33,20);
		CreateLineX(-10,7.5f,-33,20);
		CreateLineX(-10,8.5f,-33,20);
		CreateLineX(-10,9.5f,-33,20);
		
		CreateLineY(33,0,-10,20);
		CreateLineY(33,1.5f,-10,20);
		CreateLineY(33,2.5f,-10,20);
		CreateLineY(33,3.5f,-10,20);
		CreateLineY(33,4.5f,-10,20);
		CreateLineY(33,5.5f,-10,20);
		CreateLineY(33,6.5f,-10,20);
		CreateLineY(33,7.5f,-10,20);
		CreateLineY(33,8.5f,-10,20);
		CreateLineY(33,9.5f,-10,20);
		
		CreateLineY(-33,0,-10,20);
		CreateLineY(-33,1.5f,-10,20);
		CreateLineY(-33,2.5f,-10,20);
		CreateLineY(-33,3.5f,-10,20);
		CreateLineY(-33,4.5f,-10,20);
		CreateLineY(-33,5.5f,-10,20);
		CreateLineY(-33,6.5f,-10,20);
		CreateLineY(-33,7.5f,-10,20);
		CreateLineY(-33,8.5f,-10,20);
		CreateLineY(-33,9.5f,-10,20);
	}
	
	void CreateLineY(float x, float y, float z, int range){
		float dz = z; 
	
		while (dz<(range+z)){
			obj.Add((GameObject)Instantiate(prefab, new Vector3(x,y,dz), Quaternion.Euler(0,0,0)));
			dz++;}		
	}
	
	void CreateLineX(float x, float y, float z, int range){
		float dx = x; 
	
		while (dx<(range+x)){
			obj.Add((GameObject)Instantiate(prefab, new Vector3(dx,y,z), Quaternion.Euler(0,0,0)));
			dx++;}		
	}
	
	void CreateSquare(float x, float y, float z, int range){
		int i = 0;
		float dx = x; 
		float dz = z;
	
		while (i<2){
			if (dx<(range+x) && i==0){				
        		obj.Add((GameObject)Instantiate(prefab, new Vector3(dx,y,dz), Quaternion.Euler(0,0,0)));
				dx++;}
			else if(dz<(range+z) && i==0){
				obj.Add((GameObject)Instantiate(prefab, new Vector3(dx,y,dz), Quaternion.Euler(0,0,0)));
				dz++;}
			else if(dx>x){
				obj.Add((GameObject)Instantiate(prefab, new Vector3(dx,y,dz), Quaternion.Euler(0,0,0)));
				dx--; i=1;}
			else if(dz>z){
				obj.Add((GameObject)Instantiate(prefab, new Vector3(dx,y,dz), Quaternion.Euler(0,0,0)));
				dz--;}
			else{i++;}
		}
	}
}
