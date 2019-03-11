$RuntimeProject = "$PSScriptRoot\..\src\MeetTheFamily\MeetTheFamily.csproj"
$TestSamplePath = "$PSScriptRoot\..\test\samples"
$TestOutputPath = "$PSScriptRoot\..\test\test-output"

Remove-Item -LiteralPath $TestOutputPath -Force -Recurse -ErrorAction Ignore
New-Item $TestOutputPath -ItemType Directory -Force

$InputFiles = Get-ChildItem "$TestSamplePath\*.input.txt"

foreach ($inputFile in $InputFiles) {
    Write-Host "Running test on $($inputFile.FullName)"
    $outputFilename = "$($TestOutputPath)/$((($inputFile.Name) -split "\.")[0]).output.txt"
    $referenceOutputFilename = "$($TestSamplePath)/$((($inputFile.Name) -split "\.")[0]).output.txt"
    Write-Host "Outputting to $($outputFilename)"
    dotnet run --project .\src\MeetTheFamily\MeetTheFamily.csproj $inputFile.FullName | Out-File -Encoding ASCII $outputFilename

    if (diff $(Get-Content $referenceOutputFilename) $(Get-Content $outputFilename)) {
        Write-Error "Output for $($inputFile) is incorrect"
    } else {
        Write-Host "Output for $($inputFile) is correct"
    }
}


