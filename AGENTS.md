# Northwind adjuster-desktop: agent instructions

- Windows only. .NET 8 WPF app in `src/AdjusterDesktop`; business rules in `src/AdjusterDesktop.Core`.
- Build: `dotnet build AdjusterDesktop.sln`. Tests: `dotnet test AdjusterDesktop.sln` runs unit + integration tests (`tests/AdjusterDesktop.Core.Tests`) and the FlaUI end-to-end UI test (`tests/AdjusterDesktop.UiTests`, needs an interactive desktop).
- Run the app: `dotnet run --project src/AdjusterDesktop`.
- Money is integer cents. The adjuster approval limit is $10,000 (`ApprovalPolicy.AdjusterLimitCents`).
- UI changes and approval-rule changes must be tested end to end in the running app: select a claim, click "Approve selected claim", and read the status line.
- Every bug fix ships with a regression test.
