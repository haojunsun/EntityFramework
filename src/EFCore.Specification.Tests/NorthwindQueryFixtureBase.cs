// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class NorthwindQueryFixtureBase
    {
        private DbContextOptions _options;

        public abstract DbContextOptions BuildOptions(IServiceCollection additionalServices = null);

        public virtual NorthwindContext CreateContext(
            QueryTrackingBehavior queryTrackingBehavior = QueryTrackingBehavior.TrackAll,
            bool enableFilters = false)
        {
            EnableFilters = enableFilters;

            return new NorthwindContext(
                _options
                ?? (_options = new DbContextOptionsBuilder(BuildOptions())
                    .ConfigureWarnings(w => w.Log(CoreEventId.IncludeIgnoredWarning)).Options),
                queryTrackingBehavior);
        }

        private bool EnableFilters { get; set; }

        protected virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>();

            modelBuilder.Entity<Employee>(
                e =>
                    {
                        e.Ignore(em => em.Address);
                        e.Ignore(em => em.BirthDate);
                        e.Ignore(em => em.Extension);
                        e.Ignore(em => em.HireDate);
                        e.Ignore(em => em.HomePhone);
                        e.Ignore(em => em.LastName);
                        e.Ignore(em => em.Notes);
                        e.Ignore(em => em.Photo);
                        e.Ignore(em => em.PhotoPath);
                        e.Ignore(em => em.PostalCode);
                        e.Ignore(em => em.Region);
                        e.Ignore(em => em.TitleOfCourtesy);

                        e.HasOne(e1 => e1.Manager).WithMany().HasForeignKey(e1 => e1.ReportsTo);
                    });

            modelBuilder.Entity<Product>(
                e =>
                    {
                        e.Ignore(p => p.CategoryID);
                        e.Ignore(p => p.QuantityPerUnit);
                        e.Ignore(p => p.ReorderLevel);
                        e.Ignore(p => p.UnitsOnOrder);
                    });

            modelBuilder.Entity<Order>(
                e =>
                    {
                        e.Ignore(o => o.Freight);
                        e.Ignore(o => o.RequiredDate);
                        e.Ignore(o => o.ShipAddress);
                        e.Ignore(o => o.ShipCity);
                        e.Ignore(o => o.ShipCountry);
                        e.Ignore(o => o.ShipName);
                        e.Ignore(o => o.ShipPostalCode);
                        e.Ignore(o => o.ShipRegion);
                        e.Ignore(o => o.ShipVia);
                        e.Ignore(o => o.ShippedDate);
                    });

            modelBuilder.Entity<OrderDetail>(e => { e.HasKey(od => new { od.OrderID, od.ProductID }); });

            if (EnableFilters)
            {
                new NorthwindContext().ConfigureFilters(modelBuilder);
            }
        }
    }
}
