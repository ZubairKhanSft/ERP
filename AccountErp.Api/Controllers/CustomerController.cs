using AccountErp.Api.Helpers;
using AccountErp.Dtos.Customer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.Customer;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Threading.Tasks;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    //[Authorize]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager _customerManager;

        public CustomerController(ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody]CustomerAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (await _customerManager.IsEmailExistsAsync(model.Email))
            {
                response.StatusCode = 404;
                response.Message = "Another customer with same email already exists";
                return BadRequest(response);
            }

            try
            {
                if (model != null)
                {
                    var customerId = await _customerManager.AddAsync(model);
                    if(model.ShippingAddress != null)
                    {
                        model.ShippingAddress.CustomerId = customerId;
                        await _customerManager.AddCustomerShippingAddress(model.ShippingAddress);
                    }
                    if (model.CustomerPayment != null)
                    {
                        model.CustomerPayment.CustomerId = customerId;
                        await _customerManager.AddCustomerPayment(model.CustomerPayment);
                    }
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    response.Data = customerId;
                    return Ok(response);
                   
                }
                response.StatusCode = 404;
                response.Message = Constants.provideValues;
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] CustomerEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (await _customerManager.IsEmailExistsAsync(model.Id, model.Email))
            {
                response.StatusCode = 404;
                response.Message = "Another customer with same email already exists";
                return BadRequest(response);
            }

            try
            {
                if (model.Id != 0)
                {
                    await _customerManager.EditAsync(model);
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }
                response.StatusCode = 404;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {

            var response = new BaseResponse<object>();

            if (id != 0)
            {
                var data = await _customerManager.GetDetailAsync(id);
                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "data not Present";
                    return Ok(response);
                }
            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }

       

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var response = new BaseResponse<object>();

            if (id != 0)
            {
                await _customerManager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }


        [HttpPost]
        [Route("getall")]
        public async Task<IActionResult> GetPagedResult(int PageSize,int Page,string filterKey)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _customerManager.GetPagedResultAsync(PageSize, Page,filterKey);
            if (data.Item1.Count != 0)
            {
                response.Data = data.Item1;
                response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }



        [HttpGet]
        [Route("get-for-edit/{id}")]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var customer = await _customerManager.GetForEditAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }


        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(CustomerJqDataTableRequestModel model)
        {
            var pagedResult = await _customerManager.GetPagedResultAsync(model);
            return Ok(pagedResult);
        }

        [HttpGet]
        [Route("get-select-items")]
        public async Task<IActionResult> GetSelectItems()
        {
            return Ok(await _customerManager.GetSelectItemsAsync());
        }

        [HttpPost]
        [Route("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var response = new BaseResponse<object>();
            if (id != 0)
            {
                await _customerManager.ToggleStatusAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.status;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }

        
        [HttpGet]
        [Route("get-payment-info/{id}")]
        public async Task<IActionResult> GetPaymentInfo(int id)
        {
            return Ok(await _customerManager.GetPaymentInfoAsync(id));
        }

        [HttpPost]
        [Route("get-customer-statement")]
        public async Task<CustomerStatementDto> GetCustomerStatementAsync(CustomerStatementDto model)
        {
            //await _customerManager.SetOverdueStatus(model.CustomerId);
            var pagedResult = await _customerManager.GetCustomerStatementAsync(model);
            return (pagedResult);
        }
        [HttpGet]
        [Route("getDetailsCustomerAndVendor")]
        public async Task<IActionResult> GetDetailsCustomerAndVendorAsync()
        {
            var CustomerAndVendor = await _customerManager.GetDetailsCustomerAndVendorAsync();
            return Ok(CustomerAndVendor);
        }
    }
}