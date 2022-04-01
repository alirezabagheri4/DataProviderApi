using System.Reflection;
using Facade;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataProviderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataProvidersController : Controller
    {
        private readonly IDataProviderFacade _providerFacade;
        public DataProvidersController(IDataProviderFacade dataProviderFacade)
        {
            _providerFacade= dataProviderFacade;
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] object json)
        {
            try
            {
                //var json = jsons.ToString().Trim().TrimStart('{').TrimEnd('}');
                var dic = new Dictionary<int, Tuple<string?, string?>>();
                var command = JsonConvert.DeserializeObject(json.ToString());
                var i = 0;
                Type myType = command.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

                foreach (PropertyInfo item in props)
                {
                    //object? propValue = prop.GetValue(command, null);

                    var nameOfProperty = i;
                    i++;
                    string? propertyInfo = item.GetType().GetProperty(item.Name)?.ToString();
                    string? value = item.GetValue(command, null)?.ToString();
                    dic.Add(i, new Tuple<string?, string?>(propertyInfo, value));
                }

                _providerFacade.Add(command);
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

        [HttpPost("AddBatch")]
        public IActionResult Add([FromBody] List<object> command)
        {
            try
            {
                _providerFacade.AddBatch(command);
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
