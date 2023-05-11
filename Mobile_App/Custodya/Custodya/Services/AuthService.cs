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
        public static FirebaseAuthClient Client { get; } = new FirebaseAuthClient(config);
        public static UserCredential UserCreds { get; set; }
    }

}
