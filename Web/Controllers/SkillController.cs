using CliveBot.Application.SkillLanguages;
using CliveBot.Application.Skills;
using CliveBot.Application.Skills.Queries;
using CliveBot.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace CliveBot.Web.Controllers
{
    /// <summary>
    /// Skills of Clive and it's Eikons
    /// </summary>
    public class SkillController : ApiBaseController
    {
        /// <summary>
        /// Retrieves a List of all the skills
        /// </summary>
        /// <param name="summon">Filter for Skill Summon</param>
        /// <returns>List of Skills</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SkillDto>))]
        public async Task<List<SkillDto>> GetAllSkills(SkillSummon? summon)
        {
            return await Mediator.Send(new SkillList.Query() { Summon = summon });
        }

        /// <summary>
        /// Fetches a Skill from an id
        /// </summary>
        /// <param name="id">Id of Skill</param>
        /// <returns>Skill Id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SkillDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<SkillDto> GetSkill(int id)
        {
            return await Mediator.Send(new SkillDetails.Query() { SkillId = id });
        }

        [HttpGet("{id}/languages")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SkillDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<SkillLanguageDto>> GetSkillLanguages(int id)
        {
            return await Mediator.Send(new SkillLanguageList.Query() { SkillId = id });
        }
    }
}
