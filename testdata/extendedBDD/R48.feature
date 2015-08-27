Feature: R48 - The product shall record meeting entries

			As a Meeting Organiser
			I want to record meeting entries
			So that I can always access a persisted record of each meeting
			Which may impact record different meeting types
			Which may impact record a meeting agenda
	
Without ignoring: 
			|	Interoperability 		| Help	|

@NFR
Scenario: 	Meeting entries can be recorded in most DBMS
			Which helps Interoperability

@NFR	
Scenario:	100% of all transactions recording meeting entries in DBMS are successful
			Which helps Interoperability
	
Scenario: 	Record simplest meeting
			Given the application has been started
			And I choose to add new meeting
			When I specify a meeting name and date
			And I proceed to save the meeting
			Then the meeting should be recorded
			Which helps record meeting entries
	
Scenario: 	Record meeting for a date in the past fails with a warning to the user
			Given the application has been started
			And I choose to add new meeting
			When I specify a date in the past
			And I proceed to save the meeting
			Then the meeting should not be recorded
			And the user is warned with an appropriate message
			Which helps record meeting entries

Scenario: 	Record complex meeting
			Given the application has been started
			And I choose to add new meeting
			When I specify a meeting with 10 attendants
			And I choose an available conference room
			And I request a projector to be available
			And I proceed to save the meeting
			Then the meeting should be recorded
			Which helps record meeting entries
			Contributing to help Interoperability
