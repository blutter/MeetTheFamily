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


# Environment Setup

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


