# Socratic Scribe

Socratic Scribe is a professional academic writing assistant web application designed to help students improve their writing through the **Socratic method**. Instead of rewriting or correcting student text directly, it provides targeted, probing questions that encourage critical thinking and autonomous revision.

---

## Key Features

* **Socratic Guidance:** Pinpoints structural, logical, or grammatical flaws and poses targeted hints without providing direct solutions or rewrites.
* **Real-Time Streaming:** Leverages Server-Sent Events (SSE) and C# `IAsyncEnumerable` to stream AI feedback token-by-token directly to the browser.
* **Anti-Rewrite Guardrails:** Implements an optimized Levenshtein distance text similarity algorithm to detect and block direct text rewrites, enforcing genuine Socratic dialogue.
* **Dual Analysis Modes:** Switch seamlessly between Fast and Deep analysis models via an intuitive UI toggle.
* **Robust Error Handling:** Features custom global exception middleware, character limits with animated UI feedback, and secure per-request HTTP header handling.

---

## Tech Stack

* **Backend:** C# / .NET (ASP.NET Core Web API)
* **Frontend:** HTML5, Modern CSS, JavaScript (SSE Stream Consumer)
* **AI Integration:** OpenRouter API

---

## Project Structure

```text
Socratic_Academic_Writing_Assistant/
├── Controllers/        # API Endpoints (AnalysisController.cs)
├── Dtos/               # Data Transfer Objects for API communication
├── Helpers/            # Middleware & Algorithms (ExceptionMiddleware, TextSimilarity)
├── Services/           # AI Business Logic (OpenRouterAnalysisService, IAIAnalysisService)
├── wwwroot/            # Static Frontend Assets (index.html, js/app.js, styles/main.css, resources/)
├── Program.cs          # Application entry point & dependency injection setup
└── appsettings.json    # Application configuration
```

## How to run?
* You can use this domain (working) or follow the 3 step process

## Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/en-us/)
* Active [OpenRouter API key](https://openrouter.ai/)

## Installation & Configuration
1. Clone the repository:
```bash
git clone https://github.com/ondra-honc/Socratic-Scribe.git
cd Socratic-Scribes
```

2. Add your OpenRouter API key to appsettings.json (or appsettings.Development.json):
```json
{
  "OpenRouter": {
    "ApiKey": "your_api_key_here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Alternatively you can put the API key into your environmental variables under the name "OpenRouter__ApiKey"

3. Build and run the backend project:
```bash
dotnet run
```

* Once you run the project it should immediately open your brower with the app

## License

This project is open source and available under the [MIT License](LICENSE).

---

© 2026 Socratic Scribe • Built for students.
