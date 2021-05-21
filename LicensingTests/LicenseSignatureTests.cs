using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Licensing.Security;
using Licensing;

namespace LicensingTests
{
    [TestClass()]
    public class LicenseSignatureTests
    {
        private string passPhrase;
        private string privateKey;
        private string publicKey;

        [TestInitialize]
        public void Init()
        {
            passPhrase = Guid.NewGuid().ToString();
            var keyGenerator = Licensing.Security.Cryptography.KeyGenerator.Create();
            var keyPair = keyGenerator.GenerateKeyPair();
            privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
            publicKey = keyPair.ToPublicKeyString();
        }

        private static DateTime ConvertToRfc1123(DateTime dateTime)
        {
            return DateTime.ParseExact(
                dateTime.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture)
                , "r", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        [TestMethod()]
        public void Can_Generate_And_Validate_Signature_With_Empty_License()
        {
            var license = License.New()
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.IsNotNull(license);
            Assert.IsNotNull(license.Signature);

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.IsTrue(xmlElement.HasElements);

            // validate default values when not set
            Assert.AreEqual(license.Id, Guid.Empty);
            Assert.AreEqual(license.Type, LicenseType.Trial);
            Assert.AreEqual(license.Quantity, 0);
            Assert.IsNull(license.ProductFeatures);
            Assert.IsNull(license.Customer);
            Assert.AreEqual(license.Expiration, ConvertToRfc1123(DateTime.MaxValue));

            // verify signature
            Assert.IsTrue(license.VerifySignature(publicKey));
        }

        [TestMethod()]
        public void Can_Generate_And_Validate_Signature_With_Standard_License()
        {
            var licenseId = Guid.NewGuid();
            var customerName = "Max Mustermann";
            var customerEmail = "max@mustermann.tld";
            var expirationDate = DateTime.Now.AddYears(1);
            var productFeatures = new Dictionary<string, string>
                                      {
                                          {"Sales Module", "yes"},
                                          {"Purchase Module", "yes"},
                                          {"Maximum Transactions", "10000"}
                                      };

            var license = License.New()
                                 .WithUniqueIdentifier(licenseId)
                                 .As(LicenseType.Standard)
                                 .WithMaximumUtilization(10)
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName, customerEmail)
                                 .ExpiresAt(expirationDate)
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.IsNotNull(license);
            Assert.IsNotNull(license.Signature);

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.IsTrue(xmlElement.HasElements);

            // validate default values when not set
            Assert.AreEqual(license.Id, licenseId);
            Assert.AreEqual(license.Type, LicenseType.Standard);
            Assert.AreEqual(license.Quantity, 10);
            Assert.IsNotNull(license.ProductFeatures);
            Assert.AreEqual(license.ProductFeatures.GetAll(), productFeatures);
            Assert.IsNotNull(license.Customer);
            Assert.AreEqual(license.Customer.Name, customerName);
            Assert.AreEqual(license.Customer.Email, customerEmail);
            Assert.AreEqual(license.Expiration, ConvertToRfc1123(expirationDate));

            // verify signature
            Assert.IsTrue(license.VerifySignature(publicKey));
        }

        [TestMethod()]
        public void Can_Detect_Hacked_License()
        {
            var licenseId = Guid.NewGuid();
            var customerName = "Max Mustermann";
            var customerEmail = "max@mustermann.tld";
            var expirationDate = DateTime.Now.AddYears(1);
            var productFeatures = new Dictionary<string, string>
                                      {
                                          {"Sales Module", "yes"},
                                          {"Purchase Module", "yes"},
                                          {"Maximum Transactions", "10000"}
                                      };

            var license = License.New()
                                 .WithUniqueIdentifier(licenseId)
                                 .As(LicenseType.Standard)
                                 .WithMaximumUtilization(10)
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName, customerEmail)
                                 .ExpiresAt(expirationDate)
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.IsNotNull(license);
            Assert.IsNotNull(license.Signature);

            // verify signature
            Assert.IsTrue(license.VerifySignature(publicKey));

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.IsTrue(xmlElement.HasElements);

            // manipulate xml
            Assert.IsNotNull(xmlElement.Element("Quantity"));
            xmlElement.Element("Quantity").Value = "11"; // now we want to have 11 licenses

            // load license from manipulated xml
            var hackedLicense = License.Load(xmlElement.ToString());

            // validate default values when not set
            Assert.AreEqual(hackedLicense.Id, licenseId);
            Assert.AreEqual(hackedLicense.Type, LicenseType.Standard);
            Assert.AreEqual(hackedLicense.Quantity,11); // now with 10+1 licenses
            Assert.IsNotNull(hackedLicense.ProductFeatures);
            Assert.AreEqual(hackedLicense.ProductFeatures.GetAll(), productFeatures);
            Assert.IsNotNull(hackedLicense.Customer);
            Assert.AreEqual(hackedLicense.Customer.Name,customerName);
            Assert.AreEqual(hackedLicense.Customer.Email,customerEmail);
            Assert.AreEqual(hackedLicense.Expiration, ConvertToRfc1123(expirationDate));

            // verify signature
            Assert.IsFalse(hackedLicense.VerifySignature(publicKey));
        }
    }
}
