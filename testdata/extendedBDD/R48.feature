Feature: R48 - The product shall record meeting entries

	As a Meeting Organiser
	I want to record meeting entries
	So that I can always access a persisted record of each meeting
	Which may impact delete room equipment
	
Without ignoring: 
	|	By ensuring the product can work with most DBMS									| Make	|
	|	By ensuring the product can communicate with DBMS on 100% of all transactions	| Make	| 
	
Scenario: Record simplest meeting
    Given the application has been started
	And I select to add new meeting
	When I specify a meeting name and date
	And I select to save the meeting
	Then the meeting should be recorded
	Breaking the By ensuring the product can work with most DBMS
	Which helps By ensuring the product can communicate with DBMS on 100% of all transactions
	Contributing to help By ensuring the product can work with most DBMS	
	With some positive contribution to record meeting entries