using CliveBot.Application.SkillLanguages;
using CliveBot.Application.SkillLanguages.Commands;
using CliveBot.Application.Skills;
using CliveBot.Application.Skills.Commands;
using CliveBot.Application.Skills.Queries;
using CliveBot.Database.Models;
using CliveBot.Web.Policies;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ActionResult<SkillDto>>))]
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
        public async Task<ActionResult<SkillDto>> GetSkill(int id)
        {
            return await Mediator.Send(new SkillDetails.Query() { SkillId = id });
        }


        /// <summary>
        /// Searches for Skill
        /// </summary>
        /// <param name="skillName">Name of skill</param>
        /// <returns>Skill Id</returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SkillDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SkillDto>> SearchSkill(string skillName)
        {
            return await Mediator.Send(new SkillDetails.Query() { SkillName = skillName });
        }

        [HttpPost]
        [ModAuthorize(ManageSkills: true)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SkillDto))]
        public async Task<ActionResult<SkillDto>> EditSkill(SkillCreate.Command skill)
        {
            return await Mediator.Send(skill);
        }

        /// <summary>
        /// Edits a Skill
        /// </summary>
        /// <param name="id">Skill Id</param>
        /// <param name="skill">Skill Object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ModAuthorize(ManageSkillInfo: true)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SkillDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SkillDto>> EditSkill(int id, SkillEdit.Command skill)
        {
            skill.SkillId = id;
            return await Mediator.Send(skill);
        }

        [HttpGet("{id}/languages")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SkillLanguageDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<SkillLanguageDto>> GetSkillLanguages(int id)
        {
            return await Mediator.Send(new SkillLanguageList.Query() { SkillId = id });
        }

        [HttpPost("{id}/languages/{locale}")]
        [ModAuthorize(ManageSkillInfo: true)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SkillLanguageDto>))]
        public async Task<List<SkillLanguageDto>> CreateOrUpdateSkillLanguage(
            int id,
            string locale,
            ModeratorCreateOrEdit.Command language
        )   {
            language.EditSkillId = id;
            language.EditLocale = locale;
            return await Mediator.Send(language);
        }

        [HttpDelete("{id}/languages/{locale}")]
        [ModAuthorize(ManageSkillInfo: true)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SkillLanguageDto>))]
        public async Task<List<SkillLanguageDto>> RemoveSkillLanguage(
            int id,
            string locale
        )
        {
            var language = new SkillLanguageRemove.Command
            {
                EditSkillId = id,
                EditLocale = locale
            };
            return await Mediator.Send(language);
        }

        [HttpPost("{id}/images/icon")]
        [ModAuthorize(ManageSkillInfo: true)]
        public async Task<ActionResult<SkillDto>> UpdateSkillIconImage(int id, IFormFile iconFile)
        {
            return await Mediator.Send(new SkillUploadIconImage.Command { File = iconFile, SkillId = id });
        }

        [HttpPost("{id}/images/preview")]
        [ModAuthorize(ManageSkillInfo: true)]
        public async Task<ActionResult<SkillDto>> UpdateSkillPreviewImage(int id, IFormFile previewFile)
        {
            return await Mediator.Send(new SkillUploadPreviewImage.Command { File = previewFile, SkillId = id });
        }
    }
}
