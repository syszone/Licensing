using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using Licensing.Manager.Model;
using Licensing.Manager.ViewModels;
using Licensing.Validation;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Verification
{
    public class VerifyLicenseQueryHandler : IRequestHandler<VerifyLicenseQuery, VerifyResponseModel>
    {

        private readonly ApplicationDbContext _context;
        public VerifyLicenseQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VerifyResponseModel> VerifyLicense(MachineKeysViewModel req)
        {
            VerifyResponseModel response = new VerifyResponseModel();
            bool isValid = false;
            string publicKey = "";
            string DurationEnum = "";
            try
            {
                var connection = new SqlConnection(Utils.GetConnectionString());
                var result = await connection.QueryAsync("GetLicense", new 
                { 
                    LicenseId = req.license.Id 
                }, commandType: CommandType.StoredProcedure);

                if (result != null && result.Count() > 0)
                {
                    response = result.Select(x => new VerifyResponseModel { ExpiryDate = x.ExpiryDate }).FirstOrDefault();
                    publicKey = result.Select(x => x.PublicKey).FirstOrDefault();
                    DurationEnum = result.Select(x => x.DurationEnum).FirstOrDefault();
                }
                isValid = ValidateSignature(req.license, publicKey);
                if (!isValid)
                {
                    response.IsSuccess = true;
                    response.Message = "License Signature Invalid!!!";
                }

                if (req.license.Type == LicenseType.Subscription)
                {
                    await connection.OpenAsync();

                    result = await connection.QueryAsync("VerifyMachineKey", new 
                    { 
                        LicenseId = req.license.Id, 
                        Email = req.Email, 
                        MachineKey = req.MachineKey 
                    }, commandType: CommandType.StoredProcedure);

                    if (result != null && result.Count() > 0)
                    {
                        var rowCount = result.Select(x => x.count).FirstOrDefault();
                        if (rowCount > 0)
                        {
                            if (response.ExpiryDate > DateTime.Now)
                            {
                                response.IsSuccess = true;
                                response.Message = "Success";
                                response.Features = await GetProductFeartre(req.license.Id.ToString());
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Message = "License was Expired!!!";
                            }
                        }
                        else
                        {
                            response.ExpiryDate = await UpdateExpiryDate(req.license.Id.ToString(), DurationEnum);
                            var res = await connection.QueryAsync("InsertMachineKey", new
                            {
                                LicenseId = req.license.Id,
                                Email = req.Email,
                                MachineKey = req.MachineKey
                            }, commandType: CommandType.StoredProcedure);

                            if (response.ExpiryDate > DateTime.Now)
                            {
                                response.IsSuccess = true;
                                response.Message = "Success";
                                response.Features = await GetProductFeartre(req.license.Id.ToString());
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Message = "License was Expired!!!";
                            }
                        }
                    }
                }

                else if (req.license.Type == LicenseType.Standard)
                {
                    await connection.OpenAsync();
                    result = await connection.QueryAsync("VerifyMachineKey",
                        new
                        {
                            LicenseId = req.license.Id,
                            Email = req.Email,
                            MachineKey = req.MachineKey
                        }, commandType: CommandType.StoredProcedure);


                    if (result != null && result.Count() > 0)
                    {
                        var rowCount = result.Select(x => x.count).FirstOrDefault();
                        if (rowCount > 0)
                        {
                            response.IsSuccess = true;
                            response.Message = "Success";
                            response.Features = await GetProductFeartre(req.license.Id.ToString());

                        }
                        else
                        {
                            int Qty = req.license.Quantity;
                            var res = await connection.QueryAsync("CheckMachineKeyStandard", new
                            {
                                LicenseId = req.license.Id
                            }, commandType: CommandType.StoredProcedure);
                            
                            rowCount = res.Select(x => x.count).FirstOrDefault();
                            if (rowCount < req.license.Quantity)
                            {
                                response.ExpiryDate = null;
                                var data = await connection.QueryAsync("InsertMachineKey", new
                                {
                                    LicenseId = req.license.Id,
                                    Email = req.Email,
                                    MachineKey = req.MachineKey
                                }, commandType: CommandType.StoredProcedure);

                                response.IsSuccess = true;
                                response.Message = "Success";
                                response.Features = await GetProductFeartre(req.license.Id.ToString());
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Message = "License was Expired!!!";
                            }
                        }
                    }
                }

                else if (req.license.Type == LicenseType.Trial)
                {

                    if (response.ExpiryDate > DateTime.Now)
                    {
                        response.IsSuccess = true;
                        response.Message = "Success";
                        response.Features = await GetProductFeartre(req.license.Id.ToString());
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "License was Expired!!!";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<VerifyResponseModel> Handle(VerifyLicenseQuery request, CancellationToken cancellationToken)
        {
            return await VerifyLicense(request.QueryParameters);
        }

        public async Task<DateTime> UpdateExpiryDate(string LicenseId, string TotalDays)
        {
            using (var connection = new SqlConnection(Utils.GetConnectionString()))
            {
                await connection.OpenAsync();

                double days = Convert.ToDouble(TotalDays);
                DateTime NewExpriyDate = DateTime.Now.AddDays(days);

                IEnumerable<dynamic> res = await connection.QueryAsync("UpdateLicenseExpireDate",
                  new { LicenseId = LicenseId, ExpiryDate = NewExpriyDate },
                  commandType: CommandType.StoredProcedure);

                return NewExpriyDate;

            }
        }

        public async Task<List<string>> GetProductFeartre(string id)
        {
            List<string> featureList = new List<string>();
            using (var connection = new SqlConnection(Utils.GetConnectionString()))
            {

                var resultList = await connection.QueryAsync("GetProductFeatureFromLicense", new
                {
                    LicenseId = id
                }, commandType: CommandType.StoredProcedure);
                if (resultList != null)
                {

                    var featureResult = resultList.Select(x => new ProductFeatureList
                    {
                        name = x.NAME

                    }).ToList();

                    foreach (var item in featureResult)
                    {
                        featureList.Add(item.name);

                    }
                }
            }
            return featureList;

        }

        public bool ValidateSignature(License license, string publicKey)
        {
            var isValid = license.VerifySignature(publicKey);

            var validationResults = license
                .Validate()
                .Signature(publicKey)
                .AssertValidLicense()
                .ToList();

            if(validationResults.Count == 0)
            {
                return true;
            }
            return false;

        }

    }

}
