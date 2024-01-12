using DapperApp.Core.Interfaces;
using DapperApp.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DapperApp.Infrastructure
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IProductRepository _productRepository;
        private bool _disposed;
        #endregion

        public DapperUnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("SqlConnection"));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        #region IUnitOfWork Members
        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository
                    ?? (_productRepository = new ProductRepository(_transaction));
            }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                resetRepositories();
                _transaction = _connection.BeginTransaction();
            }
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        private void resetRepositories()
        {
            _productRepository = null;
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~DapperUnitOfWork()
        {
            dispose(false);
        }
        #endregion
    }
}
