﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SklepEntities : DbContext
    {
        public SklepEntities()
            : base("name=SklepEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Dostawcy> Dostawcy { get; set; }
        public virtual DbSet<Faktury> Faktury { get; set; }
        public virtual DbSet<Kaczki> Kaczki { get; set; }
        public virtual DbSet<Kraje> Kraje { get; set; }
        public virtual DbSet<Odbiorcy> Odbiorcy { get; set; }
        public virtual DbSet<Pracownicy> Pracownicy { get; set; }
    }
}