using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CobimExplorer.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// object -> Dictionary<string, object> 변환 메서드 
        /// </summary>
        #region ToDictionary

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            return dictionary;
        }

        #endregion ToDictionary
    }
}
