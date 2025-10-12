using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChatClientWpf.ViewModels;

namespace ChatClientWpf.Views
{
    public partial class ChatAreaView : UserControl
    {
        public ChatAreaView()
        {
            InitializeComponent();
            Loaded += ChatAreaView_Loaded;
        }

        private void ChatAreaView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ChatAreaViewModel viewModel)
            {
                viewModel.ScrollToBottom += ScrollToBottom;
            }
            ScrollToBottom(sender, e);
        }

        private void ScrollToBottom(object sender, System.EventArgs e)
        {
            MessagesScrollViewer.ScrollToEnd();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.KeyboardDevice.IsKeyDown(Key.LeftShift))
            {
                if (DataContext is ChatAreaViewModel viewModel && viewModel.SendMessageCommand.CanExecute(null))
                {
                    viewModel.SendMessageCommand.Execute(null);
                    e.Handled = true;
                    viewModel.NewMessage = String.Empty;
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ChatAreaViewModel viewModel && viewModel.SendMessageCommand.CanExecute(null))
            {
                viewModel.SendMessageCommand.Execute(null);
                e.Handled = true;
                viewModel.NewMessage = String.Empty;
            }
        }
    }
}