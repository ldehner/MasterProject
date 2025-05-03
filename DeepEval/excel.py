import os
from typing import List
from deepeval.evaluate import TestResult
from openpyxl import Workbook


def write_to_excel(results: List[TestResult], name: str) -> None:
    print(results)
    # Prepare Excel file
    file_name = f"DeepEval_Results_{name}.xlsx"
    file_path = os.path.join(os.getcwd(), file_name)
    workbook = Workbook()
    sheet = workbook.active
    sheet.title = "Evaluation Results"

    # Write header row
    header = ["Run Number", "Test Number", "Success", "Score"] # Test and run numbers were switched
    sheet.append(header)

    # Track test and run numbers
    test_number = 1  # Start with Test 1
    run_number = 1   # Start with Run 1

    # Populate rows with evaluation results
    for i, result in enumerate(results):
        print("Ergebnisse:")
        print(i)
        print(result)
        # Calculate test and run numbers
        if run_number > 10:  # Reset run number after 10 runs
            run_number = 1
            test_number += 1

        row = [
            test_number,                            # Test number (increments after every 10 runs)
            run_number,                             # Run number (resets to 1 after 10 runs)
            1 if result.success else 0,            # Success (1 for True, 0 for False)
            result.metrics_data[0].score,          # Score from the metric data
        ]
        sheet.append(row)

        run_number += 1  # Increment run number

    # Save the Excel file
    workbook.save(file_path)
    print(f"Results saved to {file_path}")
