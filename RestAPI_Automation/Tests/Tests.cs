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
        // getting the list of users
        var content1 = APIHelper.CreateGetRequest<CreatedUsers>(Constants.getUersEndpoint);

        // verifying that there are correct data in the response
        Assert.AreEqual(2, content1.page);
        Assert.AreEqual("Michael", content1.Data[0].first_name);

        // creating random id and verifying that it is successfully created
        var content2 = APIHelper.CreatePostRequest<UserToCreate>(Payloads.payloadTask1);
        Assert.AreEqual("Mike", content2.name);
        Assert.AreEqual(280, content2.id);

        // sending request with new id
        var response = APIHelper.GetRequest($"?id={content2.id}");

        // verifying that we got "Not Found" response
        Assert.AreEqual("Not Found", response.StatusDescription);
    }

    // Создать юзера и проверить ответ, в том числе время создания (между отправкой запроса и получением ответа)
    [Test]
    public void Task2()
    {
        // using stopwatch to verify time duration
        var stopwatch = new Stopwatch();

        // starting stopwatch before request creation
        stopwatch.Start();

        // creating a new user
        var content = APIHelper.CreatePostRequest<UserToCreate>(Payloads.payloadTask2);

        // stopping stopwatch after receiving response
        stopwatch.Stop();

        // getting the request-response duration in milliseconds
        var elapsedTime = stopwatch.Elapsed.Milliseconds;

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
        var content1 = APIHelper.CreateGetRequest<CreatedUsers>(Constants.getUersEndpoint);

        // saving information received for the user
        Data userData = content1.Data[5];

        // getting information of a single user
        var content2 = APIHelper.CreateGetRequest<DataAndSupport>($"/{userData.id.ToString()}");

        // verifying that user data are the same
        Assert.AreEqual(userData.first_name, content2.Data.first_name);
        Assert.AreEqual(userData.last_name, content2.Data.last_name);
        Assert.AreEqual(userData.email, content2.Data.email);
        Assert.AreEqual(userData.id, content2.Data.id);
        Assert.AreEqual(userData.avatar, content2.Data.avatar);
    }
}