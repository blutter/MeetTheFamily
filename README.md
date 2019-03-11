# Meet the Family Coding Test

# Overview

The files are structured in the following directories:

| | |
| - | - |
| build-scripts | Scripts to build, test and package the code |
| src | Source code |
| src/MeetTheFamily | Console driver for the Family Tree processor |
| src/FamilyTree | Core implementation of the Family Tree |
| src/FamilyTreeTests | Family Tree tests |
| test | Integration test files |


# Build and Environment Setup

## Pre-Requisites

Install powershell from https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6

## Enable powershell scripts

Run the `build-scripts\configure-powershell-environment.bat` batch file (assuming Windows) to set the powershell execution policy.

## Install Dotnet Core 2.2

Run the script Dotnet core installation script: `.\build-scripts\dotnet-install.ps1 -Version 2.2.104`

## Build/Test Scripts

The build, test and packaging scripts are:

| | |
| - | - |
| build-scripts/build.ps1 | builds the solution |
| build-scripts/test.ps1 | runs unit tests |
| build-scripts/clean.ps1 | cleans the solution |
| build-scripts/run-integration-tests.ps1 | runs integration tests |
| build-scripts/package.ps1 | packages the solution into a zip file |


# Code Overview

## Assumptions

- Only a single family tree is supported. This means the direct relations of spouse who are not direct descendents of Arthur/Margret are not included
- Names are unique (the input mechanism can't distinguish between non-unique names)
- Loops in the family tree are not supported (name uniqueness helps prevent this as well)
- A person only ever has one partner
- A mother must have a spouse before a child can be created

## Model

The core model is represented in `src/FamilyTree/Person.cs`. This entity models a Person with relationships to a Mother, Father and Children.

## Finding Relations

The `RelationshipResolver` encodes the algorithms to find relations for a given person.

## Manipulating the Model

The `ModelProcessor` manipulates the model to:
- create the Arthur family tree as the reference model (encoded in the `src/ReferenceModel/arthur-clan.txt` file)
- abstract the add child and get relationship operations on the model
- ensures only unique names are allowed via the `PersonLookupCache`

## Handling User Input

The `InputProcessor` parses commands from standard input, interacts with the `ModelProcessor` and outputs to standard output.

# Tests

Tests are written in as combination of London and Chicago styles depending on the nature of the test.

The `InputProcessor` outputs to the Console so its tests do not run in parallel.