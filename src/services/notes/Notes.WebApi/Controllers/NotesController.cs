﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Epos.Eventing;

using Notes.Model;
using Notes.WebApi.IntegrationEvents;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Notes.WebApi.Controllers
{
    /// <summary> Notes V1 Controller </summary>
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class NotesController : Controller
    {
        private readonly INoteRepository myNoteRepository;
        private readonly IIntegrationEventPublisher myIntegrationEventPublisher;

        /// <summary> Creates an instance of the <b>NotesController</b> class. </summary>
        /// <param name="noteRepository"> Note repository </param>
        /// <param name="integrationEventPublisher"> Integration Event Publisher </param>
        public NotesController(INoteRepository noteRepository, IIntegrationEventPublisher integrationEventPublisher) {
            myIntegrationEventPublisher = integrationEventPublisher;
            myNoteRepository = noteRepository;
        }

        /// <summary> Gets all notes. </summary>
        /// <returns> All notes </returns>
        /// <response code="200"> Success. </response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Note>), 200)]
        [SwaggerOperation(nameof(GetAllNotes))]
        public IEnumerable<Note> GetAllNotes() => myNoteRepository.GetAllNotes();

        /// <summary> Gets a note by id. </summary>
        /// <param name="id"> Note id </param>
        /// <returns> Note </returns>
        /// <response code="200"> The note is found. </response>
        /// <response code="404"> The note is not found. </response>
        [HttpGet("{id}", Name = nameof(GetNoteById))]
        [ProducesResponseType(typeof(Note), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(nameof(GetNoteById))]
        public IActionResult GetNoteById(string id) {
            Note theNote = myNoteRepository.GetNoteById(id);
            if (theNote == null) {
                return NotFound();
            }

            return new ObjectResult(theNote);
        }

        /// <summary> Adds a note. </summary>
        /// <param name="note"> Note </param>
        /// <returns> Note </returns>
        /// <response code="201"> The note is created. </response>
        [HttpPost]
        [ProducesResponseType(201)]
        [SwaggerOperation(nameof(AddNote))]
        public IActionResult AddNote([FromBody] Note note) {
            myNoteRepository.AddNote(note);

            myIntegrationEventPublisher.Publish(new NoteAddedIntegrationEvent { AddedNote = note });

            return CreatedAtRoute(nameof(GetNoteById), new { id = note.Id }, note);
        }

        /// <summary> Deletes a note. </summary>
        /// <param name="id"> Note id </param>
        /// <returns> Note </returns>
        /// <response code="204"> The note is deleted. </response>
        /// <response code="404"> The note is not found. </response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [SwaggerOperation(nameof(DeleteNote))]
        public IActionResult DeleteNote(string id) {
            if (myNoteRepository.Delete(id)) {
                return NoContent();
            }

            return NotFound();
        }


        /// <summary> Updates a note. </summary>
        /// <param name="id"> Note id </param>
        /// <param name="note"> Note </param>
        /// <returns> Note </returns>
        /// <response code="204"> The note is updated. </response>
        /// <response code="400"> The specified id is not the note's id. </response>
        /// <response code="404"> The note is not found. </response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerOperation(nameof(UpdateNote))]
        public IActionResult UpdateNote(string id, [FromBody] Note note) {
            if (id != note.Id) {
                return BadRequest();
            }

            if (myNoteRepository.Update(note)) {
                return NoContent();
            }

            return NotFound();
        }
    }
}
