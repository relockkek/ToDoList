using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ToDoList1.Models;

namespace ToDoList1;

public partial class TagsPage : ContentPage
{
    private readonly DB db = new();
    private ObservableCollection<string> tags = new ObservableCollection<string>();
    
    private List<string> tempTags = new List<string>();
    
    public TagsPage(Tag tags)
	{
		InitializeComponent();
        LoadTagsAsync();
        var editButton = this.FindByName<Button>("EditButton");

        if (editButton == null)
        {
            var toolbarItem = new ToolbarItem
            {
                Text = "Редактировать",
                Priority = 0
            };
            toolbarItem.Clicked += EditTag_Clicked;

            this.ToolbarItems.Add(toolbarItem);
        }
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
    private async void EditTag_Clicked(object sender, EventArgs e)
    {
        tempTags = new List<string>(tags);

        var result = await DisplayAlert("Редактирование тегов",
            "Вы хотите редактировать теги?", "Да", "Нет");
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

    private void CancelButton_Clicked(object sender, EventArgs e)
    {

    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {

    }
}