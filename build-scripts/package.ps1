$PublishDirectory = "$PSScriptRoot\..\publish"
$PublishFile = "$PublishDirectory\MeetTheFamily.zip"
git clean --force -d -x

Remove-Item -LiteralPath $PublishDirectory -Force -Recurse
New-Item $PublishDirectory -ItemType Directory -Force

Add-Type -A "System.IO.Compression.FileSystem"
[IO.Compression.ZipFile]::CreateFromDirectory(".", $PublishFile);