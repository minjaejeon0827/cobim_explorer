using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CobimNet;

namespace CobimExplorer.Settings
{
    public class SettingPartBase : BindableBase
    {
        [JsonIgnore]
        public SortedDictionary<string, object> Properties { get; protected set; } = new SortedDictionary<string, object>();

        protected void SetProperty(object value, [CallerMemberName] string key = null)
        {
            this.Properties[key] = value;
            this.Changed(key);
        }
    }
}
