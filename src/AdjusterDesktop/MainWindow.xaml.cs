using System.Windows;
using AdjusterDesktop.Core;

namespace AdjusterDesktop;

public partial class MainWindow : Window
{
    private readonly ClaimQueue _queue = ClaimQueue.Sample();

    public MainWindow()
    {
        InitializeComponent();
        ClaimsGrid.ItemsSource = _queue.All;
    }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (ClaimsGrid.SelectedItem is not Claim claim)
        {
            StatusText.Text = "Select a claim first.";
            return;
        }
        var result = _queue.Approve(claim.Id);
        StatusText.Text = result == ClaimStatus.Approved
            ? $"{claim.Id} approved."
            : $"{claim.Id} is over your limit and was sent to a supervisor.";
        ClaimsGrid.Items.Refresh();
    }
}
