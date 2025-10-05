# MovieCatalog Web API - Backend

In tis projct I have been given the following task to achive a backend of ASP.NET Core Web API built with **Phase 1 - LINQ + JSON (Core C# & LINQ - Read Only)** and **Phase 2 – CRUD + EF Core (Real Backend)**

The API provides endpoints for querying, filtering, and managing a movie catalog.

## Contents
- [Getting Started](#getting-started)
	- [Important](#important)
- [Features](#features)
	- [Phase 1 - JSON + LINQ (Read-only)](#phase-1-json-linq-read-only)
	- [Phase 2 - Transition JASON into EF Core + adding CRUD](#phase-2-transition-json-into-ef-core-adding-crud)
		- ["Selected" query added](#selected-query-added)
		- [CRUD added](#crud-added)
- [Summary](#summary)

<a id="getting-started"></a>
## Getting Started

1. Clone the repository:
	
	* clone the project from the github repository(the link was provided into this path),
	* then open the gitBash using right click (if you have already generally installed the git bush into your PC)
   ```bash
   git clone https://github.com/yourusername/MovieCatalog.git (whatever the link is copied)
   cd MovieCatalog

2. Install dependencies:

	The project uses .NET 8 and EF Core (InMemory provider).
	
	In Visual Studio in the PowerShell terminal run:
	```bash
	dotnet restore

3. Run the application: 

	To run the application from PowerShell terminal you **NEED** to go to other **MovieCtalog** folder where the .csproject file is contained and from there the following command it will work:
	```bash
	cd MovieCatalog
	
4.	Then
	```bash
	dotnet run
	
Now you are able to open the browser using **Swagger UI** with the following path: "http://localhost:5289/swagger" to check and test all endpoints ;)

<a id="important"></a>
### Important

Regarding **Phase 1 - LINQ + JSON (Core C# & LINQ - Read Only)**:

At *MoviesController.cs* file to up and running the LINQ + JSON you have to comment the following phases:

- "**Phase 2b - Transition JSON to EF Core:**"
- "**Phase 2c - Add CRUD Endpoints**"

Now the "**Phase 1 "LINQ + JSON" which is still works after "EF Core" setup only without transition yet (keep for references)**" need to be uncommented to could work **READ ONLY** endpoints at Swagger UI.

Also the command "dotnet run" in the terminal is not **USED** because you need to run the debugger using **"F5" as "http"**(or click on debugger) and the Swagger UI will open itself in the web browser.

<a id="features"></a>
## Features

<a id="phase-1-json-linq-read-only"></a>
### Phase 1 - JSON + LINQ (Read-only)
- JSON file

<img src="./Screenshots_Readme/Screenshot 1.jpg" width="70%"/>

- Swagger UI

<img src="./Screenshots_Readme/Screenshot 2.jpg" width="70%"/>

- GET /api/movies - Get all movies

<img src="./Screenshots_Readme/Screenshot 3.jpg" width="70%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 4.jpg" width="70%"/>

- GET /api/movies/top-rated (default is 5) - Get top-rated movies

<img src="./Screenshots_Readme/Screenshot 5.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 6.jpg" width="70%"/>

- GET /api/movies/rating (rate_provide) - Filter movies by rating

<img src="./Screenshots_Readme/Screenshot 7.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 8.jpg" width="70%"/>

- GET /api/movies/descending order by year - Movies ordered by year (desc)

<img src="./Screenshots_Readme/Screenshot 9.jpg" width="70%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 10.jpg" width="70%"/>

- GET /api/movies/genre/{genre} (genre required)- Filter by genre

<img src="./Screenshots_Readme/Screenshot 11.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 12.jpg" width="80%"/>

<a id="phase-2-transition-into-ef-core-adding-crud"></a>
### Phase 2 - Transition JSON into EF Core + adding CRUD
1. Setup for EF Core (Entity Framework Core)

	In the PowerShell terminal I typed the following command to install Entity Framework Core into this project:
	```bash
	dotnet add package Microsoft.EntityFrameworkCore.InMemory
	```
	**PLEASE NOTE:** This is only a preset showing how I provide the EF Core during the step-by-step project creation.
	However, this project already includes the EF Core package because it was cloned from the repository.

2. Than I needed to create a **"AppDbContext.cs"** and implement the plain class as follow:

<img src="./Screenshots_Readme/Screenshot 13.jpg" width="80%"/>

3. Next I have to register the EF Core service in the “Program.cs” file as follows:

<img src="./Screenshots_Readme/Screenshot 14.jpg" width="80%"/>

4. Again at **"Program.cs"** i need **seed** data from movies.json - basically that means take the JSON file I used in movies.json as my source of truth and use it to populate the database automatically: read the JSON file at startup, convert it into C# objects(Deserialize), and insert them into the database tables, so you start with meaningful data (sensownych danych) instead of an empty DB.

<img src="./Screenshots_Readme/Screenshot 15.jpg" width="80%"/>
<br><br>


#### Transistion:
To transist JSON file to EF Core I have to inject AppDbContext to interact with the database in the controller instead of loading **ALL** movies from the JSON file as it was in previous example. Also this Dependency Injection I have to provide in MovieController constructor:

<img src="./Screenshots_Readme/Screenshot 16.jpg" width="80%"/>
<br><br>

The Swagger UI endpoints display this same output as was provided above so I only  providing the code which was changed slidelly.

- GET /api/movies - Get all movies (from EF Core):

<img src="./Screenshots_Readme/Screenshot 17.jpg" width="80%"/>
<br><br>

- GET /api/movies/top-rated (default is 1) - Get top-rated movies:

<img src="./Screenshots_Readme/Screenshot 18.jpg" width="80%"/>
<br><br>

- GET /api/movies/rating (rate_provide) - Filter movies by rating:

<img src="./Screenshots_Readme/Screenshot 19.jpg" width="80%"/>
<br><br>

- GET /api/movies/descending order by year - Movies ordered by year:

<img src="./Screenshots_Readme/Screenshot 20.jpg" width="80%"/>
<br><br>

- GET /api/movies/genre/{genre} (genre required)- Filter by genre:

<img src="./Screenshots_Readme/Screenshot 21.jpg" width="80%"/>
<br><br>

<a id="selected-query-added"></a>
#### "Selected" query added

- GET /api/movies/id/title/genre/{id_selected} - Custom: Return only Id, Title, and Genre (with custom BadRequest message if Id invalid)

<img src="./Screenshots_Readme/Screenshot 22.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 23.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 24.jpg" width="80%"/>

<a id="crud-added"></a>
#### CRUD added

As you can see there are only POST(create), PUT(update), DELETE endpoints provided. The GET(reload) endpoints are mentioned already in the above section.  

- POST /api/movies/create new movie - Add new movie:

<img src="./Screenshots_Readme/Screenshot 25.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 26.jpg" width="80%"/>
<br><br>

- PUT /api/movies/update_movie/{id} - Update movie (partial update supported):

<img src="./Screenshots_Readme/Screenshot 27.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 28.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 29.jpg" width="80%"/>
<br><br>

- DELETE /api/movies/delete_movie{id} - Delete movie by Id:

<img src="./Screenshots_Readme/Screenshot 30.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 31.jpg" width="80%"/>
<br><br>
<img src="./Screenshots_Readme/Screenshot 32.jpg" width="80%"/>
<br><br>

Lastly the Swagger UI should look as follows:

<img src="./Screenshots_Readme/Screenshot 33.jpg" width="80%"/>
<br><br>

<a id="summary"></a>
## Summary

- Phase 1 (JSON + LINQ) endpoints are still available but no longer required once EF Core is running.
- Database is seeded automatically from Data/movies.json when the app starts (if empty).
- Works with InMemory provider - data resets on restart.

This backend of ASP.NET Core Web API project is fully compatible with Blazor. I plan to use it in a future Blazor project to build the frontend interface and see how it all works together.