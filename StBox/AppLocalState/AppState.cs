using StBox.Environment.Exceptions;
using StBox.Locator;

namespace StBox.AppLocalState
{
    public static class AppState
    {
        public static T GetStateReducer<T>()
            where T : class, IStateReducer
        {
            if (ViewModelLocator.IsRegistred<T>())
            {
                return ViewModelLocator.Resolve<T>();
            }
            else
            {
                throw new StBoxException($"The {typeof(T).FullName} is not registered as `app state reducer`. " +
                    $"Check does this type provided through your `{typeof(DependenciesProvider).Name}`.");
            }
        }
    }
}
