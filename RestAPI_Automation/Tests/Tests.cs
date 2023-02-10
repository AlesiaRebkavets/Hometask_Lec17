using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using RestAPI_Automation.Requests;

namespace RestAPI_Automation.Tests;

public class Tests
{
    // Получить список юзеров, создать рандомный айдишник (нет в списке юзеров), отправить запрос с этим айдишником и проверить ответ – Not Found
    [Test]
    public void Task1()
    {
        var userToCreate = new UserToCreate("Mike", "Team leader");

        // getting the list of users
        var content1 = APIHelper.CreateGetRequest<CreatedUsers>(Constants.GetUsersEndpoint);

        // verifying that there are correct data in the response
        Assert.AreEqual(2, content1.Page);
        Assert.AreEqual("Michael", content1.Data[0].FirstName);

        // creating random id and verifying that it is successfully created
        var content2 = APIHelper.CreatePostRequest<UserToCreate>(JsonConvert.SerializeObject(userToCreate));
        Assert.AreEqual("Mike", content2.Name);
        Assert.AreEqual("Team leader", content2.Job);

        // sending request with new id
        var response = APIHelper.GetRequest($"?id={content2.Id}");

        // verifying that we got "Not Found" response
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    // Создать юзера и проверить ответ, в том числе время создания (между отправкой запроса и получением ответа)
    [Test]
    public void Task2()
    {
        var userToCreate = new UserToCreate("Mike", "Team leader");
        // using stopwatch to verify time duration
        var stopwatch = new Stopwatch();

        // starting stopwatch before request creation
        stopwatch.Start();

        // creating a new user
        var content = APIHelper.CreatePostRequest<UserToCreate>(JsonConvert.SerializeObject(userToCreate));

        // stopping stopwatch after receiving response
        stopwatch.Stop();

        // getting the request-response duration in milliseconds
        var elapsedTime = stopwatch.Elapsed.Milliseconds;

        // verifying that we got correct data in the response
        Assert.AreEqual("Mike", content.Name);
        Assert.AreEqual("Team leader", content.Job);
        Assert.That(elapsedTime < 1000);
    }

    // Получить список юзеров, взять любого юзера (сохранить информацию по нему) и отправить запрос на получение информации по конкретному юзеру. Сравнить данные между двумя запросами
    [Test]
    public void Task3()
    {
        var content1 = APIHelper.CreateGetRequest<CreatedUsers>(Constants.GetUsersEndpoint);

        // saving information received for the user
        Data userData = content1.Data[5];

        // getting information of a single user
        var content2 = APIHelper.CreateGetRequest<DataAndSupport>($"/{userData.Id.ToString()}");

        // verifying that user data are the same
        Assert.AreEqual(userData.FirstName, content2.Data.FirstName);
        Assert.AreEqual(userData.LastName, content2.Data.LastName);
        Assert.AreEqual(userData.Email, content2.Data.Email);
        Assert.AreEqual(userData.Id, content2.Data.Id);
        Assert.AreEqual(userData.Avatar, content2.Data.Avatar);
    }
}