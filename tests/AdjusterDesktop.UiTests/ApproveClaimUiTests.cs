using System.Reflection;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using Xunit;

[Trait("Category", "E2E")]
public sealed class ApproveClaimUiTests : IDisposable
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(15);

    private readonly Application _app;
    private readonly UIA3Automation _automation = new();
    private readonly Window _window;

    public ApproveClaimUiTests()
    {
        var exe = Path.GetFullPath(typeof(ApproveClaimUiTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(a => a.Key == "AppExePath").Value!);
        Assert.True(File.Exists(exe), $"App not built: {exe}");

        _app = Application.Launch(exe);
        _window = _app.GetMainWindow(_automation, Timeout)
            ?? throw new InvalidOperationException("Main window did not appear.");
    }

    [Fact]
    public void ApprovingAClaimOfAnUnlistedTypeDoesNotCrashTheApp()
    {
        var grid = WaitFor("ClaimsGrid").AsGrid();
        var row = Retry.WhileNull(
            () => grid.Rows.FirstOrDefault(r => r.Cells[0].Value == "CLM-2004"),
            Timeout).Result ?? throw new InvalidOperationException("CLM-2004 row not found.");

        Assert.Equal("Water damage", row.Cells[2].Value);
        row.Select();

        WaitFor("ApproveButton").AsButton().Invoke();

        var status = WaitFor("StatusText");
        var text = Retry.WhileEmpty(() => status.Name, Timeout).Result;
        Assert.StartsWith("CLM-2004 ", text);
        Assert.False(_app.HasExited, "The app exited after approving CLM-2004.");

        var updatedRow = grid.Rows.Single(r => r.Cells[0].Value == "CLM-2004");
        Assert.NotEqual("InReview", updatedRow.Cells[4].Value);
    }

    private AutomationElement WaitFor(string automationId) =>
        Retry.WhileNull(() => _window.FindFirstDescendant(cf => cf.ByAutomationId(automationId)), Timeout).Result
            ?? throw new InvalidOperationException($"Element '{automationId}' not found.");

    public void Dispose()
    {
        _app.Close();
        _app.Dispose();
        _automation.Dispose();
    }
}
