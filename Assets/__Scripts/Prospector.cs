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
	} //LayoutGame
	
} // Prospector
