using System;
using Xunit;
using Moq;
using NotesAPI.Services;
using NotesAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using NotesAPI.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace NotesAPI.Tests
{
    public class NotesAPITests
    {
        public Task<Note> DataNotes()
        {
            var notes = new Note()
            {
                ID = 1,
                Title = "My first note",
                Text = "Writing my first note",
                Pinned = true,
                Labels = new List<Label>
                    {
                        new Label { ID = 1, LabelName = "Work"},
                        new Label { ID = 2, LabelName = "Important"}
                    },
                CheckedList = new List<CheckedListItem>
                    {
                        new CheckedListItem { ID = 1, ListItem = "Laptop"},
                        new CheckedListItem { ID = 2, ListItem = "Bag"}
                    }
            };
            return Task.FromResult(notes);
        }

        public Task<List<Note>> DataNotesList()
        {
            var notes = new List<Note>()
                {
                    new Note()
                    {
                        ID = 1,
                        Title = "My first note",
                    Text = "Writing my first note",
                    Pinned = true,
                    Labels = new List<Label>
                    {
                        new Label { ID = 1, LabelName = "Archive"},
                        new Label { ID = 2, LabelName = "Work"}
                    },
                    CheckedList = new List<CheckedListItem>
                    {
                        new CheckedListItem { ID = 1, ListItem = "Charger"},
                        new CheckedListItem { ID = 2, ListItem = "Pen"}
                    }
                    },
                    new Note()
                    {
                        ID = 2,
                        Title = "My second note",
                    Text = "Writing my first note",
                    Pinned = false,
                    Labels = new List<Label>
                    {
                        new Label { ID = 3, LabelName = "Archive"},
                        new Label { ID = 4, LabelName = "Priority"}
                    },
                    CheckedList = new List<CheckedListItem>
                    {
                        new CheckedListItem { ID = 3, ListItem = "Laptop"},
                        new CheckedListItem { ID = 4, ListItem = "Bag"}
                    }
                    }
                };
            return Task.FromResult(notes);
        }

        public Task<List<Note>> DataSingleList()
        {
            var notes = new List<Note>()
            {
                new Note()
                {
                    ID = 1,
                    Title = "First note",
                    Text = "Writing my first note",
                    Pinned = true,
                    Labels = new List<Label>
                    {
                        new Label { ID = 1, LabelName = "Archive"},
                        new Label { ID = 2, LabelName = "Work"}
                    },
                    CheckedList = new List<CheckedListItem>
                    {
                        new CheckedListItem { ID = 1, ListItem = "Charger"},
                        new CheckedListItem { ID = 2, ListItem = "Pen"}
                    }
                }
            };
            return Task.FromResult(notes);
        }

        [Fact]
        public async void TestGetAllNotes()
        {
            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.GetNotes("", "", null)).Returns(DataNotesList());
            var controller = new NotesController(testMock.Object);

            var result = await controller.GetNotes("", "", null);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;          
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(2, notes.Count);
        }

        [Fact]
        public async void TestGetNotesByTitle()
        {
            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.GetNotes("First note", "", null)).Returns(DataSingleList());
            var controller = new NotesController(testMock.Object);

            var result = await controller.GetNotes("First note", "", null);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("First note", notes[0].Title);
        }


        [Fact]
        public async void TestGetNotesByLabel()
        {
            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.GetNotes("", "Work", null)).Returns(DataSingleList());
            var controller = new NotesController(testMock.Object);

            var result = await controller.GetNotes("", "Work", null);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("Work", notes[0].Labels[1].LabelName);
        }

        [Fact]
        public async void TestGetNoteByID()
        {
            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.GetNotesByID(1)).Returns(DataNotes());
            var controller = new NotesController(testMock.Object);
            var result = await controller.GetNotesByID(1);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as Note;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(1, notes.ID);
        }

        [Fact]
        public async void TestGetNoteByPinned()
        {
            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.GetNotes("", "", true)).Returns(DataSingleList());
            var controller = new NotesController(testMock.Object);

            var result = await controller.GetNotes("", "", true);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.True(notes[0].Pinned);
        }

        [Fact]
        public async void TestPostNote()
        {
            var note = new Note()
            {
                ID = 3,
                Title = "My third note",
                Text = "Writing my third note",
                Pinned = false,
                Labels = new List<Label>
                {
                    new Label { ID = 1, LabelName = "Work"},
                    new Label { ID = 2, LabelName = "Play"}
                },
                CheckedList = new List<CheckedListItem>
                {
                    new CheckedListItem { ID = 1, ListItem = "Pen"},
                    new CheckedListItem { ID = 2, ListItem = "Bag"}
                }
            };
            
            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.PostNotes(note)).Returns(Task.FromResult(note));
            var controller = new NotesController(testMock.Object);

            var result = await controller.PostNotes(note);
            var okObjectResult = result as CreatedAtActionResult;
            var notes = okObjectResult.Value as Note;
            Assert.Equal(201, okObjectResult.StatusCode);
            Assert.NotNull(notes);
        }

        [Fact]
        public async void TestPutNote()
        {
            var note = new Note()
            {

                ID = 4,
                Title = "My fourth note",
                Text = "Writing my fourth note",
                Pinned = false,
                Labels = new List<Label>
                    {
                        new Label { ID = 1, LabelName = "Work"},
                        new Label { ID = 2, LabelName = "Play"}
                    },
                CheckedList = new List<CheckedListItem>
                    {
                        new CheckedListItem { ID = 1, ListItem = "Pen"},
                        new CheckedListItem { ID = 2, ListItem = "Bag"}
                    }
            };

            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.PutNotes(note)).Returns(Task.FromResult(note));
            var controller = new NotesController(testMock.Object);

            var result = await controller.PutNotes(4, note);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as Note;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.NotNull(notes);
        }

        [Fact]
        public async void TestDeleteNoteByID()
        {
            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.DeleteNotes(1)).Returns(DataNotes());
            var controller = new NotesController(testMock.Object);
            var result = await controller.DeleteNotes(1);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as Note;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(1, notes.ID);
        }

        [Fact]
        public async void TestDeleteNoteByTitle()
        {
            var testMock = new Mock<INotesServices>();
            testMock.Setup(x => x.DeleteNotesByTitle("My first note")).Returns(DataNotesList());
            var controller = new NotesController(testMock.Object);
            var result = await controller.DeleteNotesByTitle("My first note");
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal("My first note", notes[0].Title);
        }
    }
}
