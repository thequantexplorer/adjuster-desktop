# Northwind Insurance: adjuster-desktop

Windows desktop app that claims adjusters use to review and approve claims.

- Adjusters can approve claims up to $10,000.
- Larger claims go to a supervisor.

```powershell
dotnet build AdjusterDesktop.sln
dotnet test tests/AdjusterDesktop.Core.Tests
dotnet run --project src/AdjusterDesktop
```
