#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/dev -Credentials "Aspc1606"

$libraryName = "LargeLibrary"

$originalFilePath = "C:\Temp\Document.docx"

$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Test")
$folderNames | Foreach { 
    $folderName = $_ 
    (0..2) | % {
        $folder = "$libraryName/$folderName"
        $fileName = "TestDocument$folderName$_.docx"
        $file = "C:\temp\$fileName"
        Copy-Item -LiteralPath $originalFilePath -Destination $file
#        Add-SPOFile -Path $file -Folder "$folder/$fileName"
        Remove-Item -Path $file
    }
}
