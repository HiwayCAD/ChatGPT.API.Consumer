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
                ApiKey = "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"
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
