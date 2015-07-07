using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// these are the only places a card should be in the game
// setting this up for convenience in naming
public enum CardState {
	drawpile,
	tableau,
	target,
	discard
}


// Note this extends Card, not MonoBehavior
public class CardProspector : Card {

	public CardState			state = CardState.drawpile;	
	public List<CardProspector>	hiddenBy = new List<CardProspector>();  //which cards keep this face down
	public int					layoutID;		// matches to layout if in tableau
	public SlotDef				slotDef;		// data pulled from layout.xml 


	override public void OnMouseUpAsButton() {
		Prospector.S.CardClicked(this);
		base.OnMouseUpAsButton();
	}

}  // CardProspector
