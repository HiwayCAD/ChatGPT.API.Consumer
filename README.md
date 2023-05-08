# ChatGPT.API.Consumer


Blazor wasm consumes ChatGPT API
JL 2023-05-08

I wanted to implement ChatGPT API service in a Blazor wasm project. Since no sample found in the web search, I decided to create myself follow a DeskTop sample.

1.	Create a default Blazor wasm project in Visual Studio Community 2022 with .NET 7.0
2.	Nuget Install the package Betalgo.OpenAI.GPT3. This is the ChatGPT API
3.	Nuget Install the package Radzen.Blazor. This is used for client site layout
4.	_Imports.razor: Added
a.	@using Radzen
b.	@using Radzen.Blazor
5.	index.html: Added
a.	<link rel="stylesheet" href="_content/Radzen.Blazor/css/default-base.css">
b.	<link rel="stylesheet" href="_content/Radzen.Blazor/css/default.css">
6.	Created Index.razor.cs: Added
7.	Modified Index.razor: Added

The changes in the solution looks like following red box highlighted files:
 

Index.razor:

@page "/"

<PageTitle>Index</PageTitle>

<h1>@title</h1>

<div class="row">
    <div class="col-md-4">
        <RadzenCard>
            <div class="row">
                @lblRequest <RadzenTextBox Placeholder=@lblRequest @bind-Value=@txtRequest Class="w-100" />
                <br />
                <br />
                <br />
                <center>
                    <button class="btn btn-primary" @onclick="(() => Submit())">@submit</button>
                </center>
            </div>
        </RadzenCard>
    </div>
    <div class="col-md-8">
        <RadzenCard>
            <p>
                <b>@lblResponse</b>
            </p>
            <p>
                @txtResponse
            </p>
        </RadzenCard>
    </div>
</div>

<br />
<br />

Index.razor.cs:

using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Reflection;

namespace ChatGPT.API.Consumer.Client.Pages
{
    public partial class Index
    {
        string title = "ChatGPT API Consumer",
            lblRequest = "",
            lblResponse = "",
            txtRequest = "",
            txtResponse = "",
            submit = "Submit";

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            var isEnglish = Thread.CurrentThread.CurrentCulture.Name == "en-US";

            if (isEnglish)
            {
                title = "ChatGPT API";
                lblRequest = "Input Request：";
                lblResponse = "Response：";
                submit = "Submit";
            }
            else
            {
                title = "人工智能服务";
                lblRequest = "输入咨询问题：";
                lblResponse = "人工智能答复：";
                submit = "提交";
            }
        }

        /// <summary>
        /// Button click event handler
        /// </summary>
        private async Task Submit()
        {
            var gpt3 = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "YOUR OWN CHATGPT SECURITY KEY HERE"
            });

            var completionResult = await gpt3.ChatCompletion.CreateCompletion
            (
                new ChatCompletionCreateRequest()
                {
                    Messages = new List<ChatMessage>(new ChatMessage[] { new ChatMessage("user", txtRequest) }),
                    Model = Models.ChatGpt3_5Turbo,
                    Temperature = 0.5F,
                    MaxTokens = 100
                }
            );

            if (completionResult.Successful)
            {
                foreach (var choice in completionResult.Choices)
                {
                    txtResponse = choice.Message.Content;
                    Console.WriteLine(choice.Message.Content);
                }
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message} ");
            }
        }
    }
}

Please note:
1.	The code ApiKey = "YOUR OWN CHATGPT SECURITY KEY HERE" means that: To run this sample, you need to get your own security key from login in your account in ChatGPT website. If not clear, please googling the web: How to get own ChatGPT API Key?. Or check: https://www.awesomescreenshot.com/blog/knowledge/chat-gpt-api
2.	Model = Models.ChatGpt3_5Turbo: The model is the most current one. Changing to others might cause app no more work

Run the application. It should show up following default state screenshot:
 

Try to prompt “what is the distance between Toronto to Montreal?”. Click the button Submit. It gives a result on the right panel:”The distance between Toronto and Montreal is approximately 542 kilometers (337 miles) by road or 504 kilometers (313 miles) by are.”
 

