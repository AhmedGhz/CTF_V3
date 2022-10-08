using UnityEngine;
using System.Collections;
using CraftingAnims;

public class GUIControls : MonoBehaviour{
	public CrafterController crafterController;

	void OnGUI(){
		if(crafterController.charState == CrafterController.CharacterState.Idle && !crafterController.isMoving && crafterController.isGrounded){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Get Hammer")){
				crafterController.animator.SetTrigger("ItemBeltTrigger");
				StartCoroutine(crafterController._MovePause(1f));
				StartCoroutine(crafterController._ShowItem("hammer", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Hammer;
			}
			if(GUI.Button(new Rect(195, 25, 150, 30), "Get Paintbrush")){
				crafterController.animator.SetTrigger("ItemBeltTrigger");
				StartCoroutine(crafterController._MovePause(1f));
				StartCoroutine(crafterController._ShowItem("paintbrush", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Painting;
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Get Axe")){
				crafterController.animator.SetTrigger("ItemBackTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("axe", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Axe;
			}
			if(GUI.Button(new Rect(195, 65, 150, 30), "Get Spear")){
				crafterController.animator.SetTrigger("ItemBackTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("spear", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Spear;
				crafterController.isSpearfishing = true;
				crafterController.animator.SetTrigger("SpearfishTrigger");
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Get PickAxe")){
				crafterController.animator.SetTrigger("ItemBackTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("pickaxe", 0.5f));
				crafterController.charState = CrafterController.CharacterState.PickAxe;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Pickup Shovel")){
				crafterController.animator.SetTrigger("ItemPickupTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("shovel", 0.3f));
				crafterController.charState = CrafterController.CharacterState.Shovel;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "PullUp Fishing Pole")){
				crafterController.animator.SetTrigger("ItemPullUpTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("fishingpole", 0.5f));
				crafterController.charState = CrafterController.CharacterState.FishingPole;
			}
			if(GUI.Button(new Rect(25, 225, 150, 30), "Take Food")){
				crafterController.animator.SetTrigger("ItemTakeTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("food", 0.3f));
				crafterController.charState = CrafterController.CharacterState.Food;
			}
			if(GUI.Button(new Rect(25, 265, 150, 30), "Recieve Drink")){
				crafterController.animator.SetTrigger("ItemRecieveTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("drink", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Drink;
			}
			if(GUI.Button(new Rect(25, 305, 150, 30), "Pickup Box")){
				crafterController.animator.SetTrigger("CarryPickupTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("box", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Box;
			}
			if(GUI.Button(new Rect(195, 305, 150, 30), "Pickup Lumber")){
				crafterController.animator.SetTrigger("LumberPickupTrigger");
				StartCoroutine(crafterController._MovePause(1.6f));
				StartCoroutine(crafterController._ShowItem("lumber", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Lumber;
			}
			if(GUI.Button(new Rect(370, 305, 150, 30), "Pickup Overhead")){
				crafterController.animator.SetTrigger("CarryOverheadPickupTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("sphere", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Overhead;
			}
			if(GUI.Button(new Rect(25, 345, 150, 30), "Recieve Box")){
				crafterController.animator.SetTrigger("CarryRecieveTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("box", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Box;
			}
			if(GUI.Button(new Rect(25, 385, 150, 30), "Get Saw")){
				crafterController.animator.SetTrigger("ItemBeltTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("saw", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Saw;
			}
			if(GUI.Button(new Rect(25, 425, 150, 30), "Get Sickle")){
				crafterController.animator.SetTrigger("ItemBeltTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("sickle", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Sickle;
			}
			if(GUI.Button(new Rect(25, 465, 150, 30), "Get Rake")){
				crafterController.animator.SetTrigger("ItemBackTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("rake", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Rake;
			}
			if(GUI.Button(new Rect(200, 465, 150, 30), "Use")){
				crafterController.animator.SetBool("Use", true);
				crafterController.charState = CrafterController.CharacterState.Use;
			}
			if(GUI.Button(new Rect(375, 465, 150, 30), "Crawl")){
				crafterController.animator.SetTrigger("CrawlStartTrigger");
				StartCoroutine(crafterController._MovePause(1f));
				crafterController.charState = CrafterController.CharacterState.Crawl;
			}
			if(GUI.Button(new Rect(25, 505, 150, 30), "Sit")){
				crafterController.animator.SetTrigger("ChairSitTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("chair", 0.3f));
				crafterController.charState = CrafterController.CharacterState.Sit;
			}
			if(GUI.Button(new Rect(200, 505, 150, 30), "Push Cart")){
				crafterController.animator.SetTrigger("CartPullGrabTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("cart", 0.25f));
				crafterController.charState = CrafterController.CharacterState.Cart;
			}
			if(GUI.Button(new Rect(375, 505, 150, 30), "Laydown")){
				crafterController.animator.SetTrigger("LaydownLaydownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Laydown;
			}
			if(GUI.Button(new Rect(25, 545, 150, 30), "Gather")){
				crafterController.animator.SetTrigger("GatherTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(200, 545, 150, 30), "Gather Kneeling")){
				crafterController.animator.SetTrigger("GatherKneelingTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(200, 585, 150, 30), "Wave1")){
				crafterController.animator.SetTrigger("Wave1Trigger");
				StartCoroutine(crafterController._MovePause(2.2f));
			}
			if(GUI.Button(new Rect(375, 545, 150, 30), "Cheer1")){
				crafterController.animator.SetTrigger("Cheer1Trigger");
				StartCoroutine(crafterController._MovePause(2.2f));
			}
			if(GUI.Button(new Rect(25, 585, 150, 30), "Scratch Head")){
				crafterController.animator.SetTrigger("Bored1Trigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(375, 585, 150, 30), "Cheer2")){
				crafterController.animator.SetTrigger("Cheer2Trigger");
				StartCoroutine(crafterController._MovePause(2.2f));
			}
			if(GUI.Button(new Rect(375, 630, 150, 30), "Cheer3")){
				crafterController.animator.SetTrigger("Cheer3Trigger");
				StartCoroutine(crafterController._MovePause(2.2f));
			}
			if(GUI.Button(new Rect(375, 670, 150, 30), "Fear")){
				crafterController.animator.SetTrigger("FearTrigger");
				StartCoroutine(crafterController._MovePause(4f));
			}
			if(GUI.Button(new Rect(25, 625, 150, 30), "Climb")){
				crafterController.animator.SetTrigger("ClimbStartTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("ladder", 0.3f));
				crafterController.charState = CrafterController.CharacterState.Climb;
			}
			if(GUI.Button(new Rect(200, 625, 150, 30), "Climb Top")){
				this.gameObject.transform.position += new Vector3(0, 3, 0);
				crafterController.animator.SetTrigger("ClimbOnTopTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("ladder", 0.3f));
				crafterController.charState = CrafterController.CharacterState.Climb;
			}
			if(GUI.Button(new Rect(200, 665, 150, 30), "Pray")){
				crafterController.animator.SetTrigger("Pray-DownTrigger");
				crafterController.charState = CrafterController.CharacterState.Pray;
				crafterController.isPaused = true;
			}
			if(GUI.Button(new Rect(25, 665, 150, 30), "Push Pull")){
				crafterController.animator.SetTrigger("PushPullStartTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("pushpull", 0.3f));
				crafterController.charState = CrafterController.CharacterState.PushPull;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Cart){
			if(GUI.Button(new Rect(200, 505, 150, 30), "Release Cart")){
				crafterController.animator.SetTrigger("CartPullReleaseTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.75f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Pray){
			if(GUI.Button(new Rect(200, 665, 150, 30), "Stand")){
				crafterController.animator.SetTrigger("Pray-StandTrigger");
				StartCoroutine(crafterController._MovePause(1.1f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Hammer){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Hammer Wall")){
				crafterController.animator.SetTrigger("HammerWallTrigger");
				StartCoroutine(crafterController._MovePause(1.9f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Hammer Table")){
				crafterController.animator.SetTrigger("HammerTableTrigger");
				StartCoroutine(crafterController._MovePause(1.9f));
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give Hammer")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				StartCoroutine(crafterController._ChangeCharacterState(0.4f, CrafterController.CharacterState.Idle));
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Away Hammer")){
				crafterController.animator.SetTrigger("ItemBeltAwayTrigger");
				StartCoroutine(crafterController._MovePause(1f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				StartCoroutine(crafterController._ChangeCharacterState(0.4f, CrafterController.CharacterState.Idle));
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Put Down Hammer")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 225, 150, 30), "Drop Hammer")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 265, 150, 30), "Kneel")){
				crafterController.animator.SetTrigger("ItemKneelDownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Kneel;
			}
			if(GUI.Button(new Rect(25, 305, 150, 30), "Chisel")){
				crafterController.animator.SetTrigger("ItemChiselTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Painting){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Paint Wall")){
				crafterController.animator.SetTrigger("ItemPaintTrigger");
				StartCoroutine(crafterController._MovePause(1.9f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Fill Brush")){
				crafterController.animator.SetTrigger("ItemPaintRefillTrigger");
				StartCoroutine(crafterController._MovePause(1.9f));
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give Paintbrush")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				StartCoroutine(crafterController._ChangeCharacterState(0.4f, CrafterController.CharacterState.Idle));
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Away Paintbrush")){
				crafterController.animator.SetTrigger("ItemBeltAwayTrigger");
				StartCoroutine(crafterController._MovePause(1f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				StartCoroutine(crafterController._ChangeCharacterState(0.4f, CrafterController.CharacterState.Idle));
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Put Down Paintbrush")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 225, 150, 30), "Drop Paintbrush")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Kneel){
			if(GUI.Button(new Rect(25, 30, 150, 30), "Hammer")){
				crafterController.animator.SetTrigger("ItemKneelHammerTrigger");
				StartCoroutine(crafterController._MovePause(1.1f));
			}
			if(GUI.Button(new Rect(25, 265, 150, 30), "Stand")){
				crafterController.animator.SetTrigger("ItemKneelStandTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Hammer;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Drink){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Drink")){
				crafterController.animator.SetTrigger("DrinkUpperTrigger");
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Drink){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Drink")){
				crafterController.animator.SetTrigger("ItemDrinkTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Water")){
				crafterController.animator.SetTrigger("ItemWaterTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give Drink")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Drink Away")){
				crafterController.animator.SetTrigger("ItemBeltAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Put Drink Down")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 225, 150, 30), "Drop Drink")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Food){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Eat Food")){
				crafterController.animator.SetTrigger("EatUpperTrigger");
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Food){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Eat Food")){
				crafterController.animator.SetTrigger("ItemEatTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Give Food")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Put Food Down")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Food Away")){
				crafterController.animator.SetTrigger("ItemBeltAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Drop Food")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 225, 150, 30), "Plant Food")){
				crafterController.animator.SetTrigger("ItemPlantTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.6f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Sickle){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Use Sickle")){
				crafterController.animator.SetTrigger("ItemSickleUse");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Give Sickle")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Put Sickle Down")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Sickle Away")){
				crafterController.animator.SetTrigger("ItemBeltAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Drop Sickle")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Axe){
			crafterController.isPaused = false;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Start Chopping")){
				crafterController.animator.SetTrigger("ChoppingStartTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Chopping;
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Put Axe Away")){
				crafterController.animator.SetTrigger("ItemBackAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give Axe")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Axe Down")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Drop Axe")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.PickAxe){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Chop Upper Horizontal")){
				crafterController.animator.SetTrigger("ChopHorizontalUpperTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Chop Upper Vertical")){
				crafterController.animator.SetTrigger("ChopVerticalUpperTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.PickAxe){
			crafterController.isPaused = false;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Start PickAxing")){
				crafterController.animator.SetTrigger("ChoppingStartTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.PickAxing;
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Put PickAxe Away")){
				crafterController.animator.SetTrigger("ItemBackAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give PickAxe")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put PickAxe Down")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Drop PickAxe")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Saw){
			crafterController.isPaused = false;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Start Sawing")){
				crafterController.animator.SetTrigger("SawStartTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Sawing;
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Put Saw Away")){
				crafterController.animator.SetTrigger("ItemBeltAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give Saw")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Drop Saw")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Sawing){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Finish Sawing")){
				crafterController.animator.SetTrigger("SawFinishTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Saw;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Chopping){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Chop Vertical")){
				crafterController.animator.SetTrigger("ChopVerticalTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Chop Horizontal")){
				crafterController.animator.SetTrigger("ChopHorizontalTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Chop Diagonal")){
				crafterController.animator.SetTrigger("ChopDiagonalTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Chop Ground")){
				crafterController.animator.SetTrigger("ChopGroundTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Finish Chopping")){
				crafterController.animator.SetTrigger("ChopFinishTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Axe;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.PickAxing){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Swing Vertical")){
				crafterController.animator.SetTrigger("ChopVerticalTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Swing Horizontal")){
				crafterController.animator.SetTrigger("ChopHorizontalTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Swing Ground")){
				crafterController.animator.SetTrigger("ChopGroundTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Swing Ceiling")){
				crafterController.animator.SetTrigger("ChopCeilingTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Swing Diagonal")){
				crafterController.animator.SetTrigger("ChopDiagonalTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 225, 150, 30), "Finish PickAxing")){
				crafterController.animator.SetTrigger("ChopFinishTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.PickAxe;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Shovel){
			crafterController.isPaused = false;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Start Digging")){
				crafterController.animator.SetTrigger("DiggingStartTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Digging;
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Put Shovel Away")){
				crafterController.animator.SetTrigger("ItemBackAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give Shovel")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Shovel Down")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Drop Shovel")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Rake){
			crafterController.isPaused = false;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Start Raking")){
				crafterController.animator.SetTrigger("DiggingStartTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Raking;
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Put Rake Away")){
				crafterController.animator.SetTrigger("ItemBackAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give Rake")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Rake Down")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Drop Rake")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Raking){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Rake")){
				crafterController.animator.SetTrigger("ItemRakeUse");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Finish Raking")){
				crafterController.animator.SetTrigger("DiggingFinishTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Rake;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Digging){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Dig")){
				crafterController.animator.SetTrigger("DiggingScoopTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Finish Digging")){
				crafterController.animator.SetTrigger("DiggingFinishTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Shovel;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.FishingPole){
			crafterController.isPaused = false; 
			if(GUI.Button(new Rect(25, 25, 150, 30), "Cast Reel")){
				crafterController.animator.SetTrigger("FishingCastTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Fishing;
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Put Fishing Pole Away")){
				crafterController.animator.SetTrigger("ItemBackAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Give Fishing Pole")){
				crafterController.animator.SetTrigger("ItemHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Put Fishing Pole Down")){
				crafterController.animator.SetTrigger("ItemPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 185, 150, 30), "Drop FishingPole")){
				crafterController.animator.SetTrigger("ItemDropTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.4f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Sawing){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Finish Sawing")){
				crafterController.animator.SetTrigger("SawFinishTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Saw;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Sit){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Talk1")){
				crafterController.animator.SetTrigger("ChairTalk1Trigger");
			}

			if(GUI.Button(new Rect(25, 65, 150, 30), "Eat")){
				crafterController.animator.SetTrigger("ChairEatTrigger");
				StartCoroutine(crafterController._ShowItem("chaireat", 0.2f));
				StartCoroutine(crafterController._ShowItem("chair", 1.1f));
			}

			if(GUI.Button(new Rect(25, 105, 150, 30), "Drink")){
				crafterController.animator.SetTrigger("ChairDrinkTrigger");
				StartCoroutine(crafterController._ShowItem("chairdrink", 0.2f));
				StartCoroutine(crafterController._ShowItem("chair", 1.1f));
			}

			if(GUI.Button(new Rect(25, 145, 150, 30), "Stand")){
				crafterController.animator.SetTrigger("ChairStandTrigger");
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Fishing){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Reel In")){
				crafterController.animator.SetTrigger("FishingReelTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Finish Fishing")){
				crafterController.animator.SetTrigger("ItemBackAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Box){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Put Down Box")){
				crafterController.animator.SetTrigger("CarryPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.7f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Throw Box")){
				crafterController.animator.SetTrigger("CarryThrowTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
			if(GUI.Button(new Rect(25, 104, 150, 30), "Give Box")){
				crafterController.animator.SetTrigger("CarryHandoffTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.6f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Lumber){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Put Down Lumber")){
				crafterController.animator.SetTrigger("CarryPutdownTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 1f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Overhead){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Throw Sphere")){
				crafterController.animator.SetTrigger("CarryOverheadThrowTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Climb){
			crafterController.isPaused = true;
			if(GUI.Button(new Rect(25, 25, 150, 30), "Climb Off Bottom")){
				crafterController.animator.SetTrigger("ClimbOffBottomTrigger");
				StartCoroutine(crafterController._ShowItem("none", 0.9f));
				StartCoroutine(crafterController._ChangeCharacterState(0.9f, CrafterController.CharacterState.Idle));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Climb Up")){
				crafterController.animator.SetTrigger("ClimbUpTrigger");
			}
			if(GUI.Button(new Rect(25, 105, 150, 30), "Climb Down")){
				crafterController.animator.SetTrigger("ClimbDownTrigger");
			}
			if(GUI.Button(new Rect(25, 145, 150, 30), "Climb Off Top")){
				Vector3 posPivot = crafterController.animator.pivotPosition;
				crafterController.animator.SetTrigger("ClimbOffTopTrigger");
				StartCoroutine(crafterController._ShowItem("none", 2f));
				StartCoroutine(crafterController._ChangeCharacterState(2f, CrafterController.CharacterState.Idle));
				crafterController.animator.stabilizeFeet = true;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.PushPull){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Release")){
				crafterController.animator.SetTrigger("PushPullReleaseTrigger");
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				StartCoroutine(crafterController._ChangeCharacterState(0.5f, CrafterController.CharacterState.Idle));
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Laydown){
			if(GUI.Button(new Rect(375, 505, 150, 30), "Getup")){
				crafterController.animator.SetTrigger("LaydownGetupTrigger");
				StartCoroutine(crafterController._MovePause(2f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Use){
			if(GUI.Button(new Rect(200, 465, 150, 30), "Use")){
				crafterController.animator.SetBool("Use", false);
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Crawl){
			if(GUI.Button(new Rect(375, 465, 150, 30), "Getup")){
				crafterController.animator.SetTrigger("CrawlGetupTrigger");
				StartCoroutine(crafterController._MovePause(1f));
				crafterController.charState = CrafterController.CharacterState.Idle;
			}
		}
		if(crafterController.charState == CrafterController.CharacterState.Spear){
			if(GUI.Button(new Rect(25, 25, 150, 30), "Spear")){
				crafterController.animator.SetTrigger("SpearfishAttackTrigger");
				StartCoroutine(crafterController._MovePause(1f));
			}
			if(GUI.Button(new Rect(25, 65, 150, 30), "Finish Spearfishing")){
				crafterController.animator.SetTrigger("ItemBackAwayTrigger");
				StartCoroutine(crafterController._MovePause(1.2f));
				crafterController.charState = CrafterController.CharacterState.Idle;
				StartCoroutine(crafterController._ShowItem("none", 0.5f));
				crafterController.isSpearfishing = false;
			}
		}
	}
}