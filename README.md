# Northwind Insurance: adjuster-desktop

Windows desktop app that claims adjusters use to review and approve claims.

- Adjusters can approve claims up to $10,000.
- Larger claims go to a supervisor.

```powershell
dotnet build AdjusterDesktop.sln
dotnet test AdjusterDesktop.sln   # unit, integration, and WPF end-to-end UI tests
dotnet run --project src/AdjusterDesktop
```
