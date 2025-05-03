from typing import List

from deepeval import evaluate
from deepeval.metrics import AnswerRelevancyMetric, ContextualRelevancyMetric
from deepeval.test_case import LLMTestCase
from deepeval.evaluate import TestResult

from excel import write_to_excel

testresults: List[TestResult] = []

for i in range(10):
    # Replace this with the actual output from your LLM application
    actual_output = "We offer a 30-day full refund for all shoes at no extra cost."
    retrieval_context = ["All customers are eligible for a 30 day full refund at no extra cost."]

    # Define the metric and test case
    metric = ContextualRelevancyMetric(threshold=0.7, model="gpt-4o", include_reason=True)
    test_case = LLMTestCase(input="What if these shoes don't fit?", actual_output=actual_output, retrieval_context=retrieval_context)

    # Evaluate the test case
    results = evaluate(test_cases=[test_case], metrics=[metric])
    res = results.test_results
    testresults.extend(res)

write_to_excel(testresults, "test")
