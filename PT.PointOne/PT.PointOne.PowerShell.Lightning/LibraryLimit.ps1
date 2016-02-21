#
# LibraryLimit.ps1
#
Connect-SPOnline https://aspc1606.sharepoint.com/sites/large -Credentials "Aspc1606"
Connect-SPOnline https://aspc1606.sharepoint.com/sites/dev -Credentials "Aspc1606"

$libraryName = "LargeLibrary"


$folderNames = ("HR","Sales","Marketing","Dev","Research","Social","Test")
$folderNames | Foreach { 
    $folderName = $_ 
    (0..2) | % {
        Write-Output "$libraryName/$folderName/TestDocument$folderName$_.docx"
    }
}
get-help Copy-Item -Detailed
get-help Remove-Item -Detailed
