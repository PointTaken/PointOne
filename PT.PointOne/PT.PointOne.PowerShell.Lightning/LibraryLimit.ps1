#
# LibraryLimit.ps1
#
$url = "https://aspc1606.sharepoint.com/sites/dev"
Connect-SPOnline $url -Credentials "Aspc1606"

$libraryName = "LargeLibrary2"

$originalFilePath = "C:\Temp\Document.docx"

$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Test")
$folderCount = $folderNames.Length
$lowerLimit = 0
$upperLimit = 12000
$items = $upperLimit - $lowerLimit
$counter = 0
($lowerLimit..$upperLimit) | % {
    if ($_ % 50 -eq 0) { Connect-SPOnline $url -Credentials "Aspc1606" }
    $leaf = $_ 
    Write-Progress -ID 1 -Activity "Uploading files" -Status $_ -PercentComplete (100*$counter / $items)
    $counter++
	$dept = Get-Random -InputObject $folderNames
    $folder = "$libraryName"
    $fileName = "TestDocument$dept$leaf.docx"
    $file = "C:\temp\$fileName"
    Copy-Item -LiteralPath $originalFilePath -Destination $file
    Add-SPOFile -Path $file -Folder $folder
    Remove-Item -Path $file
}
