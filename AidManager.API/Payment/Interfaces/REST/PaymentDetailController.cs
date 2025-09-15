using System.Net.Mime;
using AidManager.API.Payment.Application.Internal.QueryServices;
using AidManager.API.Payment.Domain.Model.Queries;
using AidManager.API.Payment.Domain.Services;
using AidManager.API.Payment.Interfaces.REST.Resources;
using AidManager.API.Payment.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace AidManager.API.Payment.Interfaces.REST;
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PaymentDetailController(IPaymentDetailCommandService paymentDetailCommandService, IPaymentDetailQueryService paymentDetailQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreatePaymentDetail([FromBody] CreatePaymentDetailResource resource)
    {
        var createPaymentDetailCommand = CreatePaymentDetailCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await paymentDetailCommandService.Handle(createPaymentDetailCommand);
        if(result == null) return Ok(new {status_code= 500, message = "Internal server error"});
        return Ok(new { status_code = 202, message = "Payment detail created successfully" });
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetPaymentDetailById(int id)
    {
        var getPaymentDetailByIdQuery = new GetPaymentDetailByIdQuery(id);
        var result = await paymentDetailQueryService.Handle(getPaymentDetailByIdQuery);
        var resource = PaymentDetailResourceFromEntityAssembler.ToResourceFromEntity(result);
        
        return Ok(resource);
    }
}