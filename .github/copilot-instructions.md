# Copilot Instructions

## Output Rules
- Return **only modified code** by default. Do not repeat unchanged code.
- Keep responses concise and implementation-focused.
- When modifying files, include the target file path for each code block.
- Do not include placeholder comments like `// existing code...` inside modified blocks unless requested.

## Change Safety
- Preserve existing behavior unless the prompt explicitly asks to change it.
- Prefer minimal, surgical diffs over broad refactors.
- Do not rename public APIs, files, or classes unless requested.
- Do not introduce breaking changes without explicitly calling them out.

## .NET / C# Conventions
- Target `.NET 10` and latest C# language features when appropriate.
- Use nullable reference types correctly; avoid suppressions unless necessary.
- Use `async/await` end-to-end; avoid blocking calls (`.Result`, `.Wait()`).
- Pass `CancellationToken` through async flows where relevant.
- Use dependency injection instead of service locators or static state.
- Add logging with structured templates (`logger.LogInformation("Processed {ItemId}", itemId)`).

## Worker Service Guidelines
- For long-running background tasks, implement using `BackgroundService`.
- Keep `ExecuteAsync` non-blocking and cancellation-aware.
- Use scoped dependencies correctly inside background loops (`IServiceScopeFactory` when needed).
- Handle transient failures with retries/backoff where appropriate.

## Blazor Guidelines
- Prefer Blazor patterns and idioms over MVC/Razor Pages patterns.
- Keep UI state explicit and predictable.
- Use DI for services and keep component code-behind clean.
- Avoid heavy logic in `.razor` markup; move to code-behind or services.

## Testing & Validation
- Add or update tests for changed behavior when tests exist nearby.
- Keep test changes minimal and focused on the modified behavior.
- Ensure code compiles after edits; avoid speculative changes.

## Style
- Follow existing project naming and file organization.
- Prefer clear names over abbreviations.
- Avoid unnecessary comments; code should be self-explanatory.
- Keep methods small and single-purpose where practical.