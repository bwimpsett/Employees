using BusinessLogic;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Presentation.Controllers
{
    public class EmployeesController : ApiController
    {
        //todo: how do I use multiple routes?
        private BusinessCrud _businessCrud = new BusinessCrud();
        
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            var retVal = _businessCrud.Retrive();
            return Request.CreateResponse(HttpStatusCode.OK, retVal);
        }

        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            var retVal = _businessCrud.RetriveById(id);
            if (retVal != null) return Request.CreateResponse(HttpStatusCode.OK, retVal);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID " + id.ToString() + "was not found");
        }

        [HttpPost]
        public HttpResponseMessage SearchForEmployees(EmployeeDTO value)
        {
            var employees = _businessCrud.Search(value);
            return Request.CreateResponse(HttpStatusCode.OK, employees);
        }

        [HttpPost]
        public HttpResponseMessage CreateEmployee(EmployeeDTO value)
        {
            _businessCrud.Create(value);

            /*According to REST standard a new object created should return a 201 along with the URI
                of the item.*/

            var message = Request.CreateResponse(HttpStatusCode.Created, value);
            message.Headers.Location = new Uri(Request.RequestUri + "/" + value.ID.ToString());
            return message;
        }

        [HttpPut]
        public HttpResponseMessage UpdateEmployee(EmployeeDTO value)
        {
            try
            {
                _businessCrud.Update(value);
                return Request.CreateResponse(HttpStatusCode.OK, value);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteEmployee(int id)
        {
            try
            {
                _businessCrud.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}