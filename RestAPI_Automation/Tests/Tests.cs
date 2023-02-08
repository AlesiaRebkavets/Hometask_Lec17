using System.Diagnostics;
using NUnit.Framework;
using RestAPI_Automation.Requests;

namespace RestAPI_Automation.Tests;

public class Tests
{
    // Получить список юзеров, создать рандомный айдишник (нет в списке юзеров), отправить запрос с этим айдишником и проверить ответ – Not Found
    [Test]
    public void Task1()
    {
        var users = new APIHelper<CreatedUsers>();
        // getting the list of users
        var request1 = users.CreateGetRequest("/api/users?page=2");
        var response1 = users.GetReponse(request1);
        CreatedUsers content1 = users.GetContent<CreatedUsers>(response1);
        // verifying that there are correct data in the response
        Assert.AreEqual(2, content1.page);
        Assert.AreEqual("Michael", content1.Data[0].first_name);

        // creating random id and verifying that it is successfully created
        string payload =
            @"{
                ""id"":""280"",
                ""name"":""Mike""
            }";
        var user = new APIHelper<UserToCreate>();
        var url = user.SetUrl("/api/users");
        var request2 = user.CreatePostRequest(payload, "/api/users");
        var response2 = user.GetReponse(request2);
        UserToCreate content2 = user.GetContent<UserToCreate>(response2);
        Assert.AreEqual("Mike", content2.name);
        Assert.AreEqual(280, content2.id);

        // sending request with new id
        var createdUser = new APIHelper<Data>();
        var request3 = createdUser.CreateGetRequest("/api/users?id=280");
        var response3 = createdUser.GetReponse(request3);

        // verifying that we got "Not Found" response
        Assert.AreEqual("Not Found", response3.StatusDescription);
    }

    // Создать юзера и проверить ответ, в том числе время создания (между отправкой запроса и получением ответа)
    [Test]
    public void Task2()
    {
        string payload =
            @"{
                ""name"":""Mike"",
                ""job"":""Team leader"",
                ""id"":""222""
            }";
        var user = new APIHelper<UserToCreate>();
        var url = user.SetUrl("/api/users");

        // using stopwatch to verify time duration
        var stopwatch = new Stopwatch();

        // starting stopwatch before request creation
        stopwatch.Start();

        // creating a new user
        var request = user.CreatePostRequest(payload, "/api/users");
        var response = user.GetReponse(request);

        // stopping stopwatch after receiving response
        stopwatch.Stop();

        // getting the request-response duration in milliseconds
        var elapsedTime = stopwatch.Elapsed.Milliseconds;

        UserToCreate content = user.GetContent<UserToCreate>(response);

        // verifying that we got correct data in the response
        Assert.AreEqual("Mike", content.name);
        Assert.AreEqual("Team leader", content.job);
        Assert.AreEqual(222, content.id);
        Assert.That(elapsedTime < 1000);
    }

    // Получить список юзеров, взять любого юзера (сохранить информацию по нему) и отправить запрос на получение информации по конкретному юзеру. Сравнить данные между двумя запросами
    [Test]
    public void Task3()
    {
        var users = new APIHelper<CreatedUsers>();
        var request1 = users.CreateGetRequest("/api/users?page=2");
        var response1 = users.GetReponse(request1);
        // getting the list of users
        CreatedUsers content1 = users.GetContent<CreatedUsers>(response1);
        // saving information received for the user
        Data userData = content1.Data[5];

        // getting information of a single user
        var singleUser = new APIHelper<DataAndSupport>();
        var request2 = singleUser.CreateGetRequest($"/api/users/{userData.id.ToString()}");
        var response2 = singleUser.GetReponse(request2);
        DataAndSupport content2 = singleUser.GetContent<DataAndSupport>(response2);

        // verifying that user data are the same
        Assert.AreEqual(userData.first_name, content2.Data.first_name);
        Assert.AreEqual(userData.last_name, content2.Data.last_name);
        Assert.AreEqual(userData.email, content2.Data.email);
        Assert.AreEqual(userData.id, content2.Data.id);
        Assert.AreEqual(userData.avatar, content2.Data.avatar);
    }
}