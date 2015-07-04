using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Prospector : MonoBehaviour {

	static public Prospector 	S;
	public Deck					deck;
	public TextAsset			deckXML;
	
	public Layout				layout;		// p 589
	public TextAsset			layoutXML;
	
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

}
