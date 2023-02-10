using CliveBot.Web.Policies;
using CliveBot.Application.Moderators.Queries;
using Microsoft.AspNetCore.Mvc;
using CliveBot.Application.Moderators;
using CliveBot.Application.Moderators.Commands;
using System.Runtime.ConstrainedExecution;

namespace CliveBot.Web.Controllers
{
    //[ModAuthorize(ManageModerators: true)]
    public class ModeratorController : ApiBaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<ModeratorDto>>> GetAllModerators()
        {
            return await Mediator.Send(new ModeratorList.Query());
        }

        [HttpPost]
        public async Task<ActionResult<List<ModeratorDto>>> AddMod(ModeratorAdd.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<ModeratorDto>>> AddMod(int id, ModeratorEdit.Command command)
        {
            command.ModeratorId = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ModeratorDto>>> RemoveMod(int id)
        {   
            var command = new ModeratorRemove.Command() {
                ModeratorId = id,
            };
            return await Mediator.Send(command);
        }
    }
}
