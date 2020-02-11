using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Repository
{
    public class UnitOfWork:IDisposable
    {
        private SklepEntities context = new SklepEntities();
        private GenericRepository<Faktury> invoiceRepository;
        private GenericRepository<Kaczki> duckRepository;
        private GenericRepository<Pracownicy> employeeRepository;
        private GenericRepository<Kraje> countryRepository;
        private GenericRepository<Dostawcy> supplierRepository;
        private GenericRepository<Odbiorcy> recipientRepository;

        public GenericRepository<Faktury> InvoiceRepository
        {
            get
            {

                if (this.invoiceRepository == null)
                {
                    this.invoiceRepository = new GenericRepository<Faktury>(context);
                }
                return invoiceRepository;
            }
        }
        
        public GenericRepository<Kaczki> DuckRepository
        {
            get
            {

                if (this.duckRepository == null)
                {
                    this.duckRepository = new GenericRepository<Kaczki>(context);
                }
                return duckRepository;
            }
        }  public GenericRepository<Pracownicy> EmployeeRepository
        {
            get
            {

                if (this.employeeRepository == null)
                {
                    this.employeeRepository = new GenericRepository<Pracownicy>(context);
                }
                return employeeRepository;
            }
        }  public GenericRepository<Dostawcy> SupplierRepository
        {
            get
            {

                if (this.supplierRepository == null)
                {
                    this.supplierRepository = new GenericRepository<Dostawcy>(context);
                }
                return supplierRepository;
            }
        }  public GenericRepository<Odbiorcy> RecipientRepository
        {
            get
            {

                if (this.recipientRepository == null)
                {
                    this.recipientRepository = new GenericRepository<Odbiorcy>(context);
                }
                return recipientRepository;
            }
        }
        public GenericRepository<Kraje> CountryRepository
        {
            get
            {

                if (this.countryRepository == null)
                {
                    this.countryRepository = new GenericRepository<Kraje>(context);
                }
                return countryRepository;
            }
        }
        
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
