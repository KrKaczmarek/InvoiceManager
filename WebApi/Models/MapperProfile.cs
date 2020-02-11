using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
namespace WebApi.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            EmployeeMap();
            InvoiceMap();
            SupplierMap();
            RecipientMap();
            DuckMap();
            CountryMap();

        }

        private void EmployeeMap()
        {
            CreateMap<Pracownicy, EmployeeViewModel>()
            .ForMember(d => d.EmployeeId, opt => opt.MapFrom(src => src.Pracownik_id))
            .ForMember(d => d.EmployeeName, opt => opt.MapFrom(src => src.Imie))
            .ForMember(d => d.EmployeeSalary, opt => opt.MapFrom(src => src.Pensja))
            .ForMember(d => d.EmployeeSurname, opt => opt.MapFrom(src => src.Nazwisko))
            .ForMember(d => d.Gender, opt => opt.MapFrom(src => src.Plec))
            .ReverseMap()
            .ForMember(d => d.Pracownik_id, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(d => d.Imie, opt => opt.MapFrom(src => src.EmployeeName))
            .ForMember(d => d.Pensja, opt => opt.MapFrom(src => src.EmployeeSalary))
            .ForMember(d => d.Nazwisko, opt => opt.MapFrom(src => src.EmployeeSurname))
            .ForMember(d => d.Plec, opt => opt.MapFrom(src => src.Gender));
        }

        private void InvoiceMap()
        {
            CreateMap<Faktury, InvoiceViewModel>()
           .ForMember(d => d.InvoiceId, opt => opt.MapFrom(src => src.Faktura_id))
           .ForMember(d => d.RecipientId, opt => opt.MapFrom(src => src.Odbiorca_id))
           .ForMember(d => d.SupplierId, opt => opt.MapFrom(src => src.Dostawca_id))
           .ForMember(d => d.EmployeeId, opt => opt.MapFrom(src => src.Pracownik_id))
           .ForMember(d => d.DuckId, opt => opt.MapFrom(src => src.Kaczka_id))
           .ForMember(d => d.Quantity, opt => opt.MapFrom(src => src.Ilosc))
           .ForMember(d => d.Date, opt => opt.MapFrom(src => src.Data_wystawienia))
           .ReverseMap()
           .ForMember(d => d.Faktura_id, opt => opt.MapFrom(src => src.InvoiceId))
           .ForMember(d => d.Odbiorca_id, opt => opt.MapFrom(src => src.RecipientId))
           .ForMember(d => d.Dostawca_id, opt => opt.MapFrom(src => src.SupplierId))
           .ForMember(d => d.Pracownik_id, opt => opt.MapFrom(src => src.EmployeeId))
           .ForMember(d => d.Kaczka_id, opt => opt.MapFrom(src => src.DuckId))
           .ForMember(d => d.Ilosc, opt => opt.MapFrom(src => src.Quantity))
           .ForMember(d => d.Data_wystawienia, opt => opt.MapFrom(src => src.Date));

        }
        private void SupplierMap()
        {
            CreateMap<Dostawcy, SupplierViewModel>()
           .ForMember(d => d.SupplierId, opt => opt.MapFrom(src => src.Dostawca_id))
           .ForMember(d => d.SupplierName, opt => opt.MapFrom(src => src.Dostawca_nazwa))
           .ForMember(d => d.SupplierCountryId, opt => opt.MapFrom(src => src.Kraj_id))
           .ReverseMap()
           .ForMember(d => d.Dostawca_id, opt => opt.MapFrom(src => src.SupplierId))
           .ForMember(d => d.Dostawca_nazwa, opt => opt.MapFrom(src => src.SupplierName))
           .ForMember(d => d.Kraj_id, opt => opt.MapFrom(src => src.SupplierCountryId));
        }
        private void RecipientMap()
        {
            CreateMap<Odbiorcy, RecipientViewModel>()
           .ForMember(d => d.RecipientId, opt => opt.MapFrom(src => src.Odbiorca_id))
           .ForMember(d => d.RecipientName, opt => opt.MapFrom(src => src.Odbiorca_nazwa))
           .ForMember(d => d.RecipientCountryId, opt => opt.MapFrom(src => src.Kraj_id))
           .ReverseMap()
           .ForMember(d => d.Odbiorca_id, opt => opt.MapFrom(src => src.RecipientId))
           .ForMember(d => d.Odbiorca_nazwa, opt => opt.MapFrom(src => src.RecipientName))
           .ForMember(d => d.Kraj_id, opt => opt.MapFrom(src => src.RecipientCountryId));
        }
        private void DuckMap()
        {
            CreateMap<Kaczki, DuckViewModel>()
           .ForMember(d => d.DuckId, opt => opt.MapFrom(src => src.Kaczka_id))
           .ForMember(d => d.DuckType, opt => opt.MapFrom(src => src.Rodzaj))
           .ForMember(d => d.DuckPrice, opt => opt.MapFrom(src => src.Cena))
           .ForMember(d => d.DuckCountryId, opt => opt.MapFrom(src => src.Kraj_id))
           .ReverseMap()
           .ForMember(d => d.Kaczka_id, opt => opt.MapFrom(src => src.DuckId))
           .ForMember(d => d.Rodzaj, opt => opt.MapFrom(src => src.DuckType))
           .ForMember(d => d.Cena, opt => opt.MapFrom(src => src.DuckPrice))
           .ForMember(d => d.Kraj_id, opt => opt.MapFrom(src => src.DuckCountryId));
        }
        private void CountryMap()
        {
            CreateMap<Kraje, CountryViewModel>()
            .ForMember(d => d.CountryId, opt => opt.MapFrom(src => src.Kraj_id))
            .ForMember(d => d.CountryName, opt => opt.MapFrom(src => src.Kraj_nazwa))
            .ReverseMap()
            .ForMember(d => d.Kraj_id, opt => opt.MapFrom(src => src.CountryId))
            .ForMember(d => d.Kraj_nazwa, opt => opt.MapFrom(src => src.CountryName));
        }
    }
}
