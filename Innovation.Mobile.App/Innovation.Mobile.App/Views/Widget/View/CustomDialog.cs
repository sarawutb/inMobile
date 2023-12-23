using Xamarin.Forms;

public class CustomDialog : ContentView
{
    public CustomDialog()
    {
        // Create the dialog's content here, for example, labels, buttons, etc.
        var content = new StackLayout
        {
            Children = {
                new Label { Text = "This is a custom dialog." },
                new Button { Text = "Close", Command = new Command(CloseDialog) }
            }
        };

        Content = content;
        BackgroundColor = Color.White; // Optional: Set background color
        Padding = new Thickness(20);   // Optional: Set padding
    }

    private void CloseDialog()
    {
        // Close the dialog by removing it from the page
        (Parent as Page)?.Navigation.PopModalAsync();
    }
}
