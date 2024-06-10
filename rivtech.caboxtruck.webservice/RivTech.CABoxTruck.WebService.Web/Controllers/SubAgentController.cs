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
    public class SubAgentController : ControllerBase
    {
        private readonly ISubAgentService _service;

        public SubAgentController(ISubAgentService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        [Produces(typeof(List<AgentSubAgencyDTO>))]
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
        [Produces(typeof(List<AgentSubAgencyDTO>))]
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
        [Produces(typeof(AgentSubAgencyDTO))]
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
        [Produces(typeof(AgentSubAgencyDTO))]
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
        [Produces(typeof(AgentSubAgencyDTO))]
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
        [Produces(typeof(AgentSubAgencyDTO))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] AgentSubAgencyDTO resource)
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
        [Produces(typeof(AgentSubAgencyDTO))]
        public async Task<IActionResult> Post([FromBody] AgentSubAgencyDTO resource)
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
        [Produces(typeof(AgentSubAgencyDTO))]
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
        public List<AgentSubAgencyDTO> Data = new List<AgentSubAgencyDTO> // TODO: temporary only
                    {
                        new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("70d9adee-55f1-49ad-ad23-a5d2de1e2ca6"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "SubJames", LastName = "Business",
                                 WorkPhone = "(800) 980 - 1950",
                                 WorkEmailAddress = "SubJamesb@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("70d9adee-55f1-49ad-ad23-a5d2de1e2ca6"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "SubJames2", LastName = "Business2",
                                 WorkPhone = "(800) 980 - 1950",
                                 WorkEmailAddress = "SubJamesb2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("03157b6a-2a78-4574-9e8a-d6d7365f2c16"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),// guid used in agent entry for demo data
                                 IsIndividual = true,
                                 FirstName = "SubJoe", LastName = "Specialty",
                                 WorkPhone = "(561) 683 - 1220",
                                 WorkEmailAddress = "SubJoeS@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                         new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("03157b6a-2a78-4574-9e8a-d6d7365f2c16"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),// guid used in agent entry for demo data
                                 IsIndividual = true,
                                 FirstName = "SubJoe2", LastName = "Specialty2",
                                 WorkPhone = "(561) 683 - 1220",
                                 WorkEmailAddress = "SubJoeS2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("d5738aab-c459-4027-9e88-b29f4dba70a7"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                   IsIndividual = true,
                                 FirstName = "SubCamille", LastName = "Preferred",
                                 WorkPhone = "(973) 845 - 6004",
                                 WorkEmailAddress = "SubCamilleP@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("d5738aab-c459-4027-9e88-b29f4dba70a7"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                   IsIndividual = true,
                                 FirstName = "SubCamille2", LastName = "Preferred2",
                                 WorkPhone = "(973) 845 - 6004",
                                 WorkEmailAddress = "SubCamilleP2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("7bc3e47e-73a5-4121-b5b3-3963efab5dd7"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "SubMark", LastName = "Risk",
                                 WorkPhone = "(609) 714 - 7760",
                                 WorkEmailAddress = "SubmarkR@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                          new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("7bc3e47e-73a5-4121-b5b3-3963efab5dd7"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "SubMark2", LastName = "Risk2",
                                 WorkPhone = "(609) 714 - 7760",
                                 WorkEmailAddress = "SubmarkR2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentSubAgencyDTO
                        {
                           Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("bc86fcfc-8c89-4d9f-bb9c-dfe071914676"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "SubMiriam", LastName = "Quiambo",
                                 WorkPhone = "(440) 934 - 7766",
                                 WorkEmailAddress = "SubMiriamQ@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                         new AgentSubAgencyDTO
                        {
                           Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("bc86fcfc-8c89-4d9f-bb9c-dfe071914676"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "SubMiriam2", LastName = "Quiambo2",
                                 WorkPhone = "(440) 934 - 7766",
                                 WorkEmailAddress = "SubMiriamQ2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("f4bb2db1-e11a-4683-94ee-e6ce810d03a5"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                  IsIndividual = true,
                                 FirstName = "SubCarlo", LastName = "Onyx",
                                 WorkPhone = "(415) 374 - 2772",
                                 WorkEmailAddress = "SubCarlo@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentSubAgencyDTO
                        {
                            Id = Guid.NewGuid(), // guid used in agent entry for demo data
                            SubAgencyId = Guid.Parse("f4bb2db1-e11a-4683-94ee-e6ce810d03a5"),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                  IsIndividual = true,
                                 FirstName = "SubCarlo2", LastName = "Onyx2",
                                 WorkPhone = "(415) 374 - 2772",
                                 WorkEmailAddress = "SubCarlo2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        }
                    };
        #endregion Data
    }
}
