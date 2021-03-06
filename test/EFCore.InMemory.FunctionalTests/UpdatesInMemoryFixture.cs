﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.TestModels.UpdatesModel;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore
{
    public class UpdatesInMemoryFixture : UpdatesFixtureBase<InMemoryTestStore>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContextOptionsBuilder _optionsBuilder;

        public UpdatesInMemoryFixture()
        {
            _serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddSingleton(TestModelSource.GetFactory(OnModelCreating))
                .BuildServiceProvider();

            _optionsBuilder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(nameof(UpdatesInMemoryFixture))
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(_serviceProvider);
        }

        public override InMemoryTestStore CreateTestStore()
            => InMemoryTestStore.CreateScratch(
                _serviceProvider,
                nameof(UpdatesInMemoryFixture),
                () =>
                    {
                        using (var context = new UpdatesContext(_optionsBuilder.Options))
                        {
                            UpdatesModelInitializer.Seed(context);
                        }
                    });

        public override UpdatesContext CreateContext(InMemoryTestStore testStore)
            => new UpdatesContext(_optionsBuilder.Options);
    }
}
