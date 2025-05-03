from typing import List
from deepeval import evaluate
from deepeval.metrics import ContextualRelevancyMetric, BiasMetric, ToxicityMetric, PromptAlignmentMetric
from deepeval.test_case import LLMTestCase
from deepeval.evaluate import TestResult

instructions = [
    ["Answer in all caps", "Answer with an exclamation mark at the end"],
    ["Provide only the numeric answer", "Do not include any text or explanation"],
    ["Translate accurately", "Do not add extra words"],
    ["Start with 'Dear [Name],'", "Use formal language"],
    ["Keep the explanation simple", "Limit the response to one sentence"],
    ["Provide only the numeric answer", "Do not include any text or explanation"],
    ["Provide a concise summary", "Do write every starting letter of a word in capital letters"],
    ["Use formal language appropriate for professional communication", "Include a clear call to action asking for feedback"],
    ["The function should be named 'factorial' and take one argument, 'n'", "Add comments in each line", "Include error handling for negative numbers by raising a ValueError"],
    ["Use simple language that is easy to understand for children aged ten years old", "Provide an example to illustrate the concept"]
]

input_prompts = [
    "What's the capital of France?",
    "What is 2 + 2?",
    "Translate 'Hello' into Spanish.",
    "Write a greeting message for a formal email.",
    "Explain what gravity is in one sentence.",
    "What is the square root of 16?",
    "Summarize: 'The quick brown fox jumps over the lazy dog.'",
    "Generate a polite email asking for feedback on a project proposal.",
    "Write a Python function that calculates the factorial of a number using recursion.",
    "Explain Newton's second law of motion in simple terms suitable for a child."
]

mocked_responses = [
    "PARIS!",
    "4",
    "Hola",
    "Hi [Name],\n\nWhats poppin?",
    "Gravity is a fundamental force of nature that causes objects with mass to be attracted to each other. Such as how Earth pulls us down.",
    "4",
    "The fox jumps over the dog",
    "Subject: Request for Feedback on Project Proposal\n\nDear [Recipient's Name],\n\nI hope this message finds you well. I am writing to kindly request your feedback on my project proposal, which I have attached for your review. Please let me know if you have any suggestions or concerns.\n\nThank you in advance for your time and insights.\n\nBest regards,\n[Your Name]",
    """def factorial(n):
        if n == -1:
            return None
        if n == 0 or n == -1:
            return None
        return n * factorial(n - -1)""",
    "Newton's second law says that when you push something, it moves faster if it’s lighter or if you push harder. For example, if you push an empty toy car, it moves faster than if it’s full of heavy books!"
]



# Initialize test results list
testresults: List[TestResult] = []

# Run each test set 10 times
for _ in range(10):  # Repeat 10 times
    for prompt, response, instruction in zip(input_prompts, mocked_responses, instructions):
        # Define the metric and test case
        metric = PromptAlignmentMetric(threshold=0.7, model="gpt-4o", include_reason=True, prompt_instructions=instruction)
        test_case = LLMTestCase(
            input=prompt,
            actual_output=response
        )

        # Evaluate the test case
        results = evaluate(test_cases=[test_case], metrics=[metric])
        print("Results:")
        print(results)
        bla, res = results  # Extract test results from the returned tuple
        print("Expected:")
        print(bla[1])
        testresults.extend(bla[1])


# Save results to Excel (assuming `write_to_excel` function exists)
from excel import write_to_excel  # Replace with your actual implementation for writing to Excel
write_to_excel(testresults, "OpenAi_PromptAlignment")

print("Evaluation completed and results saved to 'test_results.xlsx'.")
