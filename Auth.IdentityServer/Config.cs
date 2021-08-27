// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using Auth.IdentityServer.Settings;

namespace Auth.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("Auth.oscgre.api", "API de Sistema OSCGRE")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "Auth.oscgre.spa",
                    ClientName = "SPA de Generico",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,


                    // where to redirect to after login
                    RedirectUris = {  },
                    AllowOfflineAccess = true,
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Auth.oscgre.api"
                    },
                    AllowedCorsOrigins = {  }
                }
            };

        public static IEnumerable<Client> GetClients(ClientsSettings settings) 
        {
            var clients = Clients;
            var targetClient = clients.First();
            foreach (var clientUrl in settings.SPAUrls)
            {
                targetClient.AllowedCorsOrigins.Add(clientUrl);
                var redirectUri = $"{clientUrl}/oidc-callback";
                targetClient.RedirectUris.Add(redirectUri);
            }

            return clients;
        }

    }
}