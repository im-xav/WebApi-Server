﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using Common;
using Common.Messages;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI_BAL;
using WebAPI_BAL.ApplicationBAL;
using WebAPI_DataAccess.ApplicationContext;
using WebAPI_Model;
using WebAPI_Model.Test;
using WebAPI_Server.AppStart;
using WebAPI_ViewModel.DTO;

namespace WebAPI_Server.Controllers.v1
{
    /// <inheritdoc />
    [ApiVersion(ApiVersionNumber.V1)]
    [Route("api/test")]
    [ApiController]
    public class TestRepoController : BaseController
    {
        private IHttpContextAccessor _httpContextAccessor;
        private static readonly HttpClient Client = new HttpClient();

        private readonly ITestRepoBal _testRepoBal;
        private readonly ICommonStoreProcBusinessLogic<IApplicationDbContext> _cBalProc;
        private readonly ILogger<TestRepoController> _logger;

        /// <inheritdoc />
        public TestRepoController(IHttpContextAccessor httpContextAccessor,
            ITestRepoBal testRepoBal,
            ICommonStoreProcBusinessLogic<IApplicationDbContext> cBalProc,
            ILogger<TestRepoController> logger)
        {
            _testRepoBal = testRepoBal;
            _cBalProc = cBalProc;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("adddata")]
        public IActionResult AddData([FromBody] TestRepoViewModel data)
        {
            (bool, TestRepoViewModel) result = _testRepoBal.Insert(User, data);
            return Ok(result.Item2, InfoMessages.CommonInfoMessage);
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("updatedata")]
        public IActionResult UpdateData([FromBody] TestRepoViewModel data)
        {
            (bool, TestRepoViewModel) result = _testRepoBal.Update(User, x => new {x.FirstName, x.LastName }, data);
            if (result.Item1)
                return Ok(result.Item2, InfoMessages.CommonInfoMessage);

            //return StatusCodeResult(StatusCodes.Status400BadRequest, result.Item2, ErrorMessages.RecordNotFoundUpdate);

            return BadRequest(ErrorMessages.RecordNotFoundUpdate, result.Item2);
        }


        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("updatetestdata")]
        [AllowAnonymous]
        [AllowNoAccessToken]
        public IActionResult UpdateTestData()
        {
            //var ids = new List<int> { 33, 34 };
            List<int> testId = new List<int> { 38, 39, 40 };

            //(bool, TestRepoViewModel) result = _testRepoBal.Update(User, x=> x.FirstName.StartsWith("asd"), 
            //    x => new { x.FirstName, x.LastName },
            //    new TestRepoViewModel(){FirstName = "Update With In Query",LastName = "Last Name IN Q"});
            //if (result.Item1)
            //    return Ok(result.Item2, InfoMessages.CommonInfoMessage);

            var allData = _testRepoBal.FindAll();

            //Working
            //var rowVersion = allData.FirstOrDefault(x => x.Id == 33).RowVersion;
            //var data = _testRepoBal.Update(User, x => x.Id == 33, x => x.FirstName,
            //    new TestRepoViewModel() {FirstName = "Update With In Query 123465", RowVersion = rowVersion });

            //Not Working
            //var rowVersion = allData.FirstOrDefault(x => x.FirstName == "Update With In Query").RowVersion;
            //var data = _testRepoBal.Update(User, x => x.FirstName == "Update With In Query", x => x.FirstName,
            //    new TestRepoViewModel() { FirstName = "Update With In Query 123", RowVersion = rowVersion });

            //Not Working
            var data = _testRepoBal.Update(User, x => testId.Contains(x.Id), x => x.FirstName,
                new TestRepoViewModel() { FirstName = "Update With In Query 456" });

            return Ok(data, InfoMessages.CommonInfoMessage);

            //return StatusCodeResult(StatusCodes.Status400BadRequest, result.Item2, ErrorMessages.RecordNotFoundUpdate);

            //return BadRequest(ErrorMessages.RecordNotFoundUpdate, result.Item2);
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("deletedata")]
        public IActionResult DeleteData([FromBody] TestRepoViewModel data)
        {
            //object dd = _cBal.HandleTransaction((IDbTransaction x) => { return null as object; });

            bool result = _testRepoBal.HandleTransaction((IDbTransaction trans) =>
            {
                TestRepoViewModel trData = _testRepoBal.FindById(data.Id, transaction: trans);
                return _testRepoBal.Delete(User, trData, transaction: trans);
            });

            //bool result = _cBal.Delete(User, x => x.Id == data.Id && x.RowVersion == data.RowVersion);
            if (result)
                return Ok(true, InfoMessages.CommonInfoMessage);

            return BadRequest("Error in deleting model");
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        [AllowAnonymous]
        public IActionResult CountData()
        {
            int result = _testRepoBal.Count();
            return Ok(result, InfoMessages.CommonInfoMessage);
        }

        /// <summary>
        /// Select All
        /// </summary>
        /// <returns></returns>
        [HttpGet("selectall")]
        [AllowAnonymous]
        public IActionResult SelectAll()
        {
            var result = _testRepoBal.FindAll();
            return Ok(result, InfoMessages.CommonInfoMessage);
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <returns></returns>
        [HttpPost("runproc")]
        [AllowAnonymous]
        public IActionResult CallProcedure([FromBody] TestTicketCustomProcedureParamViewModel paramData)
        {
            IEnumerable<PROC_Ticket_Custom_Search_ViewModel> result =
                _cBalProc.ExecuteStoreProcedure<PROC_Ticket_Custom_Search_Model, TestTicketCustomProcedureParam,
                    PROC_Ticket_Custom_Search_ViewModel, TestTicketCustomProcedureParamViewModel>(paramData);

            return Ok(result, InfoMessages.CommonInfoMessage);
        }
    }
}