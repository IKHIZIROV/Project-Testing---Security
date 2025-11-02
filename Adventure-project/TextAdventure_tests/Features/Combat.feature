Feature: Combat System
	As a player
	I want to fight monsters
	So that I can defeat them and collect their loot

Background:
	Given a game world has been set up

Scenario: Successfully defeating a monster in combat
	Given the player starts in the "Start" room
	And the player has a weapon "Sword" with 100 damage
	And there is a room "MonsterRoom" with a weak monster "WeakOrc" that has 10 health
	And "MonsterRoom" is connected to the current room
	When the player takes the "Sword" weapon
	And the player moves to "MonsterRoom"
	And the player attacks the monster until it is defeated
	Then the monster should be dead
	And the player should be able to enter "MonsterRoom"
	And the room should reflect the monster is dead

Scenario: Monster drops loot when defeated
	Given the player starts in the "Start" room
	And there is a monster "Orc" with loot "Potion"
	And the player defeats the monster
	When the monster dies
	Then the monster should drop loot
	And the room should contain the "Potion" item

Scenario: Cannot fight without a weapon
	Given the player starts in the "Start" room
	And the player does not have any weapons
	And there is a room "MonsterRoom" with a monster
	And "MonsterRoom" is connected to the current room
	When the player tries to move to "MonsterRoom"
	Then the player should be blocked by the monster
	And the player should not be able to fight

Scenario: Taking damage from a monster during combat
	Given the player starts with 100 health
	And the player has a weapon
	And there is a monster that attacks for 10 damage
	When the player fights the monster
	And the monster attacks the player
	Then the player health should decrease
	And the player should have less than 100 health

Scenario: Complete combat flow - pick weapon, fight, collect loot
	Given the player starts in the "Start" room
	And the "Start" room contains a "Sword" weapon
	And there is a room "MonsterRoom" with a weak monster that has loot
	And "MonsterRoom" is connected to the current room
	When the player takes the "Sword" weapon
	And the player moves to "MonsterRoom"
	And the player defeats the monster
	And the player collects the dropped loot
	Then the player should have the weapon in inventory
	And the player should have the loot in inventory
	And the monster should be defeated

