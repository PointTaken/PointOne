#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/large -Credentials "Aspc1606"
Connect-SPOnline https://aspc1606.sharepoint.com/sites/dev -Credentials "Aspc1606"

$libraryName = "LargeLibrary"

$originalFilePath = "C:\Temp\Document.docx"

$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Test")
$folderNames | Foreach { 
    $folderName = $_ 
    (0..2) | % {
        $folder = "$libraryName/$folderName/"
        $fileName = "TestDocument$folderName$_.docx"
        Remove-Item -Path "C:\temp\$fileName"
    }
}
