using CommonFW.Domain.Model.Payment;
using DAL.Entities;
using DAL.Models;
using DAL.Models.ConDao;
using DAL.Models.Customer;
using DAL.Models.GatePermission;
using DAL.Models.Invoice;
using DAL.Models.partner;
using DAL.Models.Payoo;
using DAL.Models.Product;
using DAL.Models.Promotion;
using DAL.Models.Report;
using DAL.Models.SoatVe;
using DAL.Models.Ticket;
using DAL.Models.TicketOrder;
using DAL.Models.TokenMisa;
using DAL.Models.UserInfo;
using DAL.Models.WebHookSePay;
using DAL.Models.Zalo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class EntityDataContext : DbContext
    {
        public EntityDataContext(DbContextOptions<EntityDataContext> options) : base(options)
        {

        }

        /*============ SQL table ======================================*/

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<RoleInfo> RoleInfo { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuRole> MenuRole { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<CustomerType> CustomerType { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetail { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<TicketOrder> TicketOrder { get; set; }
        public DbSet<TicketOrderSubNum> TicketOrderSubNum { get; set; }
        public DbSet<LoaiIn> LoaiIn { get; set; }
        public DbSet<PaymentInfo> PaymentInfo { get; set; }
        public DbSet<SaleChannel> SaleChannel { get; set; }
        public DbSet<ResultCode> ResultCode { get; set; }
        public DbSet<GateList> GateList { get; set; }
        public DbSet<AccountZNS> AccountZNS { get; set; }
        public DbSet<AccountChanelZNS> AccountChanelZNS { get; set; }
        public DbSet<ReportInLaiModel> ReportInLaiModel { get; set; }

        /*============ for Excute SQL query=============================== */

        public DbSet<UserInfoGridModel> UserInfoGridModel { get; set; }
        public DbSet<ProductGridModel> ProductGridModel { get; set; }
        public DbSet<CustomerGridModel> CustomerGridModel { get; set; }
        public DbSet<InvoiceSearchResultModel> InvoiceSearchResultModel { get; set; }
        public DbSet<TicketGridModel> TicketGridModel { get; set; }
        public DbSet<SaleHistoryGridModel> SaleHistoryGridModel { get; set; }
        public DbSet<SaleReportGridModel> SaleReportGridModel { get; set; }
        public DbSet<ResOrderInfoDto> ResOrderInfoDto { get; set; }
        public DbSet<ResPaymentInfoModel> ResPaymentInfoModel { get; set; }
        public DbSet<OrderGridModel> OrderGridModel { get; set; }
        public DbSet<SubOrderPrintModel> SubOrderPrintModel { get; set; }
        public DbSet<PrintPdfOrderModel> PrintPdfOrderModel { get; set; }
        public DbSet<ReportSaleCounterModel> ReportSaleCounterModel { get; set; }
        public DbSet<SoatVeReportGridModel> SoatVeReportGridModel { get; set; }
        public DbSet<TicketTypeRPGridModel> TicketTypeRPGridModel { get; set; }
        public DbSet<TicketOrderModel> TicketOrderModel { get; set; }
        public DbSet<ResOrderInfoSendZaloDto> ResOrderInfoSendZaloDto { get; set; }
        public DbSet<ZaloNotiConfigModel> ZaloNotiConfigModel { get; set; }
        public DbSet<WebHookReceiveModel> WebHookReceiveModel { get; set; }
        public DbSet<ResComonGridModel> ResComonGridModel { get; set; }
        public DbSet<SubOrderCodeModel> SubOrderCodeModel { get; set; }
        public DbSet<ComboBoxModel> ComboBoxModel { get; set; }
        public DbSet<ScanResultModel> ScanResultModel { get; set; }
        public DbSet<DetailCheckTicketModel> DetailCheckTicketModel { get; set; }
        public DbSet<GateListModel> GateListModel { get; set; }
        public DbSet<GatePermissionGridModel> GatePermissionModel { get; set; }
        public DbSet<ResultSoatVeOfflineModel> ResultSoatVeOfflineModel { get; set; }
        public DbSet<StaffSaleCounterModel> StaffSaleCounterModel { get; set; }
        public DbSet<SaleTicketMisaStatusModel> SaleTicketMisaStatusModel { get; set; }
        public DbSet<ColumnChartModel> ColumnChartModel { get; set; }
        public DbSet<MisaConfigModel> MisaConfigModel { get; set; }
        public DbSet<TokenMisaModel> TokenMisaModel { get; set; }
        public DbSet<PartnerGridModel> PartnerGridModel { get; set; }
        public DbSet<PartnerModelViewModel> PartnerModelViewModel { get; set; }
        public DbSet<PromotionGridModel> PromotionGridModel { get; set; }
        public DbSet<InfoVoucherViewModel> InfoVoucherViewModel { get; set; }
        public DbSet<InfoPartnerViewModel> InfoPartnerViewModel { get; set; }
        public DbSet<SaleReportByPartnerGridModel> SaleReportByPartnerGridModel { get; set; }
        public DbSet<TicketGroupModel> TicketGroupModel { get;set;}
        public DbSet<TicketPricePolicyModel> TicketPricePolicyModel { get; set; }
        public DbSet<ReportSaleByTicketGridModel> ReportSaleByTicketGridModel { get; set; }
        public DbSet<ReportCheckinGridModel> ReportCheckinGridModel { get; set; }
        public DbSet<CheckinReportCounterModel> CheckinReportCounterModel { get; set; }
        //===== Soát vé =============

        public DbSet<HistoryInOutModel> HistoryInOutModel { get; set; }
        public DbSet<ReportBanVeByCustTypeGridModel> ReportBanVeByCustTypeGridModel { get; set; }
        public DbSet<WaitingPaymentOrderModel> WaitingPaymentOrderModel { get; set; }
        public DbSet<TicketUserModel> TicketUserModel { get; set; }
        
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleInfo>().HasKey(o => o.Code);
            modelBuilder.Entity<SubOrderPrintModel>().HasKey(o => o.SubId);
            modelBuilder.Entity<PrintPdfOrderModel>().HasKey(o => o.SubId);
            modelBuilder.Entity<TicketTypeRPGridModel>().HasKey(o => o.Id);
            modelBuilder.Entity<ResOrderInfoSendZaloDto>().HasKey(o => o.SubOrderCodeId);
            modelBuilder.Entity<ComboBoxModel>().HasKey(o => o.Value);
            modelBuilder.Entity<ScanResultModel>().HasKey(o => o.TicketCode);
            modelBuilder.Entity<DetailCheckTicketModel>().HasKey(o => o.TotalCheck);
            modelBuilder.Entity<GateListModel>().HasKey(o => o.GateCode);
            modelBuilder.Entity<GatePermissionGridModel>().HasKey(o => o.GateCode);
            modelBuilder.Entity<ResultSoatVeOfflineModel>().HasKey(o => o.SubId);
            modelBuilder.Entity<StaffSaleCounterModel>().HasKey(o => o.FullName);
            modelBuilder.Entity<SaleTicketMisaStatusModel>().HasKey(o => o.KyHieu);
            modelBuilder.Entity<ColumnChartModel>().HasKey(o => o.Thang);
            modelBuilder.Entity<MisaConfigModel>().HasKey(o => o.appId);
            modelBuilder.Entity<TokenMisaModel>().HasKey(o => o.TokenKey);
            modelBuilder.Entity<PartnerGridModel>().HasKey(o => o.Id);
            modelBuilder.Entity<InfoPartnerViewModel>().HasKey(o => o.PartnerCode);
            modelBuilder.Entity<InfoVoucherViewModel>().HasKey(o => o.VoucherCode);
            modelBuilder.Entity<PartnerModelViewModel>().HasKey(o => o.Id);
            modelBuilder.Entity<SaleReportByPartnerGridModel>().HasKey(o => o.PartnerCode);
            modelBuilder.Entity<TicketGroupModel>().HasKey(o => o.GroupCode);
            modelBuilder.Entity<ReportBanVeByCustTypeGridModel>().HasKey(o => o.STT);
            modelBuilder.Entity<WaitingPaymentOrderModel>().HasKey(o => o.OrderId);
            modelBuilder.Entity<CheckinReportCounterModel>().HasKey(o => o.TicketCode);
            modelBuilder.Entity<Area>().HasKey(o => o.Code);
        }
    }
}
