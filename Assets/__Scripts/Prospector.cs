using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Prospector : MonoBehaviour {

	static public Prospector 	S;
	public Deck					deck;
	public TextAsset			deckXML;
	
	public Layout				layout;		// p 589
	public TextAsset			layoutXML;
	
	public Vector3				layoutCenter;
	public float				xoffset = 3f;
	public float				yoffset = -2.5f;
	public Transform			layoutAnchor;
	
	public CardProspector		target;
	public List<CardProspector>	tableau;
	public List<CardProspector>	discardPile;
	
	
	public List<CardProspector> drawPile;

	void Awake(){
		S = this;
	}

	void Start() {
		deck = GetComponent<Deck> ();
		deck.InitDeck (deckXML.text);
		Deck.Shuffle (ref deck.cards);
		
		// added p 589
		layout = GetComponent<Layout>();
		layout.ReadLayout(layoutXML.text);
		
		drawPile = ConvertListCardsToListCardProspector(deck.cards);
		LayoutGame();
	}


	// A utility function designed to take Cards in and make them
	// CardProspector's. Returns the deck, converted ready for game play
	List<CardProspector> ConvertListCardsToListCardProspector(List<Card> lCD){
		List<CardProspector> lCP = new List<CardProspector>();
		CardProspector tCP;
		
		foreach(Card tCD in lCD){
			tCP = tCD as CardProspector;
			lCP.Add (tCP);
		}
		return(lCP);
	} // ConvertListCardsToListCardProspector
	
	
	// Draw() - draw a single card from the drawPile and returns it
	CardProspector Draw(){
		CardProspector cd = drawPile[0];
		drawPile.RemoveAt(0);
		return (cd);
	} // Draw


	// LayoutGame() - positions the original tableau of cards
	void LayoutGame(){
	
		// first create an empty game object to anchor the tableau
		if (layoutAnchor == null) {
			GameObject tGO = new GameObject("_LayoutAnchor");
			layoutAnchor = tGO.transform;
			layoutAnchor.transform.position = layoutCenter;
		}	
		
		// then position all of the cards on the basis of the data from layoutXML
		CardProspector cp;
		
		foreach(SlotDef tSD in layout.slotDefs) {
			cp = Draw ();
			cp.faceUP = tSD.faceUp;
			cp.transform.parent = layoutAnchor;  // move from deck to tableau in hierarchy
			
			cp.transform.localPosition = new Vector3(
				layout.multiplier.x * tSD.x,
				layout.multiplier.y * tSD.y,
				-tSD.layerID ); // end new Vector3
				
			cp.layoutID = tSD.id;
			cp.slotDef = tSD;
			cp.state = CardState.tableau;
			
			cp.SetSortingLayerName(tSD.layerName); //Get things ready to handle all off the image layers
			
			tableau.Add (cp);		
		} //foreach SlotDef
		
		// Set up initial Target card
		MoveToTarget(Draw ());
		
		//set up draw pile
		UpdateDrawPile();
	} //LayoutGame
	
	// Called any time a card is clicked
	public void CardClicked(CardProspector cd) {
		switch(cd.state){
			case CardState.target:
				// doesn't do anything
				break;
			case CardState.drawpile:
				MoveToDiscard(target);
				MoveToTarget(Draw ());
				UpdateDrawPile();
				break;
			case CardState.tableau:
				//clicking will cause check to see it is a valid play
				break;
		} // switch cd.state
	} // CardClicked
	
	// Move the current target to the discard pile
	void MoveToDiscard(CardProspector cd){
		// set state to discard
		cd.state = CardState.discard;
		discardPile.Add (cd);
		cd.transform.parent = layoutAnchor;
		
		cd.transform.localPosition = new Vector3(
			layout.multiplier.x * layout.discardPile.x,
			layout.multiplier.y * layout.discardPile.y, 
			-layout.discardPile.layerID+0.5f);
		cd.transform.localRotation = Quaternion.Euler(new Vector3 (0.0f,0.0f,Random.Range(-30.0f,30.0f)));
		cd.faceUP = true;
		
		//place it on top of pile for depth sorting
		cd.SetSortingLayerName (layout.discardPile.layerName);
		cd.SetSortOrder(-100+discardPile.Count);
	}// MoveToDiscard
	
	// make the cd the new Target card
	void MoveToTarget(CardProspector cd) {
		// if there is a target card, move it to the discard pile
		if (target != null) {
			MoveToDiscard(target);
		}
		
		target = cd;
		cd.state = CardState.target;
		cd.transform.parent = layoutAnchor;
		cd.transform.localPosition = new Vector3(
			layout.multiplier.x * layout.discardPile.x,
			layout.multiplier.y * layout.discardPile.y, 
			-layout.discardPile.layerID);
		cd.faceUP = true;
		cd.SetSortingLayerName (layout.discardPile.layerName);
		cd.SetSortOrder(0);
	} // MoveToTarget
	
	void UpdateDrawPile() {
		CardProspector cd;
		
		// go through all cards of draw pile and reposition
		// with proper sort order and stagger
		for (int i=0; i <drawPile.Count; i++)
		{
			cd = drawPile[i];
			cd.transform.parent = layoutAnchor;
			Vector2 dpStagger = layout.drawPile.stagger;
			cd.transform.localPosition = new Vector3(
				layout.multiplier.x * layout.drawPile.x + i*dpStagger.x,
				layout.multiplier.y * layout.drawPile.y + i*dpStagger.y, 
				-layout.discardPile.layerID+0.1f*i);
			cd.faceUP = false;
			cd.state = CardState.drawpile;
			cd.SetSortingLayerName (layout.drawPile.layerName);
			cd.SetSortOrder(-10*i);
		}//for in the drawPile
	} // UpdateDrawPile
	
} // Prospector
