# CLAUDE.md — NewAI_CV_builder

---

## Project Overview

**Project name:** NewAI_CV_builder  
**Solution file:** `NewAI_CV_builder.slnx`  
**Target framework:** `net10.0-windows`  
**Project types:** Desktop (WinForms)  
**Primary language:** C# 13 (implicit usings + nullable reference types enabled)  
**Description:** A WinForms desktop tool for a .NET developer/technical writer (Ola) that uses OpenAI or Claude AI to tailor resume JSON for ATS systems and generate Upwork proposals, then automates PDF generation via Playwright on resumake.io.

**GitHub repo:** https://github.com/bigboybamo/AI_CV_Builder

---

## Repository Structure

```
NewAI_CV_builder/                   # Repo root
  NewAI_CV_builder/                 # WinForms project
    Services/
      ResumakePlaywrightFlow.cs     # Playwright automation for resumake.io PDF generation
    Utilities/
      AtsResumePromptBuilder.cs     # Builds AI prompts for resume optimisation + Upwork proposals
      CheckBoxRuleCatalog.cs        # Static list of optional proposal rules (checkbox-driven)
      CheckBoxRuleItem.cs           # DTO for a checkbox rule entry
      PromptRules.cs                # Assembles runtime rules block from selected checkboxes
      UpworkPromptTemplates.cs      # Proposal templates: Web Dev, Desktop Dev, Technical Writer
      UpworkProposalRequest.cs      # Request DTO for Upwork proposal generation
    Form1.cs                        # Main form — UI, event handlers, OpenAI + Claude API calls
    Form1.Designer.cs
    Program.cs                      # Entry point — loads .env, configures Serilog, starts Form1
    .env                            # Local secrets (gitignored) — see Environment Variables below
  integrations/                     # MCP integration configs
    azure-devops/
    github/
    jira/
    slack/
  scripts/                          # PowerShell scripts (bootstrap, setup, validate)
  .claude/
    agents/
    commands/
```

---

## Architecture

### Pattern
Single-project WinForms application. No Clean Architecture layers, no CQRS, no MediatR.

**Layer summary:**
- `Form1.cs` — UI, event handlers, direct API calls to OpenAI and Claude via `HttpClient`
- `Services/` — Playwright browser automation (PDF generation)
- `Utilities/` — Static helper classes for prompt building and rule assembly

All utility classes are `static`. There are no interfaces, no DI container, and no repository pattern.

### Key flows

1. **Resume optimisation:** User pastes a job description + selects/provides a JSON resume → prompt built by `AtsResumePromptBuilder.Build` → sent to OpenAI (`gpt-4.1-mini`) or Claude (`claude-sonnet-4-5-20250929`) → result displayed, auto-stripped of markdown fences → `ResumakePlaywrightFlow.GeneratePdfFromJsonAsync` generates a PDF via resumake.io

2. **Upwork proposal generation:** User pastes a job description + selects a job title + optional checkbox rules → prompt built by `AtsResumePromptBuilder.BuildUpwork` → sent to OpenAI or Claude → output auto-copied to clipboard

### AI model selection
The user selects either OpenAI or Claude via mutually exclusive checkboxes. API keys are read from environment variables at form construction time.

---

## Naming Conventions

| Element | Convention | Example |
|---|---|---|
| Classes | PascalCase | `ResumakePlaywrightFlow` |
| Methods | PascalCase | `GeneratePdfFromJsonAsync` |
| Async methods | `Async` suffix | `CallClaudeAsync` |
| Private fields | `_` prefix + camelCase | `_debounceTimer`, `_httpClaude` |
| `readonly` fields | camelCase | `openAIApiKey` |
| Local variables | camelCase | `resumeJson`, `selectedJob` |

---

## Coding Standards

### General
- `<ImplicitUsings>enable</ImplicitUsings>` — common namespaces available without explicit `using`
- `<Nullable>enable</Nullable>` — nullable warnings are active; annotate `string?` where null is possible
- Block-scoped namespaces (`namespace X { }`) — existing files use this style; keep consistent
- `var` used freely when the type is obvious from context

### Async / await
- All I/O and API calls must be `async` and return `Task<T>`
- Use `ConfigureAwait(false)` in `Services/` and `Utilities/` code
- UI callbacks use `.ContinueWith(task => this.Invoke(...))` for WinForms thread marshalling
- Never use `.Result` or `.Wait()`

### Logging
- **Always use Serilog** — `Log.Information(...)`, `Log.Warning(...)`, `Log.Error(...)`, `Log.Debug(...)`
- Never use `Console.WriteLine` or `Debug.WriteLine`
- Log structured data with named properties: `Log.Information("Foo={Foo}", fooValue)`
- Logs write to console + rolling daily file under `%LOCALAPPDATA%\Cv_BuiderWinforms\Logs\`

### Error handling
- UI errors shown via `MessageBox.Show(...)` with appropriate icon
- API helpers return error strings (`$"Error: {statusCode} ..."`) for non-success HTTP — do not throw
- Infrastructure exceptions (file I/O, browser automation) propagate; log before re-throwing

---

## NuGet Packages

| Purpose | Package | Version |
|---|---|---|
| Environment variables | `DotNetEnv` | 3.1.1 |
| Browser automation | `Microsoft.Playwright` | 1.57.0 |
| Logging | `Serilog` | 4.3.0 |
| Logging — console sink | `Serilog.Sinks.Console` | 6.1.1 |
| Logging — file sink | `Serilog.Sinks.File` | 7.0.0 |

No ORM, no validation library, no mapping library, no resilience library, no test framework.

---

## Testing

**No test project exists.** There are no unit or integration tests in this repository.

If tests are added in future, prefer `NUnit` + `Moq` with naming convention `{ClassUnderTest}Tests` / `{Method}_{Scenario}_{ExpectedOutcome}`.

---

## Environment Variables

Secrets are loaded from a `.env` file in `NewAI_CV_builder/` at startup via `DotNetEnv`. The `.env` file **must not be committed** — it contains real API keys.

| Variable | Description |
|---|---|
| `OPENAI_API_KEY` | OpenAI API key for `gpt-4.1-mini` calls |
| `CLAUDIUS_API_KEY` | Anthropic Claude API key for Claude model calls |
| `RESUME_DOWNLOAD_PATH` | Local folder path where the generated PDF is saved |
| `RESUME_FILE_NAME` | Output PDF file name (e.g. `MyResume.pdf`) |
| `BASE_RESUME_FILE_NAME` | Full path to the default JSON resume used when "Use Base Resume" is checked |
| `OPEN_AI_BASE_URL` | OpenAI base URL (`https://api.openai.com/`) |
| `DEVELOPER_LOOM_URL` | Loom video URL appended to Web/Desktop Developer proposals |
| `TECHNICAL_WRITER_LOOM_URL` | Loom video URL appended to Technical Writer proposals |
| `AI_DEVELOPER_LOOM_URL` | Loom video URL appended to AI Developer proposals |

---

## Git Workflow

- **Main branch:** `main` (protected — no direct pushes)
- **Branch naming:** `feature/{short-description}`, `fix/{short-description}`, `feat/{description}`
- **Commit style:** Descriptive sentence (e.g. `Add Loom video links to Upwork proposals`)
- **Before committing:** run `dotnet build` — do not commit if it fails
- **Pull requests:** always target `main`

---

## Build & Local Commands

```bash
# Restore and build
dotnet restore
dotnet build

# Run the app
dotnet run --project NewAI_CV_builder/NewAI_CV_builder.csproj

# Install Playwright browsers (first time or after Playwright version update)
pwsh NewAI_CV_builder/bin/Debug/net10.0-windows/playwright.ps1 install chromium
```

> The `.env` file must exist in `NewAI_CV_builder/` before running. Fill in all required keys.

---

## MCP Integrations

| Integration | Purpose |
|---|---|
| GitHub | Read issues, create PRs, review diffs |
| Gmail | Summarise threads, draft replies (approval required before send) |
| Google Calendar | Schedule meetings, check availability |

Integration configs live under `integrations/` (azure-devops, github, jira, slack).

> Claude must never send emails, post Slack messages, or merge PRs without explicit user approval in the session.

---

## Claude Behaviour for This Project

### Always do
- Read the full context of a file before editing it
- Keep all API calls async — use `ConfigureAwait(false)` in `Services/` and `Utilities/`
- Use `Serilog` for all logging — never `Console.WriteLine`
- Check the environment variable table when adding any new configuration value
- Keep UI callbacks on the correct thread via `this.Invoke(...)` when updating WinForms controls from a background task

### Never do
- Add new NuGet packages without explaining why
- Commit the `.env` file — it contains real API keys
- Use `.Result` or `.Wait()` on async tasks
- Introduce Clean Architecture layers, CQRS, or a DI container without explicit discussion — this is intentionally a simple WinForms app

### When you are unsure
- Ask before modifying the sample proposal text inside `UpworkPromptTemplates.cs` — those examples are intentional reference material
- Ask before changing AI model identifiers (`gpt-4.1-mini`, `claude-sonnet-4-5-20250929`) — the user may want to control these explicitly

---

*Last updated: 2026-04-23*  
*Maintained by: bigboybamo (Olabamiji Oyetubo)*
