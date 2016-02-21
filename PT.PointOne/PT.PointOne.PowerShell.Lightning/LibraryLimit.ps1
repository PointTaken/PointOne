#
# LibraryLimit.ps1
#
$url = "https://aspc1606.sharepoint.com/sites/dev"
Connect-SPOnline $url -Credentials "Aspc1606"

$libraryName = "LargeLibrary"

$originalFilePath = "C:\Temp\Document.docx"

$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Test")
$folderCount = $folderNames.Length
$lowerLimit = 0
$upperLimit = 699
$items = $upperLimit - $lowerLimit
$counter = 0
($lowerLimit..$upperLimit) | % {
    if ($_ % 50 -eq 0) { Connect-SPOnline $url -Credentials "Aspc1606" }
    $leaf = $_ 
    Write-Progress -ID 1 -Activity "Uploading files $_" -PercentComplete (100*$counter / $items)
    $counter = $counter + 1
    $innerCounter = 0
    $folderNames | % { 
        $folder = "$libraryName/$_"
        $fileName = "TestDocument$_$leaf.docx"
        $file = "C:\temp\$fileName"
        Write-Progress -ID 2 -Activity "Uploading files" -Status "Please wait, this shouldn't take long" -PercentComplete (100*$innerCounter / $folderCount)
        $innerCounter = $innerCounter + 1
        Copy-Item -LiteralPath $originalFilePath -Destination $file
        Add-SPOFile -Path $file -Folder $folder
        Remove-Item -Path $file
    }
}
