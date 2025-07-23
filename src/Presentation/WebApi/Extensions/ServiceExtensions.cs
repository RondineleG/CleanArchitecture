﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using System;
using System.Collections.Generic;

namespace WebApi.Extensions;

public static class ServiceExtensions
{
    public static void AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(
            config =>
       {
           config.DefaultApiVersion = new ApiVersion(1, 0);
           config.AssumeDefaultVersionWhenUnspecified = true;
           config.ReportApiVersions = true;
       });
    }

    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            c =>
      {
          c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\CleanArchitecture.WebApi.xml");
          c.SwaggerDoc(
              "v1",
              new OpenApiInfo
              {
                  Version = "v1",
                  Title = "Clean Architecture - WebApi",
                  Description = "This Api will be responsible for overall data distribution and authorization.",
                  Contact =
                      new OpenApiContact
                      {
                          Name = "codewithmukesh",
                          Email = "hello@codewithmukesh.com",
                          Url = new Uri("https://codewithmukesh.com/contact"),
                      }
              });
          c.AddSecurityDefinition(
              "Bearer",
              new OpenApiSecurityScheme
              {
                  Name = "Authorization",
                  In = ParameterLocation.Header,
                  Type = SecuritySchemeType.ApiKey,
                  Scheme = "Bearer",
                  BearerFormat = "JWT",
                  Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
              });
          c.AddSecurityRequirement(
              new OpenApiSecurityRequirement
              {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference =
                                new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer",
                                        },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
              });
      });
    }
}