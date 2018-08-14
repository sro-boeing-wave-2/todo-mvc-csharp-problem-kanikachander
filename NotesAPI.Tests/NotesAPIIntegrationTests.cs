//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.TestHost;
//using System.Text;
//using Xunit;
//using NotesAPI.Models;
//using Newtonsoft.Json;
//using System.Net;
//using Newtonsoft.Json.Linq;
//using FluentAssertions;
//using MongoDB.Testing.Mongo;
//using MongoDB.Driver;

//namespace NotesAPI.Tests
//{
//    public class NotesIntegrationTests
//    {
//        private static HttpClient _client;

//        static NotesIntegrationTests()
//        {
//            var host = new TestServer(new WebHostBuilder()
//                .UseEnvironment("Testing")
//                .UseStartup<Startup>()
//                );
//            _client = host.CreateClient();
//        }

//        [Fact]
//        public async void Test_Post_IT()
//        {
//            var note = new Note()
//            {
//                ID = 5,
//                Title = "My fifth note",
//                Text = "Writing my fifth note",
//                Pinned = false,
//                Labels = new List<Label>
//        {
//            new Label { ID = 9, LabelName = "Work"},
//            new Label { ID = 10, LabelName = "Play"}
//        },
//                CheckedList = new List<CheckedListItem>
//        {
//            new CheckedListItem { ID = 9,ListItem = "Pen"},
//            new CheckedListItem { ID = 10, ListItem = "Bag"}
//        }
//            };

//            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
//            var response = await _client.PostAsync("api/notes", stringContent);
//            var responseString = await response.Content.ReadAsStringAsync();
//            var jsonObj = JsonConvert.DeserializeObject<Note>(responseString);

//            response.EnsureSuccessStatusCode();
//            //Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//            response.StatusCode.Should().Be(HttpStatusCode.Created);
//            jsonObj.Title.Should().Be("My fifth note");
//            jsonObj.Text.Should().Be("Writing my fifth note");
//            jsonObj.Labels[0].LabelName.Should().Be("Work");
//            jsonObj.Labels[1].LabelName.Should().Be("Play");
//            jsonObj.CheckedList[0].ListItem.Should().Be("Pen");
//            jsonObj.CheckedList[1].ListItem.Should().Be("Bag");
//        }


//        [Fact]
//        public async void Test_Get_Note_By_ID_Negative_IT()
//        {
//            var responseGet = await _client.GetAsync("api/notes/999");
//            responseGet.StatusCode.Should().Be(HttpStatusCode.NotFound);
//            //Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
//        }

//        [Fact]
//        public async void Test_Get_Note_By_ID_Positive_IT()
//        {
//            var responseGet = await _client.GetAsync("api/notes/1");
//            var responseString = await responseGet.Content.ReadAsStringAsync();
//            var jsonObj = JsonConvert.DeserializeObject<Note>(responseString);

//            responseGet.EnsureSuccessStatusCode();
//            responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
//            jsonObj.ID.Should().Be(1);
//            //Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
//            //Assert.Equal(1, jsonObj.ID);
//        }


//        [Fact]
//        public async void Test_Get_All_Notes_IT()
//        {
//            var responseGet = await _client.GetAsync("api/notes");
//            var responseString = await responseGet.Content.ReadAsStringAsync();
//            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);

//            responseGet.EnsureSuccessStatusCode();
//            responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
//            jsonObj[0].Title.Should().Be("My first note");
//            jsonObj[0].Text.Should().Be("Writing my first note");
//            jsonObj[0].Labels[0].LabelName.Should().Be("Work");
//            jsonObj[0].Labels[1].LabelName.Should().Be("Play");
//            jsonObj[0].CheckedList[0].ListItem.Should().Be("Pen");
//            jsonObj[0].CheckedList[1].ListItem.Should().Be("Bag");

//            //Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
//            //Assert.Equal("My first note", jsonObj[0].Title);
//            //Assert.Equal("Writing my first note", jsonObj[0].Text);
//            //Assert.False(jsonObj[0].Pinned);
//            //Assert.Equal("Work", jsonObj[0].Labels[0].LabelName);
//            //Assert.Equal("Play", jsonObj[0].Labels[1].LabelName);
//            //Assert.Equal("Pen", jsonObj[0].CheckedList[0].ListItem);
//            //Assert.Equal("Bag", jsonObj[0].CheckedList[1].ListItem);
//        }

//        [Fact]
//        public async void Test_Get_Note_By_Title_Positive_IT()
//        {
//            var responseGet = await _client.GetAsync("api/notes?title=My first note");
//            var responseString = await responseGet.Content.ReadAsStringAsync();
//            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);

//            responseGet.EnsureSuccessStatusCode();
//            responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
//            jsonObj[0].Title.Should().Be("My first note");
//            //Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
//            //Assert.Equal("My first note", jsonObj[0].Title);
//        }

//        [Fact]
//        public async void Test_Get_Note_By_Title_Negative_IT()
//        {
//            var responseGet = await _client.GetAsync("api/notes?title=First note");
//            responseGet.StatusCode.Should().Be(HttpStatusCode.NotFound);
//            //Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
//        }


//        [Fact]
//        public async void Test_Get_Note_By_Label_Positive_IT()
//        {
//            var responseGet = await _client.GetAsync("api/notes?label=Work");
//            var responseString = await responseGet.Content.ReadAsStringAsync();
//            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);

//            responseGet.EnsureSuccessStatusCode();
//            responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
//            jsonObj[0].Labels[0].LabelName.Should().Be("Work");
//            //Assert.Equal("Work", jsonObj[0].Labels[0].LabelName);
//        }

//        [Fact]
//        public async void Test_Get_Note_By_Label_Negative_IT()
//        {
//            var note = new Note()
//            {
//                ID = 1,
//                Title = "My first note",
//                Text = "Writing my first note",
//                Pinned = false,
//                Labels = new List<Label>
//        {
//            new Label { ID = 1, LabelName = "Work"},
//            new Label { ID = 2, LabelName = "Play"}
//        },
//                CheckedList = new List<CheckedListItem>
//        {
//            new CheckedListItem { ID = 1,ListItem = "Pen"},
//            new CheckedListItem { ID = 2, ListItem = "Bag"}
//        }
//            };

//            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
//            var response = await _client.PostAsync("api/notes", stringContent);
//            var responseGet = await _client.GetAsync("api/notes?label=note");

//            responseGet.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }


//        [Fact]
//        public async void Test_Get_Note_By_Pinned_Positive_IT()
//        {
//            var responseGet = await _client.GetAsync("api/notes?isPinned=false");
//            var responseString = await responseGet.Content.ReadAsStringAsync();
//            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);

//            responseGet.EnsureSuccessStatusCode();
//            responseGet.StatusCode.Should().Be(HttpStatusCode.OK);
//            jsonObj[0].Pinned.Should().BeFalse();
//            //Assert.False(jsonObj[0].Pinned);
//        }
        

//        [Fact]
//        public async void Test_Put_IT()
//        {
//            var note = new Note()
//            {
//                ID = 2,
//                Title = "My second note",
//                Text = "Writing my second note",
//                Pinned = false,
//                Labels = new List<Label>
//        {
//            new Label { ID = 3, LabelName = "Work"},
//            new Label { ID = 4, LabelName = "Priority"}
//        },
//                CheckedList = new List<CheckedListItem>
//        {
//            new CheckedListItem { ID = 3,ListItem = "Watch"},
//            new CheckedListItem { ID = 4, ListItem = "Bag"}
//        }
//            };

//            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
//            var responsePost = await _client.PostAsync("api/notes", stringContent);

//            var notePut = new Note()
//            {
//                ID = 2,
//                Title = "Second note",
//                Text = "Writing my second note",
//                Pinned = true,
//                Labels = new List<Label>
//        {
//            new Label { ID = 3, LabelName = "Work"},
//            new Label { ID = 4, LabelName = "Priority"}
//        },
//                CheckedList = new List<CheckedListItem>
//        {
//            new CheckedListItem { ID = 3,ListItem = "Paper"},
//            new CheckedListItem { ID = 4, ListItem = "Bag"}
//        }
//            };

//            var stringContentPut = new StringContent(JsonConvert.SerializeObject(notePut), UnicodeEncoding.UTF8, "application/json");
//            var response = await _client.PutAsync("api/notes/2", stringContentPut);
//            var responseString = await response.Content.ReadAsStringAsync();
//            var jsonObj = JsonConvert.DeserializeObject<Note>(responseString);

//            response.EnsureSuccessStatusCode();
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            jsonObj.Title.Should().Be("Second note");
//            jsonObj.Text.Should().Be("Writing my second note");
//            jsonObj.Labels[0].LabelName.Should().Be("Work");
//            jsonObj.Labels[1].LabelName.Should().Be("Priority");
//            jsonObj.CheckedList[0].ListItem.Should().Be("Paper");
//            jsonObj.CheckedList[1].ListItem.Should().Be("Bag");

//            //response.EnsureSuccessStatusCode();
//            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            //Assert.Equal("Second note", jsonObj.Title);
//            //Assert.Equal("Writing my second note", jsonObj.Text);
//            //Assert.True(jsonObj.Pinned);
//            //Assert.Equal("Work", jsonObj.Labels[0].LabelName);
//            //Assert.Equal("Priority", jsonObj.Labels[1].LabelName);
//            //Assert.Equal("Paper", jsonObj.CheckedList[0].ListItem);
//            //Assert.Equal("Bag", jsonObj.CheckedList[1].ListItem);
//        }

//        [Fact]
//        public async void Test_Delete_Note_By_ID_IT()
//        {
//            var note = new Note()
//            {
//                ID = 3,
//                Title = "My third note",
//                Text = "Writing my third note",
//                Pinned = true,
//                Labels = new List<Label>
//        {
//            new Label { ID = 5, LabelName = "Work"},
//            new Label { ID = 6, LabelName = "Archive"}
//        },
//                CheckedList = new List<CheckedListItem>
//        {
//            new CheckedListItem { ID = 5,ListItem = "Watch"},
//            new CheckedListItem { ID = 6, ListItem = "Bag"}
//        }
//            };

//            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
//            var responsePost = await _client.PostAsync("api/notes", stringContent);

//            var response = await _client.DeleteAsync("api/notes/3");
//            response.EnsureSuccessStatusCode();

//            var responseGet = await _client.GetAsync("api/notes/3");
//            responseGet.StatusCode.Should().Be(HttpStatusCode.NotFound);

//        }
        

//        [Fact]
//        public async void Test_Delete_Note_By_Title_IT()
//        {
//            var note = new Note()
//            {
//                ID = 4,
//                Title = "My fourth note",
//                Text = "Writing my fourth note",
//                Pinned = true,
//                Labels = new List<Label>
//        {
//            new Label { ID = 7, LabelName = "Important"},
//            new Label { ID = 8, LabelName = "Archive"}
//        },
//                CheckedList = new List<CheckedListItem>
//        {
//            new CheckedListItem { ID = 7,ListItem = "Bike"},
//            new CheckedListItem { ID = 8, ListItem = "Car"}
//        }
//            };

//            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
//            var responsePost = await _client.PostAsync("api/notes", stringContent);

//            var response = await _client.DeleteAsync("api/notes?title=My fourth note");
//            response.EnsureSuccessStatusCode();

//            var responseGet = await _client.GetAsync("api/notes?title=My fourth note");
//            responseGet.StatusCode.Should().Be(HttpStatusCode.NotFound);
//        }
        
//    }
//}
