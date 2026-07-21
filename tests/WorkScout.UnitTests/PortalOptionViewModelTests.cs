using JobSearchApp.Models;
using JobSearchApp.ViewModels;

namespace WorkScout.UnitTests;

[TestFixture]
public class PortalOptionViewModelTests
{
    [Test]
    public void IsSelected_WhenChanged_RaisesSelectionChangedWithPortalId()
    {
        var viewModel = new PortalOptionViewModel(new Portal
        {
            Id = 42,
            Name = "Test portal",
            IsSelected = false
        });
        (int Id, bool IsSelected)? received = null;
        viewModel.SelectionChanged += (id, isSelected) => received = (id, isSelected);

        viewModel.IsSelected = true;

        Assert.That(received, Is.EqualTo((42, true)));
    }
}
