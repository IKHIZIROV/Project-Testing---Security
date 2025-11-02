Feature: Complete Game Flow
	As a player
	I want to play through the entire game
	So that I can experience the full adventure

Background:
	Given a game world has been set up

Scenario Outline: Complete victory path - collect key, unlock door, defeat monster, reach throne
	Given the player starts in the "Start" room
	And the "Start" room contains a "<weapon>" weapon
	And there is a room "East" containing a "<key>" key
	And there is a locked room "North" connected to "Start"
	And "North" leads to a room "Deeper" with a monster
	And "Deeper" leads to a "Win" room
	When the player moves east to "East"
	And the player takes the "<key>" key
	And the player moves back to "Start"
	And the player moves north to "North"
	And the player takes the "<weapon>" weapon
	And the player moves down to "Deeper"
	And the player defeats the monster
	And the player moves north to "Win"
	Then the game should be won
	And the player should have the "<weapon>" in inventory
	And the player should have the "<key>" in inventory

	Examples:
		| weapon | key  |
		| Sword  | Key  |
		| Dagger | Key  |

Scenario: Full game path with GameSetup world
	Given a game world is created using GameSetup
	When the player navigates through the game world
	And collects all necessary items
	And defeats all monsters
	And reaches the win room
	Then the game should be won
	And the player should have collected items during the journey

