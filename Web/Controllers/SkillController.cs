using CliveBot.Application.SkillLanguages;
using CliveBot.Application.Skills;
using CliveBot.Application.Skills.Commands;
using CliveBot.Application.Skills.Queries;
using CliveBot.Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;

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

        /// <summary>
        /// Edits a Skill
        /// </summary>
        /// <param name="id">Skill Id</param>
        /// <param name="skill">Skill Object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SkillDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<SkillDto> EditSkill(int id, Edit.Command skill)
        {
            skill.SkillId = id;
            return await Mediator.Send(skill);
        }

        [HttpGet("{id}/languages")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SkillDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<SkillLanguageDto>> GetSkillLanguages(int id)
        {
            return await Mediator.Send(new SkillLanguageList.Query() { SkillId = id });
        }

        [HttpPost("{id}/images/icon")]
        
        public async Task<SkillDto> UpdateSkillIconImage(int id, IFormFile iconFile)
        {
            return await Mediator.Send(new UploadIconImage.Command { File = iconFile, SkillId = id });
        }

        [HttpPost("{id}/images/preview")]
        public async Task<SkillDto> UpdateSkillPreviewImage(int id, IFormFile previewFile)
        {
            return await Mediator.Send(new UploadPreviewImage.Command { File = previewFile, SkillId = id });

        }
    }
}
