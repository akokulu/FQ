Global TODO
===========

## Forest
* The cube which returns the player back to the town doesn't spawn

##GUI
* After selecting Play Game on the main menu, the player should be prompted to enter their name (which will be shown in high scores). Default name: Redhot Hobbit
* Main menu gui needs a semi transparent black background for better visualization
* Pause menu gui needs a semi transparent black background for better visualization
* Quest Dialog GUI (guitext in level switcher scene) needs a semi transparent black background for better visualization
* OK/CANCEL buttons should be added to the Quest Dialog GUI 
* Pause menu


## Player
* Right mouse click should block attacks, preventing damage (Resist animation of the spartanking may be good for this)
* Add keyboard controls to attack and block (O for attack P for block)
* Dropped item should be replaced with the lantern model at Models/Lantern01

## Town
* Towns need to be created from the level switcher scene
* Dialogue trigger at levelswitcher still says "I eat babies". Random dialogue is still not there




GAMEPLAY FLOW
=============
1. Main Menu -> Play Game -> Player spawns at random town (level switcher)
2. Dialogue trigger hit -> random line shown from start.txt
3. Forest entrance trigger hit -> Player spawns in random forest
4. Kill skeletons -> get lamp -> go back to town portal in the forest -> back to the last town visited
5. Dialogue trigger hit -> random line shown from end.txt, also heal the player by 20% of their maxHealth
6. Forest entrance trigger hit -> Player spawns at another random town (Go back to 2)