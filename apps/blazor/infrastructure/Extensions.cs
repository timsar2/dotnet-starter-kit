﻿using Blazored.LocalStorage;
using FSH.Blazor.Infrastructure.Auth;
using FSH.Blazor.Infrastructure.Auth.Jwt;
using Infrastructure.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System.Globalization;

namespace FSH.Blazor.Infrastructure;
public static class Extensions
{
    private const string ClientName = "FullStackHero.API";
    public static IServiceCollection AddClientServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMudServices(configuration =>
        {
            configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            configuration.SnackbarConfiguration.HideTransitionDuration = 100;
            configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
            configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
            configuration.SnackbarConfiguration.ShowCloseIcon = false;
        });
        services.AddBlazoredLocalStorage();
        services.AddAuthentication(config);
        services.AddTransient<IApiClient, ApiClient>();
        services.AddHttpClient(ClientName, client =>
        {
            client.DefaultRequestHeaders.AcceptLanguage.Clear();
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
            client.BaseAddress = new Uri("https://localhost:7000/");
        })
           .AddHttpMessageHandler<JwtAuthenticationHeaderHandler>()
           .Services
           .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ClientName));

        return services;

    }
}