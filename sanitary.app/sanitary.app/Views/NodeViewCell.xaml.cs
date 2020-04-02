using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodeViewCell : ViewCell
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(NodeViewCell),
        propertyChanged: (bindable, oldItem, newItem) =>
        {
            if (bindable != null)
            {
                ((NodeViewCell)bindable).Command = newItem as ICommand;
            }
        });

        public ICommand Command
        {
            get => GetValue(CommandProperty) as ICommand;
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty DeleteCommandProperty = BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(NodeViewCell),
        propertyChanged: (bindable, oldItem, newItem) =>
        {
            if (bindable != null)
            {
                ((NodeViewCell)bindable).DeleteCommand = newItem as ICommand;
            }
        });

        public ICommand DeleteCommand
        {
            get => GetValue(DeleteCommandProperty) as ICommand;
            set => SetValue(DeleteCommandProperty, value);
        }

        public NodeViewCell()
        {
            InitializeComponent();
        }
    }
}