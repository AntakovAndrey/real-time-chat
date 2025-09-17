namespace ChatClientWpf.ViewModels;

public class ChatViewModel : BaseViewModel
{
    private readonly MainViewModel _mainViewModel;

    public ChatViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }
}
