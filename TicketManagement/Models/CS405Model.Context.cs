﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TicketManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CS405Entities2 : DbContext
    {
        public CS405Entities2()
            : base("name=CS405Entities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblticket> tbltickets { get; set; }
        public virtual DbSet<tblaccount> tblaccounts { get; set; }
        public virtual DbSet<tblequipment> tblequipments { get; set; }
    }
}
