Feature: Gameplay Scenarios
	As a player
	I want to play through different game scenarios
	So that I can experience winning, losing, and combat

Background:
	Given a game world has been set up

Scenario: Winning the game by reaching the throne room
	Given the player starts in the "Start" room
	And there is a "Win" room connected to the current room
	When the player moves to the "Win" room
	Then the game should be won
	And the game should be over
	And the player should be in the "Win" room

Scenario: Losing the game by dying in a deadly room
	Given the player starts in the "Start" room
	And the player has 50 health
	And there is a deadly room "Deadly" connected to the current room
	When the player moves to the "Deadly" room
	Then the player should have 0 health
	And the game should be over
	And the game should not be won

Scenario: Being blocked by a living monster
	Given the player starts in the "Start" room
	And there is a room "MonsterRoom" with a living monster
	And "MonsterRoom" is connected to the current room
	When the player tries to move to "MonsterRoom"
	Then the player should remain in the "Start" room
	And the monster should still be alive

Scenario: Collecting items from rooms
	Given the player starts in the "Start" room
	And the "Start" room contains a "Sword" item
	When the player takes the "Sword" item
	Then the player inventory should contain "Sword"
	And the "Start" room should no longer contain "Sword"

Scenario: Unlocking a door with a key
	Given the player starts in the "Start" room
	And the "Start" room contains a "Key" item
	And there is a locked room "Locked" connected to the current room
	When the player takes the "Key" item
	And the player moves to "Locked"
	Then the player should be in "Locked" room
	And the door should be unlocked

Scenario: Cannot enter locked room without key
	Given the player starts in the "Start" room
	And the player does not have a key
	And there is a locked room "Locked" connected to the current room
	When the player tries to move to "Locked"
	Then the player should remain in the "Start" room
	And the door should still be locked

