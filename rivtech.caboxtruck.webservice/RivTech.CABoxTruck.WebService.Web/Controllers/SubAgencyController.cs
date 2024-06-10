using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SubAgencyController : ControllerBase
    {
        private readonly ISubAgencyService _service;

        public SubAgencyController(ISubAgencyService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        [Produces(typeof(List<SubAgencyDTO>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _service.GetAllAsync();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllInclude")]
        [Produces(typeof(List<SubAgencyDTO>))]
        public async Task<IActionResult> GetAllInclude()
        {
            try
            {
                var lst = await _service.GetAllIncludeAsync();

                return Ok(lst);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpGet("GetByAgencyIdInclude/{id}")]
        [Produces(typeof(SubAgencyDTO))]
        public async Task<IActionResult> GetByAgencyIdInclude([FromRoute] Guid id)
        {
            try
            {
                var lst = await _service.GetAllIncludeAsync();

                var result = this.Data;

                if (lst.Count == 0)
                {
                    for (var i = 0; i < result.Count; i++)
                    {
                        await _service.InsertAsync(result[i]);
                    }
                }

                var res = await _service.GetByAgencyIdIncludeAsync(id);

                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Get/{id}")]
        [Produces(typeof(SubAgencyDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetInclude/{id}")]
        [Produces(typeof(SubAgencyDTO))]
        public async Task<IActionResult> GetInclude([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByIdIncludeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Put/{id}")]
        [Produces(typeof(SubAgencyDTO))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] SubAgencyDTO resource)
        {
            try
            {
                try
                {
                    if (resource.Id != id) return BadRequest();
                    return Ok(await _service.UpdateAsync(resource));
                }
                catch (ModelNotFoundException)
                {
                    return NotFound();
                }
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Post")]
        [Produces(typeof(SubAgencyDTO))]
        public async Task<IActionResult> Post([FromBody] SubAgencyDTO resource)
        {
            try
            {
                if (resource.Id != null) return BadRequest();
                return Ok(await _service.InsertAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpDelete("Delete/{id}")]
        [Produces(typeof(SubAgencyDTO))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty) return BadRequest();
                await _service.RemoveAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        #region Data for demo
        public List<SubAgencyDTO> Data = new List<SubAgencyDTO> // TODO: temporary only
                    {
                        new SubAgencyDTO
                        {
                            Id = Guid.Parse("70d9adee-55f1-49ad-ad23-a5d2de1e2ca6"), // guid used in agent entry for demo data
                            AgencyId = Guid.Parse("3729f3b5-14b6-49d9-98ca-d934575e5d54"),
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = false,
                                 CompanyName = "American Business Insurance Services",
                                 WorkPhone = "(800) 980 - 1950",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new SubAgencyDTO
                        {
                            Id = Guid.Parse("03157b6a-2a78-4574-9e8a-d6d7365f2c16"),// guid used in agent entry for demo data
                            AgencyId = Guid.Parse("bde8bebb-1d38-4361-856f-bcac13132541"),
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),// guid used in agent entry for demo data
                                 IsIndividual = false,
                                 CompanyName = "Sub - American Specialty Insurance Group, Inc",
                                 WorkPhone = "(561) 683 - 1220",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new SubAgencyDTO
                        {
                            Id = Guid.Parse("d5738aab-c459-4027-9e88-b29f4dba70a7"),// guid used in agent entry for demo data
                            AgencyId = Guid.Parse("16350fd2-de5a-47ff-a1b8-d4173546a333"),
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = false,
                                 CompanyName = "Sub - Preferred Risk Agency",
                                 WorkPhone = "(973) 845 - 6004",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new SubAgencyDTO
                        {
                            Id = Guid.Parse("7bc3e47e-73a5-4121-b5b3-3963efab5dd7"),// guid used in agent entry for demo data
                            AgencyId = Guid.Parse("99ce9195-aefd-4f4e-87f4-cec4086c512f"),
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = false,
                                 CompanyName = "Sub - Risk Partners Inc.",
                                 WorkPhone = "(609) 714 - 7760",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new SubAgencyDTO
                        {
                            Id = Guid.Parse("bc86fcfc-8c89-4d9f-bb9c-dfe071914676"),// guid used in agent entry for demo data
                            AgencyId = Guid.Parse("05f3130e-180f-4a65-815b-d847cfc3825b"),
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = false,
                                 CompanyName = "Sub - Miriam L Morency New America Insurance",
                                 WorkPhone = "(440) 934 - 7766",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new SubAgencyDTO
                        {
                            Id = Guid.Parse("f4bb2db1-e11a-4683-94ee-e6ce810d03a5"),// guid used in agent entry for demo data
                            AgencyId = Guid.Parse("79ad4115-5cee-4ecf-9c4f-3d0bca607d8d"),
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = false,
                                 CompanyName = "Sub - Onyx Transportation Services",
                                 WorkPhone = "(415) 374 - 2772",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        }
                    };
        #endregion Data
    }
}
