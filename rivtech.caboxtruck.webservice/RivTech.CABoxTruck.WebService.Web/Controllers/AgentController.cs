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
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _service;

        public AgentController(IAgentService service)
        {
            _service = service;
          
        }

        [HttpGet("GetAll")]
        [Produces(typeof(List<AgentDTO>))]
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
        [Produces(typeof(List<AgentDTO>))]
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
        [Produces(typeof(AgentDTO))]
        public async Task<IActionResult> GetByAgencyIdInclude([FromRoute] Guid id)
        {
            try
            {
                var lst = await _service.GetAllIncludeAsync();
                var result = this.Data;

                if (lst.Count == 0)
                {
                    for (var i = 0; i < result.Count; i++) 
                        await _service.InsertAsync(result[i]);

                    lst = await _service.GetAllIncludeAsync();
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
        [Produces(typeof(AgentDTO))]
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
        [Produces(typeof(AgentDTO))]
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
        [Produces(typeof(AgentDTO))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] AgentDTO resource)
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
        [Produces(typeof(AgentDTO))]
        public async Task<IActionResult> Post([FromBody] AgentDTO resource)
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
        [Produces(typeof(AgentDTO))]
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
        public List<AgentDTO> Data = new List<AgentDTO> // TODO: temporary only
                    {
                        new AgentDTO
                        {
                            Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("3729f3b5-14b6-49d9-98ca-d934575e5d54"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "James", LastName = "Business",
                                 WorkPhone = "(800) 980 - 1950",
                                 WorkEmailAddress = "jamesb@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                            Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("3729f3b5-14b6-49d9-98ca-d934575e5d54"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "James2", LastName = "Business2",
                                 WorkPhone = "(800) 980 - 1950",
                                 WorkEmailAddress = "jamesb2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                             Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("bde8bebb-1d38-4361-856f-bcac13132541"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Joe", LastName = "Specialty",
                                 WorkPhone = "(561) 683 - 1220",
                                 WorkEmailAddress = "joeS@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                         new AgentDTO
                        {
                             Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("bde8bebb-1d38-4361-856f-bcac13132541"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Joe2", LastName = "Specialty2",
                                 WorkPhone = "(561) 683 - 1220",
                                 WorkEmailAddress = "joeS2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                             Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("16350fd2-de5a-47ff-a1b8-d4173546a333"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Camille", LastName = "Preferred",
                                 WorkPhone = "(973) 845 - 6004",
                                 WorkEmailAddress = "CamilleP@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                             Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("16350fd2-de5a-47ff-a1b8-d4173546a333"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Camille2", LastName = "Preferred2",
                                 WorkPhone = "(973) 845 - 6004",
                                 WorkEmailAddress = "CamilleP2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                             Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("99ce9195-aefd-4f4e-87f4-cec4086c512f"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Mark", LastName = "Risk",
                                 WorkPhone = "(609) 714 - 7760",
                                 WorkEmailAddress = "MarkR@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                             Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("99ce9195-aefd-4f4e-87f4-cec4086c512f"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Mark2", LastName = "Risk2",
                                 WorkPhone = "(609) 714 - 7760",
                                 WorkEmailAddress = "MarkR2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                            Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("05f3130e-180f-4a65-815b-d847cfc3825b"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Miriam", LastName = "Quiambo",
                                 WorkPhone = "(440) 934 - 7766",
                                 WorkEmailAddress = "MiriamQ@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                            Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("05f3130e-180f-4a65-815b-d847cfc3825b"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Miriam2", LastName = "Quiambo2",
                                 WorkPhone = "(440) 934 - 7766",
                                 WorkEmailAddress = "MiriamQ2@email.com",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                            Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("79ad4115-5cee-4ecf-9c4f-3d0bca607d8d"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Carlo", LastName = "Onyx",
                                 WorkEmailAddress = "CarloO@email.com",
                                 WorkPhone = "(415) 374 - 2772",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        },
                        new AgentDTO
                        {
                            Id = Guid.NewGuid(),
                            AgencyId = Guid.Parse("79ad4115-5cee-4ecf-9c4f-3d0bca607d8d"),// guid used in agent entry for demo data
                            CreatedBy = 1,
                            CreatedDate = new DateTime(2021, 1 ,1),
                            Entity = new EntityDTO
                            {
                                 Id = Guid.NewGuid(),
                                 IsIndividual = true,
                                 FirstName = "Carlo2", LastName = "Onyx2",
                                 WorkEmailAddress = "CarloO2@email.com",
                                 WorkPhone = "(415) 374 - 2772",
                                 CreatedBy = 1,
                                 CreatedDate = new DateTime(2021, 1 ,1)
                            }
                        }
                    };
        #endregion Data
    }
}
