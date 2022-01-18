using Dapper;
using Licensing.Manager.API.Common;
using Licensing.Manager.API.Context;
using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.Verification
{
    public class VerifyLicenseQueryHandler : IRequestHandler<VerifyLicenseQuery, bool>
    {

        private readonly ProductLicensingDbContext _context;
        public VerifyLicenseQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerifyLicense(MachineKeysViewModel req)
        {
            bool flag = false;
            try
            {
                DateTime ExpirationDate = Convert.ToDateTime(req.license.Expiration);
                if (req.license.Type == LicenseType.Subscription.ToString())
                {
                    RowCount rowCount = new RowCount();
                    using (var connection = new SqlConnection(Utils.GetConnectionString()))
                    {
                        await connection.OpenAsync();

                    var result = await connection.QueryAsync("VerifyMachineKey",
                        new
                        {
                            
                            LicenseId = req.license.Id,
                            Email = req.Email,
                            MachineKey = req.MachineKey
                        },
                          commandType: CommandType.StoredProcedure);


                        if (result != null && result.Count() > 0)
                        {
                            var list = result.Select(x => new RowCount
                            {
                                count = x.count
                            }).FirstOrDefault();
                            rowCount = await Task.FromResult(list);
                        }

                        if (rowCount.count > 0)
                        {
                            if (ExpirationDate > DateTime.Now)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            var res = await connection.QueryAsync("InsertMachineKey",new
                           {
                               LicenseId = req.license.Id,
                               Email = req.Email,
                               MachineKey = req.MachineKey
                           },commandType: CommandType.StoredProcedure);

                            if (ExpirationDate > DateTime.Now)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }

                    }
                }

                else if (req.license.Type == LicenseType.Standard.ToString())
                {
                    RowCount rowCount = new RowCount();
                    using (var connection = new SqlConnection(Utils.GetConnectionString()))
                    {
                        await connection.OpenAsync();

                        var result = await connection.QueryAsync("VerifyMachineKey",
                            new
                            {
                                LicenseId = req.license.Id,
                                Email = req.Email,
                                MachineKey = req.MachineKey
                            },
                              commandType: CommandType.StoredProcedure);


                        if (result != null && result.Count() > 0)
                        {
                            var list = result.Select(x => new RowCount
                            {
                                count = x.count
                            }).FirstOrDefault();
                            rowCount = await Task.FromResult(list);
                        }

                        if (rowCount.count > 0)
                        {
                             flag = true;
                        }
                        else
                        {
                            int Qty = req.license.Quantity;
                            var res = await connection.QueryAsync("CheckMachineKeyStandard", new
                            {
                                LicenseId = req.license.Id                                
                            }, commandType: CommandType.StoredProcedure);
                            var list = result.Select(x => new RowCount
                            {
                                count = x.count
                            }).FirstOrDefault();
                            rowCount = await Task.FromResult(list);
                            if (rowCount.count < req.license.Quantity)
                            {
                                var response = await connection.QueryAsync("InsertMachineKey", new
                                {
                                    LicenseId = req.license.Id,
                                    Email = req.Email,
                                    MachineKey = req.MachineKey
                                }, commandType: CommandType.StoredProcedure);
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                }

                else if (req.license.Type == LicenseType.Trial.ToString())
                {
                    if (ExpirationDate > DateTime.Now)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return flag;
        }

        public async Task<bool> Handle(VerifyLicenseQuery request, CancellationToken cancellationToken)
        {
            return await VerifyLicense(request.QueryParameters);
        }
    }
}
