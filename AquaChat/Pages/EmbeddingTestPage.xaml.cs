using AquaChat.Utils;
using CommunityToolkit.Maui.Alerts;

namespace AquaChat.Pages;

public partial class EmbeddingTestPage : ContentPage
{
    private const string MemoryCollectionName = "testCollection";

    public EmbeddingTestPage()
	{
		InitializeComponent();
	}

    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        var kernel = SemanticKernelHolder.GetKernel();

        if (kernel is null)
        {
            await Toast.Make("kernel init failed").Show();
            return;
        }

        var inputEditorText = InputEditor.Text;
        if (string.IsNullOrWhiteSpace(inputEditorText))
        {
            await Toast.Make("input something").Show();
            return;
        }

        await kernel.Memory.SaveInformationAsync(MemoryCollectionName, inputEditorText, Guid.NewGuid().ToString());
    }

    private async void SearchButton_OnClicked(object? sender, EventArgs e)
    {
        var kernel = SemanticKernelHolder.GetKernel();

        if (kernel is null)
        {
            await Toast.Make("kernel init failed").Show();
            return;
        }

        var searchText = SearchEntry.Text;
        if (string.IsNullOrWhiteSpace(searchText))
        {
            await Toast.Make("input something").Show();
            return;
        }

        var result = await kernel.Memory.SearchAsync(MemoryCollectionName, searchText, 10).ToListAsync();
        if (result.Count == 0)
        {
            OutputEditor.Text = "No Result";
        }
        var aggregate = result.Select(memoryQueryResult => memoryQueryResult.Metadata.Text + "\n" + "score:" + memoryQueryResult.Relevance)
            .Aggregate((acc, s) => acc + "\n\n" + s);

        OutputEditor.Text = aggregate;
    }

    private async void ClearAll_OnClicked(object? sender, EventArgs e)
    {
        var memoryStore = SemanticKernelHolder.GetMemoryStore();
        if (memoryStore is null)
        {
            return;
        }
        await memoryStore.DeleteCollectionAsync(MemoryCollectionName);
    }
}