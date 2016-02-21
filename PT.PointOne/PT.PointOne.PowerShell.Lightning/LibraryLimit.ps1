#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/dev -Credentials "Aspc1606"

$libraryName = "LargeLibrary"

$originalFilePath = "C:\Temp\Document.docx"

$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Test")
$folderCount = $folderNames.Length
$lowerLimit = 2
$upperLimit = 3
$items = $upperLimit - $lowerLimit
$counter = 0
($lowerLimit..$upperLimit) | % {
    $leaf = $_ 
    $counter = $counter + 1
    Write-Progress -ID 1 -Activity "Uploading files" -PercentComplete (100*$counter / $items)
    $innerCounter = 0
    $folderNames | % { 
        $folder = "$libraryName/$_"
        $fileName = "TestDocument$_$leaf.docx"
        $file = "C:\temp\$fileName"
        $innerCounter = $innerCounter + 1
        Write-Progress -ID 2 -Activity "Uploading files" -Status "Uploading file" -PercentComplete (100*$innerCounter / $folderCount)
        Copy-Item -LiteralPath $originalFilePath -Destination $file
#        Add-SPOFile -Path $file -Folder "$folder"
        Remove-Item -Path $file
    }
}
