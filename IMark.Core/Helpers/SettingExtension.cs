using IMark.Core.Models.Common;
using IMark.Data.Models.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;
namespace IMark.Core.Helpers
{
   public class SettingExtension
    {
        public static string GetStringForKey(string key)
        {
            var value = Preferences.Get(key, string.Empty);
            return value;
        }
        public static void AddOrUpdateStringForKey(string key, string value)
        {
            try
            {
                Preferences.Set(key, value);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings value: {ex}");
            }
        }
        public static T GetClassForKey<T>(string key, T @default) where T : class
        {
            string serialized = Preferences.Get(key, string.Empty);
            T result = @default;

            try
            {
                JsonSerializerSettings serializeSettings = GetSerializerSettings();
                result = JsonConvert.DeserializeObject<T>(serialized);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing settings value: {ex}");
            }
            return result;
        }


        public static void AddClassForKey<T>(string key, T obj) where T : class
        {
            try
            {
                JsonSerializerSettings serializeSettings = GetSerializerSettings();
                string serialized = JsonConvert.SerializeObject(obj, serializeSettings);
                Preferences.Set(key, serialized);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing settings value: {ex}");
            }
        }

        public static void DeleteClassForKey(string key)
        {
            try
            {
                Preferences.Remove(key);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing settings value: {ex}");
            }
        }
        public static void DeletePreferenceKey(string key)
        {
            try
            {
                Preferences.Remove(key);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing settings value: {ex}");
            }
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public static string UserEmail
        {
            get => Preferences.Get(nameof(UserEmail), string.Empty);
            set => Preferences.Set(nameof(UserEmail), value);
        }
        public static string AccessToken
        {
            get => Preferences.Get(nameof(AccessToken), string.Empty);
            set => Preferences.Set(nameof(AccessToken), value);
        }
        public static string CheckoutId
        {
            get => Preferences.Get(nameof(CheckoutId), string.Empty);
            set => Preferences.Set(nameof(CheckoutId), value);
        }
        public static string AddressId
        {
            get => Preferences.Get(nameof(AddressId), string.Empty);
            set => Preferences.Set(nameof(AddressId), value);
        }
        public static List<LineItem> CartList
        {
            get => GetClassForKey<List<LineItem>>(nameof(CartList), new List<LineItem>());
            set => AddClassForKey<List<LineItem>>(nameof(CartList), value);
        }
        public static UserSettingData UserSetting
        {
            get => GetClassForKey<UserSettingData>(nameof(UserSetting),new UserSettingData());
            set => AddClassForKey<UserSettingData>(nameof(UserSetting), value);
        }
        public static List<string> CustomizeURLS
        {
            get => GetClassForKey<List<string>>(nameof(CustomizeURLS), new List<string>());
            set => AddClassForKey<List<string>>(nameof(CustomizeURLS), value);
        }
    }
}
