using CliveBot.Application.Characters;
using CliveBot.Application.Characters.Commands;
using CliveBot.Application.Characters.Queries;
using CliveBot.Application.Skills.Commands;
using CliveBot.Application.Skills;
using CliveBot.Web.Policies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CliveBot.Web.Controllers
{
    /// <summary>
    /// Characters of Valisthea
    /// </summary>
    [ModAuthorize]
    public class CharacterController : ApiBaseController
    {
        /// <summary>
        /// Retrieves a List of all the characters
        /// </summary>
        /// <returns>List of Characters</returns>
        [HttpGet]

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<List<CharacterDto>>))]
        public async Task<List<CharacterDto>> GetAllCharacters()
        {
            return await Mediator.Send(new CharacterList.Query());
        }

        [HttpGet("{id}")]

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CharacterDto>))]
        public async Task<CharacterDto> GetCharacter(int id)
        {
            return await Mediator.Send(new CharacterDetails.Query
            {
                CharacterId = id
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CharacterDto>))]
        public async Task<CharacterDto> CreateCharacter(CharacterCreate.Command character)
        {
            return await Mediator.Send(character);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CharacterDto>))]
        public async Task<CharacterDto> EditCharacter(int id, CharacterEdit.Command character)
        {
            character.CharacterId = id;
            return await Mediator.Send(character);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Unit))]
        public async Task<Unit> DeleteCharacter(int id)
        {
            return await Mediator.Send(new CharacterRemove.Command()
            {
                CharacterId = id
            });
        }

        // VARIANT
        [HttpPost("{characterId}/variant")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CharacterVariantDto>))]
        public async Task<CharacterVariantDto> CreateVariant(int characterId, CharacterVariantCreate.Command variant)
        {
            variant.CharacterId = characterId;
            return await Mediator.Send(variant);
        }

        [HttpPut("{characterId}/variant/{variantId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CharacterVariantDto>))]
        public async Task<CharacterVariantDto> EditVariant(int characterId, int variantId, CharacterVariantEdit.Command character)
        {
            character.CharacterId = characterId;
            character.VariantId = variantId;
            return await Mediator.Send(character);
        }

        [HttpDelete("{characterId}/variant/{variantId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Unit))]
        public async Task<Unit> DeleteVariant(int variantId)
        {
            return await Mediator.Send(new CharacterVariantRemove.Command()
            {
                VariantId = variantId,
            });
        }

        [HttpPost("{characterId}/variant/{variantId}/images/preview")]
        [ModAuthorize(ManageCharacterInfo: true)]
        public async Task<ActionResult<CharacterVariantDto>> UpdateVariantPreviewImage(int variantId, IFormFile previewFile)
        {
            return await Mediator.Send(new CharacterVariantPreviewImage.Command { File = previewFile, VariantId = variantId });
        }

        // Notes
        [HttpGet("{characterId}/notes")]

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<List<CharacterNoteDto>>))]
        public async Task<List<CharacterNoteDto>> CreateNote(int characterId, CharacterNoteList.Query query)
        {
            query.CharacterId = characterId;
            return await Mediator.Send(query);
        }

        [HttpPost("{characterId}/notes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CharacterNoteDto>))]
        public async Task<CharacterNoteDto> CreateNote(int characterId, CharacterNoteCreate.Command note)
        {
            note.CharacterId = characterId;
            return await Mediator.Send(note);
        }

        [HttpPut("{characterId}/notes/{noteId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<CharacterNoteDto>))]
        public async Task<CharacterNoteDto> EditNote(int characterId, int noteId, CharacterNoteEdit.Command note)
        {
            note.CharacterId = characterId;
            note.NoteId = noteId;
            return await Mediator.Send(note);
        }

        [HttpDelete("{characterId}/notes/{noteId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Unit))]
        public async Task<Unit> DeleteNote(int noteId)
        {
            return await Mediator.Send(new CharacterNoteRemove.Command()
            {
                NoteId = noteId
            });
        }

        [HttpPost("{characterId}/notes/{noteId}/images/preview")]
        [ModAuthorize(ManageCharacterNotes: true)]
        public async Task<ActionResult<CharacterNoteDto>> UpdateSkillPreviewImage(int noteId, IFormFile previewFile)
        {
            return await Mediator.Send(new CharacterNotePreviewImage.Command { File = previewFile, NoteId = noteId });
        }
    }
}
