#!/usr/bin/env python3
import os
import sys

SOLUTION_FILE_TEMPLATE = """namespace Solutions

open System

module #### =
    let solveA (input: string list) = 1

    let solveB (input: string list) = 1
"""

TESTS_FILE_TEMPLATE = """namespace Tests

open System
open Xunit

open Solutions

module ####Test =
    [<Fact>]
    let ``####A returns correct result``() =
        let result = [ ] |> ####.solveA
        Assert.Equal(1, result)

    [<Fact>]
    let ``####B returns correct result``() =
        let result = [ ] |> ####.solveB
        Assert.Equal(1, result)
"""

SOLUTION_DIR = os.path.join(os.path.dirname(__file__), "solutions")
SOLUTION_PROJECT_FILE = os.path.join(SOLUTION_DIR, "solutions.fsproj")
INPUTS_DIR = os.path.join(SOLUTION_DIR, "inputs")

TESTS_DIR = os.path.join(os.path.dirname(__file__), "tests")
TESTS_PROJECT_FILE = os.path.join(TESTS_DIR, "tests.fsproj")


def create_file(file, template, project_file):
    filename = os.path.basename(file)

    print(f"Create {file}")
    with open(file, "w") as f:
        f.write(template)

    print(f"Add {file} to {project_file}")
    with open(project_file, "r+") as f:
        content = f"""<Compile Include="{filename}" />
    <Compile Include="Program.fs" />"""
        file_content = f.read().replace('<Compile Include="Program.fs" />', content)
        f.seek(0)
        f.write(file_content)


def create_solution_file(puzzle_number):
    puzzle_name = f"Puzzle{puzzle_number}"
    filename = f"{puzzle_name}.fs"
    puzzle_file = os.path.join(SOLUTION_DIR, filename)
    create_file(
        file=puzzle_file,
        template=SOLUTION_FILE_TEMPLATE.replace("####", puzzle_name),
        project_file=SOLUTION_PROJECT_FILE,
    )


def create_test_file(puzzle_number):
    puzzle_name = f"Puzzle{puzzle_number}"
    filename = f"{puzzle_name}_Tests.fs"
    test_file = os.path.join(TESTS_DIR, filename)
    create_file(
        file=test_file,
        template=TESTS_FILE_TEMPLATE.replace("####", puzzle_name),
        project_file=TESTS_PROJECT_FILE,
    )


def create_input_file(puzzle_number):
    filename = os.path.join(INPUTS_DIR, f"Puzzle{puzzle_number}.input")
    print(f"Create {filename}")
    with open(filename, "a") as f:
        f.write("")


if __name__ == "__main__":
    if len(sys.argv) != 2:
        print(f"Usage: {sys.argv[0]} <puzzle#>")
        exit

    puzzle_number = sys.argv[1]
    create_solution_file(puzzle_number)
    create_test_file(puzzle_number)
    create_input_file(puzzle_number)
