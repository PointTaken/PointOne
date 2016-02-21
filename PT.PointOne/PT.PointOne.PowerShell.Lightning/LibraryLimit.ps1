#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/dev -Credentials "Aspc1606"

$libraryName = "LargeLibrary"

$originalFilePath = "C:\Temp\Document.docx"

$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Test")
$folderCount = $folderNames.Length
$lowerLimit = 0
$upperLimit = 700
$items = $upperLimit - $lowerLimit
$counter = 0
($lowerLimit..$upperLimit) | % {
    $leaf = $_ 
    Write-Progress -ID 1 -Activity "Uploading files" -PercentComplete (100*$counter / $items)
    $counter = $counter + 1
    $innerCounter = 0
    $folderNames | % { 
        $folder = "$libraryName/$_"
        $fileName = "TestDocument$_$leaf.docx"
        $file = "C:\temp\$fileName"
        Write-Progress -ID 2 -Activity "Uploading files" -Status "Uploading file" -PercentComplete (100*$innerCounter / $folderCount)
        $innerCounter = $innerCounter + 1
        Copy-Item -LiteralPath $originalFilePath -Destination $file
        Add-SPOFile -Path $file -Folder "$folder"
        Remove-Item -Path $file
    }
}
