# technical-test-2023

## Brainstorming
- Users contain personal information, need to be cautious with logs
- Use DTOs so that schema changes don't force a breaking api change 
- Decided not to use minimal endpoints, decision based purely on being more comfortable with non minimal 

## Assumptions
Validation
	- User
		- First name length >= 1 and <= 250
		- Last name length >= 1 and <= 250
	- Address
		- Street number >= 0 and <= 100,000
		- Street number suffix length <= 50
		- Street name length >= 1 and <= 100
		- Suburb length <= 100
		- City length >= 1 and <= 100
		- Post code length <= 10

An unlimited amount of people can live at an address
A single person can have multiple addresses and therefore multiple accounts

## Problems
- Date of birth format isn't ideal, expects yyyy-mm-dd, would prefer it took dd/mm/yyyy

## Todo list
- Date validation requires fluent validation to make sure it's in the past and within the last 150 years (?)
