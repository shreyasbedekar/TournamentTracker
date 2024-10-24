Tournament Tracker
This is a full-stack application designed to manage sports tournaments. It allows users to create, track, and manage teams, prizes, matchups, and tournaments. The application is built using C#, ASP.NET Core Web API, React frontend, and PostgreSQL as the database.

Features
Team Management: Create and manage teams, including assigning team members.
Tournament Management: Create tournaments with an entry fee, add teams and prizes, and track rounds and matchups.
Prize Management: Configure and assign prizes for tournaments.
Rounds and Matchups: Automatically generate rounds and matchups for tournaments.
WinForms UI: A separate WinForms application for managing tournament data.
Project Structure
The solution consists of the following projects:

1. TrackerAPI
Type: ASP.NET Core Web API
Responsibilities:
Expose RESTful endpoints for managing teams, tournaments, prizes, and matchups.
Connects to PostgreSQL via Entity Framework Core.
2. TrackerLibrary
Type: C# Class Library
Responsibilities:
Contains models for TeamModel, TournamentModel, PrizeModel, and MatchupModel.
Handles data access logic with Dapper, providing database operations for tournament entries, prizes, rounds, and matchups.
3. TrackerUI (WinForms)
Type: C# WinForms Application
Responsibilities:
Provides a desktop interface for creating tournaments, adding teams, and tracking progress.
Interacts with the backend via API calls and direct database connections using Dapper.
Database
Database: PostgreSQL
Schema:
Tables: People, Teams, Tournaments, Prizes, Matchups, MatchupEntries, TournamentEntries, TournamentPrizes, TeamMembers.
Auto-incrementing primary keys for all tables.
API Endpoints
Teams
GET /api/Teams - Retrieve all teams.
POST /api/Teams - Create a new team.
PUT /api/Teams/{id} - Update an existing team.
DELETE /api/Teams/{id} - Delete a team.
Tournaments
GET /api/Tournaments - Retrieve all tournaments.
POST /api/Tournaments - Create a new tournament.
PUT /api/Tournaments/{id} - Update an existing tournament.
DELETE /api/Tournaments/{id} - Delete a tournament.
Getting Started
Prerequisites
.NET 6.0 SDK
PostgreSQL database
Visual Studio or Visual Studio Code
Setup
Clone the repository:

bash
Copy code
git clone https://github.com/yourusername/tournament-tracker.git
cd tournament-tracker
Configure the database:

Update the appsettings.json in the TrackerAPI with your PostgreSQL connection string.
Run the migrations:

bash
Copy code
dotnet ef database update
Run the API:

bash
Copy code
cd TrackerAPI
dotnet run
Run the WinForms UI:

Open the TrackerUI project in Visual Studio and run the project.
