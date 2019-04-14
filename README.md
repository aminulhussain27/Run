# Run
Infinity running

Control:
For character Jump Swipe up, Slide swipe down and left right movement to move the character's running lane.

The whole game is using a state machine to handle the different states of the game : Loadout, Game and Gameover.
The State Machine, called GameManager (Scripts/GameManager) is manually updating the state on the top of its states list. When pushing a new state on top, you can decide to pop the previous one or leave it and place the new one on top.
That allow things like pushing the GameOver state on top of the Game State, keeping all game state intact to go back to it as it was.
