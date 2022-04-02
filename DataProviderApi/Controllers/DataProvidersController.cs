using System.Dynamic;
using System.Reflection;
using System.Text.Json.Nodes;
using AutoMapper;
using DataProviderApi.Tools;
using DomainModel.DataModel;
using Facade;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DataProviderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProvidersController : Controller
    {
        private readonly IDataProviderFacade _providerFacade;
        private readonly IMapper _mapper;
        public DataProvidersController(IDataProviderFacade dataProviderFacade, IMapper mapper)
        {
            _mapper = mapper;
            _providerFacade = dataProviderFacade;
        }

        [HttpPost("AddBatch")]
        public IActionResult Add([FromBody] JsonObject command)
        {
            try
            {
                var propRecord = JsonTools.Convert(command);
                var value = _mapper.Map<List<DynamicObjectDO>>(propRecord);
                _providerFacade.AddBatch(value);
                var result = new OkObjectResult(new { message = "200 OK" });
                return result;
            }
            catch (Exception e)
            {
                var result = new BadRequestObjectResult(new { message = e.Message });
                //log
                return result;
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var resList = _providerFacade.GetAll();
                var result = new OkObjectResult(new { message = "200 OK", currentDate = resList });
                return result;
            }
            catch (Exception e)
            {
                var result = new BadRequestObjectResult(new { message = e.Message });
                //log
                return result;
            }
        }

        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var res = _providerFacade.Get(id);
                var result = new OkObjectResult(new { message = "200 OK", currentDate = res });
                return result;
            }
            catch (Exception e)
            {
                var result = new BadRequestObjectResult(new { message = e.Message });
                //log
                return result;
            }
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] object json)
        {
            try
            {
                var result = new BadRequestObjectResult(new { message = "در آینده" });
                return result;
            }
            catch (Exception e)
            {
                var result = new BadRequestObjectResult(new { message = e.Message });
                //log
                return result;
            }
        }

        [HttpPut("Edit")]
        public IActionResult Edit([FromBody] object command)
        {
            try
            {
                _providerFacade.Update(command);
                var result = new OkObjectResult(new { message = "200 OK" });
                return result;
            }
            catch (Exception e)
            {
                var result = new BadRequestObjectResult(new { message = e.Message });
                //log
                return result;
            }
        }
    }
}
