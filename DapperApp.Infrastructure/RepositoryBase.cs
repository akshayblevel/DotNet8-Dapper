using Dapper;
using System.Data;
using static Dapper.SqlMapper;

namespace DapperApp.Infrastructure
{
    public abstract class RepositoryBase
    {
        private IDbTransaction _transaction;
        private IDbConnection _connection;

        public RepositoryBase(IDbTransaction transaction)
        {
            _transaction = transaction;
            _connection = _transaction.Connection;
        }

        ///
        /// <summary>
        /// Executes a query and maps the first result
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns></returns>
        protected async Task<T> QueryFirstAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await _connection.QueryFirstAsync<T>(sql, param, _transaction,commandTimeout,commandType);
        }

        /// <summary>
        /// Executes a query and map the first result or a default value 
        /// if the sequence contains no elements
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns></returns>
        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, _transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Executes a query and maps the result.  
        /// It throws an exception if there is not exactly one element in the sequence
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns></returns>
        protected async Task<T> QuerySingleAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await _connection.QuerySingleAsync<T>(sql, param, _transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Executes a query and maps the result or a default value if the sequence is empty. 
        /// It throws an exception if there is more than one element in the sequence
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns></returns>
        protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await _connection.QuerySingleOrDefaultAsync<T>(sql, param, _transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Execute a query and map the result
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns></returns>
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await _connection.QueryAsync<T>(sql, param, _transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Executes a command one or multiple times and return the number of affected rows
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns></returns>
        protected async Task<int> ExecuteAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await _connection.ExecuteAsync(sql, param, _transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Executes multiple queries within the same command and maps results
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns></returns>
        protected async Task<GridReader> QueryMultipleAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await _connection.QueryMultipleAsync(sql, param, _transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="command">The command to execute for this query.</param>
        protected async Task<GridReader> QueryMultipleAsync(CommandDefinition command)
        {
            return await _connection.QueryMultipleAsync(command);
        }
    }
}
