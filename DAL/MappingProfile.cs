using AutoMapper;
using DAL.Entities;
using DAL.Models.Invoice;
using DAL.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Invoice, InvoiceModel>();
            CreateMap<Branch, BranchModel>();
            CreateMap<Ticket, TicketModel>();
        }
    }
}
