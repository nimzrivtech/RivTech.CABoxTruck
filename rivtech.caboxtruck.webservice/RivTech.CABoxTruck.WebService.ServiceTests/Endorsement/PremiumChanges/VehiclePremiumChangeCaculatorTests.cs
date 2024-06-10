using Microsoft.VisualStudio.TestTools.UnitTesting;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.PremiumChanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.ServiceTests.Endorsement.PremiumChanges
{
    [TestClass]
    public class VehiclePremiumChangeCaculatorTests  
    {

        [TestMethod]
        public void CalculateTest()
        {
            var calculator = new VehiclePremiumChangeCalculator();

            var oldVehicle = new Vehicle()
            {
                Id = Guid.NewGuid(),
                AutoLiabilityPremium = 1200
            };

            var newVehicle = new Vehicle()
            {
                Id = Guid.NewGuid(),
                PreviousVehicleVersionId = oldVehicle.Id,
                AutoLiabilityPremium = 100
            };

            var result = calculator.Calculate(newVehicle, oldVehicle);

        }
    }
}
