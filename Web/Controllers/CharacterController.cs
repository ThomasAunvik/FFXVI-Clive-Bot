using CliveBot.Application.SkillLanguages;
using CliveBot.Application.SkillLanguages.Commands;
using CliveBot.Application.Skills;
using CliveBot.Application.Skills.Commands;
using CliveBot.Application.Skills.Queries;
using CliveBot.Database.Models;
using CliveBot.Web.Policies;
using Microsoft.AspNetCore.Mvc;

namespace CliveBot.Web.Controllers
{
    /// <summary>
    /// Skills of Clive and it's Eikons
    /// </summary>
    [ModAuthorize]
    public class CharacterController : ApiBaseController
    {
        /// <summary>
        /// Retrieves a List of all the skills
        /// </summary>
        /// <param name="summon">Filter for Skill Summon</param>
        /// <returns>List of Skills</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ActionResult<SkillDto>>))]
        public async Task<List<SkillDto>> GetAllCharacters(SkillSummon? summon)
        {
            return await Mediator.Send(new SkillList.Query() { Summon = summon });
        }
    }
}
