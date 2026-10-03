# EducareSA

A South African matric and university course finder. Learners capture their
matric subjects and marks, and the site calculates their APS, matches them
against university programme requirements, and recommends bursaries.

## Features

### For learners
- Capture matric subjects and marks
- Automatic APS calculation (NSC 7-point scale, best 6 subjects, LO capped at 4)
- See which university programmes you qualify for — with reasons when you don't
- Browse universities, campuses, and faculties
- Browse TVET college programmes
- Browse bursaries from NSFAS, government, banks, and corporates
- A built-in AI assistant for general questions about programmes and funding

### For admins
- Manage universities, campuses, faculties, and programmes
- Manage bursaries and TVET colleges
- Mark programme data as verified against the official prospectus
- Dashboard with catalogue-wide statistics

## Tech Stack

- **Framework:** ASP.NET Core MVC (.NET 8)
- **Database:** SQL Server (LocalDB for development), Entity Framework Core 8
- **Auth:** ASP.NET Core Identity with Admin role
- **Frontend:** Razor views, Bootstrap 5, custom CSS, Font Awesome, AOS animations
- **AI:** Local LLM via [Ollama](https://ollama.com) (llama3.2)

## Project Structure
