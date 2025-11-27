using ToDoList1.Models;

namespace ToDoList1;

public partial class TagsPage : ContentPage
{
    private readonly DB db = new();
    public TagsPage(Tag tags)
	{
		InitializeComponent();
        LoadTagsAsync();
	}
    async Task LoadTagsAsync()
    {
        try
        {
            var tags = await db.GetTagsAsync();
            TagsCollection.ItemsSource = tags;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }
    private async void AddTag_Clicked(object sender, EventArgs e)
    {
        string name = TagNameEntry.Text;
        string color = TagColorEntry.Text;

        if (!string.IsNullOrWhiteSpace(name))
        {
            try
            {
                var tags = await db.GetTagsAsync();
                int newId = tags.Count > 0 ? tags.Max(t => t.Id) + 1 : 1;
                var newTag = new Tag
                {
                    Id = newId,
                    Name = name,
                    Color = string.IsNullOrWhiteSpace(color) ? "#3498db" : color,
                };
                await db.AddTagAsync(newTag);
                await LoadTagsAsync();

                TagNameEntry.Text = "";
                TagColorEntry.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "ОК");
            }
        }
    }
    private void EditTag_Clicked(object sender, EventArgs e)
    {
        //TagNameEntry.Text = Tag.Name;
    }
    private async void DeleteTag_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var tag = (Tag)button.CommandParameter;

        var confirm = await DisplayAlert("Удаление", $"Удалить тэг '{tag.Name}'?", "Да", "Нет");
        if (confirm)
        {
            await db.DeleteTagAsync(tag.Id);
        }
    }
}