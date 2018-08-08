using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Text;
using Xunit;
using NotesAPI.Models;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;

namespace NotesAPI.Tests
{
    public class NotesIntegrationTests
    {
        private static HttpClient _client;

        static NotesIntegrationTests()
        {
            var host = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>()
                );
            _client = host.CreateClient();

        }



        [Fact]
        public async void Test_Post_IT()
        {
            var note = new Notes()
            {
                ID = 5,
                Title = "My fifth note",
                Text = "Writing my fifth note",
                Pinned = false,
                Label = new List<Labels>
        {
            new Labels { ID = 9, Label = "Work"},
            new Labels { ID = 10, Label = "Play"}
        },
                CheckedList = new List<CheckedList>
        {
            new CheckedList { ID = 9,ListItem = "Pen"},
            new CheckedList { ID = 10, ListItem = "Bag"}
        }
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/notes", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<Notes>(responseString);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("My fifth note", jsonObj.Title);
            Assert.Equal("Writing my fifth note", jsonObj.Text);
            Assert.False(jsonObj.Pinned);
            Assert.Equal("Work", jsonObj.Label[0].Label);
            Assert.Equal("Play", jsonObj.Label[1].Label);
            Assert.Equal("Pen", jsonObj.CheckedList[0].ListItem);
            Assert.Equal("Bag", jsonObj.CheckedList[1].ListItem);
        }


        [Fact]
        public async void Test_Get_Note_By_ID_Negative_IT()
        {
            var responseGet = await _client.GetAsync("api/notes/999");
            
            Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
        }

        [Fact]
        public async void Test_Get_Note_By_ID_Positive_IT()
        {
            var responseGet = await _client.GetAsync("api/notes/1");
            var responseString = await responseGet.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<Notes>(responseString);

            responseGet.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            Assert.Equal(1, jsonObj.ID);
        }


        [Fact]
        public async void Test_Get_All_Notes_IT()
        {
            var responseGet = await _client.GetAsync("api/notes");
            var responseString = await responseGet.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Notes>>(responseString);

            responseGet.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            Assert.Equal("My first note", jsonObj[0].Title);
            Assert.Equal("Writing my first note", jsonObj[0].Text);
            Assert.False(jsonObj[0].Pinned);
            Assert.Equal("Work", jsonObj[0].Label[0].Label);
            Assert.Equal("Play", jsonObj[0].Label[1].Label);
            Assert.Equal("Pen", jsonObj[0].CheckedList[0].ListItem);
            Assert.Equal("Bag", jsonObj[0].CheckedList[1].ListItem);
        }

        [Fact]
        public async void Test_Get_Note_By_Title_Positive_IT()
        {
            var responseGet = await _client.GetAsync("api/notes?title=My first note");
            var responseString = await responseGet.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Notes>>(responseString);

            responseGet.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            Assert.Equal("My first note", jsonObj[0].Title);
        }

        [Fact]
        public async void Test_Get_Note_By_Title_Negative_IT()
        {
            var responseGet = await _client.GetAsync("api/notes?title=First note");
            
            Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
        }


        [Fact]
        public async void Test_Get_Note_By_Label_Positive_IT()
        {
            var responseGet = await _client.GetAsync("api/notes?label=Work");
            var responseString = await responseGet.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Notes>>(responseString);

            responseGet.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            Assert.Equal("Work", jsonObj[0].Label[0].Label);
        }

        [Fact]
        public async void Test_Get_Note_By_Label_Negative_IT()
        {
            var note = new Notes()
            {
                ID = 1,
                Title = "My first note",
                Text = "Writing my first note",
                Pinned = false,
                Label = new List<Labels>
        {
            new Labels { ID = 1, Label = "Work"},
            new Labels { ID = 2, Label = "Play"}
        },
                CheckedList = new List<CheckedList>
        {
            new CheckedList { ID = 1,ListItem = "Pen"},
            new CheckedList { ID = 2, ListItem = "Bag"}
        }
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/notes", stringContent);
            var responseGet = await _client.GetAsync("api/notes?label=note");

            Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
        }


        [Fact]
        public async void Test_Get_Note_By_Pinned_Positive_IT()
        {
            var responseGet = await _client.GetAsync("api/notes?isPinned=false");
            var responseString = await responseGet.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Notes>>(responseString);

            responseGet.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            Assert.False(jsonObj[0].Pinned);
        }
        

        [Fact]
        public async void Test_Put_IT()
        {
            var note = new Notes()
            {
                ID = 2,
                Title = "My second note",
                Text = "Writing my second note",
                Pinned = false,
                Label = new List<Labels>
        {
            new Labels { ID = 3, Label = "Work"},
            new Labels { ID = 4, Label = "Priority"}
        },
                CheckedList = new List<CheckedList>
        {
            new CheckedList { ID = 3,ListItem = "Watch"},
            new CheckedList { ID = 4, ListItem = "Bag"}
        }
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var responsePost = await _client.PostAsync("api/notes", stringContent);

            var notePut = new Notes()
            {
                ID = 2,
                Title = "Second note",
                Text = "Writing my second note",
                Pinned = true,
                Label = new List<Labels>
        {
            new Labels { ID = 3, Label = "Work"},
            new Labels { ID = 4, Label = "Priority"}
        },
                CheckedList = new List<CheckedList>
        {
            new CheckedList { ID = 3,ListItem = "Paper"},
            new CheckedList { ID = 4, ListItem = "Bag"}
        }
            };

            var stringContentPut = new StringContent(JsonConvert.SerializeObject(notePut), UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PutAsync("api/notes/2", stringContentPut);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<Notes>(responseString);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Second note", jsonObj.Title);
            Assert.Equal("Writing my second note", jsonObj.Text);
            Assert.True(jsonObj.Pinned);
            Assert.Equal("Work", jsonObj.Label[0].Label);
            Assert.Equal("Priority", jsonObj.Label[1].Label);
            Assert.Equal("Paper", jsonObj.CheckedList[0].ListItem);
            Assert.Equal("Bag", jsonObj.CheckedList[1].ListItem);
        }

        [Fact]
        public async void Test_Delete_Note_By_ID_IT()
        {
            var note = new Notes()
            {
                ID = 3,
                Title = "My third note",
                Text = "Writing my third note",
                Pinned = true,
                Label = new List<Labels>
        {
            new Labels { ID = 5, Label = "Work"},
            new Labels { ID = 6, Label = "Archive"}
        },
                CheckedList = new List<CheckedList>
        {
            new CheckedList { ID = 5,ListItem = "Watch"},
            new CheckedList { ID = 6, ListItem = "Bag"}
        }
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var responsePost = await _client.PostAsync("api/notes", stringContent);

            var response = await _client.DeleteAsync("api/notes/3");
            response.EnsureSuccessStatusCode();

            var responseGet = await _client.GetAsync("api/notes/3");
            Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);

        }
        

        [Fact]
        public async void Test_Delete_Note_By_Title_IT()
        {
            var note = new Notes()
            {
                ID = 4,
                Title = "My fourth note",
                Text = "Writing my fourth note",
                Pinned = true,
                Label = new List<Labels>
        {
            new Labels { ID = 7, Label = "Important"},
            new Labels { ID = 8, Label = "Archive"}
        },
                CheckedList = new List<CheckedList>
        {
            new CheckedList { ID = 7,ListItem = "Bike"},
            new CheckedList { ID = 8, ListItem = "Car"}
        }
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var responsePost = await _client.PostAsync("api/notes", stringContent);

            var response = await _client.DeleteAsync("api/notes?title=My fourth note");
            response.EnsureSuccessStatusCode();

            var responseGet = await _client.GetAsync("api/notes?title=My fourth note");
            Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
        }
        
    }
}
