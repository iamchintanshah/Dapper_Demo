using LearnDapper.Domain.Interfaces;
using LearnDapper.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Text.Json;

namespace LearnDapper.API.Controllers
{
    //[Authorize(Roles = "admin,HR,staff")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository repo)
        {
            _employeeRepository = repo;
        }

        // Testing for Single, SingleOrDefault, First, FirstOrDefault
        //[HttpGet("GlobalErrorHandling/{id}/{name}/{age}")]
        //public IActionResult GlobalErrorHandling(Employee emp, [FromQuery] string Email)
        [HttpPost("GlobalErrorHandling")]
        public IActionResult GlobalErrorHandling(Employee emp)
        {
            List<Employee> employeeList = new List<Employee>(){
                new Employee() { Id = 1, Name = "Sunny" },
                new Employee() { Id = 2, Name="Pinki"}
            };

            //var data = employeeList.Single(); // Throws exception if more than one row found
            //var data = employeeList.SingleOrDefault(); // Throws exception if more than one row found
            //var data = employeeList.FirstOrDefault();
            var data = employeeList.Where(x => x.Id == 2).First();

            //var JsonString = JsonSerializer.Serialize(data);
            return Ok(data);
        }

        [HttpPost]
        [Route("User/Create")]
        [Route("User/CreateUpdate")]
        public ActionResult UserCreateUpdate([FromRoute] int id)
        {
            return Ok(id);
        }

        [HttpPost]
        [Route("UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationtoken)
        {
            var result = await WriteFile(file);
            return Ok(result);
        }


        private async Task<string> WriteFile(IFormFile file)
        {
            string filename;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return filename;
        }

        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var _list = await _employeeRepository.GetAll();
            if (_list != null)
            {
                //return new JsonResult(_list);
                return Ok(_list);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var _list = await _employeeRepository.GetEmployeeById(id);
            if (_list != null)
            {
                return Ok(_list);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (employee.Name!.Length < 3 || employee.Name.Length > 30)
            {
                return BadRequest("Name should be between 3 and 30 characters.");
            }

            var _result = await _employeeRepository.Create(employee);
            return Ok(_result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] Employee employee, int id)
        {
            var _result = await _employeeRepository.Update(employee, id);
            return Ok(_result);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            var _result = await _employeeRepository.Remove(id);
            return Ok(_result);
        }

    }
}
