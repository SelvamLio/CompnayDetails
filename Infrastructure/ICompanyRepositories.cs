using CompnayDetails.CompanyDto;
using CompnayDetails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompnayDetails.Infrastructure
{
    public interface ICompanyRepositories
    {
        List<CompanyDetailsDTO> GetAll();

        List<StockExchangeDTO> GetAllStock();

        CompanyDetailsDTO GetCompanyDetailsByCode(string code);

        CompanyDetailsDTO CreateCompany(CompanyDetailsDTO companyDetails);
        
        CompanyDetailsDTO UpdateCompany(CompanyDetailsDTO companyDetails);
        
        bool DeleteCompany(string code);
    }
}
