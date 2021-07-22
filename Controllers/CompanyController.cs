using Microsoft.AspNetCore.Mvc;
using CompnayDetails.Infrastructure;
using CompnayDetails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompnayDetails.CompanyDto;

namespace CompnayDetails.Controllers
{
    [Route("api/v1.0/market/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICompanyRepositories companyRepositories;

        public CompanyController(ICompanyRepositories _companyRepositories)
        {
            if(_companyRepositories == null)
            {
                throw new NullReferenceException();
            }

            this.companyRepositories = _companyRepositories;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            try
            {
                var company = companyRepositories.GetAll();
                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }            
        }

        [HttpGet]
        [Route("getallStock")]
        public IActionResult GetAllStock()
        {
            try
            {
                var stock = companyRepositories.GetAllStock();
                if (stock == null)
                {
                    return NotFound();
                }

                return Ok(stock);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [Route("info/{code}")]
        [HttpGet]
        public IActionResult GetCompanyById(string code)
        {
            try
            {
                var company = companyRepositories.GetCompanyDetailsByCode(code);
                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Route("delete/{code}")]
        [HttpDelete]
        public IActionResult DeleteCompanyByCode(string code)
        {
            try
            {
                var company = companyRepositories.DeleteCompany(code);
                if (!company)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Route("register")]
        [HttpPost]
        public IActionResult CreateRegister([FromBody] CompanyDetailsDTO companyDetail)
        {
            try
            {
                var company = companyRepositories.CreateCompany(companyDetail);
                if (company == null)
                {
                    return BadRequest();
                }

                return Created("Success", company);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }


        [Route("UpdateCompany/{code}")]
        [HttpPut]
        public IActionResult UpdateRegister([FromBody] CompanyDetailsDTO companyDetail)
        {
            try
            {
                var company = companyRepositories.UpdateCompany(companyDetail);
                if (company == null)
                {
                    return BadRequest();
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }
    }
}
