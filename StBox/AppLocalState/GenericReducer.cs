using Newtonsoft.Json;
using Plugin.Settings;
using System.Threading.Tasks;

namespace StBox.AppLocalState
{
    public abstract class GenericReducer<TModel>
        where TModel : class, new()
    {
        public abstract string StateKey { get; protected set; }

        public TModel State { get; private set; }

        public Task<TModel> UpdateStateAsync(TModel source) =>
            Task<TModel>.Run(async () =>
            {
                string jState = string.Empty;

                if (source == null)
                {
                    jState = JsonConvert.SerializeObject(new TModel());
                }
                else
                {
                    jState = JsonConvert.SerializeObject(source);
                }

                CrossSettings.Current.AddOrUpdateValue(StateKey, jState);

                await ReadStateAsync();

                return State;
            });

        public Task<TModel> ReadStateAsync() =>
            Task.Run(() =>
            {
                string jState = CrossSettings.Current.GetValueOrDefault(StateKey, string.Empty);
                TModel state;

                if (string.IsNullOrEmpty(jState))
                {
                    state = new TModel();
                }
                else
                {
                    state = JsonConvert.DeserializeObject<TModel>(jState);
                }

                State = state;

                return State;
            });
    }
}
