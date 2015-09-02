Feature: R53 - The product shall store new conference rooms
	
			As a Facilities Manager
			I want to record new conference rooms
			So that I can keep my facilities inventory updated
			Which may impact update existing conference rooms
			Which may impact delete conference rooms
			
Constrained by:
			|	Interoperability 		| Help	|

# This scenario does not include a contribution.
# Therefore, a default contribution from scenario to goal will be added
Scenario: 	Record conference room with minimum details
			Given the application has been started
			And I choose to add new conference room
			When I specify a conference room name, building identifier and floor
			And I proceed to save the conference room
			Then the conference room should be recorded

@NFR	
Scenario: 	Conference rooms can be recorded in most DBMS
			Contributing to help Interoperability

@NFR
Scenario:	100% of all transactions recording conference rooms in DBMS are successful
			Contributing to help Interoperability