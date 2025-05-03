# MindCheck Library - README

## Overview

The **MindCheck Library** is a powerful framework for evaluating and validating the performance of Large Language Models (LLMs). It provides a flexible and extensible system for chaining multiple tests, executing them, and analyzing the results. The library supports various test types, such as bias detection, contextual relevancy, hallucination detection, and more.

---

## Key Features

- **Flexible Test Configuration**: Chain multiple tests using `.Then()` and execute them with `.Execute()`.
- **Custom LLM Callers**: Easily integrate with different LLM APIs by implementing the `ILlmCaller` interface.
- **Extensible Test Types**: Create and configure tests like `BiasCheck`, `ContextualRelevancyCheck`, `HallucinationCheck`, etc.
- **Output Options**: Results can be printed to the console or exported to Excel for further analysis.

---

## Prerequisites

1. **Environment Setup**:
   - Install .NET 6.0 or later.
   - Set up environment variables for LLM API keys and endpoints:
     - `API-KEY`
     - `API-URL`

2. **Dependencies**:
   - Ensure all required NuGet packages are installed (e.g., `System.Text.Json`, `Microsoft.Extensions.AI`).

---

## How to Use

### 1. **Creating an LLM Caller**

To interact with an LLM, you need to implement the `ILlmCaller` interface. This ensures a consistent way to communicate with different LLM APIs.

#### Example: Azure LLM Caller
```csharp
using System.Net.Http.Json;
using MindCheck.Interfaces;

public class AzureLlmCaller : ILlmCaller
{
    public async Task<string> CallLlmServiceAsync(string prompt, string system)
    {
        var apiKey = Environment.GetEnvironmentVariable("API-KEY");
        var endpoint = Environment.GetEnvironmentVariable("API-URL");
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint))
        {
            throw new Exception("API-KEY or API-URL environment variable is missing.");
        }

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("api-key", apiKey);

        var url = $"{endpoint}/openai/deployments/gpt-35-turbo/chat/completions?api-version=2024-10-01-preview";

        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = system },
                new { role = "user", content = prompt }
            },
            max_tokens = 500,
            temperature = 0.7
        };

        var response = await client.PostAsJsonAsync(url, requestBody);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API Error: {await response.Content.ReadAsStringAsync()}");
        }

        var responseBody = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
        return responseBody?.Choices?[0]?.Message?.Content ?? "No response from model.";
    }
}
```

#### Example: OpenAI LLM Caller
```csharp
using MindCheck.Interfaces;
using OpenAI.Chat;

public class OpenAiLlmCaller : ILlmCaller
{
    public async Task<string> CallLlmServiceAsync(string prompt, string system)
    {
        var client = new ChatClient(model: "gpt-4o", apiKey: "your-api-key");
        var completion = await client.CompleteChatAsync(system + "\n\n" + prompt);
        return completion.Content[0].Text;
    }
}
```

---

### 2. **Creating a Test**

Each test type is implemented as a class that inherits from `BaseCheck`. Below are examples of how to create and configure each test type.

#### **Bias Check**
Evaluates the LLM's responses for potential biases.
```csharp
var biasCheck = new BiasCheck(inputPrompt, llmCaller, opinionCount: 1);
```

#### **Contextual Relevancy Check**
Checks if the LLM's responses are contextually relevant to the input.
```csharp
var contextCheck = new ContextualRelevancyCheck(inputPrompt, llmCaller, statementCount: 3);
```

#### **Hallucination Check**
Detects factual inaccuracies or hallucinations in the LLM's responses.
```csharp
var hallucinationCheck = new HallucinationCheck(inputPrompt, context, llmCaller);
```

#### **Document Relevancy Check**
Validates if the LLM's responses align with the required documents.
```csharp
var documentCheck = new DocumentCheck(AiSearchType.Semantic, requiredDocuments);
```

#### **Key Phrase Check**
Verifies if the LLM includes specific key phrases in its responses.
```csharp
var keyPhraseCheck = new KeyPhraseCheck(keyPhrases);
```

#### **Keyword Check**
Checks if the LLM includes specific keywords in its responses.
```csharp
var keywordCheck = new KeyWordCheck(keywords);
```

#### **Prompt Alignment Check**
Evaluates if the LLM follows specific instructions or prompts accurately.
```csharp
var promptAlignmentCheck = new PromptAlignmentCheck(inputPrompt, instructions, llmCaller);
```

#### **Toxicity Check**
Assesses the LLM's responses for toxic or harmful content.
```csharp
var toxicityCheck = new ToxicityCheck(inputPrompt, llmCaller);
```

---

### 3. **Chaining and Executing Tests**

Use the `CheckConfiguration` class to chain multiple tests and execute them.

#### Example
```csharp
var check = new CheckConfiguration(mockedResponse);

check.Then(new BiasCheck(inputPrompt, llmCaller, opinionCount: 1))
     .Then(new ContextualRelevancyCheck(inputPrompt, llmCaller, statementCount: 3))
     .Then(new HallucinationCheck(inputPrompt, context, llmCaller));

var results = check.Execute();
```

---

### 4. **Output Formats**

- **Console**: Print results directly to the console.
- **Excel**: Export results to an Excel file for further analysis.

#### Example
```csharp
var data = Helpers.ExtractResults(results);
Helpers.PrintResults(data, "TestType");
```

Disclaimer: This readme was autogenerated and may contain false information
