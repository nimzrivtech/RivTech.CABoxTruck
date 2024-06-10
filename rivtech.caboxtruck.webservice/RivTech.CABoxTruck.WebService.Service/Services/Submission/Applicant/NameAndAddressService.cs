using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DomainEntities;
using RivTech.CABoxTruck.WebService.DomainEntities.Security;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission;
using DocumentFormat.OpenXml.Bibliography;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission.Applicant
{
    public class NameAndAddressService : INameAndAddressService
    {
        public IConfiguration Configuration { get; }
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRiskSequenceService _riskSequence;
        private readonly IRiskRepository _riskRepository;
        private readonly IBrokerInfoService _brokerInfoService;
        private readonly IBackgroundServiceChecker _backgroundServiceChecker;
        private readonly string _genericBaseUrl;
        private readonly IRiskPolicyContactRepository _riskPolicyContactRepository;
        private readonly IRiskSpecificDestinationRepository _riskSpecificDestinationRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        private readonly ITokenProvider _tokenProvider;
        private readonly IConfigurationSection _genericServiceSettings;

        public NameAndAddressService(IRiskDetailRepository riskRepository,
            IAddressRepository addressRepository,
            IEntityAddressRepository entityAddressRepository,
            IConfiguration configuration,
            IHttpClientFactory factory,
            IRiskSequenceService riskSequence,
            IRiskRepository riskRepository1,
            IBrokerInfoService brokerInfoService,
            IRiskPolicyContactRepository riskPolicyContactRepository,
            IRiskSpecificDestinationRepository riskSpecificDestinationRepository,
            IMapper mapper,
            IBackgroundServiceChecker backgroundServiceChecker,
            ITokenProvider tokenProvider)
        {
            Configuration = configuration;
            _riskDetailRepository = riskRepository;
            _entityAddressRepository = entityAddressRepository;
            _riskSequence = riskSequence;
            _riskRepository = riskRepository1;
            _client = factory.CreateClient("agencyClient");
            _tokenProvider = tokenProvider;
            _backgroundServiceChecker = backgroundServiceChecker;
            _addressRepository = addressRepository;
            _brokerInfoService = brokerInfoService;
            _riskPolicyContactRepository = riskPolicyContactRepository;
            _riskSpecificDestinationRepository = riskSpecificDestinationRepository;
            _genericServiceSettings = Configuration.GetSection("GenericService");
            // _genericBaseUrl = "https://app-generic-api-prodtest.azurewebsites.net/api";
            _genericBaseUrl = _genericServiceSettings.GetValue<string>("BaseUrl");
            _mapper = mapper;
        }


        public async Task<NameAndAddressDTO> GetNameAndAddress(Guid id)
        {
            //Get Risk and AddressList
            var risk = await _riskDetailRepository.GetInsuredAsync(id);
            var entityAddresses = await _entityAddressRepository.GetAsync(x => x.EntityId == risk.Insured.EntityId && x.AddressTypeId != null,
                                    i => i.Address);

            //Set IsMainGaragingAddress of Entity Addresses if property is empty
            foreach (var item in entityAddresses)
            {
                if (item.IsMainGaragingAddress == null)
                {
                    item.IsMainGaragingAddress = item.Address.IsMainGarage;
                    _entityAddressRepository.Update(item);
                }
            }
            var result = await _entityAddressRepository.SaveChangesAsync();

            //Container for Business Address
            var businessAddress = new AddressDTO();
            var entityAddressDtos = new List<EntityAddressDTO>();
            foreach (var item in entityAddresses) //Iterate IEnumerable. Better if this is a List to have linq or lambda features
            {
                //Get Business Address
                if (item.AddressTypeId == (short)AddressTypesEnum.Business)
                {
                    businessAddress.StreetAddress1 = item.Address.StreetAddress1;
                    businessAddress.ZipCode = item.Address.ZipCode;
                    businessAddress.City = item.Address.City;
                    businessAddress.State = item.Address.State;
                }

                var address = new EntityAddressDTO()
                {
                    Id = item.Id,
                    EntityId = item.EntityId,
                    AddressTypeId = item.AddressTypeId,
                    AddressId = item.AddressId,
                    IsMainGaragingAddress = item.IsMainGaragingAddress ?? item.Address.IsMainGarage,
                    Address = new AddressDTO()
                    {
                        Id = item.Address.Id,
                        StreetAddress1 = item.Address.StreetAddress1,
                        ZipCode = item.Address.ZipCode,
                        City = item.Address.City,
                        State = item.Address.State,
                        County = item.Address.County,
                        IsMainGarage = item.IsMainGaragingAddress ?? item.Address.IsMainGarage,
                    }
                };

                if (item.AddressTypeId == (short)AddressTypesEnum.Billing)
                {
                    var billingAddress = await _riskPolicyContactRepository.GetByRiskDetailIdIncludeAsync(risk.Id, (short)AddressTypesEnum.Billing);
                    if (billingAddress != null)
                    {
                        address.ContactName = billingAddress.Entity.FullName;
                        address.Phone = billingAddress.Entity.WorkPhone;
                        address.Email = billingAddress.Entity.WorkEmailAddress;
                    }
                }

                //Collect Addresses
                entityAddressDtos.Add(address);
            }

            var primaryCityAddress = await _entityAddressRepository.FindAsync(x => x.EntityId == risk.Insured.EntityId && x.AddressTypeId == null,
                                    i => i.Address);

            return new NameAndAddressDTO()
            {
                RiskDetailId = risk.Id,
                SubmissionNumber = risk.SubmissionNumber,
                BusinessName = risk.Insured.Entity.CompanyName,
                InsuredDBA = risk.Insured.Entity.DBA,
                BusinessPrincipal = risk.Insured.Entity.BusinessPrincipal,
                Email = risk.Insured.Entity.WorkEmailAddress,
                PhoneExt = risk.Insured.Entity.WorkPhoneExtension,
                BusinessAddress = businessAddress.StreetAddress1,
                City = businessAddress.City,
                ZipCode = businessAddress.ZipCode,
                State = businessAddress.State,
                CityOperations = primaryCityAddress?.Address?.City,
                StateOperations = primaryCityAddress?.Address?.State,
                ZipCodeOperations = primaryCityAddress?.Address?.ZipCode,
                Addresses = entityAddressDtos,
            };
        }

        public async Task<AddressDTO> GetAddress(Guid id)
        {
            var address = await _addressRepository.FindAsync(x => x.Id == id);
            return _mapper.Map<AddressDTO>(address);
        }
        private async Task AddAuthHeaderToken()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var token = Data.Common.Services.GetAuthToken();
            if (_backgroundServiceChecker.IsFromBackgroundService())
            {
                token = await _tokenProvider.GetAdminTokenAsync();
            }

            if (!String.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<(List<string> cities, string ZipCode, string Status)> GetCityState(string zipCode)
        {
            var response = await _client.GetAsync(_genericBaseUrl + "/ZipCodes/" + zipCode);
            string stateCode = "";
            List<string> cities = new List<string>();
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);
                JArray jsonArray = JArray.Parse(jsonResponse);
                JObject firstObject = (JObject)jsonArray[0];
                stateCode = (string)firstObject["stateCode"];
                foreach (JObject cityList in jsonArray)
                {
                    var f = (string)firstObject["city"];
                    cities.Add((string)cityList["city"]);
                }

            }
            return (cities, stateCode, response.IsSuccessStatusCode ? "success" : "failed");
        }
        async Task<bool> ValidateAddressesAsync(List<EntityAddressDTO> addresses)
        {
            await AddAuthHeaderToken();
            foreach (var address in addresses)
            {
                (List<string> cities, string stateCode, string status) = await GetCityState(address.Address.ZipCode);
                var cityMatch = cities.Exists(it => it == address.Address.City);
                if (address.Address.State != stateCode || !cityMatch || status == "failed")
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> validateAddress(NameAndAddressDTO model)
        {
            await AddAuthHeaderToken();
            // Call asynchronous network methods in a try/catch block to handle exceptions
            (List<string> cities, string stateCode, string status) = await GetCityState(model.ZipCode);
            bool cityMatch = cities.Exists(it => it == model.City) || (cities.Count == 1 && model.City == null);
            if (model.State != stateCode || !cityMatch || status == "failed")
            {
                throw new Exception("Invalid Address");
            }

            (cities, stateCode, status) = await GetCityState(model.ZipCodeOperations);
            cityMatch = cities.Exists(it => it == model.CityOperations);
            if (model.StateOperations != stateCode || !cityMatch || status == "failed")
            {
                throw new Exception("Invalid Address");
            }
            var result = await this.ValidateAddressesAsync(model.Addresses);
            if (!result)
            {
                throw new Exception("Invalid Address");
            }
            return true;
        }

        public async Task<NameAndAddressDTO> InsertAsync(NameAndAddressDTO model)
        {
            var entityId = Guid.NewGuid();
            model.RiskDetailId = Guid.NewGuid();
            var tempEffectiveDate = DateTime.Now;
            await this.validateAddress(model);

            var risk = new Data.Entity.Submission.Risk
            {
                QuoteNumber = _riskSequence.GenerateQuoteNumber(),
                SubmissionNumber = _riskSequence.GenerateSubmissionNumber(),
                Status = RiskStatus.Pending,
                RiskDetails = new List<RiskDetail>(),
                IsRenewal = model.IsRenewal,
                PreviousPolicyNumber = model.PreviousPolicyNumber
            };

            risk.RiskDetails.Add(new RiskDetail()
            {
                Id = model.RiskDetailId.Value,
                SubmissionNumber = risk.SubmissionNumber,
                Status = RiskDetailStatus.Received,
                AssignedToId = 0,
                Insured = new Insured()
                {
                    Id = Guid.NewGuid(),
                    Entity = new Entity()
                    {
                        Id = entityId,
                        CompanyName = model.BusinessName?.Trim(),
                        DBA = model.InsuredDBA.NullIfEmpty(),
                        BusinessPrincipal = model.BusinessPrincipal.NullIfEmpty(),
                        WorkEmailAddress = model.Email.NullIfEmpty(),
                        PersonalEmailAddress = model.Email.NullIfEmpty(),
                        WorkPhoneExtension = model.PhoneExt.NullIfEmpty()
                    }
                }
            });

            var newRisk = await _riskRepository.AddAsync(risk);
            model.SubmissionNumber = newRisk.SubmissionNumber;
            model.PolicyNumber = newRisk.PolicyNumber;
            model.RiskId = newRisk.Id;
            model.IsRenewal = newRisk.IsRenewal;
            model.PreviousPolicyNumber = newRisk.PreviousPolicyNumber;

            // PrimaryCityOfOperations Address
            var primaryAddress = new EntityAddressDTO()
            {
                Id = Guid.NewGuid(),
                EntityId = entityId,
                Address = new AddressDTO()
                {
                    Id = Guid.NewGuid(),
                    ZipCode = model.ZipCodeOperations,
                    City = model.CityOperations,
                    State = model.StateOperations
                }
            };

            if (model.ZipCodeOperations != null && model.CityOperations != null)
            {
                await _entityAddressRepository.AddAsync(new EntityAddress()
                {
                    Id = primaryAddress.Id.Value,
                    EntityId = primaryAddress.EntityId.Value,
                    Address = new Address()
                    {
                        Id = primaryAddress.Address.Id,
                        ZipCode = primaryAddress.Address.ZipCode,
                        City = primaryAddress.Address.City,
                        State = primaryAddress.Address.State
                    }
                });
            }

            // Save Primary Address to Destination
            await _riskSpecificDestinationRepository.AddAsync(new RiskSpecificDestination()
            {
                RiskDetailId = model.RiskDetailId.Value,
                City = primaryAddress.Address.City,
                State = primaryAddress.Address.State,
                ZipCode = primaryAddress.Address.ZipCode,
                PercentageOfTravelGiven = 100, // Default to 100%
                PercentageOfTravelActual = 100, // Default to 100%
                IsActive = true
            });

            model.Addresses.ForEach(x =>
            {
                x.Id = Guid.NewGuid();
                x.Address.Id = Guid.NewGuid();
            });

            var addresses = model.Addresses.Select(item => new EntityAddress
            {
                Id = item.Id.Value,
                EntityId = entityId,
                AddressTypeId = item.AddressTypeId,
                IsMainGaragingAddress = item.Address.IsMainGarage,
                Address = new Address()
                {
                    Id = item.Address.Id,
                    StreetAddress1 = item.Address.StreetAddress1?.Trim(),
                    ZipCode = item.Address.ZipCode,
                    City = item.Address.City,
                    State = item.Address.State,
                    County = item.Address.County,
                    IsMainGarage = item.Address.IsMainGarage
                }
            });

            var billingAddress = model.Addresses.FirstOrDefault(x => x.AddressTypeId == 1);
            await _riskPolicyContactRepository.AddAsync(new RiskPolicyContact()
            {
                Id = Guid.NewGuid(),
                RiskDetailId = model.RiskDetailId.Value,
                TypeId = 1, //Billing Contacts
                Entity = new Entity()
                {
                    FullName = billingAddress.ContactName,
                    WorkEmailAddress = billingAddress.Email,
                    WorkPhone = billingAddress.Phone
                }
            });

            await _entityAddressRepository.AddRangeAsync(addresses);

            await _riskRepository.SaveChangesAsync();
            await _entityAddressRepository.SaveChangesAsync();
            await _riskPolicyContactRepository.SaveChangesAsync();
            await _riskSpecificDestinationRepository.SaveChangesAsync();

            var fieldVersions = await _riskRepository.GetFieldVersions();
            model.FieldVersions = _mapper.Map<List<FieldVersionDTO>>(fieldVersions);

            return model;
        }

        public async Task<NameAndAddressDTO> UpdateAsync(NameAndAddressDTO model)
        {
            //Get Risk and AddressList
            await this.validateAddress(model);
            var currentPrimary = new Data.Entity.Address();
            var riskDetail = await _riskDetailRepository.GetInsuredAsync(model.RiskDetailId.Value);

            if (riskDetail.IsProcessed ?? false == true)
                return model; // Do not allow saving for processed policies

            var entityAddresses = await _entityAddressRepository.GetAsync(x => x.EntityId == riskDetail.Insured.EntityId,
                                    i => i.Address);
            foreach (var item in entityAddresses)
            {
                if (item.AddressTypeId == null)
                {
                    currentPrimary.City = item.Address.City;
                    currentPrimary.ZipCode = item.Address.ZipCode;
                }
                _addressRepository.Remove(item.Address);
            }
            var result = await _addressRepository.SaveChangesAsync();

            // Update policyNumber 
            var risk = await _riskRepository.FindAsync(x => x.Id == riskDetail.RiskId);
            if (risk.PolicyNumber != null) // &&
                                           //(risk.IsRenewal == false || risk.IsFromSN == true || risk.IsFromRenewal == true))
            {
                var brokerInfo = await _brokerInfoService.GetByRiskDetailIdAsync(riskDetail.RiskId);
                risk.PolicyNumber = _riskSequence.UpdatePolicyNumber(risk.PolicyNumber, brokerInfo.ExpirationDate.Value, brokerInfo.EffectiveDate.Value, model.State);
            }
            model.PolicyNumber = risk.PolicyNumber;
            model.RiskId = risk.Id;

            if (!risk.IsFromRenewal && string.IsNullOrWhiteSpace(risk.PolicyNumber))
            {
                risk.IsRenewal = model.IsRenewal;
                risk.PreviousPolicyNumber = model.PreviousPolicyNumber;
            }

            //risk.PrimaryCityZipCodeOfOperationsId = model.PrimaryCityZipCodeOfOperationsId;
            riskDetail.Insured.Entity.CompanyName = model.BusinessName?.Trim();
            riskDetail.Insured.Entity.DBA = StringExtensions.NullIfEmpty(model.InsuredDBA);
            riskDetail.Insured.Entity.BusinessPrincipal = StringExtensions.NullIfEmpty(model.BusinessPrincipal);
            riskDetail.Insured.Entity.WorkPhoneExtension = StringExtensions.NullIfEmpty(model.PhoneExt);
            riskDetail.Insured.Entity.WorkEmailAddress = StringExtensions.NullIfEmpty(model.Email);
            riskDetail.Insured.Entity.PersonalEmailAddress = StringExtensions.NullIfEmpty(model.Email);

            //PrimaryCityOfOperations Address
            await _entityAddressRepository.AddAsync(new Data.Entity.EntityAddress()
            {
                Id = Guid.NewGuid(),
                EntityId = riskDetail.Insured.EntityId,
                CreatedBy = 1, //temp
                Address = new Data.Entity.Address()
                {
                    Id = Guid.NewGuid(),
                    ZipCode = model.ZipCodeOperations,
                    City = model.CityOperations,
                    State = model.StateOperations,
                    UniqueId = Guid.NewGuid()
                }
            });

            // Update destination info if exist
            var destination = await _riskSpecificDestinationRepository.FindAsync(x => x.City == currentPrimary.City && x.ZipCode == currentPrimary.ZipCode && x.RiskDetailId == model.RiskDetailId.Value);
            if (destination != null && model.CityOperations != null && model.ZipCodeOperations != null)
            {
                var isExist = await _riskSpecificDestinationRepository.AnyAsync(x => x.City == model.CityOperations && x.ZipCode == model.ZipCodeOperations
                                && x.RiskDetailId == model.RiskDetailId.Value);
                if (!isExist)
                {
                    destination.City = model.CityOperations;
                    destination.ZipCode = model.ZipCodeOperations;
                    destination.State = model.StateOperations;
                    _riskSpecificDestinationRepository.Update(destination);
                }
            }

            //Save EntityAddresses
            foreach (var item in model.Addresses)
            {
                var id = item.Id.HasValue ? item.Id.Value : Guid.NewGuid();
                await _entityAddressRepository.AddAsync(new Data.Entity.EntityAddress()
                {
                    Id = id,
                    EntityId = riskDetail.Insured.EntityId,
                    AddressTypeId = item.AddressTypeId,
                    IsMainGaragingAddress = item.Address.IsMainGarage,
                    CreatedBy = 1, //temp
                    Address = new Data.Entity.Address()
                    {
                        Id = item.AddressId ?? Guid.Empty,
                        StreetAddress1 = item.Address.StreetAddress1?.Trim(),
                        ZipCode = item.Address.ZipCode,
                        City = item.Address.City,
                        State = item.Address.State,
                        County = item.Address.County,
                        IsMainGarage = item.Address.IsMainGarage,
                        UniqueId = Guid.NewGuid()
                    }
                });

                //Update Policy Contact Billing
                if (item.AddressTypeId == (short)AddressTypesEnum.Billing)
                {
                    var billingAddress = await _riskPolicyContactRepository.GetByRiskDetailIdIncludeAsync(model.RiskDetailId.Value, (short)AddressTypesEnum.Billing);
                    if (billingAddress != null)
                    {
                        billingAddress.Entity.FullName = item.ContactName;
                        billingAddress.Entity.WorkEmailAddress = item.Email;
                        billingAddress.Entity.WorkPhone = item.Phone;
                    }
                    else
                    {
                        await _riskPolicyContactRepository.AddAsync(new RiskPolicyContact()
                        {
                            Id = Guid.NewGuid(),
                            RiskDetailId = model.RiskDetailId.Value,
                            TypeId = 1, //Billing Contacts
                            Entity = new Entity()
                            {
                                FullName = item.ContactName,
                                WorkEmailAddress = item.Email,
                                WorkPhone = item.Phone
                            }
                        });
                    }
                }
            }

            await _riskRepository.SaveChangesAsync();
            await _riskDetailRepository.SaveChangesAsync();
            await _entityAddressRepository.SaveChangesAsync();
            await _riskPolicyContactRepository.SaveChangesAsync();
            await _riskSpecificDestinationRepository.SaveChangesAsync();

            return model;
        }

        public async Task validateMidtermAsyncAddress(Address itemAddress, AddressDTO modelAddress)
        {
            if (itemAddress.ZipCode != modelAddress.ZipCode || itemAddress.City != modelAddress.City)
            {
                throw new Exception("Invalid Address");
            }
            else
            {
                await AddAuthHeaderToken();
                (List<string> cities, string stateCode, string status) = await GetCityState(modelAddress.ZipCode);
                bool cityMatch = cities.Exists(it => it == modelAddress.City) || (cities.Count == 1 && modelAddress.City == null);
                if (modelAddress.State != stateCode || !cityMatch || status == "failed")
                {
                    throw new Exception("Invalid Address");
                }
            }
        }

        public async Task<NameAndAddressDTO> UpdateMidtermAsync(NameAndAddressDTO model)
        {
            //Get Risk and AddressList
            var nameAndAddress = await GetNameAndAddress(model.RiskDetailId.Value);
            if (nameAndAddress.BusinessAddress != model.BusinessAddress)
            {
                throw new Exception("Invalid business address.");
            }
            var risk = await _riskDetailRepository.GetInsuredAsync(model.RiskDetailId.Value);
            var entityAddress = await _entityAddressRepository.GetAsync(x => x.EntityId == risk.Insured.EntityId, i => i.Address);

            //Update IsMainGaragingAddress Property of Entity Address
            foreach (var item in entityAddress)
            {
                var addr = model.Addresses.FirstOrDefault(x => x.AddressId == item.AddressId);
                if (addr == null) continue;
                item.IsMainGaragingAddress = addr.Address.IsMainGarage;
                await validateMidtermAsyncAddress(item.Address, addr.Address);
                _entityAddressRepository.Update(item);
            }
            var result = await _entityAddressRepository.SaveChangesAsync();

            foreach (var item in entityAddress)
            {
                var addr = model.Addresses.FirstOrDefault(x => x.AddressId == item.AddressId);
                if (addr == null) continue;
                item.AddressTypeId = addr.AddressTypeId;
                item.Address.IsMainGarage = addr.Address.IsMainGarage;
                item.Address.StreetAddress1 = addr.Address.StreetAddress1;
                item.Address.City = addr.Address.City;
                item.Address.State = addr.Address.State;
                item.Address.ZipCode = addr.Address.ZipCode;
                _addressRepository.Update(item.Address);

                //Update Policy Contact Billing
                if (item.AddressTypeId == (short)AddressTypesEnum.Billing)
                {
                    var billingAddress = await _riskPolicyContactRepository.GetByRiskDetailIdIncludeAsync(model.RiskDetailId.Value, (short)AddressTypesEnum.Billing);
                    if (billingAddress != null)
                    {
                        billingAddress.Entity.FullName = addr.ContactName;
                        billingAddress.Entity.WorkEmailAddress = addr.Email;
                        billingAddress.Entity.WorkPhone = addr.Phone;
                    }
                    else
                    {
                        await _riskPolicyContactRepository.AddAsync(new RiskPolicyContact()
                        {
                            Id = Guid.NewGuid(),
                            RiskDetailId = model.RiskDetailId.Value,
                            TypeId = 1, //Billing Contacts
                            Entity = new Entity()
                            {
                                FullName = addr.ContactName,
                                WorkEmailAddress = addr.Email,
                                WorkPhone = addr.Phone
                            }
                        });
                    }
                }

            }
            result = await _addressRepository.SaveChangesAsync();

            //Save EntityAddresses
            //var newAddr = model.Addresses.Where(x => entityAddress.All(current => current.AddressId != x.AddressId));
            //var newAddresses = new List<EntityAddress>();
            //foreach (var addr in newAddr)
            //{
            //    var id = addr.Id.HasValue ? addr.Id.Value : Guid.NewGuid();
            //    var newEntityAddress = new Data.Entity.EntityAddress()
            //    {
            //        Id = id,
            //        EntityId = risk.Insured.EntityId,
            //        AddressTypeId = addr.AddressTypeId,
            //        CreatedBy = 1, //temp
            //        Address = new Data.Entity.Address()
            //        {
            //            Id = id,
            //            StreetAddress1 = addr.Address.StreetAddress1,
            //            ZipCode = addr.Address.ZipCode,
            //            City = addr.Address.City,
            //            State = addr.Address.State,
            //            County = addr.Address.County,
            //            IsMainGarage = addr.Address.IsMainGarage
            //        }
            //    };
            //    newAddresses.Add(newEntityAddress);
            //}

            //await _entityAddressRepository.AddRangeAsync(newAddresses);

            // update only business name
            risk.Insured.Entity.CompanyName = model.BusinessName;

            await _riskDetailRepository.SaveChangesAsync();
            await _entityAddressRepository.SaveChangesAsync();
            await _riskPolicyContactRepository.SaveChangesAsync();

            return model;
        }

        public async Task RemoveAddressAsync(Guid id)
        {
            //Get Risk and AddressList
            var entityAddress = await _entityAddressRepository.GetAsync(x => x.AddressId == id, i => i.Address);

            _addressRepository.Remove(entityAddress.FirstOrDefault().Address);
            _entityAddressRepository.Remove(entityAddress.FirstOrDefault());

            var result = await _addressRepository.SaveChangesAsync();
            await _entityAddressRepository.SaveChangesAsync();
        }

        public async Task InsertEntityAddressAsync(EntityAddressDTO model)
        {
            var risk = await _riskDetailRepository.GetInsuredAsync(model.RiskDetailId);
            var id = Guid.NewGuid();
            var newAddress = _mapper.Map<EntityAddress>(model);
            newAddress.Id = id;
            newAddress.EntityId = risk.Insured.EntityId;
            newAddress.AddressTypeId = model.AddressTypeId;
            newAddress.CreatedBy = 1; //temp
            newAddress.Address.UniqueId = Guid.NewGuid();
            newAddress.IsMainGaragingAddress = model.IsMainGaragingAddress;

            await _entityAddressRepository.AddAsync(newAddress);
            await _entityAddressRepository.SaveChangesAsync();
        }
    }
}
