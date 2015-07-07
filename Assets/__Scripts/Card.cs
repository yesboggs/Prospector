using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public string    			suit;
	public int       			rank;
	public Color     			color = Color.black;
	public string    			colS = "Black";  // or "Red"
	
	public List<GameObject> 	decoGOs = new List<GameObject>();
	public List<GameObject> 	pipGOs = new List<GameObject>();
	
	public GameObject 			back;     // back of card;
	public CardDefinition 		def;  // from DeckXML.xml	
	
	// added on page 595
	// array of SpriteRenderers of this object and its children
	public SpriteRenderer[]		spriteRenderers;		

	
	void Start() {
		SetSortOrder(0);
	} //Start
	
	// property
	public bool faceUP {
		get {
			return (!back.activeSelf);
		}		
		set {
			back.SetActive(!value);
		}
	} // property FaceUp
	
		
	// if spriteRenderers not yet defined, define them	
	public void PopulateSpriteRenderers() {
		if (spriteRenderers == null || spriteRenderers.Length ==0) {
			spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		}
	
	} //PopulateSpriteRenderers 
	
			
	//Set sorting layer name on all sprite renderer components
	public void SetSortingLayerName(string tSLN){
		PopulateSpriteRenderers();
		
		foreach(SpriteRenderer tSR in spriteRenderers) {
			tSR.sortingLayerName = tSLN;
		}
	} // SetSortingLayerName						
	
	
	// Set the sort order of all of the SpriteRenderer Components
	public void SetSortOrder(int sOrd)	{
		PopulateSpriteRenderers();
		
		// order is
		// sOrd   - white background
		// sOrd+1 - decorators and pips
		// sOrd+2 - card back
		
		foreach(SpriteRenderer tSR in spriteRenderers){
			if (tSR.gameObject == this.gameObject) {
			// if we are looking at ourselves, we must be the background...
				tSR.sortingOrder = sOrd;
				continue;  // our work for this one is done
			}
			
			// else dealing with pips, decorators and back
			switch(tSR.gameObject.name){
				case "back":
					tSR.sortingOrder = sOrd+2;	
					break;
				case "face":
				default:
					tSR.sortingOrder = sOrd+1;
				break;
			} // switch
		}//foreach SpriteRenderer
	} //SetSortOrder
	
						
	// virtual methods for overriding by game play
	virtual public void OnMouseUpAsButton() {
		print (name);
	}// OnMouseUpAsButton										
																
																										
} // class Card

[System.Serializable]
public class Decorator{
	public string	type;			// For card pips, tyhpe = "pip"
	public Vector3	loc;			// location of sprite on the card
	public bool		flip = false;	//whether to flip vertically
	public float 	scale = 1.0f;
}

[System.Serializable]
public class CardDefinition{
	public string	face;	//sprite to use for face cart
	public int		rank;	// value from 1-13 (Ace-King)
	public List<Decorator>	
					pips = new List<Decorator>();  // Pips Used					
} // Class CardDefinition
