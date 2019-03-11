$PublishDirectory = "$PSScriptRoot\..\..\publish"
$RootDirectory = "$PSScriptRoot\.."
$PublishFile = "$PublishDirectory\MeetTheFamily.zip"

git clean --force -d -x

New-Item $PublishDirectory -ItemType Directory -Force

Write-Host "Creating archive in $($PublishFile)"

Add-Type -A "System.IO.Compression.FileSystem"
[IO.Compression.ZipFile]::CreateFromDirectory($RootDirectory, $PublishFile);