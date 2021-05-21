using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licensing.Manager.Data;
using Licensing.Manager.Models;
using Licensing.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace License.Manager.Controllers
{
    public class LicenseModelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public LicenseModelController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.LicenseModel.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.LicenseModel
                .FirstOrDefaultAsync(m => m.LicenseModelId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LicenseModelId,ProductId,ExpiryTerm,LicenseType")] LicenseModel licenseModel)
        {
            if (ModelState.IsValid)
            {
                //product.LicenseModelId = Guid.NewGuid();
                string keySection = Guid.NewGuid().ToString();
                //var machineKeySection = WebConfigurationManager.GetSection("system.web/machineKey") as MachineKeySection;
                //if (machineKeySection == null ||
                //    StringComparer.OrdinalIgnoreCase.Compare(machineKeySection.Decryption, "Auto") == 0)
                //    throw new Exception(Properties.Resources.InvalidMachineKeySection);

                licenseModel.KeyPair =  GenerateKeyPair(keySection);
                    
                _context.Add(licenseModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(licenseModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.LicenseModel.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LicenseModelId,ProductId,ExpiryTerm,LicenseType")] LicenseModel licenseModel)
        {
            if (id != licenseModel.LicenseModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licenseModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(licenseModel.LicenseModelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(licenseModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.LicenseModel
                .FirstOrDefaultAsync(m => m.LicenseModelId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.LicenseModel.Any(e => e.LicenseModelId == id);
        }

        private static Licensing.Manager.Models.KeyPair GenerateKeyPair(string privateKeyPassPhrase)
        {
            var keyGenerator = KeyGenerator.Create();
            var keyPair = keyGenerator.GenerateKeyPair();

            var result =
                new Licensing.Manager.Models.KeyPair
                {
                    PublicKey = keyPair.ToPublicKeyString(),
                    EncryptedPrivateKey = keyPair.ToEncryptedPrivateKeyString(privateKeyPassPhrase)
                };

            return result;
        }

    }
}
