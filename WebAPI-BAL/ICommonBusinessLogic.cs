﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper.Repositories;
using Dapper.Repositories.DbContext;
using Dapper.Repositories.Extensions;

namespace WebAPI_BAL
{
    public interface ICommonBusinessLogic<TDbContext, TEntity, TEntityViewModel>
        where TEntity : class, IDefaultColumns
        where TEntityViewModel : class
        where TDbContext : IDapperDbContext
    {
        #region Handle Transaction
        TReturn HandleTransaction<TReturn>(Func<IDbTransaction, TReturn> repoFunc);

        TReturn HandleTransaction<TReturn, TData>(Func<TData, IDbTransaction, TReturn> repoFunc, TData data);

        TReturn HandleTransaction<TReturn, TData1, TData2>(
            Func<TData1, TData2, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2);

        TReturn HandleTransaction<TReturn, TData1, TData2, TData3>(
            Func<TData1, TData2, TData3, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2, TData3 data3);

        TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4>(
            Func<TData1, TData2, TData3, TData4, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4);

        TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5>(
            Func<TData1, TData2, TData3, TData4, TData5, IDbTransaction, TReturn> repoFunc, TData1 data1, TData2 data2,
            TData3 data3, TData4 data4, TData5 data5);

        TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5, TData6>(
            Func<TData1, TData2, TData3, TData4, TData5, TData6, IDbTransaction, TReturn> repoFunc, TData1 data1,
            TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, TData6 data6);

        TReturn HandleTransaction<TReturn, TData1, TData2, TData3, TData4, TData5, TData6, TData7>(
            Func<TData1, TData2, TData3, TData4, TData5, TData6, TData7, IDbTransaction, TReturn> repoFunc,
            TData1 data1, TData2 data2,
            TData3 data3, TData4 data4, TData5 data5, TData6 data6, TData7 data7);
        #endregion

        #region Store Procedure
        IEnumerable<TData> ExecuteStoreProcedure<TData, TParams>(TParams spParam)
            where TParams : class, ISProcParam;

        Task<IEnumerable<TData>> ExecuteStoreProcedureAsync<TData, TParams>(TParams spParam)
            where TParams : class, ISProcParam;
        #endregion

        #region Insert
        /// <summary>
        ///     Insert object to DB
        /// </summary>
        (bool, TEntityViewModel) Insert(ClaimsPrincipal claim, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Insert object to DB
        /// </summary>
        Task<(bool, TEntityViewModel)> InsertAsync(ClaimsPrincipal claim, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Bulk Insert objects to DB
        /// </summary>
        int BulkInsert(ClaimsPrincipal claim, IEnumerable<TEntityViewModel> viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Bulk Insert objects to DB
        /// </summary>
        Task<int> BulkInsertAsync(ClaimsPrincipal claim, IEnumerable<TEntityViewModel> viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);
        #endregion

        #region Update
        /// <summary>
        ///     Update object in DB
        /// </summary>
        (bool, TEntityViewModel) Update(ClaimsPrincipal claim, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Update object in DB
        /// </summary>
        Task<(bool, TEntityViewModel)> UpdateAsync(ClaimsPrincipal claim, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Update object in DB
        /// </summary>
        (bool, TEntityViewModel) Update(ClaimsPrincipal claim, Expression<Func<TEntityViewModel, bool>> where, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Update object in DB
        /// </summary>
        Task<(bool, TEntityViewModel)> UpdateAsync(ClaimsPrincipal claim, Expression<Func<TEntityViewModel, bool>> where, TEntityViewModel viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Bulk Update objects in DB
        /// </summary>
        Task<bool> BulkUpdateAsync(ClaimsPrincipal claim, IEnumerable<TEntityViewModel> viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Bulk Update objects in DB
        /// </summary>
        bool BulkUpdate(ClaimsPrincipal claim, IEnumerable<TEntityViewModel> viewModelData, IDbTransaction transaction = null, bool manageTransaction = true);
        #endregion 

        #region Delete
        /// <summary>
        ///     Delete object from DB
        /// </summary>
        bool Delete(ClaimsPrincipal claim, TEntityViewModel data, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Delete object from DB
        /// </summary>
        Task<bool> DeleteAsync(ClaimsPrincipal claim, TEntityViewModel data, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Delete objects from DB
        /// </summary>
        bool Delete(ClaimsPrincipal claim, Expression<Func<TEntityViewModel, bool>> where, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Delete objects from DB
        /// </summary>
        Task<bool> DeleteAsync(ClaimsPrincipal claim, Expression<Func<TEntityViewModel, bool>> where, IDbTransaction transaction = null, bool manageTransaction = true);
        #endregion 

        #region Count
        /// <summary>
        ///     Get number of rows
        /// </summary>
        int Count(IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get number of rows with WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntityViewModel, bool>> where, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get number of rows with DISTINCT clause
        /// </summary>
        int Count(Expression<Func<TEntityViewModel, object>> distinct, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntityViewModel, bool>> where, Expression<Func<TEntityViewModel, object>> distinct, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get number of rows
        /// </summary>
        Task<int> CountAsync(IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get number of rows with WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntityViewModel, bool>> where, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get number of rows with DISTINCT clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntityViewModel, object>> distinct, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntityViewModel, bool>> where, Expression<Func<TEntityViewModel, object>> distinct, IDbTransaction transaction = null, bool manageTransaction = true);
        #endregion

        #region Select

        #region Find
        /// <summary>
        ///     Get first object
        /// </summary>
        TEntityViewModel Find(IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object
        /// </summary>
        TEntityViewModel Find(Expression<Func<TEntityViewModel, bool>> where, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntityViewModel Find<TChild1>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntityViewModel Find<TChild1, TChild2>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntityViewModel Find<TChild1, TChild2, TChild3>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntityViewModel Find<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntityViewModel Find<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        TEntityViewModel Find<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5,
            Expression<Func<TEntityViewModel, object>> tChild6, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object
        /// </summary>
        Task<TEntityViewModel> FindAsync(IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object
        /// </summary>
        Task<TEntityViewModel> FindAsync(Expression<Func<TEntityViewModel, bool>> where, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntityViewModel> FindAsync<TChild1>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntityViewModel> FindAsync<TChild1, TChild2>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntityViewModel> FindAsync<TChild1, TChild2, TChild3>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntityViewModel> FindAsync<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntityViewModel> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get first object with join objects
        /// </summary>
        Task<TEntityViewModel> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5,
            Expression<Func<TEntityViewModel, object>> tChild6, IDbTransaction transaction = null, bool manageTransaction = true);
        #endregion

        #region Find By Id
        /// <summary>
        ///     Get object by Id
        /// </summary>
        TEntityViewModel FindById(object id, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntityViewModel FindById<TChild1>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntityViewModel FindById<TChild1, TChild2>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntityViewModel FindById<TChild1, TChild2, TChild3>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntityViewModel FindById<TChild1, TChild2, TChild3, TChild4>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntityViewModel FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        TEntityViewModel FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5,
            Expression<Func<TEntityViewModel, object>> tChild6, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id
        /// </summary>
        Task<TEntityViewModel> FindByIdAsync(object id, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntityViewModel> FindByIdAsync<TChild1>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2, TChild3>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get object by Id with join objects
        /// </summary>
        Task<TEntityViewModel> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5,
            Expression<Func<TEntityViewModel, object>> tChild6, IDbTransaction transaction = null, bool manageTransaction = true);
        #endregion

        #region Find All
        /// <summary>
        ///     Get all objects
        /// </summary>
        IEnumerable<TEntityViewModel> FindAll(IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects
        /// </summary>
        IEnumerable<TEntityViewModel> FindAll(Expression<Func<TEntityViewModel, bool>> where, IDbTransaction transaction = null, bool manageTransaction = true);


        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntityViewModel> FindAll<TChild1>(Expression<Func<TEntityViewModel, bool>> where, Expression<Func<TEntityViewModel, object>> tChild1, IDbTransaction transaction = null, bool manageTransaction = true);


        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2, TChild3>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2, TChild3, TChild4>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        IEnumerable<TEntityViewModel> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5,
            Expression<Func<TEntityViewModel, object>> tChild6, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllAsync(IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllAsync(Expression<Func<TEntityViewModel, bool>> where, IDbTransaction transaction = null, bool manageTransaction = true);


        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1>(Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2, IDbTransaction transaction = null, bool manageTransaction = true);


        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2, TChild3>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2, TChild3, TChild4>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            Expression<Func<TEntityViewModel, bool>> where,
            Expression<Func<TEntityViewModel, object>> tChild1,
            Expression<Func<TEntityViewModel, object>> tChild2,
            Expression<Func<TEntityViewModel, object>> tChild3,
            Expression<Func<TEntityViewModel, object>> tChild4,
            Expression<Func<TEntityViewModel, object>> tChild5,
            Expression<Func<TEntityViewModel, object>> tChild6, IDbTransaction transaction = null, bool manageTransaction = true);
        #endregion

        #region Find All Between
        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntityViewModel> FindAllBetween(object from, object to, Expression<Func<TEntityViewModel, object>> btwField, IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntityViewModel> FindAllBetween(object from, object to, Expression<Func<TEntityViewModel, object>> btwField,
            Expression<Func<TEntityViewModel, bool>> where = null, IDbTransaction transaction = null,
            bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntityViewModel> FindAllBetween(DateTime from, DateTime to,
            Expression<Func<TEntityViewModel, object>> btwField, IDbTransaction transaction = null,
            bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntityViewModel> FindAllBetween(DateTime from, DateTime to,
            Expression<Func<TEntityViewModel, object>> btwField, Expression<Func<TEntityViewModel, bool>> where,
            IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntityViewModel, object>> btwField, IDbTransaction transaction = null,
            bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntityViewModel, object>> btwField, Expression<Func<TEntity, bool>> where,
            IDbTransaction transaction = null, bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntityViewModel, object>> btwField, IDbTransaction transaction = null,
            bool manageTransaction = true);

        /// <summary>
        ///     Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntityViewModel>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntityViewModel, object>> btwField, Expression<Func<TEntity, bool>> where,
            IDbTransaction transaction = null, bool manageTransaction = true);

        #endregion

        #endregion
    }
}
