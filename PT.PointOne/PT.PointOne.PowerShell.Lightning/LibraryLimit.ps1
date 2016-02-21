#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/dev -Credentials "Aspc1606"

$libraryName = "LargeLibrary"

$originalFilePath = "C:\Temp\Document.docx"

$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Test")
$lowerLimit = 0
$upperLimit = 1000
$items = $upperLimit - $lowerLimit
$counter = 0
($lowerLimit..$upperLimit) | % {
    $leaf = $_ 
    Write-Progress -Activity "Uploading files" -PercentComplete (100*$counter / $items)
    $counter = $counter + 1
    $folderNames | % { 
        $folder = "$libraryName/$_"
        $fileName = "TestDocument$_$leaf.docx"
        $file = "C:\temp\$fileName"
        Copy-Item -LiteralPath $originalFilePath -Destination $file
#        Add-SPOFile -Path $file -Folder "$folder/$fileName"
        Remove-Item -Path $file
    }
}
