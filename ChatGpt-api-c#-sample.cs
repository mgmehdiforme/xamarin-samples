using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatGPT
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Replace YOUR_API_KEY with your OpenAI API key
            string apiKey = "YOUR_API_KEY";

            Console.Write("Enter your prompt: ");
            string prompt = Console.ReadLine();

            // Create a new HTTP client to send requests to the OpenAI API
            using (var client = new HttpClient())
            {
                // Set the authorization header to include the API key
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // Define the request parameters
                var requestBody = new
                {
                    prompt = prompt,
                    temperature = 0.7,
                    max_tokens = 50,
                    top_p = 1,
                    frequency_penalty = 0,
                    presence_penalty = 0
                };
                var requestUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";

                // Send the request to the OpenAI API
                var response = await client.PostAsync(requestUrl, new StringContent(JsonSerializer.Serialize(requestBody)));

                // Parse the response and display the generated text
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<CompletionResponse>(responseJson);
                Console.WriteLine(responseData.choices[0].text);
            }
        }
    }

    // Define a class to represent the response from the OpenAI API
    class CompletionResponse
    {
        public CompletionChoice[] choices { get; set; }
    }

    class CompletionChoice
    {
        public string text { get; set; }
        public double score { get; set; }
        public int length { get; set; }
    }
}
