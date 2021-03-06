﻿using System;
using System.Collections.Generic;

using BR.MadenIlan.Auth.Services;

using IdentityServer4;
using IdentityServer4.Models;


namespace BR.MadenIlan.Auth
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_product_api")
            {
                Scopes={"api_product_fullpermission"},
                ApiSecrets=new []{ new Secret("product_api_secret".Sha256())}
                },
            new ApiResource("resource_photo_api")
            {
                Scopes={ "api_photo_fullpermission" },
                ApiSecrets=new []{ new Secret("photo_api_secret".Sha256())}
            },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api_product_fullpermission","PRODUCT API ICIN TUM IZINLER"),
                new ApiScope("api_photo_fullpermission","PHOTO API ICIN TUM IZINLER"),
               new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
            };
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "MobileClient_CC",
                    ClientName = "Mobile Client CC",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("madenilan_mobile_client_secret".Sha256()) },

                    AllowedScopes = { IdentityServerConstants.LocalApi.ScopeName }
                },

                new Client
                {
                    ClientId = "MobileClient_ROP",
                    ClientName = "Mobile Client ROP",

                    ClientSecrets = { new Secret("madenilan_mobile_client_secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api_product_fullpermission",
                        "api_photo_fullpermission",
                        "Roles"
                    },
                    AccessTokenLifetime =(int)TimeSpan.FromMinutes(1).TotalSeconds,
                    RefreshTokenUsage=TokenUsage.ReUse,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)TimeSpan.FromMinutes(2).TotalSeconds
                }
            };
    }
}