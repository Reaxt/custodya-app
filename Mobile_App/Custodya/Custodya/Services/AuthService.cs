using Firebase.Auth.Providers;
using Firebase.Auth;
using Custodya.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Services
{
    public static class AuthService
    {
        public const string CACHE_EMAIL = "UserEmail";
        public const string CACHE_PASSWORD = "UserPassword";

        // Configure...
        private static FirebaseAuthConfig config = new FirebaseAuthConfig
        {
            ApiKey = App.Settings.FireBaseAPIKey,
            AuthDomain = App.Settings.FireBase_Authorized_Domain,
            Providers = new FirebaseAuthProvider[]
            {
                // Add and configure individual providers
                new EmailProvider()
            },
        };
        // ...and create your FirebaseAuthClient
        internal static FirebaseAuthClient Client { get; } = new FirebaseAuthClient(config);
        internal static UserCredential UserCreds { get; set; }
        internal static bool HasCachedCredentials()
        {
            var cachedEmail = Preferences.Get(CACHE_EMAIL, null);
            var cachedPassword = Preferences.Get(CACHE_PASSWORD, null);
            return (cachedEmail != null && cachedPassword != null);
        }
        internal static (string username, string password)GetCachedCredentials()
        {
            if(!HasCachedCredentials())
            {
                return (null, null);
            } else
            {
                return (Preferences.Get(CACHE_EMAIL, null), Preferences.Get(CACHE_PASSWORD, null));
            }
        }
        internal static void SetCachedCredentials(string username, string password)
        {
            Preferences.Set(CACHE_EMAIL, username);
            Preferences.Set(CACHE_PASSWORD, password);
        }

        internal static void ResetCachedCredentials()
        {
            Preferences.Remove(CACHE_EMAIL);
            Preferences.Remove(CACHE_PASSWORD);
        }
    }

}
