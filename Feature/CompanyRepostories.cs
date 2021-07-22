using CompnayDetails.CompanyDto;
using CompnayDetails.Infrastructure;
using CompnayDetails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompnayDetails.Feature
{
    public class CompanyRepostories : ICompanyRepositories
    {
        private StockMarketContext stockMarketContext;

        public CompanyRepostories(StockMarketContext stockMarketContext)
        {
            this.stockMarketContext = stockMarketContext;
        }

        public List<CompanyDetailsDTO> GetAll()
        {
            if (stockMarketContext != null)
            {
                var result = (from c in stockMarketContext.CompanyDetails
                              orderby c.CompanyName
                              select new CompanyDetailsDTO
                              {
                                  CompanyCode = c.CompanyCode,
                                  CompanyName = c.CompanyName,
                                  CompanyCeo = c.CompanyCeo,
                                  Turnover = c.Turnover,
                                  Website = c.Website,
                                  StockExchange = c.StockExchangeNavigation.ExchangeName
                              }).ToList();

                return result;
            }

            return null;
        }

        public List<StockExchangeDTO> GetAllStock()
        {
            if (stockMarketContext != null)
            {
                var result = (from c in stockMarketContext.StockExchanges
                              orderby c.ExchangeName
                              select new StockExchangeDTO
                              {
                                  StockID = c.ExchangeId,
                                  StockName = c.ExchangeName
                              }).ToList();

                return result;
            }

            return null;
        }

        public CompanyDetailsDTO GetCompanyDetailsByCode(string code)
        {
            if (stockMarketContext != null)
            {
                var result = (from c in stockMarketContext.CompanyDetails
                              where c.CompanyCode == code
                              orderby c.CompanyName
                              select new CompanyDetailsDTO
                              {
                                  CompanyCode = c.CompanyCode,
                                  CompanyName = c.CompanyName,
                                  CompanyCeo = c.CompanyCeo,
                                  Turnover = c.Turnover,
                                  Website = c.Website,
                                  StockExchange = c.StockExchangeNavigation.ExchangeName
                              }).FirstOrDefault();

                return result;
            }

            return null;
        }

        public CompanyDetailsDTO CreateCompany(CompanyDetailsDTO companyDetailsDTO)
        {
            if (companyDetailsDTO != null)
            {
                var result = GetCompanyLastID(companyDetailsDTO.CompanyCode);
                if (result != -1)
                {
                    var companyDetails = new CompanyDetail()
                    {
                        CompanyId = result + 1,
                        CompanyCode = companyDetailsDTO.CompanyCode,
                        CompanyName = companyDetailsDTO.CompanyName,
                        CompanyCeo = companyDetailsDTO.CompanyCeo,
                        Turnover = companyDetailsDTO.Turnover,
                        Website = companyDetailsDTO.Website,
                        StockExchange = CheckStockList(companyDetailsDTO.StockExchange)
                    };

                    stockMarketContext.Add(companyDetails);
                    stockMarketContext.SaveChanges();

                    return companyDetailsDTO;
                }

                return null;
            }

            return null;
        }

        public CompanyDetailsDTO UpdateCompany(CompanyDetailsDTO companyDetailsDTO)
        {
            if (companyDetailsDTO != null)
            {
                var result = stockMarketContext.CompanyDetails.Where(x => x.CompanyCode == companyDetailsDTO.CompanyCode).FirstOrDefault();
                if (result != null)
                {
                    result.CompanyName = companyDetailsDTO.CompanyName;
                    result.CompanyCeo = companyDetailsDTO.CompanyCeo;
                    result.Turnover = companyDetailsDTO.Turnover;
                    result.Website = companyDetailsDTO.Website;
                    result.StockExchange = CheckStockList(companyDetailsDTO.StockExchange);

                    stockMarketContext.Update<CompanyDetail>(result);
                    stockMarketContext.SaveChanges();
                    return companyDetailsDTO;
                }
            }

            return null;
        }

        public bool DeleteCompany(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var result = (from c in stockMarketContext.CompanyDetails
                              where c.CompanyCode == code
                              orderby c.CompanyName
                              select c).FirstOrDefault();

                if (result != null)
                {
                    stockMarketContext.Remove(result);
                    stockMarketContext.SaveChanges();
                    return true;
                }

                return false;
            }

            return false;
        }

        private int GetCompanyLastID(string code)
        {
            var companyList = stockMarketContext.CompanyDetails.OrderByDescending(x => x.CompanyId).ToList();

            var existingData = companyList.Where(x => x.CompanyCode == code).FirstOrDefault();
            if (existingData != null)
            {
                return -1;
            }

            return companyList.Select(x => x.CompanyId).FirstOrDefault();
        }

        private int CheckStockList(string code)
        {
            var stockList = stockMarketContext.StockExchanges.OrderByDescending(x => x.ExchangeId).ToList();

            var existingData = stockList.Where(x => x.ExchangeName == code).FirstOrDefault();
            var stockId = stockList.Select(x => x.ExchangeId).FirstOrDefault();

            if (existingData == null)
            {
                var stockDetails = new StockExchange
                {
                    ExchangeId = stockId + 1,
                    ExchangeName = code
                };

                stockMarketContext.Add(stockDetails);
                stockMarketContext.SaveChanges();

                return stockDetails.ExchangeId;
            }

            return stockId;
        }
    }
}
