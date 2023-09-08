# technical-test-2023

## Setup
This project is built on .NET 7.0

Checkout the code

	Open a terminal
 	cd to the root directory
 	Build the solution with dotnet build
 	Run dotnet run --project .\TechnicalTest2023\TechnicalTest2023.csproj
 	To run tests run dotnet test

	Also runs in Visual studio exactly as you'd expect

## Brainstorming
- Users contain personal information, need to be cautious with logs
- Use DTOs so that schema changes don't force a breaking api change 
- Decided not to use minimal endpoints, decision based purely on being more comfortable with non minimal 

## Assumptions
Validation

	- User
		- First name length >= 1 and <= 250
		- Last name length >= 1 and <= 250
		- Date of birth >= today but 150 years ago and <= today
	- Address
		- Street number >= 0 and <= 100,000
		- Street number suffix length <= 50
		- Street name length >= 1 and <= 100
		- Suburb length <= 100
		- City length >= 1 and <= 100
		- Post code length <= 10

An unlimited amount of people can live at an address.

A single person can have multiple addresses and therefore multiple accounts

Allow any date of birth to exist in the database, we're trusting the validation to occur before the insertion as otherwise it'll be unusable as time progressed

## Problems
- Date of birth format isn't ideal, expects yyyy-mm-dd, would prefer it was formatted dd/mm/yyyy 
