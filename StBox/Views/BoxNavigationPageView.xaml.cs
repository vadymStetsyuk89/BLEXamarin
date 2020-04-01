using StBox.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StBox.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoxNavigationPageView : NavigationPage
    {
        public BoxNavigationPageView()
        {
            InitializeComponent();
        }

        public BoxNavigationPageView(Page root)
            : base(root)
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Page lastPageInTheStack = Pages.LastOrDefault<Page>();

            /// If navigation stack contains only one page - dispose and pop that page (without navigation service)
            if (Pages.Count() <= 1)
            {
                if (lastPageInTheStack.BindingContext != null 
                    && lastPageInTheStack.BindingContext is ViewModelBase viewModel)
                {
                    viewModel.Dispose();
                }
                return base.OnBackButtonPressed();
            }

            if (lastPageInTheStack != null &&
                lastPageInTheStack.BindingContext != null &&
                lastPageInTheStack.BindingContext is ViewModelBase &&
                (lastPageInTheStack.BindingContext as ViewModelBase).BackCommand != null)
            {
                ((ViewModelBase)lastPageInTheStack.BindingContext).BackCommand.Execute(null);

                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
    }
}