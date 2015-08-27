Feature: R58 - The product shall be able to delete room equipment

			As a Facilities Manager
			I want to delete room equipment
			So that I can keep my conference rooms equipment inventory updated
	
Without ignoring: 
			|	Interoperability 		| Help	|

Scenario: 	Delete room equipment not associated with a meeting succeeds

# This scenario shows that by producing examples we can uncover
# other non-functional requirements
Scenario: 	Delete room equipment with a meeting associated notifies meeting organiser

@NFR
Scenario: 	Notifications of deleted room equipment can be sent through various email servers	
			Which helps Interoperability

@NFR	
Scenario: 	Room equipment can be deleted from most DBMS
			Contributing to help  Interoperability

@NFR
Scenario:	100% of all transactions deleting room equipment from DBMS are successful
			Contributing to help  Interoperability