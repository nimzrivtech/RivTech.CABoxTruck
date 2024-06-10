using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration
{
    public enum AmountKind
    {
        VehicleAL = 101,
        VehiclePD = 102,
        VehiclePDCollision = 1021,
        VehiclePDComprehensive = 1022,
        VehicleCargo = 103,
        GeneralLiability = 201,
        ManuscriptAL = 301,
        ManuscriptPD = 302,
        PDHired = 401,
        PDTrailer = 402,
        ServiceFeeAL = 501,
        ServiceFeePD = 502,
        AdditionalPremiumAI = 901,
        AdditionalPremiumWOS = 902,
        AdditionalPremiumPNC = 903,
    }
}
