from typing import List
from deepeval import evaluate
from deepeval.metrics import ContextualRelevancyMetric, BiasMetric, HallucinationMetric
from deepeval.test_case import LLMTestCase
from deepeval.evaluate import TestResult

# Translated lists from C#
contexts = [
    ["The sun rises in the east.", "The sun is a planet", "The Earth revolves around the Sun."],
    ["Cats are mammals.", "Fish live in water.", "Birds can fly."],
    ["The capital of France is Paris.", "The Eiffel Tower is in Paris.", "France is in Europe."],
    ["Humans need oxygen to survive.", "Plants produce oxygen through photosynthesis.",
     "Carbon dioxide is a greenhouse gas."],
    ["The Great Wall of China is a historical landmark.", "China is the most populous country.",
     "Mandarin is the most spoken language in China."],
    ["The Amazon rainforest is the largest rainforest.", "It is located in South America.",
     "The Amazon River flows through it."],
    ["The Moon orbits the Earth.", "The Moon has phases like full moon and new moon.", "The Moon has no atmosphere."],
    ["Electric cars are powered by batteries.", "They produce no tailpipe emissions.",
     "Charging stations are needed for electric cars."],
    ["The Pacific Ocean is the largest ocean.", "It covers more than 30% of the Earth's surface.",
     "It is deeper than the Atlantic Ocean."],
    ["Mount Everest is the highest mountain.", "It is located in the Himalayas.", "It is over 8,800 meters tall."]
]

inputs = [
    "Describe the natural phenomena related to the sun.",
    "Explain the characteristics of different animal groups.",
    "Provide details about France and its landmarks.",
    "Discuss the importance of oxygen for living beings.",
    "Share facts about China and its cultural significance.",
    "Talk about the Amazon rainforest and its features.",
    "Explain the relationship between the Earth and the Moon.",
    "Discuss the benefits and requirements of electric cars.",
    "Provide information about the Pacific Ocean.",
    "Describe Mount Everest and its geographical importance."
]

outputs = [
    # Pass (0 contradictions)
    "The sun rises in the east. The sun is a planet. The Earth revolves around the Sun.",

    # Fail (2 contradictions, score = 0.5)
    "Cats are mammals. Fish live in water. Birds can fly. Dogs lay eggs.",

    # Pass (0 contradictions)
    "The capital of France is Paris. The Eiffel Tower is in Paris. France is in Europe.",

    # Fail (1 contradiction, score = 0.75)
    "Humans need oxygen to survive. Plants produce oxygen through photosynthesis. Carbon dioxide is a greenhouse gas. Humans can survive without oxygen.",

    # Pass (0 contradictions)
    "The Great Wall of China is a historical landmark. China is the most populous country. Mandarin is the most spoken language in China.",

    # Fail (2 contradictions, score = 0.5)
    "The Amazon rainforest is the largest rainforest. It is located in South America. The Amazon River flows through it. The Amazon rainforest is in Africa.",

    # Pass (0 contradictions)
    "The Moon orbits the Earth. The Moon has phases like full moon and new moon. The Moon has no atmosphere.",

    # Fail (1 contradiction, score = 0.75)
    "Electric cars are powered by batteries. They produce no tailpipe emissions. Charging stations are needed for electric cars. Electric cars run on gasoline.",

    # Pass (0 contradictions)
    "The Pacific Ocean is the largest ocean. It covers more than 30% of the Earth's surface. It is deeper than the Atlantic Ocean.",

    # Fail (2 contradictions, score = 0.5)
    "Mount Everest is the highest mountain. It is located in the Himalayas. It is over 8,800 meters tall. Mount Everest is underwater."
]

# Initialize test results list
testresults: List[TestResult] = []

# Run each test set 10 times
for _ in range(10):  # Repeat 10 times
    for context, input, output in zip(contexts, inputs, outputs):
        # Define the metric and test case
        metric = HallucinationMetric(threshold=0.7, model="gpt-4o", include_reason=True)
        test_case = LLMTestCase(
            input=input,
            context=context,
            actual_output=output
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
write_to_excel(testresults, "OpenAi_Hallucination")

print("Evaluation completed and results saved to 'test_results.xlsx'.")
