using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licensing.Manager.ViewModels
{
    public class LicenseModelFeature
    {
        public Guid LicenseModelFeatureId { get; set; }

        [ForeignKey("LicenseModel")]
        public Guid LicenseModelId { get; set; }         
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual LicenseModel LicenseModel { get; set; }
    }
}
